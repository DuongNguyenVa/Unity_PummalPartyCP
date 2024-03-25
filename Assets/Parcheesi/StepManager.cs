using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class StepManager : MonoBehaviour
{
    public static StepManager Instance;

    public event Action<Step> OnChestSpawn;

    public List<Step> stepsCanSpawnChest = new List<Step>();
    
    public Step stepsSpawn;

    public GameObject pfTreaserChestVisual;
    private GameObject treaserChestVisual;

    private Step stepIsVisualForChest;
    private StepEffect.EffectType stepEffectType;

    Step stepForNewChest;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        treaserChestVisual = Instantiate(pfTreaserChestVisual, transform);
        treaserChestVisual.SetActive(false);
        CameraMovement.Instance.SetTagetForCamChest(treaserChestVisual.transform);
        SpawnNewTeasureChest();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            SpawnNewTeasureChest();
        }
    }
    public void ChangeStepToTreaserChestStep(Step st)
    {
        RestoreStepVisualForChest();


        stepIsVisualForChest = st;
        treaserChestVisual.transform.position = st.transform.position;

        if (st.gameObject.TryGetComponent(out StepEffect sEff))
        {
            stepEffectType = sEff.effectType;

            sEff.effectType = StepEffect.EffectType.Goblet;
        }
        else
        {
            StepEffect se = st.gameObject.AddComponent<StepEffect>();
            se.effectType = StepEffect.EffectType.Goblet;
            stepEffectType = StepEffect.EffectType.none;           
        }

        for (int i = 0; i < st.transform.childCount; i++)
        {
            st.transform.GetChild(i).gameObject.SetActive(false);
        }
        treaserChestVisual.SetActive(true);
        ChestLanding();
    }
    public void AsSomeGetGoblet()
    {
        treaserChestVisual.GetComponentInChildren<Animation>().Play("chestOpen");
        Invoke(nameof(ChestTakeOff), treaserChestVisual.GetComponentInChildren<Animation>().GetClip("chestOpen").length + 1);

    }
    void ChestTakeOff()
    {
        treaserChestVisual.GetComponentInChildren<Animation>().Play("chestTakeOff");
        GameManagerParchessi.Instance.SetState(GameManagerParchessi.StateGameParchessi.LightSpace);
        Invoke(nameof(SpawnNewTeasureChest), treaserChestVisual.GetComponentInChildren<Animation>().GetClip("chestTakeOff").length + 2);

    }
    void ChestLanding()
    {
        treaserChestVisual.GetComponentInChildren<Animation>().Play("chestLanding");
        Invoke(nameof(AfterChestLanding), treaserChestVisual.GetComponentInChildren<Animation>().GetClip("chestLanding").length + 2);

    }
    void AfterChestLanding()
    {
        CameraMovement.Instance.ToogleChestView();
    }
    private void RestoreStepVisualForChest()
    {
        if (stepIsVisualForChest)
        {
            for (int i = 0; i < stepIsVisualForChest.transform.childCount; i++)
            {
                Transform child = stepIsVisualForChest.transform.GetChild(i);
                if (!child.GetComponent<DirectionArrowItem>())
                    child.gameObject.SetActive(true);
            }
            StepEffect seff = stepIsVisualForChest.GetComponent<StepEffect>();
            if (stepEffectType != StepEffect.EffectType.none)
            {
                seff.effectType = stepEffectType;
                stepEffectType = StepEffect.EffectType.none;
                //seff.SetParams(stepEffectIsVisualForChest.updateKey, stepEffectIsVisualForChest.updateHp, stepEffectIsVisualForChest.attacked);
            }
            else
            {
                Destroy(seff);
            }

        }

    }


    public void SpawnNewTeasureChest()
    {
        CameraMovement.Instance.ToogleChestView();
        GetListStepCantUseByUser(PlayerControllerParchessi.Instance.currentPositionStep, 0);
        stepForNewChest = stepsCanSpawnChest[UnityEngine.Random.Range(0, stepsCanSpawnChest.Count)];
        OnChestSpawn?.Invoke(stepForNewChest);
        //todo; reduce first step
        //rightWay.Remove(rightWay[0]);
        ChangeStepToTreaserChestStep(stepForNewChest);
    }

    void GetListStepCantUseByUser(Step st, int countStep = 0)
    {
        ResetListStepCanSpawnChest();
        RemoveStepsCantSpawnChest(st, countStep + 1);
    }
    void RemoveStepsCantSpawnChest(Step st, int countStep)
    {
        if (countStep <= 0) return;
        Step ns = st;
        if (ns.stepType == Step.StepType.Multi)
        {
            foreach (Step step in ns.nextSteps)
            {
                RemoveStepsCantSpawnChest(step, countStep - 1);
            }
        }
        else
            RemoveStepsCantSpawnChest(ns.nextStep, countStep - 1);
        stepsCanSpawnChest.Remove(ns);
    }
    void ResetListStepCanSpawnChest()
    {
        stepsCanSpawnChest.Clear();
        foreach (Step st in GetComponentsInChildren<Step>())
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

}