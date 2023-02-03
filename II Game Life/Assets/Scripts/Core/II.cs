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
        try
        {
            if (temp <= (minTemp + maxTemp) / 2)
            {
                if (world.warmMap.map[importedY, importedX] < minTemp) // 1 ++
                {
                    result = (minTemp - world.warmMap.map[importedY, importedX]) * (-2) * tempMetric +
                             world.foodMap.map[importedY, importedX] * foodMetric;
                }
                else if (world.warmMap.map[importedY, importedX] > maxTemp) // 4 ++
                {
                    result = ((maxTemp - temp) - (world.warmMap.map[importedY, importedX] - maxTemp) * 2) * tempMetric +
                             world.foodMap.map[importedY, importedX] * foodMetric;
                }
                else if ((world.warmMap.map[importedY, importedX] >= minTemp) &&
                         (world.warmMap.map[importedY, importedX] < (minTemp + maxTemp) / 2)) // 2 ++
                {
                    if (world.warmMap.map[importedY, importedX] >= temp)
                    {
                        result = ((world.warmMap.map[importedY, importedX] - minTemp) * 0.5 +
                                  (world.warmMap.map[importedY, importedX] - temp)) * tempMetric +
                                 world.foodMap.map[importedY, importedX] * foodMetric;
                    }
                    else
                    {
                        result = (world.warmMap.map[importedY, importedX] - minTemp) * 0.5 * tempMetric +
                                 world.foodMap.map[importedY, importedX] * foodMetric;
                    }
                }
                else if ((world.warmMap.map[importedY, importedX] >= (minTemp + maxTemp) / 2) &&
                         (world.warmMap.map[importedY, importedX] <= maxTemp)) // 3 ++
                {
                    result =
                        ((maxTemp - temp) - (maxTemp - world.warmMap.map[importedY, importedX]) * 0.5) * tempMetric +
                        world.foodMap.map[importedY, importedX] * foodMetric;
                }
            }
            else
            {
                if (world.warmMap.map[importedY, importedX] < minTemp) // 1 ++
                {
                    result = ((temp - minTemp) - (minTemp - world.warmMap.map[importedY, importedX]) * 2) * tempMetric +
                             world.foodMap.map[importedY, importedX] * foodMetric;
                }
                else if (world.warmMap.map[importedY, importedX] > maxTemp) // 4 ++
                {
                    result = (world.warmMap.map[importedY, importedX] - maxTemp) * (-2) * tempMetric +
                             world.foodMap.map[importedY, importedX] * foodMetric;
                }
                else if ((world.warmMap.map[importedY, importedX] >= minTemp) &&
                         (world.warmMap.map[importedY, importedX] < (minTemp + maxTemp) / 2)) // 2 ++
                {
                    result =
                        ((temp - minTemp) - (world.warmMap.map[importedY, importedX] - minTemp) * 0.5) * tempMetric +
                        world.foodMap.map[importedY, importedX] * foodMetric;
                }
                else if ((world.warmMap.map[importedY, importedX] >= (minTemp + maxTemp) / 2) &&
                         (world.warmMap.map[importedY, importedX] <= maxTemp)) // 3 ++
                {
                    if (world.warmMap.map[importedY, importedX] <= temp)
                    {
                        result = ((maxTemp - world.warmMap.map[importedY, importedX]) * 0.5 +
                                  (temp - world.warmMap.map[importedY, importedX])) * tempMetric +
                                 world.foodMap.map[importedY, importedX] * foodMetric;
                    }
                    else
                    {
                        result = (maxTemp - world.warmMap.map[importedY, importedX]) * 0.5 * tempMetric +
                                 world.foodMap.map[importedY, importedX] * foodMetric;
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