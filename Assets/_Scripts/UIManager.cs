using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject dice;
    [SerializeField] TextMeshProUGUI diceValueText;

    [SerializeField] List<TextMeshProUGUI> coinsText;

    [SerializeField] CardGameObject cardPrefab;
    [SerializeField] List<PlayerGameObject> players;

    [SerializeField] List<Sprite> monumentSprites;

    [Header("PopUp")]
    [SerializeField] GameObject popUp;
    [SerializeField] float popUpAnimTime;

    [SerializeField] GameObject choiceContent, selectContent;

    [Header("PopUp Choice")]
    [SerializeField] TextMeshProUGUI popUpInside, popUpTitle;

    [Header("PopUp PlayerSelect")]
    [SerializeField] UIElement playerSelectPrefab;
    [SerializeField] Transform playerSelectGrid;
    List<UIElement> playerSelects = new();

    public delegate void PopUpCallback(bool valid);
    public PopUpCallback currentPopupCallback;

    public delegate void PopUpSelectCallback(int selectedID);
    public PopUpSelectCallback currentSelectPopUpCallback;

    private void Awake()
    {
        dice.SetActive(false);
        popUp.transform.localScale = Vector3.zero;
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
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        CardGameObject card = Instantiate(cardPrefab, pos, Quaternion.identity);

        card.Init(cardSO.texture, players[playerID].playerTransform.position);

        players[playerID].AddCard(card);
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

        ShowPopUp();
    }

    public void ShowSelectPopUp(PopUpSelectCallback callback)
    {
        List<Player> players = Game.instance.GetAllPlayers();

        for(int i = 0; i < players.Count; i++)
        {
            playerSelects[i].SetText(1, "" + players[i].coins);
        }

        choiceContent.SetActive(false);
        selectContent.SetActive(true);

        currentSelectPopUpCallback = callback;

        ShowPopUp();
    }

    public void ShowPopUp()
    {
        LeanTween.cancel(popUp);

        popUp.transform.localScale = Vector3.zero;

        LeanTween.scale(popUp, Vector3.one, popUpAnimTime).setEaseInOutExpo().setIgnoreTimeScale(true);
    }

    public void HidePopUp()
    {
        LeanTween.cancel(popUp);
        LeanTween.scale(popUp, Vector3.zero, popUpAnimTime).setEaseInOutExpo().setIgnoreTimeScale(true);
    }

    public void ChooseSelectPopUp(bool valid)
    {
        currentPopupCallback?.Invoke(valid);
    }

    public void PlayerSelectDone(int id)
    {
        currentSelectPopUpCallback?.Invoke(id);
    }
}
