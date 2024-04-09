using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManagerParchessi : MonoBehaviour
{
    public static GameManagerParchessi Instance;
    public enum StateGameParchessi
    {
        none, BeforStartTurn, ChestSpawn, StartTurn, WaitSomeoneEnd, NextOrder, EndTurn, DarkSpace, LightSpace, SomeOneChosing,
        SomeOneRoll, SomeOneMove, SomeOneGetEffect, SomeOneGetGoblrtStillMoving, WaitCameraMoving, EndOrder, SomeOneUsingItem,
        SomeOneWin, SomeOneDead, SpawnPlayer
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

    public ParticleSystem vfxWinOBJ;

    private List<PlayerControllerParchessi> listPlayerRepawn = new List<PlayerControllerParchessi>();

    public GameObject pfDice;
    private GameObject dice;
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
        CanvasManager.Instance.InitStatCanvas(listturnPlayOrder);

        lightMain = GetComponentInChildren<Light>();
        treaserChest = Instantiate(pfTreaserChest, transform);
        treaserChest.gameObject.SetActive(false);

        currentOrder = 0;
        //todo_test: set player 1st
        SetState(StateGameParchessi.BeforStartTurn);
        dice = Instantiate(pfDice, transform);
        dice.transform.GetChild(0).gameObject.SetActive(false);
    }

    public void ActiveDice(Vector3 pos)
    {
        if (!dice.transform.GetChild(0).gameObject.activeSelf)
        {
            dice.transform.GetChild(0).gameObject.SetActive(true);
            dice.GetComponentInChildren<Rigidbody>().isKinematic = true;
            dice.GetComponentInChildren<Animation>().Play();
            dice.transform.position = pos;
        }
    }
    public void FireDice()
    {
        dice.GetComponentInChildren<Animation>().Play("dice_fire");
        dice.GetComponentInChildren<Rigidbody>().isKinematic = false;
        dice.GetComponentInChildren<Rigidbody>().AddForce(Vector3.forward*500f,ForceMode.Force);

        Invoke(nameof(DelayHideDice),5f);
        void DelayHideDice()
        {
            //dice.transform.GetChild(0).gameObject.SetActive(false);
        }
    }
    private void Update()
    {
        if (RunDelayTime()) return;

        switch (state)
        {
            case StateGameParchessi.BeforStartTurn:
                {
                    SetCurrentPlayerTurn(0);
                    InventoryCanvasManager.Instance.LoadInventoryVisual(currentPlayerInTurn.GetComponent<InventoryController>().items);
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
                    if (!currentPlayerInTurn.CheckNumSaving())
                    {
                        CanvasManager.Instance.PostNoti(currentPlayerInTurn.name + "\' Turn", 3f);
                    }
                    CameraManager.Instance.FocusPlayer(currentPlayerInTurn.transform);
                    state = StateGameParchessi.SomeOneRoll;
                    SetDelayTime(3);
                }
                break;
            case StateGameParchessi.SomeOneRoll:
                {
                    if (currentPlayerInTurn.isBotController)
                    {
                        OnGameStateChangeTo?.Invoke(state);
                        state = StateGameParchessi.none;
                    }
                    else
                    {
                        if (!currentPlayerInTurn.CheckNumSaving(false))
                        {//todo: player and not have numsaving
                            currentPlayerInTurn.SetState(PlayerControllerParchessi.State.readyToRoll);
                            state = StateGameParchessi.none;

                        }
                    }
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
                        currentTurn += 1;
                    }

                    SetCurrentPlayerTurn(currentOrder);
                    InventoryCanvasManager.Instance.LoadInventoryVisual(currentPlayerInTurn.GetComponent<InventoryController>().items);
                    currentPlayerInTurn.GetComponent<InventoryController>().SetCanUseItem(true);

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
            case StateGameParchessi.SomeOneDead:
                {
                }
                break;
            case StateGameParchessi.SpawnPlayer:
                {
                    foreach (PlayerControllerParchessi pl in listPlayerRepawn)
                    {
                        Step spawnbase = StepManager.Instance.GetSpawnBaseAvalable();
                        pl.currentPositionStep.ReMoveCurrentPlayerToNextTo(pl);
                        pl.currentPositionStep = spawnbase;
                        pl.UpdateStat(PlayerStatController.UpdateStatType.hp, 100);
                        spawnbase.CheckAndMoveCurrentPlayerToNextTo(pl, true);
                    }
                    listPlayerRepawn.Clear();
                    state = StateGameParchessi.SomeOneRoll;
                }
                break;
            case StateGameParchessi.SomeOneWin:
                {
                    vfxWinOBJ.transform.position = currentPlayerInTurn.transform.position;
                    vfxWinOBJ.gameObject.SetActive(true);
                    state = StateGameParchessi.none;
                }
                break;
            case StateGameParchessi.SomeOneGetGoblrtStillMoving:
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
                    //state = StateGameParchessi.SomeOneMove;
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
    public StateGameParchessi GetState()
    {
        return state;
    }

    public void SpawnNewTeasureChest()
    {
        GetListStepCantUseByUser(currentPlayerInTurn.currentPositionStep, 0);
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


    public List<PlayerControllerParchessi> GetListCurrentPlayerTurn()
    {
        return listturnPlayOrder;
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
    public void WaitEndEffect(float effTime = 0, bool isGetGoblet = false, bool isStillMove = false)
    {
        //EndOrder();

        if (isGetGoblet)
        {
            isChestAlready = false;
            if (isStillMove)
            {
                state = StateGameParchessi.SomeOneGetGoblrtStillMoving;
                StartCoroutine(SpawnChestDelay());
                return;
            }
            //else
            //{
            //    StartCoroutine(SpawnChestDelay());
            //    return;
            //}
            IEnumerator SpawnChestDelay()
            {
                yield return new WaitForSeconds(effTime);
                state = StateGameParchessi.ChestSpawn;
            }
        }
        else
            state = StateGameParchessi.SomeOneGetEffect;
        StartCoroutine(End());

        IEnumerator End()
        {
            yield return new WaitForSeconds(effTime);

            state = StateGameParchessi.EndOrder;

        }
    }

    public void SetListPlayerSpawn(PlayerControllerParchessi pl)
    {
        if (!pl) listPlayerRepawn.Clear();
        else
        {
            if (!listPlayerRepawn.Find(x => x == pl))
            {
                SetState(StateGameParchessi.SomeOneDead);
                listPlayerRepawn.Add(pl);
            }
        }
    }


    //delay time custom
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
