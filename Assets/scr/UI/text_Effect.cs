using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class text_Effect : MonoBehaviour
{
    [SerializeField] float speed = 0.05f;
    [SerializeField] int looptext_value=3;

    TextMeshProUGUI text;
    string textstr_end;
    char[] textstr;
    string random_str = "0123456789@!?#abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

    //再表示したときにもう一度再生させる
    //ただ、OnEnableはStartより早く呼ばれるためテキストが割り当てられるまで待つ
    void OnEnable()
    {
        Invoke("set_start", 0.05f);
    }

    void set_start()
    {
        text = GetComponent<TextMeshProUGUI>();
        Debug.Log(random_str[0]);
        textstr = new char[text.text.Length];
        textstr_end = text.text;
        StartCoroutine(text_E1());
    }

    IEnumerator text_E1()
    {
        yield return null;
        text.text = "";
        for(int i=0;i< textstr_end.Length;i++)
        {
            for(int j = 0; j < looptext_value; j++)
            {
                textstr[i] = random_str[Random.Range(0, random_str.Length)];
                yield return new WaitForSecondsRealtime(speed);
                Debug.Log(new string(textstr));
                text.text = new string(textstr);
            }
            yield return null;
            textstr[i] = textstr_end[i];
            text.text = new string(textstr);
        }
        Debug.Log(new string(textstr));
    }
}
