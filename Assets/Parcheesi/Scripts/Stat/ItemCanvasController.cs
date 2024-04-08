using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemCanvasController : MonoBehaviour
{
    public Transform nameText;
    public TextMeshProUGUI keyText;
    public TextMeshProUGUI hpText;
    public Slider hpSlider;
    public Transform gobs;
    public void UpdateUI(int keys, int hp, float hpSlideVL = -1, int gobsindex=-1)
    {
        keyText.text = "" + keys;
        hpText.text = "" + hp;
        if (hpSlideVL == -1)
            hpSlider.value = hpSlider.maxValue- hpSlider.maxValue;
        else
            hpSlider.value = hpSlider.maxValue - Mathf.Abs(hpSlideVL);
        UpdateGoblet(gobsindex);
    }
    private void UpdateGoblet(int glcount = -1)
    {
        if (glcount >= 0)
        {
            for (int i = 0; i < gobs.childCount; i++)
            {
                if (i == glcount)
                {
                    //goblets.GetChild(i).GetComponent<Image>().color = Color.white;
                    AnimationSprite animationSprite = gobs.GetChild(i).GetComponentInChildren<AnimationSprite>();
                    animationSprite.Func_PlayUIAnim(2, true);
                    animationSprite.GetComponentInChildren<Image>().color = Color.white;
                }
            }
        }
    }
}
