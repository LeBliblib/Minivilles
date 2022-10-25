using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CoinsFromOtherEvent : CardEvent
{
    public int coinsNumber;

    bool allPlayers;
    bool currentPlayer;

    public CoinsFromOtherEvent(int _coinsNumber, bool _allPlayers, bool _currentPlayer)
    {
        coinsNumber = _coinsNumber;
        allPlayers = _allPlayers;
        currentPlayer = _currentPlayer;
    }

    public override void Activate(Player player)
    {
        Game game = Game.instance;

        if (currentPlayer)
        {
            Player p2 = game.GetPlayer(game.GetCurrentPlayerID());

            if(p2.coins >= coinsNumber)
            {
                p2.ChangeCoins(-coinsNumber);
                player.ChangeCoins(coinsNumber);
            }
            else
            {
                p2.ChangeCoins(-p2.coins);
                player.ChangeCoins(p2.coins);
            }
        }
        else if (allPlayers)
        {
            List<Player> players = game.GetAllPlayers();

            foreach(Player p in players)
            {
                if(p != player)
                {
                    if(p.coins >= coinsNumber)
                    {
                        p.ChangeCoins(-coinsNumber);
                        player.ChangeCoins(coinsNumber);
                    }
                    else
                    {
                        p.ChangeCoins(-p.coins);
                        player.ChangeCoins(p.coins);
                    }
                }
            }
        }
        else
        {
            UIManager.PopUpSelectCallback callback = TargettingCallback;

            game.ui.ShowSelectPopUp(callback);
            game.StartCoroutine(WaitForTarget(player));
        }
    }

    int target = -1;
    IEnumerator WaitForTarget(Player player)
    {
        Game game = Game.instance;

        while(target == -1)
        {
            yield return null;
        }

        Player p2 = game.GetPlayer(target);

        if (p2.coins >= coinsNumber)
        {
            p2.ChangeCoins(-coinsNumber);
            player.ChangeCoins(coinsNumber);
        }
        else
        {
            p2.ChangeCoins(-p2.coins);
            player.ChangeCoins(p2.coins);
        }

        target = -1;
    }

    public void TargettingCallback(int id)
    {
        target = id;
    }
}
