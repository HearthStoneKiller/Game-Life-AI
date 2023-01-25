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

    public string condition;
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
    public void GetCondition()
    {
        if (satiety <= maxSatiety / 2)
        {
            if (temp <= maxTemp / 3)
            {
                condition = "Холодно";
            }
            else if (temp > maxTemp - maxTemp / 3)
            {
                condition = "Жарко";
            }
            else
            {
                condition = "Оптимально";
            }
        }
        else
        {
            if (temp <= maxTemp / 3)
            {
                condition = "Голод и Холодно";
            }
            else if (temp > maxTemp - maxTemp / 3)
            {
                condition = "Голод и Жарко";
            }
            else
            {
                condition = "Голод";
            }
        }
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
        GetCondition();
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