using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//�E��ɂ���u���b�N���̃e�L�X�g��l���󂯎��Ȃ���ύX����X�N���v�g
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
