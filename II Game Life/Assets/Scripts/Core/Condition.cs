using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Condition
{
    public string name;

    public int foodMetric;
    public int tempMetric;
    public Condition CalculateCondition(int temp, int maxTemp, int minTemp, int satiety, int maxSatiety)
    {
        Condition condition;
        if (satiety <= maxSatiety / 2)
        {
            if (temp <= maxTemp / 3)
            {
                condition = new Cold();
            }
            else if (temp > maxTemp - maxTemp / 3)
            {
                condition = new Hot();
            }
            else
            {
                condition = new Optimal();
            }
        }
        else
        {
            if (temp <= maxTemp / 3)
            {
                condition = new HungerAndCold();
            }
            else if (temp > maxTemp - maxTemp / 3)
            {
                condition = new HungerAndHot();
            }
            else
            {
                condition = new Hunger();
            }
        }
        return condition;
    }
}
public class Optimal: Condition
{
    public Optimal()
    {
        foodMetric = 1;
        tempMetric = 1;
    }
}
public class Cold: Condition
{
    public Cold()
    {
        foodMetric = 1;
        tempMetric = 1;

    }
}
public class Hot: Condition
{
    public Hot()
    {
        foodMetric = 1;
        tempMetric = 1;
    }
}
public class Hunger: Condition
{
    public Hunger()
    {
        foodMetric = 1;
        tempMetric = 1;
    }
}
public class HungerAndCold: Condition
{
    public HungerAndCold()
    {
        foodMetric = 1;
        tempMetric = 1;
    }
}
public class HungerAndHot: Condition
{
    public HungerAndHot()
    {
        foodMetric = 1;
        tempMetric = 1;
    }
}