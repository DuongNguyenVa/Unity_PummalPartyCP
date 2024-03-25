using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[CreateAssetMenu(menuName = "SO/Item", order = 1)]
public class ItemSO : ScriptableObject
{
    public enum ItemType
    {
       none, rocket,heal
    }

    public ItemType itemType;
    public  string itemName;
    public Sprite sprite;
    public int hpBuff;
    public void Use(ItemType t)
    {
        switch (t)
        {
            case ItemType.rocket:
                {
                    PlayerControllerParchessi.Instance.UseRocket();
                }
                break;
            case ItemType.heal:
                {
                    PlayerStatManager.Instance.UpdateHp(hpBuff);
                }
                break;
            default:
                break;
        }

    }
    public static ItemType GetRamdomItem()
    {
        return (ItemType)Random.Range(1, System.Enum.GetValues(typeof(ItemType)).Length);
    }
 }
