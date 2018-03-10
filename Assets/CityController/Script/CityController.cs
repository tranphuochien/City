using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CityController : MonoBehaviour, ICityController
{

    public String hardcodeFileMap = "";

    // total number of chunks that actually exist in the scene
    private int NUMBER_OF_CHUNK = 5;
    private int[,] mapData;
    private ArrayList listFileMap = new ArrayList();
    private readonly System.Random rnd = new System.Random();

    private void Awake()
    {
        ReadMapFromFile();
    }

    private int[,] ReadMapFromFile()
    {
        String fileName = GetFileMap();
        using (var reader = new StreamReader(@fileName))
        {
            NUMBER_OF_CHUNK = Int32.Parse(reader.ReadLine()[0].ToString());
            mapData = new int[NUMBER_OF_CHUNK, NUMBER_OF_CHUNK];
            for (int i = 0; i < NUMBER_OF_CHUNK; i++)
            {
                var line = reader.ReadLine();
                var values = line.Split(',');
                for (int j = 0; j < NUMBER_OF_CHUNK; j++)
                {
                    mapData[i, j] = Int32.Parse(values[j]);
                }
            }
            reader.Close();
        }
        return mapData;
    }

    private string GetFileMap()
    {
        DirectoryInfo d = new DirectoryInfo(@"./Maps");
        FileInfo[] Files = d.GetFiles("*.csv"); //Getting Text files
        foreach (FileInfo file in Files)
        {
            listFileMap.Add(file.Name);
        }

        if (hardcodeFileMap != "")
        {
            return hardcodeFileMap;
        }
        //Get random file map
        int idx = rnd.Next(0, listFileMap.Count);

        return @"./Maps/" + (String)listFileMap[idx];
    }

    public int[,] GetMapCityData()
    {
        return mapData;
    }

    public int GetNumberOfChunk()
    {
        return NUMBER_OF_CHUNK;
    }

    public System.Random GetRandomSystem()
    {
        return rnd;
    }
}
