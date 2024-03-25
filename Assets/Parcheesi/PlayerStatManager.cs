using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStatManager : MonoBehaviour
{
    public static PlayerStatManager Instance;


    public TextMeshProUGUI textKey;
    public int maxHeal;
    public int maxKey;
    public TextMeshProUGUI textHp;
    public Transform goblets;

    private int keys;
    private int hp;
    private int gobletsCount;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        hp = maxHeal;
        keys = maxKey;
        gobletsCount = 0;
        UpdateKey();
        UpdateHp();
        //set goblet init
        for (int i = 0; i < goblets.childCount; i++)
        {
            //goblets.GetChild(i).GetComponent<Image>().color = Color.black;
        }
    }
    public void UpdateKey(int k = 0)
    {
        if (keys - k < 0)
            keys = 0;
        else keys += k;

        textKey.text = "" + keys;

        if (k < 0)
        {
        SpawnKeyEx.Istaince.SpawnIntKeysUsed(Mathf.Abs(k),PlayerControllerParchessi.Instance.transform);
        }
    }

    public void UpdateHp(int h = 0)
    {       
        textHp.text = "" + Mathf.Clamp(hp + h, 0, maxHeal);
    }
    public void UpdateGoblet(int gl=0)
    {
        gobletsCount += gl;
        for (int i = 0; i < gobletsCount; i++)
        {
            //goblets.GetChild(i).GetComponent<Image>().color = Color.white;
            AnimationSprite animationSprite = goblets.GetChild(i).GetComponentInChildren<AnimationSprite>();
            animationSprite.Func_PlayUIAnim(2, true);
            animationSprite.GetComponentInChildren<Image>().color=Color.white;
        }
    }
   
}
