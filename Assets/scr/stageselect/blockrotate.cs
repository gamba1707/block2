using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�X�e�[�W�Z���N�g��ʂŃu���b�N����]������
public class blockrotate : MonoBehaviour
{
    //��]����C�ӂɒ�߂���悤��
    [Header("��]��")]
    [SerializeField]float x, y, z;

    void FixedUpdate()
    {
        //����������l�ŉ�]������
        transform.Rotate(x, y, z);
    }
}
