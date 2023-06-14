using UnityEngine;

//�{�X�X�e�[�W�Œǂ������Ă���m�C�Y
public class noise_move : MonoBehaviour
{
    //�����l�ɖ߂����߂̊i�[�ꏊ
    private Vector3 firstpos;
    
    void Start()
    {
        //�{�X�X�e�[�W����Ȃ���΂��̃I�u�W�F�N�g�͏����Ă���
        if (!MapData.mapinstance.Boss) this.gameObject.SetActive(false);
        //�����l�o�^
        firstpos = transform.position;
    }

    void FixedUpdate()
    {
        //�v���C���Ȃ�
        if (GameManager.I.gamestate("Play"))
        {
            //�E�����Ɉړ�������
            transform.position += new Vector3(0, 0, 0.025f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //�v���C���ŁAPlayer�ɓ���������
        if (GameManager.I.gamestate("Play") && other.gameObject.name.Equals("Player"))
        {
            //�Q�[���I�[�o�[�ɂ���
            GameManager.I.OnGameOver();
        }
    }

    //���Z�b�g�ŌĂ΂��
    public void noise_reset()
    {
        //�����l�ɖ߂�
        transform.position = firstpos;
    }
}
