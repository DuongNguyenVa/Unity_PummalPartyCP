using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerParchessi : MonoBehaviour
{
    public static GameManagerParchessi Instance;
    public enum StateGameParchessi
    {
        StartTurn, WaitSomeoneEnd, NextOrder, EndTurn, DarkSpace, LightSpace, SomeOneChosing, SomeOneGetEffect

    }
    private StateGameParchessi state;



    public Step pfTreaserChest;
    private Step treaserChest;

    private Light lightMain;


    public List<Step> stepsCanSpawnChest = new List<Step>();
    private DirectionArrowItem currentArrowTarget;

    private int currentTurn;
    private int currentOrder;
    private PlayerControllerParchessi currentPlayerInTurn;

    public List<PlayerControllerParchessi> listturnPlayOrder = new List<PlayerControllerParchessi>();
    public Transform parentAllPlayer;

    public AnimStat animStat;
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

        lightMain = GetComponentInChildren<Light>();
        treaserChest = Instantiate(pfTreaserChest, transform);
        treaserChest.gameObject.SetActive(false);

        ResetListStepCanSpawnChest();

        currentOrder = 0;
        SetState(StateGameParchessi.StartTurn);
    }

    private void Update()
    {
        switch (state)
        {
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
                    currentOrder += 1;

                    if (currentOrder == listturnPlayOrder.Count)
                    {
                        EndOrder();
                        listturnPlayOrder.Clear();
                        state = StateGameParchessi.EndTurn;
                    }

                    else
                    {
                        SetCurrentPlayerTurn(currentOrder);
                        state = StateGameParchessi.WaitSomeoneEnd;
                    }
                }
                break;

            case StateGameParchessi.SomeOneGetEffect:
                {
                  
                }
                break;
            case StateGameParchessi.EndTurn:
                {

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
        StepManager.Instance.ChangeStepToTreaserChestStep(stepsCanSpawnChest[Random.Range(0, stepsCanSpawnChest.Count)]);
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
    public void WaitEndEffect(float effTime)
    {
        Debug.Log(effTime);
        EndOrder();
        state=StateGameParchessi.SomeOneGetEffect;
        StartCoroutine(NextOrder());
        IEnumerator NextOrder()
        {
            yield  return new WaitForSeconds(3f);
            state = StateGameParchessi.NextOrder;

        }
    }
}
