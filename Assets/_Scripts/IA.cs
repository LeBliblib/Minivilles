using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class IA : Player
{
    public Strategie strat;
    public IA(string name, int playerID, bool isIA) : base(name, playerID, isIA)
    {
        strat = CreateStrategie();
        Game.instance.onCardBuy += LowerStartegie;
        Game.instance.onBuyStart += ChoseCard;
    }
    public void ChoseCard()
    {
        //Choisit la carte en fct de la priorit� de la strategie definit
        List<int> indexes = new List<int>();
        int priority = -1;
        int rand = Random.Range(0, 20);
        if(rand < 15)
            priority = 1;
        else if(rand < 19 && rand >= 15)
            priority = 2;
        else if(rand == 19)
            priority = 3;
        for (int i = 0; i < strat.priority.GetLength(1); i++)
            if (strat.priority[0, i] == priority)
                indexes.Add(i);
        Game.instance.BuyCard(indexes[Random.Range(0,indexes.Count)]);
    }

    public void LowerStartegie(Card card,int left)
    {
        int index = -1;
        if(left == 0)
            index = Game.instance.GetIndexSO(card.values);
        if (strat.priority[0,index] <= 3)
        {
            int priority = strat.priority[0, index];
            for (int i = 0; i < strat.priority.Length; i++)
            {
                if (strat.priority[0, i] >= priority)
                    strat.priority[0, i]--;
            }
        }
    }
    public int TradeIAChoice()
    {
        return 0;
    }
    public int CoinsFromOtherIAChoice()
    {
        return -1;
    }
    public void CheckRadioTower()
    {

    }
    public void CheckMonumentToBuy()
    {

    }
    public void Destroy()
    {

    }
    public Strategie CreateStrategie()
    {
        //Choisit une strat�gie au hasard en fct de celle pre-d�finit
        //Tableau   (0 � 11 les 12 cartes de base
        //          (12 � 14 les 3 cartes violette)
        //          (15 � 18 les 4 monuments
        int rand = Random.Range(0, 3);
        int[,] strategie;
        switch (rand)
        {
            case 0:
                //Strat Ferme + Fromagerie
                strategie = new int[2, 19] { { 8, 1, 4, 3, 3, 2, 2, 3, 5, 5, 6, 7, 0, 0, 0, -1, -1, -1, -1 }, { 8, 5, 8, 7, 7, 4, 1, 2, 3, 6, 7, 8, 10, 10, 10, -1, -1, -1, -1 } };
                break;
            case 1:
                strategie = new int[2, 19] { { 8, 1, 4, 3, 3, 2, 2, 3, 5, 5, 6, 7, 0, 0, 0, -1, -1, -1, -1 }, { 8, 5, 8, 7, 7, 4, 1, 2, 3, 6, 7, 8, 10, 10, 10, -1, -1, -1, -1 } };
                //Strat meuble
                break;
            case 2:
                strategie = new int[2, 19] { { 8, 1, 4, 3, 3, 2, 2, 3, 5, 5, 6, 7, 0, 0, 0, -1, -1, -1, -1 }, { 8, 5, 8, 7, 7, 4, 1, 2, 3, 6, 7, 8, 10, 10, 10, -1, -1, -1, -1 } };
                //Strat 1D6
                break;
            default:
                strategie = new int[2, 19] { { 8, 1, 4, 3, 3, 2, 2, 3, 5, 5, 6, 7, 0, 0, 0, -1, -1, -1, -1 }, { 8, 5, 8, 7, 7, 4, 1, 2, 3, 6, 7, 8, 10, 10, 10, -1, -1, -1, -1 } };
                break;
        }
        return new Strategie(strategie);
    }
    public struct Strategie
    {
        public int[,] priority;
        public Strategie(int[,] priority)
        {
            this.priority = priority;
        }
    }
}