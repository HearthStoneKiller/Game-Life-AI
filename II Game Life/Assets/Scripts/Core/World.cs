using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World
{
    public int width;
    public int height;
    public FoodMap foodMap;
    public WarmMap warmMap;
    World()
    {
 
    }
    public World(int importedX, int importedY, int minFood, int maxFood, int minWarm, int maxWarm)
    {
        width = importedX;
        height = importedY;
        foodMap = new FoodMap(importedX, importedY, minFood, maxFood);
        warmMap = new WarmMap(importedX, importedY, minWarm, maxWarm);
    }
    public void ChangeWorld()
    {
        foodMap.Change();
        warmMap.Change();
    }
}
public class FoodMap: IChangeable
{
    public int[,] map;
    public int maxVolume;
    public int minVolume;
    public FoodMap(int[,] importedMap)
    {
        map = importedMap;
    }
    public FoodMap(int importedX, int importedY)
    {
        maxVolume = 0;
        minVolume = 0;
        map = new int[importedY, importedX];
        for (int i = 0; i < importedY; i++)
        {
            for (int j = 0; j < importedX; j++)
            {
                map[i, j] = 0;
            }
        }
    }
    public FoodMap(int importedX, int importedY, int min, int max)
    {
        maxVolume = max;
        minVolume = min;
        map = new int[importedY, importedX];
        for (int i = 0; i < importedY; i++)
        {
            for (int j = 0; j < importedX; j++)
            {
                map[i, j] = Random.Range(min, max + 1);
            }
        }
    }
    public void Change()
    {
        int change;
        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); j++)
            {
                change = Random.Range(-1,2);
                if ((map[i, j] + change >= minVolume) && (map[i, j] + change <= maxVolume))
                {
                    map[i, j] += change;
                }
                else
                {

                }
            }
        }
    }
}
public class WarmMap: IChangeable
{
    public int[,] map;
    public int maxVolume;
    public int minVolume;
    public WarmMap(int[,] importedMap)
    {
        map = importedMap;
    }
    public WarmMap(int importedX, int importedY)
    {
        maxVolume = 0;
        minVolume = 0;
        map = new int[importedY, importedX];
        for (int i = 0; i < importedY; i++)
        {
            for (int j = 0; j < importedX; j++)
            {
                map[i, j] = 0;
            }
        }
    }
    public WarmMap(int importedX, int importedY, int min, int max)
    {
        maxVolume = max;
        minVolume = min;
        map = new int[importedY, importedX];
        for (int i = 0; i < importedY; i++)
        {
            for (int j = 0; j < importedX; j++)
            {
                map[i, j] = Random.Range(min, max + 1);
            }
        }
    }
    public void Change()
    {
        int change;
        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); j++)
            {
                change = Random.Range(-1, 2);
                if ((map[i, j] + change >= minVolume) && (map[i, j] + change <= maxVolume))
                {
                    map[i, j] += change;
                }
                else
                {

                }
            }
        }
    }
}
public interface IChangeable
{
    void Change();
}