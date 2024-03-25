using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEst : MonoBehaviour
{
    private Func<bool> FuncBool;


    public void SetDelegate(Func<bool> FuncBool)
    {
        this.FuncBool = FuncBool;
    }
    private void Start()
    {
        bool a = FuncBool();
    }

}
