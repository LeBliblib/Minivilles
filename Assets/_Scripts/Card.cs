using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public int PlayerID;

    public CardScriptableObject values;

    CardEvent cardEvent;

    public Card(int ID, CardScriptableObject _values)
    {
        PlayerID = ID;
        values = _values;

        switch (values.EventType)
        {
            case (CardEventType.CoinsFromBank):
                cardEvent = new CoinsFromBankEvent(_values.cfb_CoinsNumber, _values.ForEachBuilding, _values.BuildingType);
                break;
            case (CardEventType.CoinsFromOther):
                cardEvent = new CoinsFromOtherEvent(_values.cfo_CoinsNumber, _values.AllPlayers, _values.CurrentPlayer);
                break;
            case (CardEventType.Trade):
                cardEvent = new TradeEvent(_values.ForbiddenType);
                break;
        }
    }

    public void UseCard()
    {
        cardEvent.Activate(new Player("Test")); //Game.instance.GetPlayer(PlayerID)
    }
}
