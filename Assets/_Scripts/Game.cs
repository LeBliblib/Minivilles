using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Game : MonoBehaviour
{
    private List<Player> players = new List<Player>();
    private Pile gamePile = new Pile();
    private List<Dice> gameDice = new List<Dice>();

    public static Game instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    #region Methode
    public void RollDice()
    {
        foreach(Dice D in gameDice)
        {
            //Renvoie un int avec la valeur du D roll
            D.Roll();
        }
    }

    public Player GetPlayer(int index)
    {
        return players[index];
    }
    #endregion
}
