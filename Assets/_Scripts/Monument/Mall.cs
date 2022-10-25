using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mall : Monument
{


    public Mall(string name, string desc, int cost, int playerID) : base(name, desc, cost, playerID) { }

    public override void Buy()
    {
        base.Buy();

        Game.instance.onCardBuy += CheckForActivation;
        Activate();
    }

    public void CheckForActivation(Card card)
    {
        if (card.PlayerID != PlayerID) return;

        UpdateCard(card);
    }

    public override void Activate()
    {
        Player player = Game.instance.GetPlayer(PlayerID);

        foreach(Card card in player.cards)
        {
            UpdateCard(card);
        }
    }

    void UpdateCard(Card card)
    {
        if (card.values.BuildingType != BuildingType.Food && card.values.BuildingType != BuildingType.Restaurant)
            return;

        switch (card.values.EventType)
        {
            case (CardEventType.CoinsFromBank):
                CoinsFromBankEvent ev = card.cardEvent as CoinsFromBankEvent;
                ev.coinsNumber++;
                Debug.Log("Event CFB : " + ev.coinsNumber);
                break;
            case (CardEventType.CoinsFromOther):
                CoinsFromOtherEvent ev2 = card.cardEvent as CoinsFromOtherEvent;
                ev2.coinsNumber++;
                Debug.Log("Event CFO : " + ev2.coinsNumber);
                break;
        }
    }

    public override void Destroy()
    {
        Game.instance.onCardBuy -= CheckForActivation;
    }
}
