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
    RandomAI AI;

    int coordinateX;
    int coordinateY;

    public GameObject creature;
    GameObject asv;
    // Start is called before the first frame update
    void Start()
    {
        coordinateX = 10;
        coordinateY = 10;
        AI = new RandomAI(coordinateX, coordinateY, 2, 8, 10, GameObject.FindGameObjectWithTag("WorldManager").GetComponent<WorldManager>().world);
        asv = Instantiate(creature, new Vector3(coordinateX, coordinateY, 0), Quaternion.identity);
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
        Debug.LogError(AI.temp);
        Debug.LogError(AI.satiety);
        Debug.LogError(AI.condition);
    }
    public void RestartCreature()
    {
        GameObject creat = GameObject.FindGameObjectWithTag("Creature");
        Destroy(creat);
        coordinateX = 10;
        coordinateY = 10;
        AI = new RandomAI(coordinateX, coordinateY, 2, 8, 10, GameObject.FindGameObjectWithTag("WorldManager").GetComponent<WorldManager>().world);
        asv = Instantiate(creature, new Vector3(coordinateX, coordinateY, 0), Quaternion.identity);
        Info();
    }
    IEnumerator Move()
    {
        while (true)
        {
            if (AI.condition.name == "Dead")
            {
                GameObject.FindGameObjectWithTag("WorldManager").GetComponent<WorldManager>().world = new World(20, 20, 0, 10, 0, 10);
                GameObject.FindGameObjectWithTag("WorldManager").GetComponent<WorldManager>().ShowWorld();
                RestartCreature();
            }
            else
            {
                AI.ChooseMove();
                Info();
                asv.transform.position = new Vector3(AI.coordinateX, AI.coordinateY, 0);
            }
            yield return new WaitForSeconds(0.5f);
        }
    }
}
