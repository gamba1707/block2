using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pointer : MonoBehaviour
{
    [SerializeField] Material redmaterial, bluematerial;
    MeshRenderer mr;
    Camera maincamera;
    // Start is called before the first frame update
    void Start()
    {
        mr = GetComponent<MeshRenderer>();
        maincamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (GameManager.I.gamestate("Play"))
        {
            if (!mr.enabled) mr.enabled=true;
            transform.position = point();
            if (GameManager.I.Playerpos == point())
            {
                mr.material = redmaterial;
            }
            else
            {
                mr.material = bluematerial;
            }
        }
        else
        {
            if (mr.enabled) mr.enabled = false;
        }

    }

    Vector3 point()
    {
        // �}�E�X�̃|�C���^������X�N���[�����W���擾
        Vector3 screen_point = Input.mousePosition;
        // z �� ���s���𐳂����ݒ肷��K�v������
        screen_point.z = maincamera.transform.position.x;
        // �X�N���[�����W�����[���h���W�ɕϊ�
        Vector3 world_position = maincamera.ScreenToWorldPoint(screen_point);
        //�{�b�N�X��1.5���݂Ȃ̂�1.5�̔{���̈ʒu�ɕϊ�
        float y = (float)Math.Round((world_position.y / 1.5f), 0, MidpointRounding.AwayFromZero) * 1.5f;
        float z = (float)Math.Round((world_position.z / 1.5f), 0, MidpointRounding.AwayFromZero) * 1.5f;
        world_position.x = 0;
        world_position.y = y;
        world_position.z = z;
        return world_position;
    }
}
