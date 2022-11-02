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

    Vector3 startPosition;
    Vector3 rollSpeed;
    Vector3 startDiceScale;
    Vector3 endDiceScale;


    public int Roll()
    {
        StartCoroutine("RollTheDice");

        int randomDiceSide; 
        randomDiceSide = Random.Range(0, 6);
       
        rend.sprite = diceSides[randomDiceSide];

        finalSide = randomDiceSide + 1;
        Debug.Log(finalSide);
        return finalSide;
    }

   private void Awake()// a supp ?
    {
        Instance = this;
        startPosition = new Vector3(-5.0f, 0, 0);
        rollSpeed = new Vector3(5.0f, 0, 0);
        startDiceScale = new Vector3(2.0f, 2.0f, 1.0f);
        endDiceScale = new Vector3(1.0f, 1.0f, 1.0f);
    }

    private void Start()
    {
        rend = diceEnd.GetComponent<SpriteRenderer>();

        animator.SetActive(false);
        animator.GetComponent<Animator>().speed =  0.7f; //Change la vitesse de l'anim
    }

    public IEnumerator RollTheDice()
    {
        transform.position = startPosition;
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
}


