using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public abstract class II
{
    public int coordinateX;
    public int coordinateY;

    public int temp; // �����������
    public int maxTemp; // ����������� ������ ��� ����
    public int minTemp; // ����������� ������ ��� ������

    public int satiety; // �������
    public int maxSatiety; // ������������ ���������� �������

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

    public List<Action> actions => new List<Action> { Stay, MoveLeft, MoveUp, MoveRight, MoveDown };
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

        //satiety = importedMaxSatiety / 2;
        satiety = importedMaxSatiety;
        maxSatiety = importedMaxSatiety;
        mindDeep = 1;

        condition = new Condition();
        condition = condition.CalculateCondition(temp, maxTemp, minTemp, satiety, maxSatiety);
    }

    public void RandomMove()
    {
        int i = UnityEngine.Random.Range(0, 5);
        actions[i]();
    }

    public override void ChooseMove(World world)
    {
        double[] metric = new double[5];

        metric[0] = CalculateMetric(coordinateX, coordinateY, world);
        metric[1] = CalculateMetric(coordinateX - 1, coordinateY, world);
        metric[2] = CalculateMetric(coordinateX, coordinateY + 1, world);
        metric[3] = CalculateMetric(coordinateX + 1, coordinateY, world);
        metric[4] = CalculateMetric(coordinateX, coordinateY - 1, world);

        int result = 0;

        for (int i = 1; i < 5; i++)
        {
            if (metric[result] <= metric[i])
            {
                result = i;
            }
        }

        actions[result]();
        
        CalculateParameters(
            result != 0,
            world.foodMap.map[coordinateY, coordinateX],
            world.warmMap.map[coordinateY, coordinateX]
        );

        condition = condition.CalculateCondition(temp, maxTemp, minTemp, satiety, maxSatiety);

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

        int localTemp = world.warmMap.map[importedY, importedX];
        int localFood = world.foodMap.map[importedY, importedX];
        
        try
        {
            if (temp <= (minTemp + maxTemp) / 2)
            {
                if (localTemp < minTemp) // 1 ++
                {
                    result = (minTemp - localTemp) * (-2) * tempMetric + localFood * foodMetric;
                }
                else if (localTemp > maxTemp) // 4 ++
                {
                    result = ((maxTemp - temp) - (localTemp - maxTemp) * 2) * tempMetric + localFood * foodMetric;
                }
                else if ((localTemp >= minTemp) && (localTemp < (minTemp + maxTemp) / 2)) // 2 ++
                {
                    if (localTemp >= temp)
                    {
                        result = ((localTemp - minTemp) * 0.5 + (localTemp - temp)) * tempMetric +
                                 localFood * foodMetric;
                    }
                    else
                    {
                        result = (localTemp - minTemp) * 0.5 * tempMetric + localFood * foodMetric;
                    }
                }
                else if ((localTemp >= (minTemp + maxTemp) / 2) && (localTemp <= maxTemp)) // 3 ++
                {
                    result = ((maxTemp - temp) - (maxTemp - localTemp) * 0.5) * tempMetric + localFood * foodMetric;
                }
            }
            else
            {
                if (localTemp < minTemp) // 1 ++
                {
                    result = ((temp - minTemp) - (minTemp - localTemp) * 2) * tempMetric + localFood * foodMetric;
                }
                else if (localTemp > maxTemp) // 4 ++
                {
                    result = (localTemp - maxTemp) * (-2) * tempMetric + localFood * foodMetric;
                }
                else if ((localTemp >= minTemp) && (localTemp < (minTemp + maxTemp) / 2)) // 2 ++
                {
                    result = ((temp - minTemp) - (localTemp - minTemp) * 0.5) * tempMetric + localFood * foodMetric;
                }
                else if ((localTemp >= (minTemp + maxTemp) / 2) && (localTemp <= maxTemp)) // 3 ++
                {
                    if (localTemp <= temp)
                    {
                        result = ((maxTemp - localTemp) * 0.5 + (temp - localTemp)) * tempMetric + localFood * foodMetric;
                    }
                    else
                    {
                        result = (maxTemp - localTemp) * 0.5 * tempMetric + localFood * foodMetric;
                    }
                }
            }
            //result = currentFood * foodMetric +
            //    (world.warmMap.maxVolume - Math.Abs(currentWarm)) * tempMetric;
            //result = tempMetric * Math.Abs(currentWarm);
        }
        catch
        {
            result = -100;
        }

        return result;
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