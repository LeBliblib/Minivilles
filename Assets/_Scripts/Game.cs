using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public UIManager ui;

    [SerializeField] List<CardScriptableObject> cardsSO = new List<CardScriptableObject>();
    [SerializeField] GameObject cardsPileGameObject;

    private List<Player> players = new List<Player>();
    public Pile gamePile = new Pile();

    public GameObject diceGO;

    int currentTurnPlayerID;
    bool canBuy;

    int dicesNumber;
    //Player
    public delegate void OnTurnStart(int turnPlayerID);
    public event OnTurnStart onTurnStart;

    public delegate void OnDiceRoll(int[] rolls, int playerID);
    public event OnDiceRoll onDiceRoll;
    Coroutine diceCoroutine;

    public delegate void OnCardBuy(Card card, int left);
    public event OnCardBuy onCardBuy;

    public delegate void OnTurnEnd(int turnPlayerID);
    public event OnTurnEnd onTurnEnd;
    //IA
    public delegate void OnBuyStart(int turnPlayerID);
    public event OnBuyStart onBuyStart;

    int turnStartCallbacks = 0;
    int diceRollCallbacks = 0;
    int cardBuyCallbacks = 0;
    int turnEndCallbacks = 0;

    public static Game instance;

    //Modifications Ydris
    [Header("Pile de cartes")]
    [SerializeField] GameObject cardsGrid;
    List<GameObject> cardsInPile;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        //modifications Ydris
        cardsInPile = new List<GameObject>();
        foreach (Transform child in cardsGrid.transform)
        {
            cardsInPile.Add(child.gameObject);
            child.gameObject.AddComponent<UnityEngine.UI.Button>();
        }
        //--------------------


        int index = 0;

        foreach (CardScriptableObject c in cardsSO)
        {
            gamePile.AddCard(c, 6);
            cardsInPile[index].GetComponent<Image>().sprite = c.texture;

            int i = index;

            cardsInPile[index].GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => { BuyCard(i); });

            index++;
        }

        Player p = new Player("jean " + 0, 0, false);
        players.Add(p);
        IA ia = new IA("jean" + 1, 1, true);
        players.Add(ia);

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

        int listeners = 0;

        if (onTurnStart?.GetInvocationList() != null) listeners = onTurnStart.GetInvocationList().Length;

        if (listeners <= 0)
            RollDice(dicesNumber);
        else
            StartCoroutine(WaitForTurnStartEvent(listeners));
    }

    IEnumerator WaitForTurnStartEvent(int callbacksNb)
    {
        while (turnStartCallbacks < callbacksNb)
        {
            yield return null;
        }

        turnStartCallbacks = 0;

        RollDice(dicesNumber);
    }

    public void ChangeDiceNumbers(int diceNb)
    {
        dicesNumber = diceNb;
    }

    public void SetCallback(CallbackTypes type)
    {
        switch (type)
        {
            case (CallbackTypes.TurnStart):
                turnStartCallbacks++;
                break;
            case (CallbackTypes.DiceRoll):
                diceRollCallbacks++;
                break;
            case (CallbackTypes.CardBuy):
                cardBuyCallbacks++;
                break;
            case (CallbackTypes.TurnEnd):
                turnEndCallbacks++;
                break;
        }
    }

    public void RollDice(int diceNumber)
    {
        int rollsSum = 0;

        int[] rolls = new int[diceNumber];

        for (int i = 0; i < diceNumber; i++)
        {
            //Renvoie un int avec la valeur du D roll
            rolls[i] = diceGO.GetComponent<Dice>().Roll();
            rollsSum += rolls[i];
        }

        Debug.Log("ergergh " + currentTurnPlayerID);
        ui.ShowDiceRoll(rollsSum);

        onDiceRoll?.Invoke(rolls, currentTurnPlayerID);

        int listeners = 0;

        if (onDiceRoll?.GetInvocationList() != null) listeners = onDiceRoll.GetInvocationList().Length;

        if (listeners <= 0)
            CheckForCardsActivation(rollsSum);
        else
            diceCoroutine = StartCoroutine(WaitForRollDiceEvent(listeners, rollsSum));
    }

    IEnumerator WaitForRollDiceEvent(int callbacksNb, int rollsSum)
    {
        while (diceRollCallbacks < callbacksNb)
        {
            yield return null;
        }

        diceRollCallbacks = 0;

        CheckForCardsActivation(rollsSum);
    }

    public void RerollDice()
    {
        if (diceCoroutine != null) StopCoroutine(diceCoroutine);
        diceRollCallbacks = 0;
        RollDice(dicesNumber);
    }


    int playerDoneActivation = 0;
    public void CheckForCardsActivation(int rollsSum)
    {
        foreach (Player p in players)
        {
            p.CheckForCards(rollsSum);
        }

        StartCoroutine(StartBuySequence());
    }
    public void SetPlayerDone()
    {
        Debug.Log("Player done");
        playerDoneActivation++;
    }

    public IEnumerator StartBuySequence()
    {
        while (playerDoneActivation < players.Count)
        {
            yield return null;
        }

        Debug.Log("Can buy : " + playerDoneActivation + " " + players.Count);
        playerDoneActivation = 0;
        canBuy = true;
        onBuyStart?.Invoke(currentTurnPlayerID);
    }

    public void BuyCard(int cardID)
    {
        if (!canBuy) return;

        Player player = players[currentTurnPlayerID];
        PileCards pile = gamePile.GetCard(cardID);

        if (pile.cardSO.Color == CardColor.Purple && player.HasCard(cardsSO[cardID])) return;
        if (pile.cardSO.Cost > player.coins || pile.nb <= 0) return;

        Card card = new Card(currentTurnPlayerID, pile.cardSO);

        card.PlayerID = currentTurnPlayerID;
        player.AddCard(card);
        gamePile.RemoveCard(cardID);

        player.ChangeCoins(-card.values.Cost);

        ui.GiveCardToPlayer(currentTurnPlayerID, pile.cardSO);

        canBuy = false;
        onCardBuy?.Invoke(card, pile.nb);

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

        ui.BuyMonument(currentTurnPlayerID, monumentID);

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

    public List<Player> GetAllPlayers()
    {
        return players;
    }

    public int GetCurrentPlayerID()
    {
        return currentTurnPlayerID;
    }

    public void SetCurrentPlayerID(int value)
    {
        currentTurnPlayerID = value;
    }

    public List<CardScriptableObject> GetSOList()
    {
        return cardsSO;
    }

    public int GetIndexSO(CardScriptableObject card)
    {
        return cardsSO.IndexOf(card);
    }
    #endregion
}

public enum CallbackTypes
{
    TurnStart,
    DiceRoll,
    CardBuy,
    TurnEnd
}