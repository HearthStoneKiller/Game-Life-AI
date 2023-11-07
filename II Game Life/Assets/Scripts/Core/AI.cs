using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class AI : IObserver<Time>
{
    public World world;
    public int coordinateX;
    public int coordinateY;

    public int temp;// �����������
    public int maxTemp;// ����������� ������ ��� ����
    public int minTemp;// ����������� ������ ��� ������

    public int satiety;// �������
    public int maxSatiety;// ������������ ���������� �������

    public int mindDeep;
    // Conditions
    public Optimal optimal;
    public Cold cold;
    public Hot hot;
    public Hunger hunger;
    public HungerAndCold hungerAndCold;
    public HungerAndHot hungerAndHot;
    Dead dead;
    public Condition condition;
    // Conditions
    public void Update(Time time)
    {
        //ChooseMove(world);
    }
    public void SetConditions(Optimal optimal, Cold cold, Hot hot, Hunger hunger, HungerAndCold hungerAndCold, HungerAndHot hungerAndHot)
    {
        this.optimal = optimal;
        this.cold = cold;
        this.hot = hot;
        this.hunger = hunger;
        this.hungerAndCold = hungerAndCold;
        this.hungerAndHot = hungerAndHot;
    }
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
    public abstract void ChooseMove();
    public void CalculateParameters(bool isMoved, int cellFood, int cellTemp)
    {
        if (isMoved)
        {
            satiety--;
        }
        if (cellFood > 0)
        {
            satiety++;
        }
        if (cellTemp > temp)
        {
            temp++;
        }
        else if (cellTemp < temp)
        {
            temp--;
        }
    }
}
public class RandomAI : AI
{
    public RandomAI(int importedX, int importedY, int importedMinTemp, int importedMaxTemp, int importedMaxSatiety, World world)
    {
        coordinateX = importedX;
        coordinateY = importedY;

        temp = (importedMaxTemp - importedMinTemp) / 2;
        maxTemp = importedMaxTemp;
        minTemp = importedMinTemp;

        //satiety = importedMaxSatiety / 2;
        satiety = importedMaxSatiety;
        maxSatiety = importedMaxSatiety;
        mindDeep = 1;

        condition = new Condition();
        condition = condition.CalculateCondition(temp, maxTemp, minTemp, satiety, maxSatiety);
        this.world = world;
    }
    public void RandomMove()
    {
        int i = UnityEngine.Random.Range(0, 5);
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
    public override void ChooseMove()
    {
        double[] metric = new double[5];
        metric[0] = CalculateMetric(coordinateX, coordinateY, world);
        metric[1] = CalculateMetric(coordinateX - 1, coordinateY, world);
        metric[2] = CalculateMetric(coordinateX, coordinateY + 1, world);
        metric[3] = CalculateMetric(coordinateX + 1, coordinateY, world);
        metric[4] = CalculateMetric(coordinateX, coordinateY - 1, world);
        int result = 0;
        for (int i = 1;i < 5; i++)
        {
            if (metric[result] <= metric[i])
            {
                result = i;
            }
        }
        if (result == 0)
        {
            Stay();
            CalculateParameters(false, world.foodMap.map[coordinateY,coordinateX], world.warmMap.map[coordinateY, coordinateX]);
            condition = condition.CalculateCondition(temp, maxTemp, minTemp, satiety, maxSatiety);
        }
        if (result == 1)
        {
            MoveLeft();
            CalculateParameters(true, world.foodMap.map[coordinateY, coordinateX], world.warmMap.map[coordinateY, coordinateX]);
            condition = condition.CalculateCondition(temp, maxTemp, minTemp, satiety, maxSatiety);
        }
        if (result == 2)
        {
            MoveUp();
            CalculateParameters(true, world.foodMap.map[coordinateY, coordinateX], world.warmMap.map[coordinateY, coordinateX]);
            condition = condition.CalculateCondition(temp, maxTemp, minTemp, satiety, maxSatiety);
        }
        if (result == 3)
        {
            MoveRight();
            CalculateParameters(true, world.foodMap.map[coordinateY, coordinateX], world.warmMap.map[coordinateY, coordinateX]);
            condition = condition.CalculateCondition(temp, maxTemp, minTemp, satiety, maxSatiety);
        }
        if (result == 4)
        {
            MoveDown();
            CalculateParameters(true, world.foodMap.map[coordinateY, coordinateX], world.warmMap.map[coordinateY, coordinateX]);
            condition = condition.CalculateCondition(temp, maxTemp, minTemp, satiety, maxSatiety);
        }
        if (world.foodMap.map[coordinateY, coordinateX] != 0)
        {
            world.foodMap.map[coordinateY, coordinateX]--;
        }
    }
    public double CalculateMetric(int importedX, int importedY, World world)
    {
        double result = 0; 
        int foodMetric = condition.foodMetric;
        int tempMetric = condition.tempMetric;
        try
        {
            if (temp <= (minTemp + maxTemp) / 2)
            {
                if (world.warmMap.map[importedY, importedX] < minTemp)// 1 ++
                {
                    result = (minTemp - world.warmMap.map[importedY, importedX]) * (-2) * tempMetric + world.foodMap.map[importedY, importedX] * foodMetric;
                }
                else if (world.warmMap.map[importedY, importedX] > maxTemp)// 4 ++
                {
                    result = ((maxTemp - temp) - (world.warmMap.map[importedY, importedX] - maxTemp) * 2) * tempMetric + world.foodMap.map[importedY, importedX] * foodMetric;
                }
                else if ((world.warmMap.map[importedY, importedX] >= minTemp) && (world.warmMap.map[importedY, importedX] < (minTemp + maxTemp) / 2))// 2 ++
                {
                    if (world.warmMap.map[importedY, importedX] >= temp)
                    {
                        result = ((world.warmMap.map[importedY, importedX] - minTemp) * 0.5 + (world.warmMap.map[importedY, importedX] - temp)) * tempMetric + world.foodMap.map[importedY, importedX] * foodMetric;
                    }
                    else
                    {
                        result = (world.warmMap.map[importedY, importedX] - minTemp) * 0.5 * tempMetric + world.foodMap.map[importedY, importedX] * foodMetric;
                    }
                }
                else if ((world.warmMap.map[importedY, importedX] >= (minTemp + maxTemp) / 2) && (world.warmMap.map[importedY, importedX] <= maxTemp))// 3 ++
                {
                    result = ((maxTemp - temp) - (maxTemp - world.warmMap.map[importedY, importedX]) * 0.5) * tempMetric + world.foodMap.map[importedY, importedX] * foodMetric;
                }
            }
            else
            {
                if (world.warmMap.map[importedY, importedX] < minTemp)// 1 ++
                {
                    result = ((temp - minTemp) - (minTemp - world.warmMap.map[importedY, importedX]) * 2) * tempMetric + world.foodMap.map[importedY, importedX] * foodMetric;
                }
                else if (world.warmMap.map[importedY, importedX] > maxTemp)// 4 ++
                {
                    result = (world.warmMap.map[importedY, importedX] - maxTemp) * (-2) * tempMetric + world.foodMap.map[importedY, importedX] * foodMetric;
                }
                else if ((world.warmMap.map[importedY, importedX] >= minTemp) && (world.warmMap.map[importedY, importedX] < (minTemp + maxTemp) / 2))// 2 ++
                {
                    result = ((temp - minTemp) - (world.warmMap.map[importedY, importedX] - minTemp) * 0.5) * tempMetric + world.foodMap.map[importedY, importedX] * foodMetric;
                }
                else if ((world.warmMap.map[importedY, importedX] >= (minTemp + maxTemp) / 2) && (world.warmMap.map[importedY, importedX] <= maxTemp))// 3 ++
                {
                    if (world.warmMap.map[importedY, importedX] <= temp)
                    {
                        result = ((maxTemp - world.warmMap.map[importedY, importedX]) * 0.5 + (temp - world.warmMap.map[importedY, importedX])) * tempMetric + world.foodMap.map[importedY, importedX] * foodMetric;
                    }
                    else
                    {
                        result = (maxTemp - world.warmMap.map[importedY, importedX]) * 0.5 * tempMetric + world.foodMap.map[importedY, importedX] * foodMetric;
                    }
                }
            }
            //result = world.foodMap.map[importedY, importedX] * foodMetric +
            //    (world.warmMap.maxVolume - Math.Abs(world.warmMap.map[importedY, importedX])) * tempMetric;
            //result = tempMetric * Math.Abs(world.warmMap.map[importedY, importedX]);
        }
        catch
        {
            result = -100;
        }
        return result;
    }
}
public class BeginnerAI : AI
{
    public BeginnerAI(int importedX, int importedY, int importedMinTemp, int importedMaxTemp, int importedMaxSatiety)
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
    public override void ChooseMove()
    {
        double[] metric = new double[5];
        metric[0] = CalculateMetric(coordinateX, coordinateY, world);
        metric[1] = CalculateMetric(coordinateX - 1, coordinateY, world);
        metric[2] = CalculateMetric(coordinateX, coordinateY + 1, world);
        metric[3] = CalculateMetric(coordinateX + 1, coordinateY, world);
        metric[4] = CalculateMetric(coordinateX, coordinateY - 1, world);
        int result = 0;
        for (int i = 1;i < 5; i++)
        {
            if (metric[result] <= metric[i])
            {
                result = i;
            }
        }
        if (result == 0)
        {
            Stay();
            CalculateParameters(false, world.foodMap.map[coordinateY,coordinateX], world.warmMap.map[coordinateY, coordinateX]);
            condition = condition.CalculateCondition(temp, maxTemp, minTemp, satiety, maxSatiety);
        }
        if (result == 1)
        {
            MoveLeft();
            CalculateParameters(true, world.foodMap.map[coordinateY, coordinateX], world.warmMap.map[coordinateY, coordinateX]);
            condition = condition.CalculateCondition(temp, maxTemp, minTemp, satiety, maxSatiety);
        }
        if (result == 2)
        {
            MoveUp();
            CalculateParameters(true, world.foodMap.map[coordinateY, coordinateX], world.warmMap.map[coordinateY, coordinateX]);
            condition = condition.CalculateCondition(temp, maxTemp, minTemp, satiety, maxSatiety);
        }
        if (result == 3)
        {
            MoveRight();
            CalculateParameters(true, world.foodMap.map[coordinateY, coordinateX], world.warmMap.map[coordinateY, coordinateX]);
            condition = condition.CalculateCondition(temp, maxTemp, minTemp, satiety, maxSatiety);
        }
        if (result == 4)
        {
            MoveDown();
            CalculateParameters(true, world.foodMap.map[coordinateY, coordinateX], world.warmMap.map[coordinateY, coordinateX]);
            condition = condition.CalculateCondition(temp, maxTemp, minTemp, satiety, maxSatiety);
        }
        if (world.foodMap.map[coordinateY, coordinateX] != 0)
        {
            world.foodMap.map[coordinateY, coordinateX]--;
        }
    }
    public double CalculateMetric(int importedX, int importedY, World world)
    {
        double result = 0; 
        int foodMetric = condition.foodMetric;
        int tempMetric = condition.tempMetric;
    }
}
public class MasterAI : AI
{
    public override void ChooseMove()
    {

    }
}
public interface IObserver<TypeDefinition>
{
    void Update(TypeDefinition data);
}
