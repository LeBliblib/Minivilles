using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinsFromOtherEvent : CardEvent
{
    int coinsNumber;

    bool allPlayers;
    bool currentPlayer;

    public CoinsFromOtherEvent(int _coinsNumber, bool _allPlayers, bool _currentPlayer)
    {
        coinsNumber = _coinsNumber;
        allPlayers = _allPlayers;
        currentPlayer = _currentPlayer;
    }

    public override void Activate()
    {
        throw new System.NotImplementedException();
    }
}
