using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//�v���C���[�̈ړ��Ɋւ���v���O����
public class Player_move : MonoBehaviour
{
    CharacterController controller;
    private Camera maincamera;
    private GameObject Player_t;//Player���������̕����ɕ������邽��
    private Animator anim;

    private Vector3 moveDirection = Vector3.zero;
    private Vector3 gravityDirection = Vector3.zero;
    private Vector3 cameraForward = Vector3.zero;
    private static float x, y;
    [SerializeField] private Vector3 playerpos_first;
    [SerializeField] private float speed=5F;
    [SerializeField] private bool falling;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        maincamera = Camera.main;
        Player_t = transform.GetChild(0).gameObject;
        anim = GetComponentInChildren<Animator>();
        playerpos_first = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.I.gamestate("Play"))
        {
            //�����̓v���C���[�̈ʒu��GameManager�ɓ`���鏊
            GameManager.I.Playerpos=transform.position;

            //���͒l
            x = Input.GetAxis("Horizontal");    //���E���L�[�̒l(-1.0~1.0)

            //�A�j���[�V����
            if (x != 0 || y != 0) anim.SetBool("walk", true);
            else anim.SetBool("walk", false);

            if (controller.isGrounded)//���n���i���Ԃ�j
            {
                falling = false;
                gravityDirection = new Vector3(0, 0, 0);
                // �J�����̕�������AX-Z���ʂ̒P�ʃx�N�g�����擾
                cameraForward = Vector3.Scale(maincamera.transform.forward, new Vector3(1, 0, 1)).normalized;
                // �����L�[�̓��͒l�ƃJ�����̌�������A�ړ�����������
                moveDirection = cameraForward * y * speed + maincamera.transform.right * x * speed;
                moveDirection = transform.TransformDirection(moveDirection);
            }
            else//�󒆂ɂ���ꍇ
            {
                falling = true;//FixcedUpdate�̕��ŏ���
            }

            //�����Ă���Ƃ��͏�ɉ�����Ă�������������Ăق���
            if (x != 0 || y != 0) Player_t.transform.localRotation = Quaternion.LookRotation(cameraForward * y + maincamera.transform.right * x);

            //�ŏI�I�ɓ�����
            controller.Move(moveDirection * Time.deltaTime);
        }
    }

    //�������̎��̂ݎg�p
    private void FixedUpdate()
    {
        if (falling)
        {
            Vector3 Direction = ((maincamera.transform.right * x * 3f) + (maincamera.transform.forward * y * 3f));
            gravityDirection.y -= 0.3f * Time.deltaTime;
            moveDirection = new Vector3(Direction.x, gravityDirection.y + moveDirection.y, Direction.z);
            moveDirection = transform.TransformDirection(moveDirection);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("trampoline"))
        {
            //�L�����N�^�[�R���g���[���[�̐ڒn���x���Ⴂ�̂ł��邯�ǂ����ŏ�����
            gravityDirection = new Vector3(0, 0, 0);
            moveDirection.y = 6.2f;
            Debug.Log("kiffnsofo"+ moveDirection.y);
        }
    }

    //�Q�[���N���A�����Ƃ���GameManager����Ăяo�����
    //
    public void Clear_move()
    {
        Player_t.transform.rotation = Quaternion.Euler(0f, 90f, 0f);
        clear_warp(transform.position);
        anim.SetTrigger("clear");
    }
    //�����蔻��I�ɓ�����ŃS�[���|�[�Y�����Ƃ����������̂ŃG�t�F�N�g�̒��S�ɗ�������
    void clear_warp(Vector3 start)
    {
        float z = (float)Math.Round((transform.position.z/1.5f), 0, MidpointRounding.AwayFromZero);
        z *= 1.5f;
        Debug.Log("�ϊ��O�F"+start.z+"  �ϊ���F"+z);
        Vector3 end=new Vector3(transform.position.x, transform.position.y, z);
        for(float i = 0; i <= 1; i += 0.01f)
        {
            transform.position = Vector3.Lerp(start, end, i);
        }
    }
    public void GameOver_move()
    {
        //��э~�肽���̌����ɂ���
        Player_t.transform.rotation = Quaternion.Euler(20f, Player_t.transform.localEulerAngles.y, 0f);
        //�������A�j���[�V����
        anim.SetBool("fall",true);
    }

    public void Reset_move()
    {
        Player_t.transform.rotation = Quaternion.Euler(0f, 0, 0f);
        transform.position = playerpos_first;
        anim.SetBool("walk", false);
        anim.SetBool("fall", false);
        Debug.Log("<color=#0000ffff>�v���C���[������</color>\nPlayerpos:" + transform.position);
    }
}
