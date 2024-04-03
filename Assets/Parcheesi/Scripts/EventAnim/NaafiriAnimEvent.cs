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
            GameManagerParchessi.Instance.GetCurrentPlayerTurn().UpdateStat(PlayerStatController.UpdateStatType.hp, hp);
            GameManagerParchessi.Instance.GetCurrentPlayerTurn().UpdateStat(PlayerStatController.UpdateStatType.key, hp);
        }
    }
}
