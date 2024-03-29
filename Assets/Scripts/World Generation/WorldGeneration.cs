using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGeneration : MonoBehaviour
{
    //Game Play
    private float chunkSpawnZ;
    private Queue<Chunk> activeChunks = new Queue<Chunk>();
    private List<Chunk> chunkPool = new List<Chunk>();

    //Configurable fields
    [SerializeField] private int firstChunkSpawnPosition = -10;
    [SerializeField] private int chunkOnScreen = 5;
    [SerializeField] private float deSpawnDistance = 5.0f;

    [SerializeField] private List<GameObject> chunkPrefab;
    [SerializeField] private Transform cameraTransform;

    #region TO DELETE $$
    private void Awake()
    {
        ResetWorld();
    }
    #endregion

    private void Start()
    {
        //check if we have an empty chunkprefab list
        if(chunkPrefab.Count == 0)
        {
            Debug.Log("No chunk prefab found on the world generator , please assign some chunks");
            return;
        }
        //try to assign the camera transform
        if(!cameraTransform)
        {
            cameraTransform = Camera.main.transform;
            Debug.Log("we've assigned  camera transform automatically");
        }
    }
    private void Update()
    {
        ScanPosition();
    }

    public void ScanPosition()
    {
        float cameraZ = cameraTransform.position.z; 
        Chunk lastChunk = activeChunks.Peek();
        if(cameraZ >= lastChunk.transform.position.z + lastChunk.ChunkLength + deSpawnDistance)
        {
            SpawnNewChunk();
            DeleteLastChunk();  
        }
    }
    private void SpawnNewChunk()
    {
        //Get a random index which prefab will be spawn
        int randomIndex = Random.Range(0, chunkPrefab.Count);

        //Does it already exist with my pool
        Chunk chunk = chunkPool.Find(x => !x.gameObject.activeSelf && x.name == (chunkPrefab[randomIndex].name + "Clone"));

        //Create a chunk is above is null
        if (!chunk)
        {
            GameObject go = Instantiate(chunkPrefab[randomIndex], transform);
            chunk = go.GetComponent<Chunk>();
        }

        //place the object and show it
        chunk.transform.position = new Vector3(0, 0, chunkSpawnZ);
        chunkSpawnZ += chunk.ChunkLength;

        //store the value to resuse in our pool
        activeChunks.Enqueue(chunk);
        chunk.ShowChunk();
    }
    private void DeleteLastChunk()
    {
        Chunk chunk = activeChunks.Dequeue();
        chunk.HideChunk();
        chunkPool.Add(chunk);
    }
    public void ResetWorld()
    {
        //Reset the chunkSpawnz
        chunkSpawnZ = firstChunkSpawnPosition;
        for (int i = activeChunks.Count; i != 0; i--)
            DeleteLastChunk();

        for (int i = 0; i < chunkOnScreen; i++)
            SpawnNewChunk();
    }
}
