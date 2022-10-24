using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public string name;
    public int coins;
    public List<Card> cards= new List<Card>();

    public Player(string name)
    {
        this.name = name;
        this.coins = 3;
        //Ajouter cartes de base
    }

    public void ChangeCoin(int value)
    {
        coins += value;
    }

    public void AddCard(Card cardToAdd)
    {
        cards.Add(cardToAdd);
    }
}
