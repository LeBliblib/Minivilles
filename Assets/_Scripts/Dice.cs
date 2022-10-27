using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    public static Dice Instance { get; private set; }
    public int finalSide;

    public Sprite[] diceSides;
    public Sprite[] animationDice;

    private SpriteRenderer rend;

    public int Roll()
    {
        //RollTheDice();
        StartCoroutine(RollTheDice());

        int randomDiceSide;

        randomDiceSide = Random.Range(0, 6);
        rend.sprite = diceSides[randomDiceSide];

        finalSide = randomDiceSide + 1;
        Debug.Log(finalSide);

        return finalSide;
    }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        rend = GetComponent<SpriteRenderer>();
    }

    public IEnumerator RollTheDice()
    {
        int randomDiceAnim = 0;

        for (int i = 0; i <= 20; i++)
        {
            //Add animations
            randomDiceAnim = Random.Range(0, 1);
            rend.sprite = animationDice[randomDiceAnim];

            yield return new WaitForSeconds(0.05f);
        }
    }
}

