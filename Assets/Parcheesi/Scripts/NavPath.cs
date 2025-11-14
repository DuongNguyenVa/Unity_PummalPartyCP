using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavPath : MonoBehaviour
{
    PlayerControllerParchessi player;
    [System.Serializable]
    public class ListStepToChest
    {
        public List<Step> liststep;
    }
    public List<ListStepToChest> listAllShortWayToChest = new List<ListStepToChest>();
    private List<Step> rightWay = new List<Step>();
    private void Start()
    {
        StepManager.Instance.OnChestSpawn += Instance_OnChestSpawn;
        player = GetComponent<PlayerControllerParchessi>();
    }

    private void Instance_OnChestSpawn(Step obj)
    {
        rightWay = ChoosingRightWay(player.currentPositionStep, obj);
    }

    public List<Step> ChoosingRightWay(Step startStep, Step targetStep)
    {
        SetListAllShortWayToChest(startStep, targetStep);
        if (listAllShortWayToChest.Count > 0)
        {
            List<Step> rightWay = listAllShortWayToChest[0].liststep;
            foreach (ListStepToChest item in listAllShortWayToChest)
            {
                if (item.liststep.Count <= rightWay.Count)
                {
                    rightWay = item.liststep;
                }
            }
            return rightWay;
        }
        return null;
    }
    private void SetListAllShortWayToChest(Step startStep, Step targetStep)
    {
        listAllShortWayToChest.Clear();
        List<Step> listStepInWay = new List<Step>();
        AddST(startStep, listStepInWay);
        void AddST(Step startStep, List<Step> listStepInUse)
        {
            if (startStep == targetStep)
            {
                listStepInUse.Add(targetStep);
                listAllShortWayToChest.Add(new ListStepToChest() { liststep = listStepInUse });
                return;
            }
            if (listStepInUse.Find(x => x == startStep))
            {
                return;
            }
            if (startStep.stepType == Step.StepType.Single)
            {

                listStepInUse.Add(startStep);
                AddST(startStep.nextStep, listStepInUse);
            }
            else
            {
                listStepInUse.Add(startStep);
                foreach (Step st in startStep.nextSteps)
                {
                    List<Step> listStepInWayMore = new List<Step>();
                    listStepInWayMore.AddRange(listStepInUse);
                    AddST(st, listStepInWayMore);
                }
            }
        }
    }

    public List<Step> GetRightWay()
    {
        return rightWay;
    }
    public void UpdateRightWay(Step st)
    {
        rightWay.Remove(st);
    }

}
