using UnityEngine;

//�S�[���̂������ɂ���X�N���v�g
public class Goal : MonoBehaviour
{
    //�S�[���������Ɋ��J����
    [SerializeField] GameObject clear_vcam;
    //�S�[���p�̃I�[�f�B�I
    AudioSource audioSource;
    //SE�����łɖ炵�Ă��邩
    bool SE_IsDone;

    private void Start()
    {
        //���g�ɂ���audioSource���擾
        audioSource = GetComponent<AudioSource>();
    }

    //�S�[������ɓ���������Ă΂��
    private void OnTriggerEnter(Collider other)
    {
        //�����������肪Player�������ꍇ
        if (other.gameObject.CompareTag("Player"))
        {
            //�S�[������������
            Debug.Log("<color=red>�S�[�����܂���</color>");
            //GameManager�ɒʒm����
            GameManager.I.OnClear();
            //�J��������点��
            clear_vcam.SetActive(true);
            //���ʉ���炷
            if(!SE_IsDone)audioSource.PlayOneShot(audioSource.clip);
            //������d�ɂȂ�Ȃ��悤�ɂ���
            SE_IsDone = true;
        }
    }
}
