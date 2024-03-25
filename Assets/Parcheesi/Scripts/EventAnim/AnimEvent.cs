using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEvent : MonoBehaviour
{
    [HideInInspector]
    public PlayerControllerParchessi playerTarget;
    [HideInInspector]
    public int hp;
    public virtual void Hit()
    {
        if (playerTarget)
        {
            playerTarget.GetAttacked();
        }
    }
}
