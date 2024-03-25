using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class Slot : MonoBehaviour, IPointerClickHandler
{
    public Item item;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (item.count > 0)
        {
            PlayerControllerParchessi.Instance.GetInventoryManager().SetCurrentItemChoses(item);
        }
    }
}
