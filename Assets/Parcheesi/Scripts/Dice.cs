using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Dice
{ 
    public static int GetDice()
    {
        return Random.Range(1, 10);
    }
    public static int GetDice(int n)
    {
        return n;
    }

}
