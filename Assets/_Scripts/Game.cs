using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public UIManager ui;

    [SerializeField] List<CardScriptableObject> cardsSO = new List<CardScriptableObject>();

    private List<Player> players = new List<Player>();
    private Pile gamePile = new Pile();
    private Dice gameDice = new Dice();

    int currentTurnPlayerID;
    bool canBuy;

    int dicesNumber;

    public delegate void OnTurnStart(int turnPlayerID);
    public event OnTurnStart onTurnStart;

    public delegate void OnDiceRoll(int rollsSum);
    public event OnDiceRoll onDiceRoll;

    public delegate void OnCardBuy(Card card);
    public event OnCardBuy onCardBuy;

    public delegate void OnTurnEnd(int turnPlayerID);
    public event OnTurnEnd onTurnEnd;

    public static Game instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        int index = 0;
        foreach (CardScriptableObject c in cardsSO) 
        {
            gamePile.AddCard(c, 6);

            index++;
        }

        for(int i = 0; i < 2; i++)
        {
            Player p = new Player("jean " + i, i);
            players.Add(p);
        }

        currentTurnPlayerID = 0;

        StartTurn();
    }

    // Update is called once per frame
    void Update()
    {

    }

    #region Methode
    public void StartTurn()
    {
        dicesNumber = 1;
        onTurnStart?.Invoke(currentTurnPlayerID);

        RollDice(dicesNumber);
    }

    public void RollDice(int diceNumber)
    {
        int rollsSum = 0;

        for(int i = 0; i < diceNumber; i++)
        {
            //Renvoie un int avec la valeur du D roll
            rollsSum += gameDice.Roll();
        }

        Debug.Log("ergergh " + currentTurnPlayerID);
        ui.ShowDiceRoll(rollsSum);

        onDiceRoll?.Invoke(rollsSum);

        CheckForCardsActivation(rollsSum);
    }

    public void CheckForCardsActivation(int rollsSum)
    {
        foreach(Player p in players)
        {
            p.CheckForCards(rollsSum);
        }

        StartBuySequence();
    }

    public void StartBuySequence()
    {
        canBuy = true;
    }

    public void BuyCard(int cardID)
    {
        if (!canBuy) return;

        Player player = players[currentTurnPlayerID];
        PileCards pile = gamePile.GetCard(cardID);

        if (pile.cardSO.Color == CardColor.Purple && player.HasCardColor(CardColor.Purple)) return;
        if (pile.cardSO.Cost > player.coins || pile.nb <= 0) return;

        Card card = new Card(currentTurnPlayerID, pile.cardSO);

        card.PlayerID = currentTurnPlayerID;
        player.AddCard(card);
        gamePile.RemoveCard(cardID);

        player.ChangeCoins(-card.values.Cost);

        //ui.GiveCardToPlayer(currentPlayerID);

        canBuy = false;
        onCardBuy?.Invoke(card);

        Debug.Log("Achat : " + card.values.Name + " Money money : " + player.coins + " Player : " + currentTurnPlayerID);
        EndTurn();
    }

    public void BuyMonument(int monumentID)
    {
        if (!canBuy) return;

        Player player = players[currentTurnPlayerID];
        Monument monument = player.monuments[monumentID];

        if (monument.isActive) return;
        if (monument.Cost > player.coins) return;

        player.ChangeCoins(-monument.Cost);
        monument.Buy();

        //ui.BuyMonument(currentPlayerID, moinumentID);

        canBuy = false;

        EndTurn();
    }

    public void DontBuy()
    {
        canBuy = false;

        EndTurn();
    }

    public void EndTurn()
    {
        currentTurnPlayerID++;
        if (currentTurnPlayerID >= players.Count) currentTurnPlayerID = 0;

        onTurnEnd?.Invoke(currentTurnPlayerID);



        StartTurn();
    }

    public Player GetPlayer(int index)
    {
        return players[index];
    }

    public int GetCurrentPlayerID()
    {
        return currentTurnPlayerID;
    }
    #endregion
}
