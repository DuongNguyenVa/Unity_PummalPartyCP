using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyAnimEvent : MonoBehaviour
{

   public void StartSpin()
    {
        GetComponent<Animation>().Play("keyspin");
        GetComponent<Collider>().enabled = true;

    }
   
}
