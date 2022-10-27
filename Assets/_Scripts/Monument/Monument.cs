using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Monument
{
    public string Name;
    public string Desc;
    public int Cost;
    protected Player player;

    public bool isActive;

    public Monument(string name, string desc, int cost, Player player)
    {
        Name = name;
        Desc = desc;
        Cost = cost;
        this.player = player;

        isActive = false;
    }

    public virtual void Buy()
    {
        isActive = true;

        //Subscribe to event
    }
 
    public abstract void Activate();

    public abstract void Destroy();
}
