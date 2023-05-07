using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//右上にあるブロック数のテキストを値を受け取りながら変更するスクリプト
public class Block_Text : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI Boxnum_Text,goalboxnum_Text;
    // Start is called before the first frame update
    void Start()
    {
        goalboxnum_Text.text = GameManager.I.Add_Blocknum_goal.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        Boxnum_Text.text = GameManager.I.Add_Blocknum.ToString();
        goalboxnum_Text.text = GameManager.I.Add_Blocknum_goal.ToString();
    }

}
