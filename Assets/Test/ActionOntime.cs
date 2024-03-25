using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionOntime : MonoBehaviour
{
    public TEst est;

    private void Start()
    {
        est.SetDelegate(
            () =>
            {
                Debug.Log("Hello");
                return true;
            }
            
            );
    }


}
