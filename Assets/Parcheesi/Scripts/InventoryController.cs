using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryController : MonoBehaviour
{
    [System.Serializable]
    public class ItemVisualInfor
    {
        public ItemSO.ItemType type;
        public GameObject obj;
    }

    public List<Item> items;

    public List<ItemVisualInfor> listItemVisualInfor;
    public GameObject objGotItemNoti;


    private ItemSO currentItemChoses;
    private GameObject itemCurrentUse;

    private bool isCanUseItem;
    private ItemSO.ItemType currentItemEquipType=ItemSO.ItemType.none;
    private GameObject currentItemEquip;
    private PlayerControllerParchessi player;
    private void Start()
    {
        HideAllObjsVisual();
        isCanUseItem = true;
        player = GetComponent<PlayerControllerParchessi>();
    }
    private void Update()
    {
        if (isCanUseItem && Input.GetKeyDown(KeyCode.Q) && currentItemChoses)
        {
            isCanUseItem = false;
            InventoryCanvasManager.Instance.ShowUI(false);
            RemoveItem(currentItemChoses);
            currentItemChoses.Use(currentItemChoses.itemType);
            currentItemChoses = null;
            currentItemEquipType = ItemSO.ItemType.none;
            //GameManagerParchessi.Instance.SetState(GameManagerParchessi.StateGameParchessi.SomeOneUsingItem);

        }
    }
    private void LateUpdate()
    {
        if (currentItemEquipType == ItemSO.ItemType.attack)
        {
            Vector3 mousePoint = GetPlayerPlaneMousePos();
            Vector3 dir = new Vector3(mousePoint.x, currentItemEquip.transform.GetChild(0).position.y, mousePoint.z) - currentItemEquip.transform.GetChild(0).position;

            Quaternion dirRos = Quaternion.LookRotation(dir);
            currentItemEquip.transform.GetChild(0).rotation = Quaternion.Lerp(transform.rotation, dirRos, 1f);
            player.transform.GetChild(0).rotation = Quaternion.Lerp(transform.rotation, dirRos, 1f);
        }
    }
    public Vector3 GetPlayerPlaneMousePos()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100, 1 << 6))
        {
            return hit.point;
        }
        return Vector3.zero;

    }
    private void HideAllObjsVisual()
    {
        foreach (ItemVisualInfor itemOBJ in listItemVisualInfor)
        {
            if (itemOBJ.obj.activeSelf)
                itemOBJ.obj.SetActive(false);
        }
        objGotItemNoti.SetActive(false);
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
    }
    public void RemoveItem(ItemSO itemSO)
    {
        foreach (Item item in items)
        {
            if (item.so == itemSO)
                item.count--;
        }
        InventoryCanvasManager.Instance.LoadInventoryVisual(items);
    }

    public ItemSO GetItemSO(ItemSO itemSO)
    {
        foreach (Item item in items)
        {
            return item.so == itemSO ? item.so : null;
        }
        return null;
    }
    public void UseItemType(ItemSO.ItemType itemType, float time = 0)
    {

        itemCurrentUse = GetVisualObjByType(itemType);
        itemCurrentUse.transform.localPosition = Vector3.zero;
        itemCurrentUse.SetActive(true);
        switch (itemType)
        {
            case ItemSO.ItemType.none:
                break;
            case ItemSO.ItemType.rocket:
                itemCurrentUse.GetComponent<Animation>().Play("rocket_fire");
                break;
            case ItemSO.ItemType.heal:
                itemCurrentUse.GetComponent<Animation>().Play("heal_fire");
                break;
            case ItemSO.ItemType.attack:
                itemCurrentUse.GetComponent<Animation>().Play("ultimate_fire");
                break;
            default:
                break;
        }
        if (time != 0)
            Invoke(nameof(AfterUseItem), time);
    }
    public void AfterUseItem()
    {
        HideAllObjsVisual();
        player.SetState(PlayerControllerParchessi.State.readyToRoll);

    }
    public void DisUseItemType()
    {
        itemCurrentUse.SetActive(false);
    }
    public void SetCurrentItemChoses(ItemSO it)
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
        AnimatedUI.Instaince.ShowHideAffter(objGotItemNoti, 2, sp);
    }
    public void SetCanUseItem(bool isCanUseI)
    {
        isCanUseItem = isCanUseI;
    }
    public void EquipItem(ItemSO itemSO)
    {
        currentItemEquipType = itemSO.itemType;
        HideAllObjsVisual();

        if (currentItemChoses?.itemType == ItemSO.ItemType.attack)
        {
            player.PrepareAttackItem(false);
        }

        if (currentItemChoses == itemSO)
        {
            currentItemChoses = null;
            player.SetState(PlayerControllerParchessi.State.readyToRoll);
        }
        else
        {
            player.SetState(PlayerControllerParchessi.State.usingItem);
            SetCurrentItemChoses(itemSO);
            switch (itemSO.itemType)
            {
                case ItemSO.ItemType.none:
                    break;
                case ItemSO.ItemType.rocket:
                    EquipRocketItem();
                    break;
                case ItemSO.ItemType.heal:
                    EquipHealItem();
                    break;
                case ItemSO.ItemType.attack:
                    EquipAttackItem();
                    break;
                default:
                    break;
            }
        }

    }
    private void EquipHealItem()
    {
        GameObject currentItemEquip = GetVisualObjByType(ItemSO.ItemType.heal);
        currentItemEquip.SetActive(true);
        currentItemEquip.GetComponent<Animation>().Play("heal_prepare");
    }
    private void EquipRocketItem()
    {
        currentItemEquip = GetVisualObjByType(ItemSO.ItemType.rocket);
        currentItemEquip.SetActive(true);
        currentItemEquip.GetComponent<Animation>().Play("rocket_prepare");
    }
    private void EquipAttackItem()
    {
        currentItemEquip = GetVisualObjByType(ItemSO.ItemType.attack);
        currentItemEquip.SetActive(true);
        player.PrepareAttackItem();

    }
}