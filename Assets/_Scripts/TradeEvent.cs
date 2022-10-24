using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TradeEvent : CardEvent
{
    BuildingType forbiddenType;

    public TradeEvent(BuildingType _forbiddenType)
    {
        forbiddenType = _forbiddenType;
    }

    public override void Activate(Player player)
    {
        throw new System.NotImplementedException();
    }
}
