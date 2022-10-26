using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TradeEvent : CardEvent
{
    BuildingType forbiddenType;

    Player targetedPlayer;

    public TradeEvent(BuildingType _forbiddenType)
    {
        forbiddenType = _forbiddenType;
    }

    public override void Activate(Player player, Card card)
    {
        Game game = Game.instance;

        UIManager.PopUpCallback callback = ValidationCallback;

        game.ui.ShowChoosePopUp("Echange", "Voulez-vous �changer une carte avec un joueur ?", callback);
        game.StartCoroutine(WaitForValidation(player, card));
    }

    int validation = 0;
    IEnumerator WaitForValidation(Player player, Card card)
    {
        Game game = Game.instance;

        while (validation == 0)
        {
            yield return null;
        }

        if (validation == 2)
        {
            card.SetFinished();
            game.ui.HidePopUp();
            validation = 0;
        }
        else
        {
            UIManager.PopUpSelectCallback callback = TargettingCallback;

            game.ui.ShowSelectPopUp("Echange", player.PlayerID, callback);
            game.StartCoroutine(WaitForTarget(player, card));
            validation = 0;
        }
    }
    public void ValidationCallback(bool isValid)
    {
        validation = isValid ? 1 : 2;
    }

    int target = -1;
    IEnumerator WaitForTarget(Player player, Card card)
    {
        Game game = Game.instance;

        while (target == -1)
        {
            yield return null;
        }

        targetedPlayer = game.GetPlayer(target);

        target = -1;

        UIManager.PopUpTradeCallback callback = TradingCallback;

        game.ui.ShowTradePopUp("Echange", player.PlayerID, targetedPlayer.PlayerID, callback);
        game.StartCoroutine(WaitForTrade(player, card));
    }
    public void TargettingCallback(int id)
    {
        target = id;
    }

    int p1Choice = -1, p2Choice = -1;
    IEnumerator WaitForTrade(Player player, Card card)
    {
        Game game = Game.instance;

        while (p1Choice == -1 || p2Choice == -1)
        {
            yield return null;
        }

        Card p1Card = player.cards[p1Choice];
        Card p2Card = targetedPlayer.cards[p2Choice];

        game.ui.DeletePlayerCard(player.PlayerID, p1Choice);
        game.ui.DeletePlayerCard(targetedPlayer.PlayerID, p2Choice);
        player.cards.RemoveAt(p1Choice);
        targetedPlayer.cards.RemoveAt(p2Choice);

        p1Card.PlayerID = targetedPlayer.PlayerID;
        p2Card.PlayerID = player.PlayerID;

        player.cards.Add(p2Card);
        targetedPlayer.cards.Add(p1Card);
        game.ui.GiveCardToPlayer(player.PlayerID, p2Card.values);
        game.ui.GiveCardToPlayer(targetedPlayer.PlayerID, p1Card.values);

        p1Choice = -1;
        p2Choice = -1;

        game.ui.HidePopUp();
        card.SetFinished();
    }

    public void TradingCallback(int p1, int p2)
    {
        p1Choice = p1;
        p2Choice = p2;
    }
}
