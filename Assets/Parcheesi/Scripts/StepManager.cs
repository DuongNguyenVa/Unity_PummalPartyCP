using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StepManager : MonoBehaviour
{
    public static StepManager Instance;
    public List<Step> spawnBases = new List<Step>();


    public event Action<Step> OnChestSpawn;

    public List<Step> stepsCanSpawnChest = new List<Step>();

    public Step stepsSpawn;

    public GameObject pfTreaserChestVisual;
    private GameObject treaserChestVisual;

    private Step stepIsVisualForChest;
    private StepEffect.EffectType stepEffectType;

    private Step stepForNewChest;
    private Transform spawnParent;
    private List<Step> allStepOnmap = new List<Step>();
    [SerializeField] private int minimumStepsToChest=9; 
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        allStepOnmap= GetComponentsInChildren<Step>().ToList();
        treaserChestVisual = Instantiate(pfTreaserChestVisual, transform);
        treaserChestVisual.SetActive(false);
        CameraManager.Instance.SetTargetForEventCam(treaserChestVisual.transform);
        spawnParent = transform.Find("Spawns");
        foreach (Step spB in spawnParent.GetComponentsInChildren<Step>())
        {
            spawnBases.Add(spB);
        }
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
        //GameManagerParchessi.Instance.SetState(GameManagerParchessi.StateGameParchessi.LightSpace);
        //Invoke(nameof(SpawnNewTeasureChest), treaserChestVisual.GetComponentInChildren<Animation>().GetClip("chestTakeOff").length + 2);

    }
    void ChestLanding()
    {
        treaserChestVisual.GetComponentInChildren<Animation>().Play("chestLanding");
        //Invoke(nameof(AfterChestLanding), treaserChestVisual.GetComponentInChildren<Animation>().GetClip("chestLanding").length + 2);

    }
    void AfterChestLanding()
    {
        //CameraManager.Instance.ToogleChestView();
        //CameraManager.Instance.FocusPlayer();
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
        CameraManager.Instance.FocusEvent();

        //GetListStepCantUseByUser(GameManagerParchessi.Instance.GetCurrentPlayerTurn().currentPositionStep, 0);
        List<Step> listStepsPlayerStanding = new List<Step>();
        foreach (PlayerControllerParchessi st in GameManagerParchessi.Instance.GetListCurrentPlayerTurn())
        {
            if (!listStepsPlayerStanding.Contains(st.currentPositionStep))
            {
                listStepsPlayerStanding.Add(st.currentPositionStep);
            }
        }
        GetListStepCantUseByUser(listStepsPlayerStanding, minimumStepsToChest);

        stepForNewChest = stepsCanSpawnChest[UnityEngine.Random.Range(0, stepsCanSpawnChest.Count)];
        OnChestSpawn?.Invoke(stepForNewChest);
        //todo; reduce first step
        //rightWay.Remove(rightWay[0]);
        ChangeStepToTreaserChestStep(stepForNewChest);
    }

    void GetListStepCantUseByUser(List<Step> listStepPlayerUsing, int minimumStepsToChest = 0)
    {
        ResetListStepCanSpawnChest();
        foreach (Step st in listStepPlayerUsing)
        {
            RemoveStepsCantSpawnChest(st, minimumStepsToChest + 1);
        }
    }
    void RemoveStepsCantSpawnChest(Step st, int countStep)
    {   //remove step player is standing + next steps until countStep=0
        if (countStep <= 0) return;
        if(stepsCanSpawnChest.Contains(st)) stepsCanSpawnChest.Remove(st);
        if (st.stepType == Step.StepType.Multi)
        {
            foreach (Step step in st.nextSteps)
            {
                RemoveStepsCantSpawnChest(step, countStep - 1);
            }
        }
        else
            RemoveStepsCantSpawnChest(st.nextStep, countStep - 1);
    }
    void ResetListStepCanSpawnChest()
    {
        stepsCanSpawnChest.Clear();
        foreach (Step st in allStepOnmap)
        {
            if (st.TryGetComponent(out StepEffect sEff))
            {
                if (sEff.effectType != StepEffect.EffectType.Goblet&& sEff.effectType != StepEffect.EffectType.SpawnBase) //not use gob or spawnbase
                    stepsCanSpawnChest.Add(st);
            }
            else
                stepsCanSpawnChest.Add(st);

        }
       
    }

    public Step GetSpawnBaseAvalable()
    {
        Step st = spawnBases[0];
        return st;
    }

}