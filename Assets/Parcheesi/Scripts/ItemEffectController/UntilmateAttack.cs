using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UntilmateAttack : MonoBehaviour
{
   public List<PlayerControllerParchessi> listPlayerTrget=new List<PlayerControllerParchessi>();
    public ParticleSystem vfx;
    private void OnTriggerEnter(Collider other)
    {    
        if (other.TryGetComponent(out PlayerControllerParchessi pl))
        {
            if(pl != GameManagerParchessi.Instance.GetCurrentPlayerTurn()){
                listPlayerTrget.Add(pl);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out PlayerControllerParchessi pl))
        {
            if (pl != GameManagerParchessi.Instance.GetCurrentPlayerTurn())
            {
                listPlayerTrget.Remove(pl);
            }
        }
    }
    private void AnimEvent_Fire()
    {

    }
}
