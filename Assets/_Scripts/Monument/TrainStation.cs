using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainStation : Monument
{
    public TrainStation(string name, string desc, int cost, int playerID) : base(name, desc, cost, playerID) { }

    public override void Buy()
    {
        base.Buy();

        Game.instance.onTurnStart += CheckForActivation;
    }

    public void CheckForActivation(int turnPlayerID)
    {
        if (turnPlayerID == PlayerID) Activate();
        else Game.instance.SetCallback(CallbackTypes.TurnStart);
    }

    public override void Activate()
    {
        Game game = Game.instance;

        UIManager.PopUpCallback callback = ChangeDiceNumber;

        game.ui.ShowChoosePopUp("Gare", "Voulez-vous lancer 2 dés ?", callback);
    }

    public void ChangeDiceNumber(bool isValid)
    {
        if(isValid)
            Game.instance.ChangeDiceNumbers(2);

        Game.instance.SetCallback(CallbackTypes.TurnStart);
    }

    public override void Destroy()
    {
        Game.instance.onTurnStart -= CheckForActivation;
    }
}
