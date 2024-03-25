using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DirectionArrowItem : MonoBehaviour, IPointerClickHandler
{
    public Step step;
    public Step stepAuthor;

    private List<Transform> arrowElse=new List<Transform>();
    private void Start()
    {
        Transform parent= transform.parent;
        for (int i = 0; i < parent.childCount; i++)
        {
            if(parent.GetChild(i).TryGetComponent(out DirectionArrowItem arrow))
                if(arrow!=this)               
                arrowElse.Add(parent.GetChild(i));
        }       
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        stepAuthor.nextStep = step;
        PlayerControllerParchessi.Instance.SetState(PlayerControllerParchessi.State.moving);
        stepAuthor.ShowDirectionArrow(false);
    }
    private void Update()
    {
       

    }
    public void ScaleSize(float s)
    {
        transform.localScale = s * Vector3.one;
        SetArrowElseScale();
    }
    private void SetArrowElseScale()
    {
        foreach (Transform item in arrowElse)
        {
            item.localScale =  Vector3.one;
        }
    }
}
