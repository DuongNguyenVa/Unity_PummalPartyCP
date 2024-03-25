using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(StepEffect))]
public class StepEffectEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        StepEffect stepEffect=(StepEffect)target;

        EditorGUILayout.PropertyField(serializedObject.FindProperty("effectType"));

        switch (stepEffect.effectType)
        {
            case StepEffect.EffectType.UpdateKey:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("updateKey"));
                break;
            case StepEffect.EffectType.UpdateHp:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("updateHp"));
                break;
            case StepEffect.EffectType.Attacked:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("attacked"));
                break;
            default:
                break;
        }

        serializedObject.ApplyModifiedProperties();
    }
}
