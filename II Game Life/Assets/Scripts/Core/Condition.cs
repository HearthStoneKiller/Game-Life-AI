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
        if (satiety >= maxSatiety / 2)
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
        if ((satiety <= 0) || (temp <= minTemp) || (temp >= maxTemp))
        {
            condition = new Dead();
        }
        return condition;
    }
}
public class Optimal: Condition
{
    public Optimal()
    {
        name = "Optimal";
        foodMetric = 3;
        tempMetric = 2;
    }
}
public class Cold: Condition
{
    public Cold()
    {
        name = "Cold";
        foodMetric = 1;
        tempMetric = 3;

    }
}
public class Hot: Condition
{
    public Hot()
    {
        name = "Hot";
        foodMetric = 1;
        tempMetric = 3;
    }
}
public class Hunger: Condition
{
    public Hunger()
    {
        name = "Hunger";
        foodMetric = 1;
        tempMetric = 3;
    }
}
public class HungerAndCold: Condition
{
    public HungerAndCold()
    {
        name = "HungerAndCold";
        foodMetric = 1;
        tempMetric = 3;
    }
}
public class HungerAndHot: Condition
{
    public HungerAndHot()
    {
        name = "HungerAndHot";
        foodMetric = 1;
        tempMetric = 3;
    }
}
public class Dead : Condition
{
    public Dead()
    {
        name = "Dead";
    }
}