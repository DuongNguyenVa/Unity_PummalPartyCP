using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaafiriAnimEvent : AnimEvent
{
    public ParticleSystem vfxHit;
   public override void Hit()
    {
        base.Hit();
        if (vfxHit)
        {
            vfxHit.Play();
            PlayerStatManager.Instance.UpdateHp(hp);
            PlayerStatManager.Instance.UpdateKey(hp);
        }
    }
}
