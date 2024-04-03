using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StepEffect : MonoBehaviour
{
    public enum EffectType
    {
        none,
        UpdateKey,
        UpdateHp,
       Attacked,
       Gift,
       Goblet,

    }
    public EffectType effectType;
    public float timeEffect;
    [System.Serializable]
    public class UpdateKeyParams
    {
        public int key;

        public void UpdateKey()
        {
            GameManagerParchessi.Instance.GetCurrentPlayerTurn().UpdateStat(PlayerStatController.UpdateStatType.key,key);

        }
    }

    [System.Serializable]
    public class UpdateHpParams
    {
        public int hp;
        public ParticleSystem vfx;

        public void UpdateHp()
        {
            GameManagerParchessi.Instance.GetCurrentPlayerTurn().UpdateStat(PlayerStatController.UpdateStatType.hp, hp);
            vfx.Play();
        }
    }
    [System.Serializable]
    public class AttackedParams
    {
        public GameObject npcOBJ;
        public int hp;
        public void Attacked()
        {
            //PlayerStatManager.Instance.UpdateHp(hp);
            npcOBJ.GetComponent<Animator>().SetTrigger("doAttack");
            npcOBJ.GetComponent<AnimEvent>().playerTarget = GameManagerParchessi.Instance.GetCurrentPlayerTurn();
            GameManagerParchessi.Instance.GetCurrentPlayerTurn().SetState(PlayerControllerParchessi.State.attacked);
            npcOBJ.GetComponent<AnimEvent>().hp = hp;
            
        }
    }
    //params
    public UpdateKeyParams updateKey;
    public UpdateHpParams updateHp;
    public AttackedParams attacked;

    private void Start()
    {
        switch (effectType)
        {
            case EffectType.none:
                break;
            case EffectType.UpdateKey: 
                break;
            case EffectType.UpdateHp:
                break;
            case EffectType.Attacked:
                //effectType= attacked.
                break;
            case EffectType.Gift:
                break;
            case EffectType.Goblet:
                break;
            default:
                break;
        }
    }
    public void ActiveEffect(PlayerControllerParchessi player)
    {
        switch (effectType)
        {
            case EffectType.UpdateKey: updateKey.UpdateKey();            
                break;
            case EffectType.UpdateHp:  updateHp.UpdateHp();
                break;
            case EffectType.Attacked:  attacked.Attacked();
                break;
            case EffectType.Gift:       RandomEffecItem(player);
                break;
            case EffectType.Goblet:     GotGoblet();
                break;
            default:
                break;
        }
    }

    public void RandomEffecItem(PlayerControllerParchessi player)
    {
        player.GetComponent<InventoryManager>().AddItem(ItemSO.GetRamdomItem());
    }

    public void GotGoblet()
    {
        PlayerControllerParchessi player = GameManagerParchessi.Instance.GetCurrentPlayerTurn();
        //GameManagerParchessi.Instance.SetState(GameManagerParchessi.StateGameParchessi.DarkSpace);
        StepManager.Instance.AsSomeGetGoblet();
        player.DoDance();
        player.FaceToST(Camera.main.transform.position);
        GameManagerParchessi.Instance.GetCurrentPlayerTurn().UpdateStat(PlayerStatController.UpdateStatType.gob, +1);
    }
    public void SetParams(UpdateKeyParams keyParam, UpdateHpParams hpParam, AttackedParams attackedParam)
    {
        updateKey = keyParam;
        updateHp = hpParam;
        attacked = attackedParam;
    }
  
}
