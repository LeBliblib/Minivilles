using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinsFromBankEvent : CardEvent
{
    int coinsNumber;

    bool forEachBuilding;
    BuildingType buildingType;

    public CoinsFromBankEvent(int _coinsNumber, bool _forEachBuilding, BuildingType _buildingType)
    {
        coinsNumber = _coinsNumber;
        forEachBuilding = _forEachBuilding;
        buildingType = _buildingType;
    }

    public override void Activate(Player player)
    {
        if (!forEachBuilding)
        {
            player.ChangeCoins(coinsNumber);
            Debug.Log("Player : " + player.name + " a gagné " + coinsNumber);
        }
        else
        {
            foreach(Card card in player.cards)
            {
                if(card.values.BuildingType == buildingType)
                    player.ChangeCoins(coinsNumber);
            }
        }
    }
}
