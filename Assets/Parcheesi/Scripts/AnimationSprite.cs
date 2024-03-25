using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationSprite : MonoBehaviour
{
    public Image m_Image;
    public Sprite[] m_SpriteArray;
    public float m_Speed = .02f;
    private int m_IndexSprite;
    Coroutine m_CorotineAnim;
    bool IsDone;
    float sp;
    public void Func_PlayUIAnim(float t, bool isOnce = false)
    {
        sp = t / m_SpriteArray.Length;
        IsDone = false;

        if (isOnce)
            m_CorotineAnim = StartCoroutine(Func_PlayAnimUI_Once());
        else
            m_CorotineAnim = StartCoroutine(Func_PlayAnimUI());
    }
   
    public void Func_StopUIAnim()
    {
        IsDone = true;
        StopCoroutine(m_CorotineAnim);
    }
    IEnumerator Func_PlayAnimUI()
    {
      
        yield return new WaitForSeconds(sp);
        if (m_IndexSprite >= m_SpriteArray.Length)
        {
            m_IndexSprite = 0;
        }
        m_Image.sprite = m_SpriteArray[m_IndexSprite];
        m_IndexSprite += 1;
        if (IsDone == false)
            m_CorotineAnim = StartCoroutine(Func_PlayAnimUI());
    }
    IEnumerator Func_PlayAnimUI_Once()
    {
        if (m_IndexSprite >= m_SpriteArray.Length)
        {
            IsDone = true;
        }
        else
        {
            yield return new WaitForSeconds(sp);
            m_Image.sprite = m_SpriteArray[m_IndexSprite];
            m_IndexSprite += 1;
            if (IsDone == false)
                m_CorotineAnim = StartCoroutine(Func_PlayAnimUI_Once());
        }
    }
}
