using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���̃X�N���v�g�ł͂��ꂼ��̃}�X���Ƀv���C���[���������Ƀv���C���[�ʒu���X�V����
//�����ɂ̓u���b�N�������o���Ȃ��悤�ɂ���
public class Block_onpleyer : MonoBehaviour
{
        private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("�v���C���[������n�_" + this.transform.position);
            GameManager.I.Playerpos = this.transform.position;
        }
    }
}
