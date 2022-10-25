using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public string name;
    public int coins;
    public List<Card> cards = new List<Card>();
    public List<Monument> monuments = new List<Monument>();

    public int PlayerID;

    public Player(string name, int playerID)
    {
        this.name = name;
        coins = 3;

        PlayerID = playerID;
        Game.instance.ui.RefreshCoins(coins, PlayerID);

        //Ajouter cartes de base
        monuments.Add(new TrainStation("Gare", "Vous pouvez lancer deux d�s.", 4, playerID));
        monuments.Add(new Mall("Centre commercial", "Vos �tablissements de type Restaurant et Food rapportent une pi�ce de plus.", 10, playerID));
        monuments.Add(new Park("Parc d'attractions", "Si votre jet de d�s est un double, rejouez un tour apr�s celui-ci.", 16, playerID));
        monuments.Add(new Radio("Tour radio", "Une fois par tour, vous pouvez choisir de relancer vos d�s.", 22, playerID));


    }

    public void ChangeCoins(int value)
    {
        coins += value;
        Game.instance.ui.RefreshCoins(coins, PlayerID);
    }

    public void AddCard(Card cardToAdd)
    {
        cards.Add(cardToAdd);
    }

    public void CheckForCards(int diceRoll)
    {
        int cardID = 0;

        foreach(Card card in cards)
        {
            if (card.UseCard(diceRoll))
            {
                Game.instance.ui.ShowCardActivation(PlayerID, cardID);
            }

            cardID++;
        }
    }

    public bool HasCardColor(CardColor color)
    {
        foreach(Card card in cards)
        {
            if (card.values.Color == color) return true;
        }

        return false;
    }
}
