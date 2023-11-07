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
        if (satiety > maxSatiety / 2)
        {
            if (temp <= minTemp + (maxTemp - minTemp) / 3)
            {
                condition = new Cold();
            }
            else if (temp > maxTemp - (maxTemp - minTemp) / 3)
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
            if (temp <= minTemp + (maxTemp - minTemp)/3)
            {
                condition = new HungerAndCold();
            }
            else if (temp > maxTemp - (maxTemp - minTemp) / 3)
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
    public Optimal(int foodMetric, int tempMetric)
    {
        this.name = "Optimal";
        this.foodMetric = foodMetric;
        this.tempMetric = tempMetric;
    }
    public Optimal()
    {
        name = "Optimal";
        foodMetric = 2;
        tempMetric = 1;
    }
}
public class Cold: Condition
{
    public Cold(int foodMetric, int tempMetric)
    {
        this.name = "Cold";
        this.foodMetric = foodMetric;
        this.tempMetric = tempMetric;
    }
    public Cold()
    {
        name = "Cold";
        foodMetric = 1;
        tempMetric = 2;

    }
}
public class Hot: Condition
{
    public Hot(int foodMetric, int tempMetric)
    {
        this.name = "Hot";
        this.foodMetric = foodMetric;
        this.tempMetric = tempMetric;
    }
    public Hot()
    {
        name = "Hot";
        foodMetric = 1;
        tempMetric = 2;
    }
}
public class Hunger: Condition
{
    public Hunger(int foodMetric, int tempMetric)
    {
        this.name = "Hunger";
        this.foodMetric = foodMetric;
        this.tempMetric = tempMetric;
    }
    public Hunger()
    {
        name = "Hunger";
        foodMetric = 2;
        tempMetric = 1;
    }
}
public class HungerAndCold: Condition
{
    public HungerAndCold(int foodMetric, int tempMetric)
    {
        this.name = "HungerAndCold";
        this.foodMetric = foodMetric;
        this.tempMetric = tempMetric;
    }
    public HungerAndCold()
    {
        name = "HungerAndCold";
        foodMetric = 1;
        tempMetric = 1;
    }
}
public class HungerAndHot: Condition
{
    public HungerAndHot(int foodMetric, int tempMetric)
    {
        this.name = "HungerAndHot";
        this.foodMetric = foodMetric;
        this.tempMetric = tempMetric;
    }
    public HungerAndHot()
    {
        name = "HungerAndHot";
        foodMetric = 1;
        tempMetric = 1;
    }
}
public class Dead : Condition
{
    public Dead()
    {
        name = "Dead";
    }
}