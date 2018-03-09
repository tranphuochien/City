using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneGenerator : MonoBehaviour {

    private static readonly int NUMBER_CHUNKS = 5;
    // road chunk prefab
    public GameObject roadChunk;
    public GameObject roadChunk90;
    public GameObject grassChunk;
    public GameObject crossTChunk;
    public GameObject crossTChunk90;
    public GameObject crossTChunk180;
    public GameObject crossTChunk270;
    public GameObject crossXChunk;

    // distance between edges of the chunk.
    public float chunkLength;

    // number of chunks to be activated at a time.
    public int drawingAmount = 3;

    // reference to player object. it is required to manage chunks on the scene.
    [SerializeField]
    private Transform player = null;

    // total number of chunks that actually exist in the scene
    [SerializeField]
    private int numberOfChunks = NUMBER_CHUNKS;

    // list of references to chunks in the scence
    private Queue<Transform> chunks;

    // reference to chunk that the player is on
    private Transform currentChunk;
    private int indexOfCurrentChunk;
    private int currentChunkPositionZ = 0;
    private int currentChunkPositionX = 0;
    private int [,] dummyMatrix = new int[,] {  { 1, 1, 1, 0, 0 }, 
                                                { 1, 0, 1, 0, 1 }, 
                                                { 0, 1, 1, 1, 1 },
                                                { 0, 0, 1, 0, 1 },
                                                { 0, 0, 0, 1, 1 },  };

    private void Awake()
    {
        InitializeChunksList();
    }

    private void InitializeChunksList()
    {
        chunks = new Queue<Transform>();
        for (int i = 0; i < numberOfChunks; i++)
        {
            for (int j = 0; j < numberOfChunks; j++)
            {
                int valueCell = dummyMatrix[i,j];
                
                GameObject _chunk = Instantiate<GameObject>(GetTextureInCell(i,j));
                _chunk.transform.position = NextChunkPosition();
                currentChunkPositionZ += (int)chunkLength;

                /*if (i != 0)
                    _chunk.SetActive(false);*/
                chunks.Enqueue(_chunk.transform);
            }
            currentChunkPositionX += (int)chunkLength;
            currentChunkPositionZ = 0;
        }
    }
    
    private GameObject GetTextureInCell(int i, int j)
    {
        int top, bottom, left, right, center;
        Debug.Log(i + "  " + j);
        center = dummyMatrix[i, j];
        if (i == 0) { top = -1; } else { top = dummyMatrix[i - 1, j]; }
        if (i == NUMBER_CHUNKS - 1) { bottom = -1; } else { bottom = dummyMatrix[i + 1, j]; }
        if (j == 0) { left = -1; } else { left = dummyMatrix[i, j - 1]; }
        if (j == NUMBER_CHUNKS - 1) { right = -1; } else { right = dummyMatrix[i, j + 1]; }

        if(center == 0)
        {
            return grassChunk;
        }
        if (center == 1 && left == 1 && right == 1 && top == 1 && bottom == 1)
        {
            return crossXChunk;
        }
        if ((left == 1 || right == 1) && top != 1 && bottom != 1)
        {
            return roadChunk;
        }
        if ((top == 1 || bottom == 1) && left != 1 && right != 1)
        {          
            return roadChunk90;
        }
        if (bottom != 1 && top == 1 && (left == 1 || right == 1)) {
            return crossTChunk;
        }
        if (left != 1 && right == 1 && (top == 1 || bottom == 1))
        {
            return crossTChunk90;
        }
        if (bottom == 1 && top != 1 && (left == 1 || right == 1))
        {
            return crossTChunk180;
        }
        if (left == 1 && right != 1 && (top == 1 || bottom == 1))
        {
            return crossTChunk270;
        }

        return roadChunk;
    }

    private void Start()
    {

    }

    private void FixedUpdate()
    {

        /*if (!player) return;

        // determine the chunk that the player is on
        currentChunk = GetCurrentChunk();
        indexOfCurrentChunk = GetIndexOfCurrentChunk();

        // Manage chunks based on current chunk that the player is on
        for (int i = indexOfCurrentChunk; i < (indexOfCurrentChunk + drawingAmount); i++)
        {
            i = Mathf.Clamp(i, 0, chunks.Count - 1);
            GameObject _chunkGO = (chunks.ToArray()[i]).gameObject;
            if (!_chunkGO.activeInHierarchy)
                _chunkGO.SetActive(true);
        }

        if (indexOfCurrentChunk > 0)
        {
            float _distance = Vector3.Distance(player.position, (chunks.ToArray()[indexOfCurrentChunk - 1]).position);
            if (_distance > (chunkLength * .75f))
                SweepPreviousChunk();
        }*/

    }

    private void SweepPreviousChunk()
    {
        Transform _chunk = chunks.Dequeue();
        _chunk.gameObject.SetActive(false);
        _chunk.position = NextChunkPosition();
        chunks.Enqueue(_chunk);
    }

    private Vector3 NextChunkPosition()
    {
        float _positionZ = currentChunkPositionZ;
        float _positionX = currentChunkPositionX;
       
        return new Vector3(_positionX, 0, _positionZ);
    }

    private Transform GetCurrentChunk()
    {
        Transform current_chunk = null;
        foreach (Transform c in chunks)
        {
            if (Vector3.Distance(player.position, c.position) <= (chunkLength / 2))
            {
                current_chunk = c;
                break;
            }
        }
        return current_chunk;
    }

    private int GetIndexOfCurrentChunk()
    {
        int index = -1;
        for (int i = 0; i < chunks.Count; i++)
        {
            if ((chunks.ToArray()[i]).Equals(currentChunk))
            {
                index = i;
                break;
            }
        }
        return index;
    }
}
