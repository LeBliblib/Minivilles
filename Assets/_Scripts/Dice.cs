using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    public static Dice Instance { get; private set; }

    private bool move = false;

    public int finalSide;
    public Sprite[] diceSides;

    private SpriteRenderer rend;
    public GameObject diceEnd;
    public  GameObject animator;

    Vector3 startPosition1;
    Vector3 startPosition2;
    Vector3 rollSpeed;
    Vector3 startDiceScale;
    Vector3 endDiceScale;


    public int Roll()
    {
        transform.position = startPosition1;

        StartCoroutine("RollTheDice");

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
        startPosition1 = new Vector3(-5.0f, 0, 0);
        startPosition2 = new Vector3(-7, 0, 0);
        rollSpeed = new Vector3(5.0f, 0, 0);
        startDiceScale = new Vector3(2.0f, 2.0f, 1.0f);
        endDiceScale = new Vector3(1.0f, 1.0f, 1.0f);
    }

    private void Start()
    {
        rend = diceEnd.GetComponent<SpriteRenderer>();

        animator.SetActive(false);
        animator.GetComponent<Animator>().speed =  0.7f; //Change la vitesse de l'anim

        diceEnd.SetActive(false);
        animator.SetActive(false);
    }

    public IEnumerator RollTheDice()
    {
        transform.localScale = startDiceScale;
        LeanTween.scale(gameObject, endDiceScale, 1.1f).setEase(LeanTweenType.easeOutBounce);
        move = true;
        animator.SetActive(true);
        diceEnd.SetActive(false);

        yield return new WaitForSeconds(1.2f);

        animator.SetActive(false);
        diceEnd.SetActive(true);
        move = false;
    }

    public void Update()
    {
        if (move)
            transform.position += rollSpeed * Time.deltaTime;
    }

    //========================================================================================

    public int randomDiceSide2;
    public int Roll2() // for the second dice
    {
        transform.position = startPosition2;

        StartCoroutine("RollTheDice");

        finalSide = randomDiceSide2 + 1;
        Debug.Log(finalSide);
        StartCoroutine("DestroyDice2");
        return finalSide;
    }

    public IEnumerator DestroyDice2() // hide the second dice when not played
    {
        randomDiceSide2 = Random.Range(0, 6);
        rend.sprite = diceSides[randomDiceSide2];

        yield return new WaitForSeconds(4.5f);

        rend.sprite = null;
        diceEnd.SetActive(false);
        animator.SetActive(false);
    }
}


