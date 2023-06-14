using System.Collections;
using UnityEngine;

//�v���C���[�̓o��@�\
public class clime : MonoBehaviour
{
    //�A�j���[�V����
    private Animator anim;
    //���������u���b�N�̈ʒu
    private Vector3 blockpos;

    // Start is called before the first frame update
    void Start()
    {
        //�R���|�[�l���g�擾
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //�ꉞ�ǂ�Ȃӂ��Ɋm�F���Ă���̂��m�F
        //�v���C���[�̎΂ߏ�ɉ����Ȃ����m�F���Ă��܂�
        Debug.DrawRay(new Vector3(transform.position.x, transform.position.y + 1.7f, transform.position.z), transform.forward * 1.0f, Color.red);
    }

    private void OnTriggerEnter(Collider other)
    {
        //�v���C���ɃL���[�u�ɓ�������
        if (GameManager.I.gamestate("Play") && other.gameObject.CompareTag("cube"))
        {
            //�����𓪂��炢�̈ʒu�ɏo����
            Ray ray = new Ray(new Vector3(transform.position.x, transform.position.y + 1.7f, transform.position.z), transform.forward);
            Debug.DrawRay(new Vector3(transform.position.x, transform.position.y + 1.7f, transform.position.z), transform.forward * 1.0f, Color.red);

            //�΂ߏ�ɉ������邩����
            RaycastHit hit;
            Physics.Raycast(ray, out hit, 1.0f);
            Debug.Log(hit.collider == null);

            //�����Ȃ���Γo��
            if (hit.collider == null)
            {
                Debug.Log("OK");
                //�ڂ̑O�̃u���b�N�̈ʒu��o�^����
                blockpos = other.transform.position;
                //�o��A�j���[�V����������
                anim.SetTrigger("clime");
            }
            else
            {
                Debug.Log("�o��Ȃ��ꏊ�̂͂�");
            }
        }
    }

    //�A�j���[�V�����̃C�x���g����Ăяo�����
    //�F�X�Ȍ��ˍ������疳�������W���グ�ċ삯�オ�����悤�Ɍ����Ă�
    IEnumerator clime_move()
    {
        float f = 0f;
        Debug.Log(blockpos);
        //���݂̃v���C���[�̈ʒu
        Vector3 startpos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        //���������u���b�N�̈��̈ʒu
        Vector3 endpos = new Vector3(blockpos.x, blockpos.y + 1f, blockpos.z);
        Debug.Log(startpos);
        Debug.Log(endpos);

        //�삯�オ��܂ŌJ��Ԃ��i�⊮�҂��j
        while (f <= 1.0f)
        {
            //�⊮���Ȃ����ɍs��
            transform.root.position = Vector3.Slerp(startpos, endpos, f);
            //�󋵉��Z
            f += 0.05f;
            //���ԒP�ʂő҂�
            yield return new WaitForSecondsRealtime(0.005f);
        }
    }
}
