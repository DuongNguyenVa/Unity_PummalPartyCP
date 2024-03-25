using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotController : MonoBehaviour
{
    public PlayerControllerParchessi player;
    public enum State
    {
        Idle, UseItem, Roll, Move, Choosing, GotGoblet, Attacked
    }
    private State botState;

    private NavPath navPath;
    private void Start()
    {
        navPath = GetComponent<NavPath>();
    }
    private void Update()
    {
         if (GameManagerParchessi.Instance.GetCurrentPlayerTurn() != player) return;
        if (player.isBotController)
        {
            GetState();
            switch (botState)
            {
                case State.Idle:
                    player.DoRooll();
                    break;
                case State.UseItem:
                    break;
                case State.Roll:
                    break;
                case State.Move:
                    player.DoMove();
                    break;
                case State.Choosing:
                    ChoosingNextWay();
                    break;
                case State.GotGoblet:
                    break;
                case State.Attacked:
                    break;
                default:
                    break;
            }
        }
    }
    private void GetState()
    {
        switch (player.GetState())
        {
            case PlayerControllerParchessi.State.idle:
                {
                    SetState(State.Idle);
                }
                break;
            case PlayerControllerParchessi.State.moving:
                SetState(State.Move);
                break;
            case PlayerControllerParchessi.State.chosing:
                SetState(State.Choosing);
                break;
            case PlayerControllerParchessi.State.usingItem:
                break;
            case PlayerControllerParchessi.State.dead:
                break;
            default:
                break;
        }
    }
    private void SetState(State st)
    {
        botState = st;
    }
    private void ChoosingNextWay()
    {
        List<Step> listStepCanUSe = GetComponent<NavPath>().GetRightWay();

        for (int i = 0; i < listStepCanUSe.Count; i++)
        {
            if (listStepCanUSe[i] == player.currentPositionStep)
            {
                //todo: check CurrentStep(type=multi), set it's nextStep is next index in RightWay[List]
                player.currentPositionStep.nextStep = listStepCanUSe[i + 1];
                player.SetState(PlayerControllerParchessi.State.moving);
                break;
            }
        }

    }
}
