using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

//テキストの登場をちょっと加工する
public class text_Effect : MonoBehaviour
{
    //速さ
    [SerializeField] float speed = 0.05f;
    //何回フェイクの文字を出すか
    [SerializeField] int looptext_value = 3;

    //今回のテキスト
    TextMeshProUGUI text;
    //元々の文字
    string textstr_end;
    //徐々に加工する文字列
    char[] textstr;
    //フェイク文字一覧
    string random_str = "0123456789@!?#abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";


    private void Start()
    {
        //コンポーネントを取得
        text = GetComponent<TextMeshProUGUI>();
        //あらかじめ元の文字列を保持しておく
        textstr_end = text.text;
    }

    //再表示したときにもう一度再生させる
    //ただ、OnEnableはStartより早く呼ばれるためテキストが割り当てられるまで待つ
    void OnEnable()
    {
        if(text==null) text = GetComponent<TextMeshProUGUI>();
        //タイトルシーン以外なら更新
        if(!SceneManager.GetActiveScene().name.Equals("title")) textstr_end = text.text;
        Debug.Log(textstr_end);
        Invoke("set_start", 0.05f);
    }

    //演出開始
    void set_start()
    {
        StartCoroutine(text_E1());
    }

    //エフェクト一つ目
    IEnumerator text_E1()
    {
        //何にも無くする
        text.text = "";
        //文字配列を元の文字列の長さ用意する
        textstr = new char[textstr_end.Length];

        //文字の長さ回る
        for (int i = 0; i < textstr_end.Length; i++)
        {
            //フェイク分回る
            for (int j = 0; j < looptext_value; j++)
            {
                //文字列にフェイク文字列からランダムに選ばれた文字を入れる
                textstr[i] = random_str[Random.Range(0, random_str.Length)];
                //指定時間待つ
                yield return new WaitForSecondsRealtime(speed);
                //テキストに文字を表示する（適当な文字）
                text.text = new string(textstr);
            }
            //待つ
            yield return null;
            //本来の文字を代入する
            textstr[i] = textstr_end[i];
            //テキストに文字を表示する（本物）
            text.text = new string(textstr);
        }
    }
}
