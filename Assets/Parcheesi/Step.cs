using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Step : MonoBehaviour
{

    public enum StepType
    {
        Single,
        Multi
    }
    
    public StepType stepType;

    public Step nextStep;
    public Step[] nextSteps;
    public DirectionArrowItem pfDirectionArrow;
    private List<DirectionArrowItem> listDirectionArrow=new List<DirectionArrowItem>();
   
    public class StepTypeInfor
    {
   
    };
    private void Start()
    {
        if (stepType==StepType.Multi)
        {
            foreach (Step item in nextSteps)
            {
                DirectionArrowItem directionArrowItem= Instantiate(pfDirectionArrow, transform);
                directionArrowItem.step = item;
                directionArrowItem.stepAuthor = this;
                directionArrowItem.transform.localRotation = Quaternion.LookRotation(-item.transform.position + transform.position);
                directionArrowItem.transform.position= (item.transform.position + transform.position)/ 2;
                directionArrowItem.gameObject.SetActive(false);
                listDirectionArrow.Add(directionArrowItem);
            }
        }
    }
    private void OnDrawGizmos()
    {
        //Step step = this;
        //UnityEditor.Handles.Label(step.transform.position, "" + step.id);
        //Step[] allstep = Resources.FindObjectsOfTypeAll(typeof(Step)) as Step[];
        //allstep.ToList();

        //if (stepType == StepType.Single)
        //{
        //    Step nexSt;
        //    if (this.id + 1 != allstep.Length)
        //    {
        //        nexSt = allstep.First<Step>(x => x.id == this.id + 1);
        //    }
        //    else
        //    {
        //        nexSt = allstep.First<Step>(x => x.id == 0);
        //    }
        //    SetNextStep(nexSt);
        //    UnityEditor.Handles.DrawLine(step.transform.position, nexSt.transform.position);

        //}
        //else
        //{
        //    foreach (Step s in nextSteps)
        //    {

        //        UnityEditor.Handles.DrawLine(step.transform.position, s.transform.position);

        //    }
        //}
        if (nextStep)
            UnityEditor.Handles.DrawLine(transform.position, nextStep.transform.position);
        else
        {
            foreach (Step s in nextSteps)
            {
                UnityEditor.Handles.DrawLine(transform.position, s.transform.position, 10f);

            }
        }

    }
    private void SetNextStep(Step st)
    {
        nextStep = st;
    }
   
    public void RemoveNextStep()
    {
        nextStep = null;
    }
    public void ShowDirectionArrow(bool isShow)
    {
        Debug.Log("show");
        foreach (var item in listDirectionArrow)
        {
            item.gameObject.SetActive(isShow);
        }
    }
  
}
