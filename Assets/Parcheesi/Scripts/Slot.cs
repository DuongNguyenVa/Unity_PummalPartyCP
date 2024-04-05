using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IPointerClickHandler
{
    public Item item;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (item.count > 0)
        {
            //GameManagerParchessi.Instance.GetCurrentPlayerTurn().GetInventoryController().SetCurrentItemChoses(item);
            GameManagerParchessi.Instance.GetCurrentPlayerTurn().GetInventoryController().EquipItem(item.so);
        }
    }
    public void UpdateUI()
    {
        GetComponentInChildren<TextMeshProUGUI>().text = item.count + "";
        if (item.count == 0)
        {
            transform.Find("Image").GetComponent<Image>().color = Color.gray;
        }
        else
            transform.Find("Image").GetComponent<Image>().color = Color.white;
    }
}
