using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    public float ChunkLength;

    public Chunk ShowChunk()
    {
        transform.gameObject.BroadcastMessage("OnShowChunk", SendMessageOptions.DontRequireReceiver);
        gameObject.SetActive(true);
        return this;
    }
    public Chunk HideChunk()
    {
        gameObject.SetActive(false);
        return this;
    }
}
