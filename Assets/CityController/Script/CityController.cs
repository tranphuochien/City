using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace City
{
    public class CityController : ICityController
    {

        public String hardcodeFileMap = "./Maps/map06.csv";

        // total number of chunks that actually exist in the scene
        private int NUMBER_OF_CHUNK_HEIGHT = 5;
        private int NUMBER_OF_CHUNK_WIDTH = 5;
        private int[,] mapData;
        private ArrayList listFileMap = new ArrayList();
        private readonly System.Random rnd = new System.Random();


        private static CityController _instance = null;

        private CityController()
        {
            ReadMapFromFile();
        }

        public static CityController GetInstance()
        {
            if (_instance == null)
            {
                _instance = new CityController();
            }
            return _instance;
        }

        private int[,] ReadMapFromFile()
        {
            String fileName = GetFileMap();
            using (var reader = new StreamReader(@fileName))
            {
                var lineFirst = reader.ReadLine();
                var valuesFirst = lineFirst.Split(',');

                NUMBER_OF_CHUNK_HEIGHT = Int32.Parse(valuesFirst[0]);
                NUMBER_OF_CHUNK_WIDTH = Int32.Parse(valuesFirst[1]);
                mapData = new int[NUMBER_OF_CHUNK_HEIGHT, NUMBER_OF_CHUNK_WIDTH];
                for (int i = 0; i < NUMBER_OF_CHUNK_HEIGHT; i++)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');
                    for (int j = 0; j < NUMBER_OF_CHUNK_WIDTH; j++)
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

        public int GetNumberOfChunkHeight()
        {
            return NUMBER_OF_CHUNK_HEIGHT;
        }

        public int GetNumberOfChunkWidth()
        {
            return NUMBER_OF_CHUNK_WIDTH;
        }

        public System.Random GetRandomSystem()
        {
            return rnd;
        }

        public int GetDataIndexOf(int i, int j)
        {
            if (i > NUMBER_OF_CHUNK_HEIGHT || j > NUMBER_OF_CHUNK_WIDTH)
            {
                return -1;
            }
            return mapData[i, j];
        }

        public Vector2 GetPositionOnMap(float x, float z)
        {
            x = (Math.Abs(x) / 10);
            z = (Math.Abs(z) / 10);
            x = (float) Math.Round(x);
            z = (float)Math.Round(z);
            return new Vector2(x, z);
        }
    }
}