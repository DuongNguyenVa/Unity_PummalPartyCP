using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PlayerControllerParchessi : MonoBehaviour
{
    public static PlayerControllerParchessi Instance;
    public enum State
    {
        idle, readyToRoll, moving, chosing, usingItem, attacked,
    }

    public Step[] steps;
    public float moveSpeed = 5f;
    public ParticleSystem vfxRun;
    public int numTest = 0;

    public TextMeshPro diceResultText3D;

    private bool isDiceResultText3DAnim;

    private int num = 0;
    private int currentWaypointIndex = 0;
    private bool ismove = false;
    private Vector3 nextPosition;
    public Step currentPositionStep; //test
    private State state;
    private InventoryManager inventoryManager;

    private float timeAttacked = 3f;


    private Animator animator;

    public bool isBotController;

    private int numSaving;

    private PlayerStatController playerStat;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        currentPositionStep = StepManager.Instance.stepsSpawn;
        inventoryManager = GetComponentInChildren<InventoryManager>();
        animator = GetComponentInChildren<Animator>();
        playerStat = GetComponent<PlayerStatController>();

        transform.position = currentPositionStep.transform.position;
        ///    steps = StepManager.Instance.steps;
        ///    
        //diceController.gameObject.SetActive(false);
        diceResultText3D.fontSize = 0;

    }

    //todo:delete

    IEnumerator DelaySetnum(int n)
    {
        yield return new WaitForSeconds(2);

        //num = Dice.GetDice();
        num = n;

    }
    void DiceResultText3DAnim()
    {
        if (diceResultText3D.fontSize <= 13)
            diceResultText3D.fontSize += Time.deltaTime * 15f;
        else
        {
            isDiceResultText3DAnim = false;
            diceResultText3D.fontSize = 0;
        }
    }

    //todo:delete
    private void Update()
    {

        if (!isBotController)
        {
            if (GameManagerParchessi.Instance.GetCurrentPlayerTurn() != this) return;

            else
            if (Input.GetKeyDown(KeyCode.Space))
            {                
                //todo:delete
                if (state==State.readyToRoll)
                {
                    state = State.idle;
                    DoRooll();
                }
            }
        }
        DoMove();

    }
    private void LateUpdate()
    {
        if (isDiceResultText3DAnim)
        {
            Vector3 lookPoint = 2 * diceResultText3D.transform.position - Camera.main.transform.position; //invert lok point

            diceResultText3D.transform.LookAt(lookPoint);
            DiceResultText3DAnim();
        }
    }
    public bool CheckNumSaving(bool isCheck=true)
    {
        if (numSaving > 0)
        {
            if (!isCheck)
                DoRooll();
            return true;
        }
        return false;
    }
    public void DoRooll()
    {
        GameManagerParchessi.Instance.SetState(GameManagerParchessi.StateGameParchessi.none);

        int numRandom;
        if (numSaving != 0)
        {
            numRandom = numSaving;
            numSaving = 0;
        }
        else
        {
            if (numTest != 0)            //test   static num                        
            {
                numRandom = numTest;
            }
            else
                numRandom = Dice.GetDice();

            diceResultText3D.text = numRandom + "";
            isDiceResultText3DAnim = true;
            //diceController.gameObject.SetActive(true);
            animator.SetTrigger("doDice");
        }

        StartCoroutine(DelaySetnum(numRandom));
        state = State.moving;
    }
    public void DoMove()
    {

        if (num != 0 && !ismove)
        {
            GetNextStep();
        }
        if (ismove)
        {
            //MoveToNextWaypoint(currentWaypointIndex);
            MoveToNexStep(nextPosition);
        }
    }

    private void MoveToNextWaypoint(int targetIndex)
    {

        if (Vector3.Distance(transform.position, steps[targetIndex].transform.position) >= 0.01f)
        {

            Vector3 targetPosition = steps[targetIndex].transform.position;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
        else
        {
            transform.position = steps[targetIndex].transform.position;
            ismove = false;
            num--;
        }

    }
    private void CheckMovePoint()
    {
        if (currentWaypointIndex + 1 <= steps.Length - 1)
        {
            currentWaypointIndex++;
        }

        else
        {
            currentWaypointIndex = 0;
        }

        ismove = true;
    }

    private void GetNextStep()
    {
        if (currentPositionStep.nextStep)
        {
            {
                nextPosition = currentPositionStep.nextStep.transform.position;
                ismove = true;

                if (num != 1 && currentPositionStep.nextStep.TryGetComponent(out StepEffect sff))
                {
                    if (sff.effectType == StepEffect.EffectType.Goblet)
                    {
                        SetNumSaving();
                    }
                }
            }
        }
        else
        {
            if (state == State.moving)
            {
                state = State.chosing;
                GameManagerParchessi.Instance.SetState(GameManagerParchessi.StateGameParchessi.SomeOneChosing);
                //currentPositionStep.ShowAllNextStep();

                //show diretionArrow if not Bot
                if (!isBotController)
                    currentPositionStep.ShowDirectionArrow(true);
                Idle();
            }
        }
    }
    private void MoveToNexStep(Vector3 nextStepPosition)
    {

        if (Vector3.Distance(transform.position, nextStepPosition) >= 0.01f)
        {
            Vector3 moveDir = nextStepPosition - transform.position;
            Quaternion q = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Lerp(transform.rotation, q, 0.1f);
            Run();
            Vector3 targetPosition = nextStepPosition;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
        else
        {
            //state = State.chosing;
            Step nextS = currentPositionStep.nextStep;
            if (currentPositionStep.stepType == Step.StepType.Multi)
            {
                currentPositionStep.RemoveNextStep();
            }
            currentPositionStep = nextS;
            transform.position = nextStepPosition;
            ismove = false;
            num--;

            if (num == 0)
            {
                //enturn

                if (currentPositionStep.TryGetComponent(out StepEffect sEff))
                {
                    sEff.ActiveEffect(this);
                }
                state = State.idle;
                Idle();

                if (currentPositionStep.TryGetComponent(out StepEffect seff))
                {
                    switch (seff.effectType)
                    {

                        case StepEffect.EffectType.Goblet:
                            {
                                if (numSaving != 0)
                                {
                                    GameManagerParchessi.Instance.WaitEndEffect(timeAttacked, true, true);
                                    Debug.Log("Got goblet but still move");
                                }
                                else
                                {
                                    GameManagerParchessi.Instance.WaitEndEffect(timeAttacked, true);
                                }
                            }
                            break;
                        default:
                            GameManagerParchessi.Instance.WaitEndEffect(timeAttacked);
                            break;
                    }

                }
                else
                    GameManagerParchessi.Instance.WaitEndEffect();


                //GameManagerParchessi.Instance.SetState(GameManagerParchessi.StateGameParchessi.NextOrder);

            }
        }

    }

    public void SetState(State st)
    {
        state = st;
    }
    public State GetState()
    {
        return state;
    }
    public void GetAttacked()
    {
        Attacked();
    }
    public InventoryManager GetInventoryManager()
    {
        return GetComponent<InventoryManager>();

    }
    public void FaceToST(Vector3 tartgetPOS)
    {
        Vector3 newTargetPOS = new Vector3(tartgetPOS.x, transform.position.y, tartgetPOS.z);
        transform.LookAt(newTargetPOS);
    }
    private void SetNumSaving()
    {
        numSaving = num - 1;
        num = 1;
    }
    public void UpdateStat(PlayerStatController.UpdateStatType updateStatType, int value)
    {

        playerStat.UpdateStat(updateStatType, value);

    }

    //anim
    private void Run()
    {
        animator.SetBool("isMove", true);
        if (!vfxRun.isPlaying)
            vfxRun.Play();
    }
    private void Idle()
    {
        animator.SetBool("isMove", false);
        if (animator.GetBool("isUseRocket") && state == State.idle)
        {
            moveSpeed /= 3;
            animator.SetBool("isUseRocket", false);
            inventoryManager.DisUseItemType();
        }
        if (vfxRun.isPlaying)
            vfxRun.Stop();
    }
    private void Attacked()
    {
        SetState(State.attacked);
        if (!animator.GetBool("doDeath"))
            animator.SetTrigger("doDeath");
        Invoke(nameof(Revival), timeAttacked + 2);
    }
    private void Revival()
    {
        SetState(State.idle);
        if (!animator.GetBool("doRevival"))
            animator.SetTrigger("doRevival");
    }

    public void UseRocket()
    {
        SetState(State.moving);
        animator.SetBool("isUseRocket", true);
        num = 10;
        moveSpeed *= 3;
        inventoryManager.UseItemType(ItemSO.ItemType.rocket);
    }

    public void DoDance()
    {

        if (!animator.GetBool("doDance"))
            animator.SetTrigger("doDance");

    }

}
