using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManagerParchessi : MonoBehaviour
{
    public static GameManagerParchessi Instance;
    public enum StateGameParchessi
    {
      none,  BeforStartTurn, ChestSpawn, StartTurn, WaitSomeoneEnd, NextOrder, EndTurn, DarkSpace, LightSpace, SomeOneChosing, SomeOneRoll, SomeOneMove, SomeOneGetEffect, WaitCameraMoving,EndOrder
    }
    private StateGameParchessi state;

    //public event EventHandler OnGameStateChange;
    public event Action<StateGameParchessi> OnGameStateChangeTo;

    public Step pfTreaserChest;
    private Step treaserChest;

    private Light lightMain;


    public List<Step> stepsCanSpawnChest = new List<Step>();
    private DirectionArrowItem currentArrowTarget;

    private int currentTurn;
    private int currentOrder;
    private PlayerControllerParchessi currentPlayerInTurn;
    private bool isChestAlready = false;

    public List<PlayerControllerParchessi> listturnPlayOrder = new List<PlayerControllerParchessi>();
    public Transform parentAllPlayer;

    public AnimStat animStat;

    private float timeDelay;
    private float countToDelay;

    private void Awake()
    {
        Instance = this;
    }


    private void Start()
    {
        for (int i = 0; i < parentAllPlayer.childCount; i++)
        {
            listturnPlayOrder.Add(parentAllPlayer.GetChild(i).GetComponent<PlayerControllerParchessi>());
        }
        //todo_test: set player 1st
        SetCurrentPlayerTurn(0);

        lightMain = GetComponentInChildren<Light>();
        treaserChest = Instantiate(pfTreaserChest, transform);
        treaserChest.gameObject.SetActive(false);

        ResetListStepCanSpawnChest();

        currentOrder = 0;
        SetState(StateGameParchessi.ChestSpawn);
    }

    private void Update()
    {
        if (RunDelayTime()) return;

        switch (state)
        {
            case StateGameParchessi.BeforStartTurn:
                {
                    SetCurrentPlayerTurn(0);
                    state = StateGameParchessi.ChestSpawn;
                }
                break;
            case StateGameParchessi.ChestSpawn:
                {                    
                     StepManager.Instance.SpawnNewTeasureChest();
                     isChestAlready = true;
                    
                    state = StateGameParchessi.WaitCameraMoving;
                    SetDelayTime(3);
                }
                break;
            case StateGameParchessi.WaitCameraMoving:
                {
                    CameraManager.Instance.FocusPlayer(currentPlayerInTurn.transform);
                    state = StateGameParchessi.SomeOneRoll;
                    SetDelayTime(3);
                }
                break;
            case StateGameParchessi.SomeOneRoll:
                {
                    OnGameStateChangeTo?.Invoke(state);
                    state = StateGameParchessi.none;
                }
                break;
            case StateGameParchessi.SomeOneMove:
                {
                }
                break;
            case StateGameParchessi.StartTurn:
                {
                    SetCurrentPlayerTurn(0);
                    state = StateGameParchessi.WaitSomeoneEnd;
                }
                break;
            case StateGameParchessi.WaitSomeoneEnd:
                {

                }
                break;
            case StateGameParchessi.NextOrder:
                {                   
                    //last order
                    if (currentOrder == listturnPlayOrder.Count)
                    {
                        currentOrder = 0;
                        //EndOrder();
                        //listturnPlayOrder.Clear();
                        //state = StateGameParchessi.EndTurn;
                    }                    
                        SetCurrentPlayerTurn(currentOrder);
                                        
                    if (!isChestAlready)
                    {
                        state = StateGameParchessi.ChestSpawn;
                    }
                    else
                    {
                        state = StateGameParchessi.WaitCameraMoving;
                    }
                }
                break;

            case StateGameParchessi.SomeOneGetEffect:
                {

                }
                break;
            case StateGameParchessi.EndOrder:
                {
                    currentOrder += 1;
                    state = StateGameParchessi.NextOrder;
                }
                break;
            case StateGameParchessi.EndTurn:
                {
                    currentTurn += 1;
                    state = StateGameParchessi.WaitCameraMoving;
                    SetCurrentPlayerTurn();
                }
                break;
            case StateGameParchessi.DarkSpace:
                {
                    if (lightMain.intensity > 0)
                        lightMain.intensity -= Time.deltaTime;
                }
                break;
            case StateGameParchessi.LightSpace:
                if (lightMain.intensity < 3)
                    lightMain.intensity += Time.deltaTime;
                break;
            case StateGameParchessi.SomeOneChosing:
                {
                    OnGameStateChangeTo?.Invoke(state);
                    state = StateGameParchessi.SomeOneMove;
                    if (currentPlayerInTurn.isBotController) return;
                    //todo: hightlight arrow : deleteeeeeee 
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                    if (Physics.Raycast(ray, out RaycastHit hit))
                    {
                        if (hit.transform.gameObject.TryGetComponent(out DirectionArrowItem arrowDir))
                        {
                            currentArrowTarget = arrowDir;
                            arrowDir.ScaleSize(2);
                        }
                        else
                        {
                            currentArrowTarget?.ScaleSize(1);
                        }
                    }

                }
                break;
            default:
                break;
        }
    }
    public void SetState(StateGameParchessi st)
    {
        state = st;
    }
    public void SpawnNewTeasureChest()
    {
        GetListStepCantUseByUser(PlayerControllerParchessi.Instance.currentPositionStep, 0);
        StepManager.Instance.ChangeStepToTreaserChestStep(stepsCanSpawnChest[UnityEngine.Random.Range(0, stepsCanSpawnChest.Count)]);
    }

    void GetListStepCantUseByUser(Step st, int countStep = 0)
    {
        StepsCanSpawnChestRemove(st, countStep + 1);
    }
    void StepsCanSpawnChestRemove(Step st, int countStep = 0)
    {
        if (countStep <= 0) return;
        Step ns = st;

        if (ns.stepType == Step.StepType.Multi)
        {
            foreach (Step step in ns.nextSteps)
            {
                StepsCanSpawnChestRemove(step, countStep - 1);
            }
        }
        else
            StepsCanSpawnChestRemove(ns.nextStep, countStep - 1);
        stepsCanSpawnChest.Remove(ns);
    }
    void ResetListStepCanSpawnChest()
    {
        stepsCanSpawnChest.Clear();
        foreach (Step st in StepManager.Instance.GetComponentsInChildren<Step>())
        {
            if (st.TryGetComponent(out StepEffect sEff))
            {
                if (sEff.effectType != StepEffect.EffectType.Goblet)
                    stepsCanSpawnChest.Add(st);
            }
            else
                stepsCanSpawnChest.Add(st);

        }

    }

    public PlayerControllerParchessi GetCurrentPlayerTurn()
    {
        return currentPlayerInTurn;
    }
    public void SetCurrentPlayerTurn(int index = -1)
    {
        currentPlayerInTurn = (0 <= index && index < listturnPlayOrder.Count) ? listturnPlayOrder[index] : null;
    }

    public void StartTurn()
    {
        state = StateGameParchessi.StartTurn;
    }
    public void EndTurn()
    {
        state = StateGameParchessi.EndTurn;
    }
    private void EndOrder()
    {
        SetCurrentPlayerTurn();
    }
    public void WaitEndEffect(float effTime=0, bool isGetGoblet=false)
    {
        //EndOrder();

        if (isGetGoblet)
        {
            isChestAlready = false;
        }       
        state = StateGameParchessi.SomeOneGetEffect;
        StartCoroutine(End());
        IEnumerator End()
        {
            yield return new WaitForSeconds(effTime);
            state = StateGameParchessi.EndOrder;

        }
    }

    private void SetDelayTime(float t)
    {
        timeDelay = t;
    }
    private bool RunDelayTime()
    {
        if (timeDelay != 0 && countToDelay < timeDelay)
        {
            countToDelay += Time.deltaTime;
            return true;
        }
        else
        {
            countToDelay = 0;
            timeDelay = 0;
            return false;
        }
    }
}
