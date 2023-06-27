using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class EndingUI : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] TextMeshProUGUI credittext;

    [Header("切り替わり時間(s)")]
    [SerializeField] float time;

    [Header("画像とクレジット表示は同時なのでその順番重要\n(とにかく配列数は合わせてください)")]

    [SerializeField] Sprite[] sprites;

    [SerializeField][Multiline] string[] creditlist;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(credit_move());
    }

    IEnumerator credit_move()
    {
        int i = 0;
        while(i < creditlist.Length)
        {
            credittext.text = creditlist[i];
            image.sprite =sprites[i];
            yield return new WaitForSecondsRealtime(time);
            i++;
        }

        //タイトルへ
        if (i >= creditlist.Length)
        {
            while (true)
            {
                if (Input.anyKey)
                {
                    SceneManager.LoadScene("title");
                    yield return null;
                }
                yield return null;
            }
        }
    }
}
