using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    [System.Serializable]
    public class ItemVisualInfor
    {
        public ItemSO.ItemType type;
        public GameObject obj;
    }

    public List<Item> items;
    public Transform panelTrans;

    public List<ItemVisualInfor> listItemVisualInfor;
    public GameObject objNoti;


    private Item currentItemChoses;
    private GameObject itemPrefabs;
    private List<Slot> listSlots = new List<Slot>();

    private void Awake()
    {
        Slot[] sls = panelTrans.GetComponentsInChildren<Slot>();
        foreach (Slot item in sls)
        {
            listSlots.Add(item);
        }
    }
    private void Start()
    {
        UpdateInventory();
        //hide item object
        foreach (ItemVisualInfor itemOBJ in listItemVisualInfor)
        {
            itemOBJ.obj.SetActive(false);
        }
        objNoti.SetActive(false); //hide obj noti
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && currentItemChoses?.so)
        {
            RemoveItem(currentItemChoses.so);
            currentItemChoses.so.Use(currentItemChoses.so.itemType);
            currentItemChoses = null;
        }
    }
    public void AddItem(ItemSO.ItemType itemSO)
    {
        foreach (Item item in items)
        {
            if (item.so.itemType == itemSO)
            {
                item.count++;
                NotiGetItemOnPlayer(item.so.sprite);
            }
        }
        UpdateInventory();
    }
    public void RemoveItem(ItemSO itemSO)
    {
        foreach (Item item in items)
        {
            if (item.so == itemSO)
                item.count--;
        }
        UpdateInventory();
    }
    public void UpdateInventory()
    {
        foreach (Item item in items)
        {
            foreach (Slot slot in listSlots)
            {
                if (slot.item.so == item.so)
                {

                    slot.item.count = item.count;
                    slot.gameObject.transform.Find("Image").GetComponent<Image>().sprite = item.so.sprite;
                    slot.GetComponentInChildren<TextMeshProUGUI>().text = item.count + "";
                    if (item.count == 0)
                    {
                        slot.gameObject.transform.Find("Image").GetComponent<Image>().color = Color.gray;
                    }
                    else
                        slot.gameObject.transform.Find("Image").GetComponent<Image>().color = Color.white;

                    break;
                }
            }
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
    public void UseItemType(ItemSO.ItemType itemType)
    {
        itemPrefabs = GetVisualObjByType(itemType);
        itemPrefabs.SetActive(true);
    }
    public void DisUseItemType()
    {
        itemPrefabs.SetActive(false);

    }
    public void SetCurrentItemChoses(Item it)
    {
        currentItemChoses = it;
    }
    private GameObject GetVisualObjByType(ItemSO.ItemType itemType)
    {
        foreach (ItemVisualInfor item in listItemVisualInfor)
        {
            if (item.type == itemType)
            {
                return item.obj;
            }
        }
        return null;
    }
    private void NotiGetItemOnPlayer(Sprite sp)
    {
        AnimatedUI.Instaince.ShowHideAffter(objNoti, 2, sp);
    }
}