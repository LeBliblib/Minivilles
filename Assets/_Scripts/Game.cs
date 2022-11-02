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

    public List<Dice> diceObjects = new();

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

        Player p = new Player("jean " + 0, 0, false);
        players.Add(p);
        IA ia = new IA("jean" + 1, 1, true);
        players.Add(ia);
    }

    IEnumerator Start()
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

        currentTurnPlayerID = 0;

        yield return new WaitForSeconds(0.8f);
        foreach (Player P in players)
        {
            P.AddCard(new Card(P.PlayerID, cardsSO[0]));
            ui.GiveCardToPlayer(P.PlayerID, cardsSO[0]);
            P.AddCard(new Card(P.PlayerID, cardsSO[2]));
            ui.GiveCardToPlayer(P.PlayerID, cardsSO[2]);
        }
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
        if (onTurnStart?.GetInvocationList() != null)
        {
            listeners = onTurnStart.GetInvocationList().Length;
        }

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

        yield return new WaitForSeconds(2f); //Dice roll anim time

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

        int maxIndex = -1;
        for (int i = 0; i < diceNumber; i++)
        {
            maxIndex++;

            if(i < diceObjects.Count)
                rolls[i] = diceObjects[i].Roll();
            else
                rolls[i] = diceObjects[0].Roll();

            rollsSum += rolls[i];
        }

        if(maxIndex < diceObjects.Count - 1)
        {
            for(int i = maxIndex; i < diceObjects.Count; i++)
            {
                diceObjects[i].HideDice();
            }
        }

        Debug.Log("ergergh " + currentTurnPlayerID);
        ui.ShowDiceRoll(rollsSum);

        onDiceRoll?.Invoke(rolls, currentTurnPlayerID);
        Debug.Log("Dice roll");
        int listeners = 0;

        if (onDiceRoll?.GetInvocationList() != null) listeners = onDiceRoll.GetInvocationList().Length;

        if (listeners <= 0)
            CheckForCardsActivation(rollsSum);
        else
            diceCoroutine = StartCoroutine(WaitForRollDiceEvent(listeners, rollsSum));
    }

    IEnumerator WaitForRollDiceEvent(int callbacksNb, int rollsSum)
    {
        Debug.Log("Start wait dice roll" + callbacksNb +"||"+ diceRollCallbacks);
        while (diceRollCallbacks < callbacksNb)
        {
            yield return null;
        }
        Debug.Log("Stop wait dice roll");
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

        //Buy start anim
        yield return new WaitForSeconds(2f); //Buy start anim time

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

        Debug.Log("current " + currentTurnPlayerID);
        ui.GiveCardToPlayer(currentTurnPlayerID, pile.cardSO);

        //pile.nb--;
        canBuy = false;
        onCardBuy?.Invoke(card, pile.nb);

        Debug.Log("Achat : " + card.values.Name + " Money money : " + player.coins + " Player : " + currentTurnPlayerID);
        EndTurn();
    }

    public void BuyMonument(int monumentID)
    {
        if (!canBuy) return;
        Debug.Log("currentplayer" + currentTurnPlayerID);
        Player player = players[currentTurnPlayerID];
        Debug.Log("currentmonument" + monumentID);
        Monument monument = player.monuments[monumentID];

        if (monument.isActive) return;
        if (monument.Cost > player.coins) return;

        player.ChangeCoins(-monument.Cost);
        monument.Buy();

        Debug.Log("monplayerIP"+monument.player.PlayerID);
        ui.BuyMonument(currentTurnPlayerID, monumentID);

        canBuy = false;

        int mCount = 0;

        player.monuments.ForEach(x => {
            if (x.isActive) mCount++;
        });

        if(mCount == player.monuments.Count)
        {
            if(player.isIA)
                ui.LaunchLoosePanel();
            else
                ui.LaunchWinPanel();
        }
        else
            EndTurn();
    }

    public void DontBuy()
    {
        if (!canBuy) return;

        canBuy = false;

        EndTurn();
    }

    public void EndTurn()
    {
        onTurnEnd?.Invoke(currentTurnPlayerID);

        currentTurnPlayerID++;
        if (currentTurnPlayerID >= players.Count) currentTurnPlayerID = 0;

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
    public CardScriptableObject GetSO(int index)
    {
        return cardsSO[index];
    }

    public int GetIndexSO(CardScriptableObject card)
    {
        return cardsSO.IndexOf(card);
    }    
    public int GetCardCost(int index)
    {
        return cardsSO[index].Cost;
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