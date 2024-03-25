using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
     void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            GameObject canvasInventory = transform.Find("CanvasInventory").gameObject;
           
            canvasInventory.SetActive(!canvasInventory.activeSelf);
        }
    }
}
