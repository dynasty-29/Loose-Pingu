using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    [HideInInspector] public Vector3 moveVector;
    [HideInInspector] public float verticalVelocity;
    [HideInInspector] public bool isGrounded;
    [HideInInspector] public int currentLane;

    public float distanceInBetweenLanes = 3.0f;
    public float baseRunSpeed = 5.0f;
    public float baseSidewaySpeed = 10.0f;
    public float gravity = 14.0f;
    public float terminalVelocity = 20.0f;

    public CharacterController controller;
    public Animator anim;
    

    private BaseState state;
    private bool isPaused;
    public void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        state = GetComponent<RunningState>();
        state.Construct();
        isPaused = true;
    }
    void Update()
    {
        if (!isPaused)
            UpdateMotor();
        //
        state.Transition();  // Handle state transitions
        Vector3 motion = state.ProcessMotion();
        //
        transform.position += Vector3.forward * Time.deltaTime * 5f; // Just move forward
    }


    /*public void Update()
    {
        if (!isPaused)
            UpdateMotor();
    }*/

    private void UpdateMotor()
    {
        //check if we're grounded
        isGrounded = controller.isGrounded;

        //How should we be moving right now
        moveVector = state.ProcessMotion();

        //are we trying to change state
        state.Transition();

        // Feed our animator some values
        anim?.SetBool("IsGrounded", isGrounded);
        anim?.SetFloat("Speed", Mathf.Abs(moveVector.z));

        //Move the player
        controller.Move(moveVector * Time.deltaTime);
    }
    public float SnapToLane()
    {
        float r = 0.0f;

        //fif we're not directly on top of a lane
        if(transform.position.x != (currentLane * distanceInBetweenLanes))
        {
            float deltaToDesiredPosition = (currentLane * distanceInBetweenLanes) - transform.position.x;
            r = (deltaToDesiredPosition > 0) ? 1 : -1;
            r *= baseSidewaySpeed;

            float actualDistance = r * Time.deltaTime;
            if(Mathf.Abs(actualDistance)>Mathf.Abs(deltaToDesiredPosition))
            {
                r = deltaToDesiredPosition * (1/Time.deltaTime);
            }
        }
        else
        {
            r = 0;
        }
        return r;
    }
    public void ChangeLane(int direction)
    {
        if (Input.GetKeyDown(KeyCode.A))
            ChangeLane(-1);
        else if (Input.GetKeyDown(KeyCode.D))
            ChangeLane(1);
        //
        Debug.Log("Changing lane in direction: " + direction);
        currentLane = Mathf.Clamp(currentLane + direction, -1, 1);
    }
    public void ChangeState(BaseState s)
    {
        state.Destruct();
        state = s;
        state.Construct();
    }
    public void ApplyGravity()
    {
        verticalVelocity -= gravity * Time.deltaTime;
        if (verticalVelocity < -terminalVelocity)
            verticalVelocity = -terminalVelocity;
    }
    public void PausePlayer()
    {
        isPaused = true;
    }
    public void ResumePlayer()
    {
        isPaused = false;
    }
    public void RespawnPlayer()
    {
        ChangeState(GetComponent<RespawnState>());
        //GameManager.Instance.ChangeCamera(GameCamera.Respawn);
    }
    public void ResetPlayer()
    {
        currentLane = 0;
        transform.position = Vector3.zero;
        anim?.SetTrigger("Idle");
        PausePlayer();
        ChangeState(GetComponent<RunningState>());
    }

    public void OnControllerColliderHit(ControllerColliderHit hit)
    {
        string hitLayerName = LayerMask.LayerToName(hit.gameObject.layer);

        if (hitLayerName == "Death")
        {
            //AudioManager.Instance.PlaySFX(hitSound, 0.7f);
            ChangeState(GetComponent<DeathState>());
        }
    }
}
