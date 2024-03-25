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
            PlayerStatManager.Instance.UpdateKey(key);

        }
    }

    [System.Serializable]
    public class UpdateHpParams
    {
        public int hp;
        public ParticleSystem vfx;

        public void UpdateHp()
        {
            PlayerStatManager.Instance.UpdateHp(hp);
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
            GameManagerParchessi.Instance.GetCurrentPlayerTurn().SetState(PlayerControllerParchessi.State.dead);
            npcOBJ.GetComponent<AnimEvent>().hp = hp;
            
        }
    }
    //params
    public UpdateKeyParams updateKey;
    public UpdateHpParams updateHp;
    public AttackedParams attacked;

    public void ActiveEffect()
    {
        switch (effectType)
        {
            case EffectType.UpdateKey: updateKey.UpdateKey();            
                break;
            case EffectType.UpdateHp:  updateHp.UpdateHp();
                break;
            case EffectType.Attacked:  attacked.Attacked();
                break;
            case EffectType.Gift:       RandomEffecItem();
                break;
            case EffectType.Goblet:     GotGoblet();
                break;
            default:
                break;
        }
    }

    public void RandomEffecItem()
    {
        PlayerControllerParchessi.Instance.GetComponent<InventoryManager>().AddItem(ItemSO.GetRamdomItem());
    }

    public void GotGoblet()
    {
        GameManagerParchessi.Instance.SetState(GameManagerParchessi.StateGameParchessi.DarkSpace);
        StepManager.Instance.AsSomeGetGoblet();
        PlayerControllerParchessi.Instance.DoDance();
        PlayerControllerParchessi.Instance.FaceToST(Camera.main.transform.position);
        PlayerStatManager.Instance.UpdateGoblet(1);
    }
    public void SetParams(UpdateKeyParams keyParam, UpdateHpParams hpParam, AttackedParams attackedParam)
    {
        updateKey = keyParam;
        updateHp = hpParam;
        attacked = attackedParam;
    }
  
}
