using UnityEngine;

//�S�[����������X�^�[�g���ɃG�t�F�N�g���o��
public class Clear_act : MonoBehaviour
{
    //�G�t�F�N�g
    [SerializeField] GameObject effect;

    //�G�t�F�N�g��\������i�A�j���[�V��������Ăяo�����j
    void OnEffect()
    {
        //�G�t�F�N�g��\������
        effect.SetActive(true);
        //�ǂ����덇���ŏ���
        Invoke("effect_reset", 0.5f);
    }

    //�G�t�F�N�g���\���ɂ���
    void effect_reset()
    {
        effect.SetActive(false);
    }

    //�v���C���[�̃S�[�����o���I���ƃA�j���[�V��������Ăяo����܂�
    void Onmove_End()
    {
        GameManager.I.OnClear_end();
    }
}
