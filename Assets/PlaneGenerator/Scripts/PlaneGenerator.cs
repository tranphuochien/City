using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using City;

public class PlaneGenerator : MonoBehaviour
{
    public Camera mCamera;
    private CityController cityController;
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

    // reference to player object. it is required to manage chunks on the scene.
    [SerializeField]
    private Transform player = null;

    // total number of chunks that actually exist in the scene
    private int NUMBER_OF_CHUNK = 5;
    private int[,] mapData;
    // list of references to chunks in the scence
    private Queue<Transform> chunks;
    // reference to chunk that the player is on
    private Transform currentChunk;
    private int indexOfCurrentChunk;
    private int currentChunkPositionZ = 0;
    private int currentChunkPositionX = 0;
    
    private void Awake()
    {
        InitData();
        InitPositionCamera();
        InitializeChunksList();
    }

    private void InitData()
    {
        cityController = CityController.GetInstance();
        mapData = cityController.GetMapCityData();
        NUMBER_OF_CHUNK = cityController.GetNumberOfChunk();
    }

    private void InitPositionCamera()
    {
        switch(NUMBER_OF_CHUNK)
        {
            case 5:
                mCamera.transform.position = new Vector3(20.3f, 45, 20.2f);
                break;
            case 6:
                mCamera.transform.position = new Vector3(23.7f, 50, 28.16f);
                break;
            case 7:
                mCamera.transform.position = new Vector3(29.4f, 60, 35.2f);
                break;
        }
      
    }

    private void InitializeChunksList()
    {
        chunks = new Queue<Transform>();
        for (int i = 0; i < NUMBER_OF_CHUNK; i++)
        {
            for (int j = 0; j < NUMBER_OF_CHUNK; j++)
            {
                int valueCell = mapData[i,j];
                
                GameObject _chunk = Instantiate<GameObject>(GetTextureInCell(i,j));
                _chunk.transform.position = NextChunkPosition();
                currentChunkPositionZ += (int)chunkLength;

                chunks.Enqueue(_chunk.transform);
            }
            currentChunkPositionX += (int)chunkLength;
            currentChunkPositionZ = 0;
        }
    }

    private GameObject GetTextureInCell(int i, int j)
    {
        int top, bottom, left, right, center;

        center = mapData[i, j];
        if (i == 0) { top = -1; } else { top = mapData[i - 1, j]; }
        if (i == NUMBER_OF_CHUNK - 1) { bottom = -1; } else { bottom = mapData[i + 1, j]; }
        if (j == 0) { left = -1; } else { left = mapData[i, j - 1]; }
        if (j == NUMBER_OF_CHUNK - 1) { right = -1; } else { right = mapData[i, j + 1]; }

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
