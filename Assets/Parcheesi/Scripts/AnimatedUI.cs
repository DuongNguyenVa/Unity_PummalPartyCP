using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class AnimatedUI : MonoBehaviour
{
    public static AnimatedUI Instaince;
    private void Awake()
    {
        Instaince = this;
    }
    public class Ef1
    {
        public int v1;
        public GameObject g1;
    }
    public void ShowHideAffter(GameObject obj, float t, Sprite sp)
    {
        obj.transform.Find("ItemVisualImage").GetComponent<Image>().sprite = sp;
        obj.SetActive(true);
        obj.transform.Find("imgbackground").GetComponent<AnimationSprite>().Func_PlayUIAnim(t);

        StartCoroutine(HideItemNotif());
        IEnumerator HideItemNotif()
        {
            yield return new WaitForSeconds(t);
            obj.transform.Find("imgbackground").GetComponent<AnimationSprite>().Func_StopUIAnim();
            obj.SetActive(false);

        }
    }
}
