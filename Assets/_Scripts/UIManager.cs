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

    private void Awake()
    {
        dice.SetActive(false);
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
}
