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
        SpawnBase

    }
    public EffectType effectType;
    private float timeEffect = 3;
    [System.Serializable]
    public class UpdateKeyParams
    {
        public int key;

        public void UpdateKey()
        {
            GameManagerParchessi.Instance.GetCurrentPlayerTurn().UpdateStat(PlayerStatController.UpdateStatType.key, key);

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
        public float timeEffect;

        public void Attacked()
        {
            //PlayerStatManager.Instance.UpdateHp(hp);
            npcOBJ.GetComponent<Animator>().SetTrigger("doAttack");
            npcOBJ.GetComponent<AnimEvent>().playerTarget = GameManagerParchessi.Instance.GetCurrentPlayerTurn();
            GameManagerParchessi.Instance.GetCurrentPlayerTurn().SetState(PlayerControllerParchessi.State.attacked);
            npcOBJ.GetComponent<AnimEvent>().hp = hp;
            GameManagerParchessi.Instance.WaitEndEffect(timeEffect);

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
            case EffectType.UpdateKey:
                updateKey.UpdateKey();
                GameManagerParchessi.Instance.WaitEndEffect(timeEffect);//todo:test

                break;
            case EffectType.UpdateHp:
                updateHp.UpdateHp();
                GameManagerParchessi.Instance.WaitEndEffect(timeEffect);//todo:test

                break;
            case EffectType.Attacked:
                attacked.Attacked();
                break;
            case EffectType.Gift:
                RandomEffecItem(player);
                GameManagerParchessi.Instance.WaitEndEffect(timeEffect);  //todo:test

                break;
            case EffectType.Goblet:
                {
                    ;
                ShowNotiOpenChest(player.GetComponent<PlayerStatController>().keys >=40);

                }
                break;
            default:
                break;
        }


    }

    public void RandomEffecItem(PlayerControllerParchessi player)
    {
        player.GetComponent<InventoryController>().AddItem(ItemSO.GetRamdomItem());
    }

    private void ShowNotiOpenChest(bool canBeOpen)
    {
        CanvasManager.Instance.ToggleOpenChestNoti(true, canBeOpen);
    }
    public void GotGoblet()
    {
        PlayerControllerParchessi player = GameManagerParchessi.Instance.GetCurrentPlayerTurn();
        //GameManagerParchessi.Instance.SetState(GameManagerParchessi.StateGameParchessi.DarkSpace);
        StepManager.Instance.AsSomeGetGoblet();
        player.FaceToST(Camera.main.transform.position);
        GameManagerParchessi.Instance.GetCurrentPlayerTurn().UpdateStat(PlayerStatController.UpdateStatType.gob, +1);
        if (player.GetComponent<PlayerStatController>().gobscount == GameManagerParchessi.Instance.soGameManager.gobletNumToWin)
        {
            player.Win();
            //GameManagerParchessi.Instance.StopAllCoroutines();
            GameManagerParchessi.Instance.SetState(GameManagerParchessi.StateGameParchessi.SomeOneWin);
            CanvasManager.Instance.PostNoti(player.name+" Win", 1000f);
        }
        else
        {
            player.DoDance();
            if (player.CheckNumSaving())
            {
                GameManagerParchessi.Instance.WaitEndEffect(timeEffect, true, true);
                Debug.Log("Got goblet but still move");
            }
            else
            {
                GameManagerParchessi.Instance.WaitEndEffect(timeEffect, true);
            }
        }
    }
    public void SetParams(UpdateKeyParams keyParam, UpdateHpParams hpParam, AttackedParams attackedParam)
    {
        updateKey = keyParam;
        updateHp = hpParam;
        attacked = attackedParam;
    }

}
