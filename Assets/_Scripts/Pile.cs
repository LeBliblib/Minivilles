using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pile
{
    private List<Card> cards;
    public Pile()
    {

    }

    public Card GetCard(int index)
    {
        Card card = cards[index];
        cards.RemoveAt(index);
        return card;
    }

    public void ShowAllCard()
    {

    }
}
