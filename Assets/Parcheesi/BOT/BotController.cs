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



    private void Instance_OnGameStateChange(GameManagerParchessi.StateGameParchessi obj)
    {
        if (GameManagerParchessi.Instance.GetCurrentPlayerTurn() != player) return;

        switch (obj)
        {
            case GameManagerParchessi.StateGameParchessi.BeforStartTurn:
                break;
            case GameManagerParchessi.StateGameParchessi.ChestSpawn:
                break;
            case GameManagerParchessi.StateGameParchessi.StartTurn:
                break;
            case GameManagerParchessi.StateGameParchessi.WaitSomeoneEnd:
                break;
            case GameManagerParchessi.StateGameParchessi.NextOrder:
                break;
            case GameManagerParchessi.StateGameParchessi.EndTurn:
                break;
            case GameManagerParchessi.StateGameParchessi.DarkSpace:
                break;
            case GameManagerParchessi.StateGameParchessi.LightSpace:
                break;
            case GameManagerParchessi.StateGameParchessi.SomeOneChosing:
                botState = State.Choosing;
                break;
            case GameManagerParchessi.StateGameParchessi.SomeOneRoll:
                botState = State.Roll;
                break;
            case GameManagerParchessi.StateGameParchessi.SomeOneMove:
                break;
            case GameManagerParchessi.StateGameParchessi.SomeOneGetEffect:
                break;
            case GameManagerParchessi.StateGameParchessi.WaitCameraMoving:
                break;
            default:
                break;
        }
    }

    private void Start()
    {
        navPath = GetComponent<NavPath>();
        botState = State.Idle;

        GameManagerParchessi.Instance.OnGameStateChangeTo += Instance_OnGameStateChange;

    }
    private void Update()
    {
        switch (botState)
        {
            case State.Idle:
                break;
            case State.UseItem:
                break;
            case State.Roll:
                {
                    player.DoRooll();
                    botState = State.Idle;
                }
                break;
            case State.Move:
                {
                    player.DoMove();
                    botState = State.Idle;
                }
                break;
            case State.Choosing:
                {
                    ChoosingNextWay();
                    botState = State.Idle;                  
                }
                break;
            case State.GotGoblet:
                break;
            case State.Attacked:
                break;
            default:
                break;
        }
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
