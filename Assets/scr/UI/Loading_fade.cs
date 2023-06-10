using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Loading_fade : MonoBehaviour
{
    private float fade;//0-1でフェイドインやアウトを制御
    [SerializeField] bool fade_move;//動作中か
    RectTransform rectTransform;
    float top, bottom, left, right;
    // Start is called before the first frame update
    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        top = -5f;
        bottom = -5f;
        left = -5f;
        right = -5f;
        rectTransform.offsetMin = new Vector2(left, bottom);
        rectTransform.offsetMax = new Vector2(-right, -top);
    }

    public bool Fade_move
    {
        get { return fade_move; }
        set { fade_move = value; }
    }

    public void Open()
    {
        top = -5f;
        bottom = -5f;
        left = -5f;
        
        if(Screen.width<1920) right = 1930;
        else right = Screen.width;
        rectTransform.offsetMin = new Vector2(left, bottom);
        rectTransform.offsetMax = new Vector2(-right, -top);
    }
    public void Close()
    {
        top = -5f;
        bottom = -5f;
        left = -5f;
        right = -5;
        rectTransform.offsetMin = new Vector2(left, bottom);
        rectTransform.offsetMax = new Vector2(-right, -top);
    }

    public void Fadein()
    {
        Debug.Log("フェードイン");
        Close();
        StartCoroutine("fadein_move");
    }
    //実際にfadeoutを動かしているのはコチラ
    IEnumerator fadein_move()
    {
        fade = 0f;
        Fade_move = true;
        float width = Screen.width;
        if (width <= 1920) width = 1930;
        while (fade <= 1)
        {
            right = fade * width - 5;
            rectTransform.offsetMax = new Vector2(-right, -top);
            fade += 0.025f;
            yield return new WaitForSecondsRealtime(0.01f);
        }
        yield return new WaitForSecondsRealtime(0.25f);
        Open();
        yield return null;
        Debug.Log("width:" + width + ",right:" + rectTransform.offsetMax);
        Fade_move = false;
    }

    public void Fadeout()
    {
        Debug.Log("フェードアウト");
        StartCoroutine("fadeout_move");
    }
    //実際にfadeoutを動かしているのはコチラ
    IEnumerator fadeout_move()
    {
        fade = 1f;//1にする
        Fade_move= true;
        float width = Screen.width;
        if (width <= 1920) width = 1930;
        while (fade >= 0)
        {
            right = fade * width - 5;//右端からの大きさを決定
            rectTransform.offsetMax = new Vector2(-right, -top);//逐一代入
            fade -= 0.025f;//段階を進める
            yield return new WaitForSecondsRealtime(0.01f);
        }
        yield return new WaitForSecondsRealtime(0.25f);
        Close();
        yield return null;
        Debug.Log("width:"+width+",right:"+ rectTransform.offsetMax);
        Fade_move = false;
    }
}
