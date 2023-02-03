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
    RandomAI _ai;

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
        _ai = new RandomAI(coordinateX, coordinateY, 2, 8, 10);
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
        Debug.Log($"Temp: {_ai.temp}");
        Debug.Log($"Satiety: {_ai.satiety}");
        Debug.Log($"Condition: {_ai.condition}");
    }
    public void RestartCreature()
    {
        Destroy(asv);
        coordinateX = 10;
        coordinateY = 10;
        _ai = new RandomAI(coordinateX, coordinateY, 2, 8, 10);
        asv = Instantiate(creature, new Vector3(coordinateX, coordinateY, 0), Quaternion.identity);
        Info();
    }
    
    IEnumerator Move()
    {
        while (true)
        {
            if (_ai.condition is Dead)
            {
                Debug.LogWarning("Creature DEAD");
                worldManager.world = new World(20, 20, 0, 10, 0, 10);
                worldManager.ShowWorld();
                RestartCreature();
            }
            else
            {
                _ai.ChooseMove(worldManager.world);
                Info();
                asv.transform.position = new Vector3(_ai.coordinateX, _ai.coordinateY, 0);
            }
            yield return new WaitForSeconds(0.5f);
        }
    }
}
