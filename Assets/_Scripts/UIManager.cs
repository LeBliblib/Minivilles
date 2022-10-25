using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject dice;
    [SerializeField] TextMeshProUGUI diceValueText;

    [SerializeField] List<TextMeshProUGUI> coinsText;

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
}
