using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
   public static Dice Instance { get; private set; }
    public int finalSide;

    public Sprite[] diceSides;
    private Sprite[] animationDice;

    private SpriteRenderer rend;

   public int Roll()
   {
        RollTheDice();
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
    public void RollTheDice()
    {
        int randomDiceSide = 0;
        finalSide = 0;
        for (int i = 0; i <= 20; i++)
        {
            randomDiceSide = Random.Range(0, 6);
            Debug.Log(diceSides.Length);
            rend.sprite = diceSides[randomDiceSide];
        }
        finalSide = randomDiceSide + 1;
        Debug.Log(finalSide);
    }
}
/* public int Roll()
{
    //StartCoroutine(RollTheDice());
    RollTheDice();

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
    animationDice = Resources.LoadAll<Sprite>("AllDiceSides/");
}

public void RollTheDice()
{
    int randomDiceAnim = 0;

    for (int i = 0; i <= 5; i++)
    {
        //Add animations
        randomDiceAnim = Random.Range(0, animationDice.Length +1);
        rend.sprite = animationDice[randomDiceAnim];

        //yield return new WaitForSeconds(0.05f);
    }
}
}*/


