using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//ステータス文管理
public class StatusText : MonoBehaviour
{
    //テキスト本体
    TextMeshProUGUI text;

    void Awake()
    {
        //コンポーネントを取得
        text = GetComponent<TextMeshProUGUI>();
        //空文字にする
        text.text = "";
    }

    //文字が送られてくる
    public void SetStatusText(string s)
    {
        //せっていする
        text.text = s;
        //文字を消す
        Invoke("ResetText", 3f);
    }

    //文字を消す
    void ResetText()
    {
        text.text = "";
    }
}
