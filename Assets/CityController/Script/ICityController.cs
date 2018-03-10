
using UnityEngine;

public interface ICityController {
    int[,] GetMapCityData();

    System.Random GetRandomSystem();

    int GetNumberOfChunkHeight();

    int GetNumberOfChunkWidth();

}
