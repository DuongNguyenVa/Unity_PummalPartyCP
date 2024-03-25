using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//[CustomEditor(typeof(Step))]
public class StepEditor
    : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        Step step = (Step)target;
       

        switch (step.stepType)
        {
            case Step.StepType.Single:
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("nextStep"));
                }
                break;
            case Step.StepType.Multi:
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("nextSteps"));
                }
                break;
            default:
                break;
        }
       


        serializedObject.ApplyModifiedProperties();
    }

    public void OnSceneGUI()
    {
        Step step = (Step)target;
        Handles.Label(step.transform.position,""+ step.name);
    }
}
