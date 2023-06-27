using UnityEngine;
using TMPro;

//右上にあるブロック数のテキストを値を受け取りながら変更するスクリプト
public class Block_Text : MonoBehaviour
{
    //目標テキストと現在のブロック数
    [SerializeField] TextMeshProUGUI Boxnum_Text, goalboxnum_Text;

    void Start()
    {
        //目標数は最初に設定する
        goalboxnum_Text.text = GameManager.I.Add_Blocknum_goal.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        //ブロック数を更新していく
        Boxnum_Text.text = GameManager.I.Add_Blocknum.ToString();
    }

}
