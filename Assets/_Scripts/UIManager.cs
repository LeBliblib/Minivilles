using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject dice;
    [SerializeField] TextMeshProUGUI diceValueText;

    [SerializeField] List<TextMeshProUGUI> coinsText;

    [SerializeField] CardGameObject cardPrefab;
    [SerializeField] List<PlayerGameObject> players;

    [SerializeField] List<Sprite> monumentSprites;

    [SerializeField] GameObject popUp;
    [SerializeField] TextMeshProUGUI popUpInside, popUpTitle;
    [SerializeField] float popUpAnimTime;

    public delegate void PopUpCallback(bool valid);
    public PopUpCallback currentPopupCallback;

    private void Awake()
    {
        dice.SetActive(false);
        popUp.transform.localScale = Vector3.zero;
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

    public void ShowPopUp(string title, string desc, PopUpCallback callback)
    {
        LeanTween.cancel(popUp);

        currentPopupCallback = callback;

        popUp.transform.localScale = Vector3.zero;

        popUpTitle.text = title;
        popUpInside.text = desc;

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
}
