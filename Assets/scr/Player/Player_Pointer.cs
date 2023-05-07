using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player_Pointer : MonoBehaviour
{
    [SerializeField] PoolManager poolm;
    Vector3 clickpos = Vector3.zero;
    float camera_length;
    // Start is called before the first frame update
    void Start()
    {
        camera_length = Camera.main.transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        //���N���b�N�������u�Ԃ���UI���������Ƃ��ł͂Ȃ��Ƃ�
        if (Input.GetMouseButtonDown(0)&&!EventSystem.current.IsPointerOverGameObject())
        {
            Debug.Log("UI?:"+EventSystem.current.IsPointerOverGameObject());
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Physics.Raycast(ray, out hit, Camera.main.transform.position.x+2.5f);
            //�����Ȃ��Ƃ���ɐ�������ꍇ
            //�����ʒu���v���C���[�̈ʒu�ł͂Ȃ�
            if (!(GameManager.I.Playerpos == point()))
            {
                //�������悤�Ƃ��Ă���ꏊ�ɉ����Ȃ��i�ꉞ���ł͂Ȃ��������肷��j
                if (hit.collider==null)
                {
                    //���݂̃Z���N�g����Ă���u���b�N�𐶐�������
                    switch (GameManager.I.Selectname)
                    {
                        case "���ʂ̃u���b�N":
                            poolm.GetNomalObject(point());//poolManager�Ɉʒu��n���Đ���������
                            GameManager.I.Add_Blocknum++;//GameManager�ɉ��Z����
                            Debug.Log("����������");
                            break;
                        case "��ׂ�u���b�N":
                            Debug.Log("����������");
                            poolm.GetTranpolineObject(point());//poolManager�Ɉʒu��n���ăg�����|�����𐶐�������
                            GameManager.I.Add_Blocknum++;//GameManager�ɉ��Z����
                            break;
                    }
                }
                else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("addBlock"))
                {
                    if (GameManager.I.Selectname.Equals("��������"))
                    {
                        Debug.Log("������Ώ�:" + hit.collider.gameObject.name);
                        poolm.EraserObject(hit.collider.gameObject);
                    }
                }
            }
        }
    }

    Vector3 point()
    {
        // �}�E�X�̃|�C���^������X�N���[�����W���擾
        Vector3 screen_point = Input.mousePosition;
        // z �ɐ������J�����̋����i���̃Q�[���ł�X���W�j�����Ȃ��Ɛ������ϊ��ł��Ȃ�
        screen_point.z = Camera.main.transform.position.x;
        // �X�N���[�����W�����[���h���W�ɕϊ�
        Vector3 world_position = Camera.main.ScreenToWorldPoint(screen_point);
        //�{�b�N�X��1.5���݂Ȃ̂�1.5�̔{���̈ʒu�ɕϊ�
        float y = (float)Math.Round((world_position.y / 1.5f), 0, MidpointRounding.AwayFromZero) * 1.5f;
        float z = (float)Math.Round((world_position.z / 1.5f), 0, MidpointRounding.AwayFromZero) * 1.5f;
        world_position.x = 0;
        world_position.y = y;
        world_position.z = z;
        return world_position;
    }

}
