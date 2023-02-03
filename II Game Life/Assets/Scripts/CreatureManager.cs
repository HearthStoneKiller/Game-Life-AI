using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class CreatureManager : MonoBehaviour
{
    RandomII II;

    int coordinateX;
    int coordinateY;

    public GameObject creature;
    GameObject asv;

    private WorldManager worldManager;
    // Start is called before the first frame update
    void Start()
    {
        coordinateX = 10;
        coordinateY = 10;
        II = new RandomII(coordinateX, coordinateY, 2, 8, 10);
        asv = Instantiate(creature, new Vector3(coordinateX, coordinateY, 0), Quaternion.identity);
        worldManager = GameObject.FindGameObjectWithTag("WorldManager").GetComponent<WorldManager>();
        Info();
        StartCoroutine(Move());
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void ShowCreature()
    {

    }
    public void RandomPlace(int maxX, int maxY)
    {

    }
    public void Info()
    {
        Debug.Log($"Temp: {II.temp}");
        Debug.Log($"Satiety: {II.satiety}");
        Debug.Log($"Condition: {II.condition}");
    }
    public void RestartCreature()
    {
        Destroy(asv);
        coordinateX = 10;
        coordinateY = 10;
        II = new RandomII(coordinateX, coordinateY, 2, 8, 10);
        asv = Instantiate(creature, new Vector3(coordinateX, coordinateY, 0), Quaternion.identity);
        Info();
    }
    
    IEnumerator Move()
    {
        while (true)
        {
            if (II.condition is Dead)
            {
                Debug.LogWarning("Creature DEAD");
                worldManager.world = new World(20, 20, 0, 10, 0, 10);
                worldManager.ShowWorld();
                RestartCreature();
            }
            else
            {
                II.ChooseMove(worldManager.world);
                Info();
                asv.transform.position = new Vector3(II.coordinateX, II.coordinateY, 0);
            }
            yield return new WaitForSeconds(0.5f);
        }
    }
}
