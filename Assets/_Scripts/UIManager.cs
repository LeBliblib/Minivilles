using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject dice;
    [SerializeField] TextMeshProUGUI diceValueText;

    [SerializeField] List<TextMeshProUGUI> coinsText;

    [SerializeField] CardGameObject cardPrefab;
    [SerializeField] List<PlayerGameObject> players;

    [SerializeField] List<Sprite> monumentSprites;

    [SerializeField] Transform gameCanvas;

    [Header("Cards Doisplay")]
    [SerializeField] List<Transform> player1Rows;
    [SerializeField] List<Transform> player2Rows;

    [Header("PopUp")]
    [SerializeField] GameObject popUp;
    [SerializeField] float popUpAnimTime;

    [SerializeField] GameObject choiceContent, selectContent;

    GameObject currentPopUp;

    [Header("PopUp Choice")]
    [SerializeField] TextMeshProUGUI popUpInside;
    [SerializeField] TextMeshProUGUI popUpTitle;

    [Header("PopUp PlayerSelect")]
    [SerializeField] UIElement playerSelectPrefab;
    [SerializeField] Transform playerSelectGrid;
    List<UIElement> playerSelects = new();

    [Header("PopUp Trade")]
    [SerializeField] GameObject tradePopUp;
    [SerializeField] List<Image> p1CardsImg, p2CardsImg;
    [SerializeField] UIElement p1Trade, p2Trade;
    [SerializeField] GameObject tradeCardSelectP1, tradeCardSelectP2;
    [SerializeField] TextMeshProUGUI popUpTradeTitle;

    [Header("Starting, Ending and PopUp Win&Loose")]
    [SerializeField] GameObject winPopup;
    [SerializeField] GameObject fireworksManager;
    [SerializeField] GameObject losePopup;
    [SerializeField] GameObject retryButton;
    [SerializeField] Image[] curtains;
    bool areCurtainsOpened;
    float timer;
    sbyte step;

    int p1Choice = -1;
    int p2Choice = -1;

    public delegate void PopUpCallback(bool valid);
    public PopUpCallback currentPopupCallback;

    public delegate void PopUpSelectCallback(int selectedID);
    public PopUpSelectCallback currentSelectPopUpCallback;

    public delegate void PopUpTradeCallback(int p1CardID, int p2CardID);
    public PopUpTradeCallback currentTradePopUpCallback;

    private void Awake()
    {
        dice.SetActive(false);
        tradeCardSelectP1.SetActive(false);
        tradeCardSelectP2.SetActive(false);

        popUp.transform.localScale = Vector3.zero;
        tradePopUp.transform.localScale = Vector3.zero;

        winPopup.transform.GetChild(1).localScale = new Vector2(0, 0);
        winPopup.transform.GetChild(0).gameObject.SetActive(false);
        losePopup.transform.GetChild(1).localScale = new Vector2(0, 0);
        losePopup.transform.GetChild(0).gameObject.SetActive(false);
        retryButton.transform.localScale = new Vector2(0, 0);

        curtains[0].transform.position = new Vector2(Screen.width / 2, Screen.height);
        curtains[0].rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height / 2);
        curtains[1].transform.position = new Vector2(Screen.width / 2, 0);
        curtains[1].rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height / 2);
        areCurtainsOpened = false;
        timer = 0;
        step = 0;
    }

    private void Start()
    {
        List<Player> players = Game.instance.GetAllPlayers();

        foreach(Player player in players)
        {
            UIElement elem = Instantiate(playerSelectPrefab, playerSelectGrid);
            elem.SetText(0, player.name);
            elem.SetText(1, "" + player.coins);

            elem.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => 
            {
                HidePopUp();
                PlayerSelectDone(player.PlayerID);
            });

            playerSelects.Add(elem);
        }
    }

    private void Update()
    {
        if (!areCurtainsOpened && step ==0) { timer += Time.deltaTime; }
        if(!areCurtainsOpened && timer >= 0.5f)
        {
            CurtainsAction();
            timer = 0;
            step++;
        }
    }

    public void ShowDiceRoll(int rollValue)
    {
        StartCoroutine(ShowDice(rollValue));
    }

    IEnumerator ShowDice(int rollValue)
    {
        dice.SetActive(true);
        diceValueText.text = "" + rollValue;

        yield return new WaitForSeconds(2f);

        dice.SetActive(false);
    }

    public void RefreshCoins(int value, int id)
    {

        coinsText[id].text = "" + value;
    }

    public void GiveCardToPlayer(int playerID, CardScriptableObject cardSO)
    {
        Debug.Log("current " + playerID);

        CardGameObject card = Instantiate(cardPrefab, gameCanvas);

        int rowsIndex = -1;
        int index = 0;

        foreach(CardGameObject c in players[playerID].cards)
        {
            if(c.cardSo == cardSO)
            {
                rowsIndex = index;
            }

            index++;
        }

        int i = players[playerID].GetCardNumber();

        Transform parent = null;

        if (rowsIndex != -1)
            parent = players[playerID].cards[rowsIndex].transform.parent;
        else
            parent = playerID == 0 ? player1Rows[i] : player2Rows[i];

        card.Init(cardSO, parent);

        players[playerID].AddCard(card);
    }

    public void RefreshCards(int playerID)
    {
        players[playerID].cards.ForEach(x => x.transform.parent = null);

        List<CardGameObject> cards = new(players[playerID].cards);
        players[playerID].cards.Clear();

        foreach (CardGameObject card in cards)
        {
            int rowsIndex = -1;
            int index = 0;

            foreach (CardGameObject c in players[playerID].cards)
            {
                if (c.cardSo == card.cardSo)
                {
                    rowsIndex = index;
                }

                index++;
            }

            int i = players[playerID].GetCardNumber();

            Transform parent = null;

            if (rowsIndex != -1)
                parent = players[playerID].cards[rowsIndex].transform.parent;
            else
                parent = playerID == 0 ? player1Rows[i] : player2Rows[i];

            card.transform.parent = parent;

            players[playerID].AddCard(card);
        }
    }

    public void DeletePlayerCard(int playerID, int cardID)
    {
        players[playerID].RemoveCard(cardID);
        RefreshCards(playerID);
    }

    public void ShowCardActivation(int playerID, int cardID)
    {
        players[playerID].ActivateCard(cardID);
    }

    public void BuyMonument(int playerID, int monumentID)
    {
        CardGameObject monumentObject = players[playerID].GetMonument(monumentID);

        monumentObject.ChangeSprite(monumentSprites[monumentID]);
    }

    public void ShowChoosePopUp(string title, string desc, PopUpCallback callback)
    {
        choiceContent.SetActive(true);
        selectContent.SetActive(false);

        currentPopupCallback = callback;

        popUpTitle.text = title;
        popUpInside.text = desc;

        ShowPopUp(popUp);
    }

    public void ShowSelectPopUp(string title, int playerID, PopUpSelectCallback callback)
    {
        List<Player> players = Game.instance.GetAllPlayers();

        for(int i = 0; i < players.Count; i++)
        {
            if (i == playerID)
            {
                playerSelects[i].gameObject.SetActive(false);
                continue;
            }

            playerSelects[i].gameObject.SetActive(true);
            playerSelects[i].SetText(1, "" + players[i].coins);
        }
        popUpTitle.text = title;
        choiceContent.SetActive(false);
        selectContent.SetActive(true);

        currentSelectPopUpCallback = callback;

        ShowPopUp(popUp);
    }
    
    public void ShowTradePopUp(string title, int player1ID, int player2ID, PopUpTradeCallback callback)
    {
        Player player1 = Game.instance.GetPlayer(player1ID);
        Player player2 = Game.instance.GetPlayer(player2ID);

        List<CardScriptableObject> knowValues = new();
        int cardImgIndex = 0;

        int cardID = 0;
        foreach(Card card in player1.cards)
        {
            if (knowValues.Contains(card.values) || card.values.BuildingType == BuildingType.Tower) 
            {
                cardID++;
                continue;
            }
            
            p1CardsImg[cardImgIndex].gameObject.SetActive(true);
            p1CardsImg[cardImgIndex].sprite = card.values.texture;
            knowValues.Add(card.values);

            UnityEngine.UI.Button btn = p1CardsImg[cardImgIndex].GetComponent<UnityEngine.UI.Button>();

            int i = cardID;
            btn.onClick.AddListener(() => TradeSelectCard(i, 0));

            cardImgIndex++;
            cardID++;
        }
        if(cardImgIndex < p1CardsImg.Count)
        {
            for(int i = cardImgIndex; i < p1CardsImg.Count; i++)
            {
                p1CardsImg[i].gameObject.SetActive(false);
            }
        }

        knowValues.Clear();
        cardImgIndex = 0;
        cardID = 0;

        foreach (Card card in player2.cards)
        {
            if (knowValues.Contains(card.values) || card.values.BuildingType == BuildingType.Tower)
            {
                cardID++;
                continue;
            }

            p2CardsImg[cardImgIndex].gameObject.SetActive(true);
            p2CardsImg[cardImgIndex].sprite = card.values.texture;
            knowValues.Add(card.values);

            UnityEngine.UI.Button btn = p2CardsImg[cardImgIndex].GetComponent<UnityEngine.UI.Button>();

            int i = cardID;
            btn.onClick.AddListener(() => TradeSelectCard(i, 1));

            cardImgIndex++;
            cardID++;
        }
        if (cardImgIndex < p2CardsImg.Count)
        {
            for (int i = cardImgIndex; i < p2CardsImg.Count; i++)
            {
                p2CardsImg[i].gameObject.SetActive(false);
            }
        }

        p1Trade.SetText(0, player1.name);
        p1Trade.SetText(1, "" + player1.coins);

        p2Trade.SetText(0, player2.name);
        p2Trade.SetText(1, "" + player2.coins);

        tradeCardSelectP1.SetActive(false);
        tradeCardSelectP2.SetActive(false);
        popUpTradeTitle.text = title;

        currentTradePopUpCallback = callback;

        ShowPopUp(tradePopUp);
    }

    public void ShowPopUp(GameObject _popUp)
    {
        currentPopUp = _popUp;

        LeanTween.cancel(_popUp);

        _popUp.transform.localScale = Vector3.zero;

        LeanTween.scale(_popUp, Vector3.one, popUpAnimTime).setEaseInOutExpo().setIgnoreTimeScale(true);
    }

    public void HidePopUp()
    {
        LeanTween.cancel(currentPopUp);
        LeanTween.scale(currentPopUp, Vector3.zero, popUpAnimTime).setEaseInOutExpo().setIgnoreTimeScale(true);

        p1CardsImg.ForEach(x => x.GetComponent<UnityEngine.UI.Button>().onClick.RemoveAllListeners());
        p2CardsImg.ForEach(x => x.GetComponent<UnityEngine.UI.Button>().onClick.RemoveAllListeners());
    }

    public void ChooseSelectPopUp(bool valid)
    {
        currentPopupCallback?.Invoke(valid);
    }

    public void PlayerSelectDone(int id)
    {
        currentSelectPopUpCallback?.Invoke(id);
    }

    public void TradeSelectCard(int id, int playerID)
    {
        if (playerID == 0)
        {
            tradeCardSelectP1.SetActive(true);
            tradeCardSelectP1.transform.position = p1CardsImg[id].transform.position;
            p1Choice = id;
        }
        else
        {
            tradeCardSelectP2.SetActive(true);
            tradeCardSelectP2.transform.position = p2CardsImg[id].transform.position;
            p2Choice = id;
        }

        if(p1Choice != -1 && p2Choice != -1)
        {
            currentTradePopUpCallback?.Invoke(p1Choice, p2Choice);

            p1Choice = -1;
            p2Choice = -1;
        }
    }

    public void LaunchWinPanel()
    {
        fireworksManager.SetActive(true);
        winPopup.transform.GetChild(0).gameObject.SetActive(true);
        LeanTween.scale(winPopup.transform.GetChild(1).gameObject, new Vector2(5, 5), 1.5f).setEase(LeanTweenType.easeOutElastic);
        StartCoroutine(RetryButtonPop());
    }

    public void LaunchLoosePanel()
    {
        losePopup.transform.GetChild(0).gameObject.SetActive(true);
        LeanTween.scale(losePopup.transform.GetChild(1).gameObject, new Vector2(5,5), 1.5f).setEase(LeanTweenType.easeOutElastic);
        StartCoroutine(RetryButtonPop());
    }

    IEnumerator RetryButtonPop()
    {
        yield return new WaitForSeconds(1.0f);
        LeanTween.scale(retryButton, new Vector2(1, 1), 0.5f).setEase(LeanTweenType.easeOutElastic);
    }

    public void CurtainsAction()
    {
        if (!areCurtainsOpened)
        {
            LeanTween.scaleY(curtains[0].gameObject, 0, 0.3f).setEase(LeanTweenType.easeOutSine);
            LeanTween.scaleY(curtains[1].gameObject, 0, 0.3f).setEase(LeanTweenType.easeOutSine);
            areCurtainsOpened = true;
        }
        else
        {
            LeanTween.scaleY(curtains[0].gameObject, 1, 0.3f).setEase(LeanTweenType.easeOutSine);
            LeanTween.scaleY(curtains[1].gameObject, 1, 0.3f).setEase(LeanTweenType.easeOutSine);
        }
    }

    public void RetryButtonClicked()
    {
        StartCoroutine(RetryButtonAction());
    }

    IEnumerator RetryButtonAction()
    {
        yield return new WaitForSeconds(0.2f);
        CurtainsAction();
        yield return new WaitForSeconds(0.4f);
        SceneManager.LoadScene(0);
    }
}
