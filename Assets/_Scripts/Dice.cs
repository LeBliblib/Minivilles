using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice
{
    private int rand;

    public int Roll() {
        rand = Random.Range(1, 7);
        return rand;
    }

}
