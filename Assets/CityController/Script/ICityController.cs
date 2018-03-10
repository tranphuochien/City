
using UnityEngine;

public interface ICityController {
    int[,] GetMapCityData();

    int GetNumberOfChunk();

    System.Random GetRandomSystem();

}
