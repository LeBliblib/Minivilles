using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    public static Dice Instance { get; private set; }
    public int finalSide;

    public Sprite[] diceSides;// Array of dice sides sprites to load from Resources folder
    private SpriteRenderer rend;// Reference to sprite renderer to change sprites

    public int Roll()
    {
        //int rand  = Random.Range(1, 7);
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

