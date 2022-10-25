using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radio : Monument
{
    bool usedThisTurn;

    public Radio(string name, string desc, int cost, int playerID) : base(name, desc, cost, playerID) { }

    public override void Buy()
    {
        base.Buy();

        Game.instance.onDiceRoll += CheckForActivation;
        Game.instance.onTurnEnd += TurnEnd;
    }

    public void CheckForActivation(int[] rolls, int id)
    {
        if (id != PlayerID || usedThisTurn)
        {
            Game.instance.SetCallback(CallbackTypes.DiceRoll);
            return;
        }

        Game game = Game.instance;

        UIManager.PopUpCallback callback = Reroll;

        game.ui.ShowChoosePopUp("Tour Radio", "Voulez-vous relancer les dés ?", callback);
    }

    public void Reroll(bool isValid)
    {
        if (isValid)
        {
            usedThisTurn = true;
            Game.instance.RerollDice();
        }

        Game.instance.SetCallback(CallbackTypes.DiceRoll);
    }

    public void TurnEnd(int id)
    {
        usedThisTurn = false;
    }

    public override void Activate()
    {
        throw new System.NotImplementedException();
    }

    public override void Destroy()
    {
        Game.instance.onDiceRoll -= CheckForActivation;
        Game.instance.onTurnEnd -= TurnEnd;
    }
}
