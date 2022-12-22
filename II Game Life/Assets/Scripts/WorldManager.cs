using System.Collections;
using System.Drawing;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEditor.TextCore.Text;
using UnityEngine;
using Microsoft.Unity.VisualStudio.Editor;

public class WorldManager : MonoBehaviour
{
    double[,] situation;
    public FoodMap foodMap;
    public WarmMap warmMap;

    public GameObject cell;
    public GameObject food;

    public GameObject creature;
    public int countOfCreatures;
    public void Start()
    {
        countOfCreatures = 1;
        int x = 20;
        int y = 20;
        CreateRandomWorld(x, y, 0, 10, 0, 10);
        ShowWorld();
        ShowWarmMap();
        ShowFoodMap();

        RandomPlace(x, y);
    }
    public void Update()
    {
         
    }
    public WorldManager()
    {

    }
    public WorldManager(int importedX, int importedY)
    {
        situation = new double[importedY, importedX];
        foodMap.map = new double[importedY, importedX];
        warmMap.map = new double[importedY, importedX];
        for (int i = 0;i < importedY;i++)
        {
            for (int j = 0; j < importedX; j++)
            {
                situation[i,j] = 0;
                foodMap.map[i, j] = 0;
                warmMap.map[i, j] = 0;
            }
        }
    }
    public void SetSituationScale(int importedX, int importedY)
    {
        situation = new double[importedY, importedX];
    }
    public void SetSituation(double[,] importemMap)
    {
        situation = importemMap;
    }
    public void SetFoodMapScale(int importedX, int importedY)
    {
        foodMap.map = new double[importedY, importedX];
        SetFoodMapScale(importedX, importedY);
        SetWarmMapScale(importedX, importedY);
    }
    public void SetFoodMap(double[,] importemMap)
    {
        foodMap.map = importemMap;
    }
    public void SetWarmMapScale(int importedX, int importedY)
    {
        warmMap.map = new double[importedY, importedX];
    }
    public void SetWarmMap(double[,] importemMap)
    {
        warmMap.map = importemMap;
    }
    public void CreateRandomWorld(int importedX, int importedY, int minFood, int maxFood, int minWarm, int maxWarm)
    {
        situation = new double[importedY, importedX];
        foodMap = new FoodMap(importedX, importedY, minFood, maxFood);
        warmMap = new WarmMap(importedX, importedY, minWarm, maxWarm);
    }
    public void ShowWorld()
    {
        for (int i = 0; i < situation.GetLength(0); i++)
        {
            for (int j = 0; j < situation.GetLength(1); j++)
            {
                SpriteRenderer spriteRenderer = cell.GetComponent<SpriteRenderer>();
                spriteRenderer.color = new UnityEngine.Color(1, 1, 1, 1f);
                Instantiate(cell,new Vector3(i,j,0),Quaternion.identity);
            }
        }
    }
    public void ShowFoodMap()
    {
        for (int i = 0; i < foodMap.map.GetLength(0); i++)
        {
            for (int j = 0; j < foodMap.map.GetLength(1); j++)
            {
                SpriteRenderer spriteRenderer = food.GetComponent<SpriteRenderer>();
                spriteRenderer.color = new UnityEngine.Color(1 - (float)foodMap.map[i, j] / foodMap.maxVolume, 1 , 1 - (float)foodMap.map[i, j] / foodMap.maxVolume, 1f);
                Instantiate(food, new Vector3(i, j, 0), Quaternion.identity);
            }
        }
    }
    public void ShowWarmMap()
    {
        for (int i = 0; i < situation.GetLength(0); i++)
        {
            for (int j = 0; j < situation.GetLength(1); j++)
            {
                SpriteRenderer spriteRenderer = cell.GetComponent<SpriteRenderer>();
                spriteRenderer.color = new UnityEngine.Color((float)warmMap.map[i, j] / warmMap.maxVolume, 0, 1 - (float)warmMap.map[i, j] / warmMap.maxVolume, 1f);
                Instantiate(cell, new Vector3(i, j, 0), Quaternion.identity);
            }
        }
    }
    public void RandomPlace(int maxX, int maxY)
    {
        int coordinateX = Random.Range(0, maxX);
        int coordinateY = Random.Range(0, maxY);
        II ii = creature.GetComponent<II>();
        ii.coordinateX = coordinateX;
        ii.coordinateY = coordinateY;
        Instantiate(creature, new Vector3(coordinateX, coordinateY, 0), Quaternion.identity);
    }
}
public class FoodMap
{
    public double[,] map;
    public int maxVolume;
    public int minVolume;
    public FoodMap(double[,] importedMap)
    {
        map = importedMap;
    }
    public FoodMap(int importedX, int importedY)
    {
        maxVolume = 0;
        minVolume = 0;
        map = new double[importedY, importedX];
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
        map = new double[importedY, importedX];
        for (int i = 0; i < importedY; i++)
        {
            for (int j = 0; j < importedX; j++)
            {
                map[i, j] = Random.Range(min, max);
            }
        }
    }
}
public class WarmMap
{
    public double[,] map;
    public int maxVolume;
    public int minVolume;
    public WarmMap(double[,] importedMap)
    {
        map = importedMap;
    }
    public WarmMap(int importedX, int importedY)
    {
        maxVolume = 0;
        minVolume = 0;
        map = new double[importedY, importedX];
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
        map = new double[importedY, importedX];
        for (int i = 0; i < importedY; i++)
        {
            for (int j = 0; j < importedX; j++)
            {
                map[i, j] = Random.Range(min, max);
            }
        }
    }
}
  