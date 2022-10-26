using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGameObject : MonoBehaviour
{
    List<CardGameObject> cards = new List<CardGameObject>();
    [SerializeField] List<CardGameObject> monuments = new List<CardGameObject>();

    public Transform playerTransform;
    
    public void AddCard(CardGameObject co)
    {
        cards.Add(co);
    }

    public void RemoveCard(int id)
    {
        Destroy(cards[id].gameObject);
        cards.RemoveAt(id);

    }

    public int GetCardNumber()
    {
        return cards.Count;
    }

    public void ActivateCard(int index)
    {
        cards[index].ShowActivation();
    }

    public CardGameObject GetMonument(int id)
    {
        return monuments[id];
    }
}
