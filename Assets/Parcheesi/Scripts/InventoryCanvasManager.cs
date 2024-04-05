using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryCanvasManager : MonoBehaviour
{
    public static InventoryCanvasManager Instance;
  
    public List<Item> items;
    public Transform panelTrans;

    private List<Slot> listSlots = new List<Slot>();

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        Slot[] sls = panelTrans.GetComponentsInChildren<Slot>();
        foreach (Slot item in sls)
        {
            listSlots.Add(item);
        }
        ShowUI(false);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && GameManagerParchessi.Instance.GetCurrentPlayerTurn().GetState()==PlayerControllerParchessi.State.readyToRoll)
        {
            ToggleShowUI();
        }
    }
    public void ShowUI(bool isShow)
    {
        gameObject.GetComponent<Canvas>().enabled=isShow;
    }
    public void ToggleShowUI()
    {
        Canvas cs = gameObject.GetComponent<Canvas>();
        cs.enabled = !cs.isActiveAndEnabled;
    }
    public void UpdateInventoryVisual()
    {
        foreach (Slot slot in listSlots)
        {
            slot.UpdateUI();
        }
    }
    public ItemSO GetItemSO(ItemSO itemSO)
    {
        foreach (Item item in items)
        {
            return item.so == itemSO ? item.so : null;
        }
        return null;
    }
   
    public void LoadInventoryVisual(List<Item> itemList)
    {
        foreach (Item item in itemList)
        {
            Slot sl = GetSlotByItemSO(item.so);
            if (sl)
            {
                sl.item.count = item.count;
            }
        }
        UpdateInventoryVisual();
    }
    private Slot GetSlotByItemSO(ItemSO so)
    {
        foreach (Slot slot in listSlots)
        {
            if (slot.item.so == so) return slot;
        }
        return null;
    }
}