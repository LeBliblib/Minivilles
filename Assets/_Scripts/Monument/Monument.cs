using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Monument
{
    public string Name;
    public string Desc;
    public int Cost;
    protected int PlayerID;

    public bool isActive;

    public Monument(string name, string desc, int cost, int playerID)
    {
        Name = name;
        Desc = desc;
        Cost = cost;
        PlayerID = playerID;

        isActive = false;
    }

    public virtual void Buy()
    {
        isActive = true;

        //Subscribe to event
    }
 
    public abstract void Activate();
}
