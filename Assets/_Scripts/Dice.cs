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

    //private Vector3 startPos = new Vector3(-10, 0, 0);
    //private Vector3 endPos = new Vector3(2, 0, 0);

    /*

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
    */


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
    }

    private void Start()
    {
        rend = diceEnd.GetComponent<SpriteRenderer>();

        animator.SetActive(false);
        animator.GetComponent<Animator>().speed =  0.7f; //Change la vitesse de l'anim
    }

    public IEnumerator RollTheDice()
    {
        move = true;
        animator.SetActive(true);
        diceEnd.SetActive(false);

        yield return new WaitForSeconds(1.2f);

        animator.SetActive(false);
        diceEnd.SetActive(true);
        ///move = false;
    }

    public void Update()
    {
        if (move)
            //transform.position = startPos;
            //transform.position = Vector3.MoveTowards(startPos, endPos, 0.5f * Time.deltaTime);

            transform.position = Vector3.left * Time.deltaTime * 3; // Marche pas, semble bloqué sur la scène.

    }
}


