using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinsFromBankEvent : CardEvent
{
    public int coinsNumber;

    bool forEachBuilding;
    BuildingType buildingType;

    public CoinsFromBankEvent(int _coinsNumber, bool _forEachBuilding, BuildingType _buildingType)
    {
        coinsNumber = _coinsNumber;
        forEachBuilding = _forEachBuilding;
        buildingType = _buildingType;
    }

    public override void Activate(Player player, Card card)
    {
        if (!forEachBuilding)
        {
            player.ChangeCoins(coinsNumber);
            Debug.Log("Player : " + player.name + " a gagné " + coinsNumber);
        }
        else
        {
            foreach(Card c in player.cards)
            {
                if(c.values.BuildingType == buildingType)
                    player.ChangeCoins(coinsNumber);
            }
        }

        card.SetFinished();
    }
}
