using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Map_pointer : MonoBehaviour
{
    [SerializeField] PoolManager poolm;
    Camera maincamera;
    // Start is called before the first frame update
    void Start()
    {
        maincamera=Camera.main;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            Ray ray = maincamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Physics.Raycast(ray, out hit, maincamera.transform.position.x + 2.5f);
            if(hit.collider == null)
            {
                //���݂̃Z���N�g����Ă���u���b�N�𐶐�������
                switch (GameManager.I.Selectname)
                {
                    case "��":
                        poolm.GetFloorObject(point());//poolManager�Ɉʒu��n���Đ���������
                        Debug.Log("����������");
                        break;
                    case "�ޗ�":
                        poolm.GetFallObject(point());//poolManager�Ɉʒu��n���Đ���������
                        Debug.Log("����������");
                        break;
                    case "�S�[��":
                        poolm.GetGoalObject_edit(point());//poolManager�Ɉʒu��n���Ĉړ�
                        Debug.Log("����������");
                        break;
                    case "���ʂ̃u���b�N":
                        poolm.GetNomalObject(point());//poolManager�Ɉʒu��n���Đ���������
                        GameManager.I.Add_Blocknum++;//GameManager�ɉ��Z����
                        Debug.Log("����������");
                        break;
                    case "��ׂ�u���b�N":
                        Debug.Log("����������");
                        poolm.GetTranpolineObject(point());//poolManager�Ɉʒu��n���ăg�����|�����𐶐�������
                        GameManager.I.Add_Blocknum+=2;//GameManager��+2���Z����
                        break;
                    case "������u���b�N":
                        Debug.Log("����������");
                        poolm.GetDownObject(point());//poolManager�Ɉʒu��n���ăg�����|�����𐶐�������
                        GameManager.I.Add_Blocknum+=2;//GameManager��+2���Z����
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
            else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Edit"))
            {
                if (GameManager.I.Selectname.Equals("��������"))
                {
                    Debug.Log("������Ώ�:" + hit.collider.gameObject.name);
                    Destroy(hit.collider.gameObject);
                }
            }
        }
    }

    Vector3 point()
    {
        // �}�E�X�̃|�C���^������X�N���[�����W���擾
        Vector3 screen_point = Input.mousePosition;
        // z �� 1 �����Ȃ��Ɛ������ϊ��ł��Ȃ�
        screen_point.z = 1.0f;
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
