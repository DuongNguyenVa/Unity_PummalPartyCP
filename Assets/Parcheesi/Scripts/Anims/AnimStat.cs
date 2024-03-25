using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AnimationStat")]
public class AnimStat : ScriptableObject
{
    [System.Serializable]
    public class AnimationStart
    {
        public string animationType;
        public float time;

    }
    public AnimationStart[] Animations;
}
