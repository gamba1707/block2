using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Clear_act : MonoBehaviour
{
    [SerializeField] GameObject effect;

    void OnEffect()
    {
        effect.SetActive(true);
        Invoke("effect_reset",0.5f);
    }

    void effect_reset()
    {
        effect.SetActive(false);
    }

    //�v���C���[�̃S�[�����o���I���ƌĂяo����܂�
    void Onmove_End()
    {
        GameManager.I.OnClear_end();
    }
}
