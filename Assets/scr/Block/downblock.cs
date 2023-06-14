using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//������u���b�N
public class downblock : MonoBehaviour
{
    //�ʏ펞�Ɖ������Ă���Ƃ��̃}�e���A��
    [SerializeField] Material nomalmaterial,downmaterial;
    //�����l�p
    Vector3 firstpos;
    //�}�e���A���ς���p
    MeshRenderer meshRenderer;
    //�������Ă���Ƃ��Ɏg�p
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        //�����l�o�^
        firstpos = transform.position;
        //�R���|�[�l���g���擾
        meshRenderer = GetComponent<MeshRenderer>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //���x���ő��-1�ŉ����葱����悤�ɂ���
        if(rb.velocity.y<-1f||rb.velocity.y>0)rb.velocity = new Vector3(0, -1f, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        //�v���C���[���G�ꂽ��
        if (other.gameObject.name.Equals("Player"))
        {
            //�������Ă���F�ɂ���
            meshRenderer.material= downmaterial;
            //2�b��ɉ�����n�߂�
            Invoke("down_move",2f);
        }
    }

    //�����o������
    void down_move()
    {
        //RiditBody�̏d�͂��g��
        rb.useGravity = true;
        
        //Y�����������ė���������
        rb.constraints = RigidbodyConstraints.FreezeRotation| RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
        //�������x���w��
        rb.velocity = new Vector3(0, -1f, 0);
    }

    //�J��������O�ꂽ��Ă΂��
    private void OnBecameInvisible()
    {
        //��U���̃I�u�W�F�N�g������
        this.gameObject.SetActive(false);
        //�����l�ɖ߂�
        transform.position = firstpos;
        //�d�͂�؂�
        rb.useGravity = false;
        //�S���Œ肷��
        rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePosition;
        //�ʏ�̐F�ɖ߂�
        meshRenderer.material= nomalmaterial;
        //���������̂ŃI�u�W�F�N�g��\������
        this.gameObject.SetActive(true);
    }
}
