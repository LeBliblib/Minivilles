using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Park : Monument
{
    public Park(string name, string desc, int cost, int playerID) : base(name, desc, cost, playerID) { }

    public override void Activate()
    {
        throw new System.NotImplementedException();
    }
}
