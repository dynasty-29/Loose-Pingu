using UnityEngine;
using UnityEngine.InputSystem;


public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    public bool SwipeLeft { get; private set; }
    public bool SwipeRight { get; private set; }
    public bool SwipeUp { get; private set; }
    public bool SwipeDown { get; private set; }

    private Vector2 startTouch;
    private Vector2 swipeDelta;

    private bool isSwiping;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Optionally: DontDestroyOnLoad(gameObject);
    }
     private void Update()
     {
        SwipeLeft = SwipeRight = SwipeUp = SwipeDown = false;
        Debug.Log("Swipe Direction: " + swipeDelta);

        #region Standalone Inputs

        if (Input.GetKeyDown(KeyCode.LeftArrow))
         {
             SwipeLeft = true;
         }
         else if (Input.GetKeyDown(KeyCode.RightArrow))
         {
             SwipeRight = true;
         }
         else if (Input.GetKeyDown(KeyCode.UpArrow))
         {
             SwipeUp = true;
         }
         else if (Input.GetKeyDown(KeyCode.DownArrow))
         {
             SwipeDown = true;
         }

         #endregion

         #region Mobile Inputs

         if (Input.touches.Length > 0)
         {
             if (Input.touches[0].phase == UnityEngine.TouchPhase.Began)
             {
                 isSwiping = true;
                 startTouch = Input.touches[0].position;
             }
             else if (Input.touches[0].phase == UnityEngine.TouchPhase.Canceled || Input.touches[0].phase == UnityEngine.TouchPhase.Ended)
             {
                 ResetSwipe();
             }
         }

         swipeDelta = Vector2.zero;
         if (isSwiping)
         {
             if (Input.touches.Length > 0)
             {
                 swipeDelta = Input.touches[0].position - startTouch;
             }

             // Check for horizontal swipe
             if (swipeDelta.magnitude > 125)
             {
                 float x = swipeDelta.x;
                 float y = swipeDelta.y;

                 if (Mathf.Abs(x) > Mathf.Abs(y))
                 {
                     if (x < 0) SwipeLeft = true;
                     else SwipeRight = true;
                 }
                 else
                 {
                     if (y < 0) SwipeDown = true;
                     else SwipeUp = true;
                 }

                 ResetSwipe();
             }
         }

         #endregion
     }

    private void ResetSwipe()
    {
        startTouch = swipeDelta = Vector2.zero;
        isSwiping = false;
    }
    /*
    //There should only be one InputManager inthe scene
    private static InputManager instance;
    public static InputManager Instance {  get { return instance; } }

    //action schemes
    private RunnerInputActions actionScheme;

    // Configuration
    [SerializeField] private float sqrSwipeDeadzone = 50.0f;


    #region privates 
    private Vector2 touchPosition;
    private bool tap;
    private Vector2 startDrag;
    private bool swipeLeft;
    private bool swipeRight;
    private bool swipeUp;
    private bool swipeDown;
    #endregion

    #region public properties
    public Vector2 TouchPosition { get { return touchPosition; } }
    public bool Tap { get { return tap; } }
    public bool SwipeLeft { get { return swipeLeft; } }
    public bool SwipeRight { get { return swipeRight; } }
    public bool SwipeUp { get { return swipeUp; } }
    public bool SwipeDown { get { return swipeDown; } }
    #endregion

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
        SetupControl();
    }
    private void LateUpdate()
    {
        ResetInputs();
    }
    private void ResetInputs()
    {
        tap = swipeLeft = swipeRight = swipeUp = swipeDown = false;
    }

    public void SetupControl()
    {
        actionScheme = new RunnerInputActions();

        //register different actions
        actionScheme.Gameplay.Tap.performed += ctx => OnTap(ctx);
        actionScheme.Gameplay.TouchPosition.performed += ctx => OnPosition(ctx);
        actionScheme.Gameplay.StartDrag.performed += ctx => OnStartDrag(ctx);
        actionScheme.Gameplay.EndDrag.performed += ctx => OnEndDrag(ctx);

    }

    private void OnEndDrag(InputAction.CallbackContext ctx)
    {
        Vector2 delta = touchPosition - startDrag;
        float sqrDistance = delta.sqrMagnitude;

        // Confirmed swipe
        if (sqrDistance > sqrSwipeDeadzone)
        {
            float x = Mathf.Abs(delta.x);
            float y = Mathf.Abs(delta.y);

            if (x > y) // Left or Right
            {
                if (delta.x > 0)
                    swipeRight = true;
                else
                    swipeLeft = true;
            }
            else // Up or Down
            {
                if (delta.y > 0)
                    swipeUp = true;
                else
                    swipeDown = true;
            }
        }

        startDrag = Vector2.zero;

    }

    private void OnStartDrag(InputAction.CallbackContext ctx)
    {
        startDrag = touchPosition;
    }

    private void OnPosition(InputAction.CallbackContext ctx)
    {
        touchPosition = ctx.ReadValue<Vector2>();
    }

    private void OnTap(InputAction.CallbackContext ctx)
    {
        tap = true;
    }
    public void OnEnable()
    {
        actionScheme.Enable();
    }
    public void OnDisable()
    {
        actionScheme.Disable();
    }*/

}
