using UnityEngine;
using TMPro;

//�E��ɂ���u���b�N���̃e�L�X�g��l���󂯎��Ȃ���ύX����X�N���v�g
public class Block_Text : MonoBehaviour
{
    //�ڕW�e�L�X�g�ƌ��݂̃u���b�N��
    [SerializeField] TextMeshProUGUI Boxnum_Text, goalboxnum_Text;

    void Start()
    {
        //�ڕW���͍ŏ��ɐݒ肷��
        goalboxnum_Text.text = GameManager.I.Add_Blocknum_goal.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        //�u���b�N�����X�V���Ă���
        Boxnum_Text.text = GameManager.I.Add_Blocknum.ToString();
    }

}
