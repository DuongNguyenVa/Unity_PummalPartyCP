using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAnimEvent:MonoBehaviour
{
    public void CheckCanUseThisItem()
    {
        GameManagerParchessi.Instance.GetCurrentPlayerTurn().GetInventoryController().SetCanUseItem(true);
    }
}
