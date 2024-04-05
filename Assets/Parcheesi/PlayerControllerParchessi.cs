using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PlayerControllerParchessi : MonoBehaviour
{
    public enum State
    {
        idle, readyToRoll, moving, chosing, usingItem, attacked,
    }

    public Step[] steps;
    public float moveSpeed = 5f;
    public ParticleSystem vfxRun;

    public int numrockettest = 10;
    public int numTest = 0;

    public TextMeshPro diceResultText3D;

    private bool isDiceResultText3DAnim;

    private int num = 0;
    private int currentWaypointIndex = 0;
    private bool ismove = false;
    private Vector3 nextPosition;
    public Step currentPositionStep; //test
    private State state;
    private InventoryController inventoryController;

    private float timeAttacked = 3f;


    private Animator animator;

    public bool isBotController;

    private int numSaving;

    private PlayerStatController playerStat;
    private int bonusTurn;

    private void Start()
    {
        currentPositionStep = StepManager.Instance.stepsSpawn;
        inventoryController = GetComponentInChildren<InventoryController>();
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
                if (state == State.readyToRoll)
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

    public void DoRooll()
    {
        GameManagerParchessi.Instance.SetState(GameManagerParchessi.StateGameParchessi.none);
        InventoryCanvasManager.Instance.ShowUI(false);
        GetComponent<InventoryController>().SetCurrentItemChoses(null);

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

        if (num > 0 && !ismove)
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
                if (currentPositionStep.nextStep.TryGetComponent(out StepEffect sff))
                {
                    if (sff.effectType == StepEffect.EffectType.Goblet)
                    {
                        //get goblet in turn bonus 
                        if (bonusTurn > 0)
                        {
                            DoIdle();
                            DoBunusTurn();
                        }
                        else if (num != 1)
                        {
                            SetNumSaving();
                        }
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
                IdleAnim();
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
                DoIdle();
                if (bonusTurn > 0)
                {
                    DoBunusTurn();
                    return;
                }
                //endturn
                if (currentPositionStep.TryGetComponent(out StepEffect sEff))
                {
                    sEff.ActiveEffect(this);
                }
                //no effect
                if (!currentPositionStep.GetComponent<StepEffect>())
                    GameManagerParchessi.Instance.WaitEndEffect();
            }
        }

    }
    private void DoBunusTurn()
    {
        bonusTurn -= 1;
        GameManagerParchessi.Instance.SetState(GameManagerParchessi.StateGameParchessi.SomeOneRoll);
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
    public InventoryController GetInventoryController()
    {
        return GetComponent<InventoryController>();

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
    public bool CheckNumSaving(bool isCheck = true)
    {
        if (numSaving > 0)
        {
            if (!isCheck)
                DoRooll();
            return true;
        }
        return false;
    }  
    public void UpdateStat(PlayerStatController.UpdateStatType updateStatType, int value)
    {
        playerStat.UpdateStat(updateStatType, value);
    }

    private void DoIdle()
    {
        state = State.idle;
        num = 0;
        ismove = false;
        IdleAnim();
    }

    //anim
    private void Run()
    {
        animator.SetBool("isMove", true);
        if (!vfxRun.isPlaying)
            vfxRun.Play();
    }
    private void IdleAnim()
    {
        animator.SetBool("isMove", false);
        if (animator.GetBool("isUseRocket") && state == State.idle)
        {
            moveSpeed /= 3;
            animator.SetBool("isUseRocket", false);
            inventoryController.DisUseItemType(); //hide rocket
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

    public void UseRocket(int numBonus)
    {
        bonusTurn += 1;
        SetState(State.moving);
        animator.SetBool("isUseRocket", true);
        if (numrockettest != 0) num = numrockettest;
        else
            num = numBonus;
        moveSpeed *= 3;
        inventoryController.UseItemType(ItemSO.ItemType.rocket);
    }

    public void DoDance()
    {

        if (!animator.GetBool("doDance"))
            animator.SetTrigger("doDance");

    }
    public void Win()
    {
        animator.SetBool("isWin", true);
    }


    public void UseHealthItem(ParticleSystem vfx, float time)
    {
        inventoryController.UseItemType(ItemSO.ItemType.heal, time);
        ParticleSystem vfxObj = Instantiate(vfx, transform);
        var main = vfxObj.main;
        main.loop = false;
        vfxObj.Play();
        if (!animator.GetBool("doHeal"))
            animator.SetTrigger("doHeal");
    }
}
