using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[CreateAssetMenu(menuName = "SO/Item", order = 1)]
public class ItemSO : ScriptableObject
{
    public enum ItemType
    {
       none, rocket,heal, attack
    }
    public ItemType itemType;
    public  string itemName;
    public Sprite sprite;
    public ParticleSystem vfx;
    public float time;

    [Header("Buff")]
    public int hpBuff;
    public int numBonus;
    public void Use(ItemType t)
    {
        PlayerControllerParchessi player = GameManagerParchessi.Instance.GetCurrentPlayerTurn();
        player.SetState(PlayerControllerParchessi.State.usingItem);
        switch (t)
        {
            case ItemType.rocket:
                {
                    player.UseRocket(numBonus);
                }
                break;
            case ItemType.heal:
                {
                   player.UseHealthItem(vfx, time);
                   player.UpdateStat(PlayerStatController.UpdateStatType.hp,hpBuff);                    
                }
                break;
            case ItemType.attack:
                {
                    foreach (PlayerControllerParchessi playerAttacked in player.GetComponentInChildren<UntilmateAttack>().listPlayerTrget)
                    {
                        playerAttacked.GetAttacked();
                        playerAttacked.UpdateStat(PlayerStatController.UpdateStatType.hp, -100);

                    }
                    player.UseAttackItemUltimate();

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
