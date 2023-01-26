using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class II
{
    public int coordinateX;
    public int coordinateY;

    public int temp;// Температура
    public int maxTemp;// Температура смерти при жаре
    public int minTemp;// Температура смерти при холоде

    public int satiety;// Сытость
    public int maxSatiety;// Максимальное количество запасов

    public int mindDeep;

    public Condition condition;
    //energy = Mathf.Round(Random.Range(0f, 20f)) / 2;
    //satiety = Mathf.Round(Random.Range(0f, 20f)) / 2;
    //mindDeep = 1;
    public void MoveRight()
    {
        WorldManager worldManager = GameObject.FindGameObjectWithTag("WorldManager").GetComponent<WorldManager>();
        coordinateX++;
        if (coordinateX >= worldManager.width)
        {
            coordinateX--;
        }
    }
    public void MoveLeft()
    {
        WorldManager worldManager = GameObject.FindGameObjectWithTag("WorldManager").GetComponent<WorldManager>();
        coordinateX--;
        if (coordinateX < 0)
        {
            coordinateX++;
        }
    }
    public void MoveUp()
    {
        WorldManager worldManager = GameObject.FindGameObjectWithTag("WorldManager").GetComponent<WorldManager>();
        coordinateY++;
        if (coordinateY >= worldManager.height)
        {
            coordinateY--;
        }
    }
    public void MoveDown()
    {
        WorldManager worldManager = GameObject.FindGameObjectWithTag("WorldManager").GetComponent<WorldManager>();
        coordinateY--;
        if (coordinateY < 0)
        {
            coordinateY++;
        }
    }
    public void Stay()
    {

    }
    public abstract void ChooseMove(World world);
}
public class RandomII : II
{
    public RandomII(int importedX, int importedY, int importedMinTemp, int importedMaxTemp, int importedMaxSatiety)
    {
        coordinateX = importedX;
        coordinateY = importedY;

        temp = (importedMaxTemp - importedMinTemp) / 2;
        maxTemp = importedMaxTemp;
        minTemp = importedMinTemp;

        satiety = importedMaxSatiety / 2;
        maxSatiety = importedMaxSatiety;
        mindDeep = 0;

        condition = new Condition();
        condition = condition.CalculateCondition(temp, maxTemp, minTemp, satiety, maxSatiety);
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
    public override void ChooseMove(World world)
    {
        int foodMetric = condition.foodMetric;
        int tempMetric = condition.tempMetric;
        int[] metric = new int[5];
        for(int i = 0;i < mindDeep;i++)
        {
            metric[i] = world.foodMap.map[coordinateY,coordinateX] + world.warmMap.map[coordinateY, coordinateX];
        }
    }
}
public class BeginnerII : II
{
    public BeginnerII(int importedX, int importedY, int importedMinTemp, int importedMaxTemp, int importedMaxSatiety)
    {
        coordinateX = importedX;
        coordinateY = importedY;

        temp = (importedMaxTemp - importedMinTemp) / 2;
        maxTemp = importedMaxTemp;
        minTemp = importedMinTemp;

        satiety = importedMaxSatiety / 2;
        maxSatiety = importedMaxSatiety;
        mindDeep = 1;
    }
    public override void ChooseMove(World world)
    {

    }
}
public class MasterII : II
{
    public override void ChooseMove(World world)
    {

    }
}
