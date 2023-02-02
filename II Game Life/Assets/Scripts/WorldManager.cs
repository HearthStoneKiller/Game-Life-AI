using System.Collections;
using System.Drawing;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEditor.TextCore.Text;
using UnityEngine;
using Microsoft.Unity.VisualStudio.Editor;
using Unity.VisualScripting;
using System.Diagnostics.Tracing;

public class WorldManager : MonoBehaviour
{
    public World world;
    public int height;
    public int width;

    public GameObject cell;
    public GameObject food;
    public void Awake()
    {
        width = 20;
        height = 20;
        //World(importedX, importedY, minFood, maxFood, minWarm, maxWarm)
        world = new World(width, height, 0, 10, 0, 20);
        ShowWorld();
        StartCoroutine(Move());
    }
    public void Update()
    {

    }
    public void ShowWorld()
    {
        GameObject[] cellsToDelete = GameObject.FindGameObjectsWithTag("Cell");
        GameObject[] foodToDelete = GameObject.FindGameObjectsWithTag("Food");
        foreach (GameObject cell in cellsToDelete)
            Destroy(cell);
        foreach (GameObject food in foodToDelete)
            Destroy(food);
        ShowWarmMap();
        ShowFoodMap();
    }
    public void ShowFoodMap()
    {
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                SpriteRenderer spriteRenderer = food.GetComponent<SpriteRenderer>();
                spriteRenderer.color = new UnityEngine.Color(1 - (float)world.foodMap.map[i, j] / world.foodMap.maxVolume, 1, 1 - (float)world.foodMap.map[i, j] / world.foodMap.maxVolume, 1f);
                Instantiate(food, new Vector3(j, i, 0), Quaternion.identity);
            }
        }
    }
    public void ShowWarmMap()
    {
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                SpriteRenderer spriteRenderer = cell.GetComponent<SpriteRenderer>();
                spriteRenderer.color = new UnityEngine.Color((float)world.warmMap.map[i, j] / world.warmMap.maxVolume, 0, 1 - (float)world.warmMap.map[i, j] / world.warmMap.maxVolume, 1f);
                Instantiate(cell, new Vector3(j, i, 0), Quaternion.identity);
            }
        }
    }
    IEnumerator Move()
    {
        int count = 0;
        while (true)
        {
            count++;
            if (count == 1)
            {
                world.ChangeWorld();
                ShowWorld();
                count = 0;
            }
            else
            {
                ShowWorld();
            }
            yield return new WaitForSeconds(0.5f);
        }
    }
}