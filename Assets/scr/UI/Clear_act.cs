using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clear_act : MonoBehaviour
{
    [SerializeField] GameObject effect;

    void OnEffect()
    {
        effect.SetActive(true);
    }

    //�v���C���[�̃S�[�����o���I���ƌĂяo����܂�
    void Onmove_End()
    {
        GameManager.I.OnClear_end();
    }
}
