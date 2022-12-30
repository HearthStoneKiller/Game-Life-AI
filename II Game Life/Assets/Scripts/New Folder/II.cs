using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class II : MonoBehaviour
{
    public int coordinateX;
    public int coordinateY;

    public double energy;
    public double satiety;
    public Enemy enemy;

    public int mindDeep;

    public GameObject WorldManager;
    WorldManager manager;
    void Start()
    {
        energy = Mathf.Round(Random.Range(0f,20f))/2;
        satiety = Mathf.Round(Random.Range(0f, 20f))/2;

        mindDeep = 3;

        WorldManager = GameObject.FindGameObjectWithTag("WorldManager");
        manager = WorldManager.GetComponent<WorldManager>();
        EnemyManager enemyManager = new EnemyManager();

        //StartCoroutine(Step(50));
        CheckCondition();
        StartCoroutine(StartII());
    }
    void Update()
    {
        
    }
    public void MoveRight()
    {
        coordinateX++;
        transform.Translate(new Vector3(1,0,0));
    }
    public void MoveLeft()
    {
        coordinateX--;
        transform.Translate(new Vector3(-1, 0, 0));
    }
    public void MoveUp()
    {
        coordinateY++;
        transform.Translate(new Vector3(0, 1, 0));
    }
    public void MoveDown()
    {
        coordinateY--;
        transform.Translate(new Vector3(0, -1, 0));
    }
    public void Stay()
    {

    }
    public void RandomMove()
    {
        int i = Random.Range(0, 5);
        switch (i)
        {
            case 0:
                Stay();
                break;
            case 1:
                MoveRight();
                break;
            case 2:
                MoveLeft();
                break;
            case 3:
                MoveUp();
                break;
            case 4:
                MoveDown();
                break;
        }
    }
    public void CheckCondition()
    {
        CheckEnergy();
        CheckSatiety();
    }
    public void CheckEnergy()
    {
        if (energy >= 5)
        {
            enemy = new HoldWarm();
            Debug.Log("Warm is normal");
        }
        else if ((energy < 5) || (energy > 0))
        {
            enemy = new FindWarm();
            Debug.Log("Warm is bad");
        }
        else if (energy <= 0)
        {
            enemy = new Hold();
            Debug.Log("Dead");
        }
    }
    public void CheckSatiety()
    {
        if (satiety >= 5)
        {
            enemy = new HoldFood();
            Debug.Log("Satiety is normal");
        }
        else if ((satiety < 5) || (satiety > 0))
        {
            enemy = new FindFood();
            Debug.Log("Satiety is bad");
        }
        else if (satiety <= 0)
        {   
            enemy = new Hold();
            Debug.Log("Dead");
        }
    }
    IEnumerator Step(int countOfSteps)
    {
        for (int i = 0;i < countOfSteps;i++)
        {
            RandomMove();
            Debug.Log("Step");
            yield return new WaitForSeconds(1);
        }
    }
    IEnumerator StartII()
    {
        while (true)
        {
            RandomMove();
            yield return new WaitForSeconds(1);
        }
    }
    public int Search(int mindDeep)
    {
        int[] way = new int[mindDeep];
        double result = 0;
        int[] number = new int[mindDeep];
        for (int i = 0;i < (int)Mathf.Pow(5, mindDeep); i++)
        {
            int[] timeNumber = new int[mindDeep];
            for (int j = 0; j < mindDeep; j++)
            {
                timeNumber[j] = i ;
            }
        }


        return way[0];
    }
}
public class SemanticNetwork
{

}
public class SemanticNetworkNode
{

}
public class SemanticNetworkEdge
{

}