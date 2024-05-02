using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatController : MonoBehaviour
{
    public enum UpdateStatType
    {
        key, hp, gob
    }
    public string playerName;
    public int keys;
    public int hp;
    public int gobscount;


    public int maxHeal;
    public int maxKey;

    private void Start()
    {
        hp = maxHeal;
        keys = maxKey;
        gobscount = 0;

    }
    private void UpdateKey(int k)
    {
        if (keys - k < 0)
            keys = 0;
        else keys += k;

        if (k < 0)
        {
            Debug.Log(k);
            SpawnKeyEx.Istaince.SpawnIntKeysUsed(Mathf.Abs(k), GameManagerParchessi.Instance.GetCurrentPlayerTurn().transform);
        }
    }

    private void UpdateHp(int h)
    {        
        hp = Mathf.Clamp(hp + h, 0, maxHeal); 
        UpdateStat();
        if (hp <= 0)//dead
        {
            GameManagerParchessi.Instance.SetListPlayerSpawn(GetComponent<PlayerControllerParchessi>());
        }
    }
    private void AddGoblet(int g)
    {
        gobscount += g;       
        CanvasManager.Instance.UpdatePlayerStat(GetComponent<PlayerControllerParchessi>(), keys, hp, hp / maxHeal, gobscount-1);      
    }
  
    public void UpdateStat(UpdateStatType updateStatType, int value)
    {
        switch (updateStatType)
        {
            case UpdateStatType.key:
                UpdateKey(value);
                UpdateStat();
                break;
            case UpdateStatType.hp:
                UpdateHp(value);
                break;
            case UpdateStatType.gob:
                AddGoblet(value);
                break;
            default:
                break;
        }
    }

    private void UpdateStat()
    {
        CanvasManager.Instance.UpdatePlayerStat(GetComponent<PlayerControllerParchessi>(), keys, hp, (float)hp / (float)maxHeal);
    }

   
}
