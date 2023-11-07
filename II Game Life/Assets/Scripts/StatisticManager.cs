
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatisticManager : MonoBehaviour
{
    // Main
    //public WorldManager wm;
    //public CreatureManager cm;
    //public TimeManager tm;
    public int[,,] values;
    // Main
    public Optimal optimal;
    public Cold cold;
    public Hot hot;
    public Hunger hunger;
    public HungerAndCold hungerAndCold;
    public HungerAndHot hungerAndHot;
    // Main
    //
    //Counters
    private int maxNumberOfSteps = 200;
    private int numberOfStep = 0;
    private int numberOfEpoch = 0;
    //Counters
    //
    // Data of one epoch
    public  int timeInOptimal;
    public float timeOfLife;
    public int countOfAlive;
    public int countOfDead;
    // Data of one epoch
    //
    // Data
    public  int middleTimeInOptimal;
    public float middleTimeOfLife;
    // Data
    // Start is called before the first frame update
    void Start()
    {
        World world = new World(10, 10, 0, 10, 0, 20);
        AI ai = new RandomAI(5, 5, 2, 18, 10, world);
        values = new int[,,] {{{1,1},{1,1},{1,1},{1,1},{1,1},{1,1}},
                              {{2,2},{1,1},{1,1},{1,1},{1,1},{1,1}},
                              {{3,3},{1,1},{1,1},{1,1},{1,1},{1,1}},
                              {{4,4},{1,1},{1,1},{1,1},{1,1},{1,1}},
                              {{5,5},{1,1},{1,1},{1,1},{1,1},{1,1}},
                              {{6,6},{1,1},{1,1},{1,1},{1,1},{1,1}},
                              {{7,7},{1,1},{1,1},{1,1},{1,1},{1,1}},
                              {{8,8},{1,1},{1,1},{1,1},{1,1},{1,1}},
                              {{9,9},{1,1},{1,1},{1,1},{1,1},{1,1}},
                              {{0,0},{1,1},{1,1},{1,1},{1,1},{1,1}}};
        Dead dead = new Dead();
        for (int i = 0;i < values.GetLength(0);i++)
        {
            world = new World(10, 10, 0, 10, 0, 20);
            ChangeValues(i);

            ai.optimal = optimal;
            ai.cold = cold;
            ai.hot = hot;
            ai.hunger = hunger;
            ai.hungerAndCold = hungerAndCold;
            ai.hungerAndHot = hungerAndHot;

            countOfDead = 0;
            countOfAlive = 0;
            for (int j = 0;j < 1000;j++)
            {
                ai = new RandomAI(5, 5, 2, 18, 10, world);
                world = new World(10, 10, 0, 10, 0, 20);
                for (int y =0;y < 1000;y++)
                {
                    if (ai.condition.GetType() == typeof(Dead))
                    {
                        countOfDead++;
                        ai = new RandomAI(5, 5, 2, 18, 10, world);
                        world = new World(10, 10, 0, 10, 0, 20);
                        break;
                    }
                    if ((y == 999) && (ai.condition.GetType() != typeof(Dead)))
                    {
                        countOfAlive++;
                        ai = new RandomAI(5, 5, 2, 18, 10, world);
                        world = new World(10, 10, 0, 10, 0, 20);
                    }
                    ai.ChooseMove();
                    world.ChangeWorld();
                    Info(ai);
                }
            }
            Debug.Log("Количество смертей: " + countOfDead);
            Debug.Log("Количество выживших: " + countOfAlive);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void Info(AI ai)
    {
        Debug.LogError("Температура: "+ai.temp);
        Debug.LogError("Сытость: "+ai.satiety);
        Debug.LogError("Состояние: "+ai.condition);
    }
    public void IncreaseNumberOfEpoch()
    {
        if (numberOfEpoch == values.Length)
        {
            EndProcess();
        }
        else
        {
            numberOfEpoch++;
        }
    }
    public void ChangeValues(int numberOfEpoch)
    {
        optimal = new Optimal(values[numberOfEpoch, 0, 0], values[numberOfEpoch, 0, 1]);
        cold = new Cold(values[numberOfEpoch, 1, 0], values[numberOfEpoch, 1, 1]);
        hot = new Hot(values[numberOfEpoch, 2, 0], values[numberOfEpoch, 2, 1]);
        hunger = new Hunger(values[numberOfEpoch, 3, 0], values[numberOfEpoch, 3, 1]);
        hungerAndCold = new HungerAndCold(values[numberOfEpoch, 4, 0], values[numberOfEpoch, 4, 1]);
        hungerAndHot = new HungerAndHot(values[numberOfEpoch, 5, 0], values[numberOfEpoch, 5, 1]);
    }
    public void SaveData()
    {

    }
    public void EndProcess()
    {
        Application.Quit();
    }
}
