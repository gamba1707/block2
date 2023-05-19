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
        // マウスのポインタがあるスクリーン座標を取得
        Vector3 screen_point = Input.mousePosition;
        // z に 奥行きを正しく設定する必要がある
        screen_point.z = maincamera.transform.position.x;
        // スクリーン座標をワールド座標に変換
        Vector3 world_position = maincamera.ScreenToWorldPoint(screen_point);
        //ボックスは1.5刻みなので1.5の倍数の位置に変換
        float y = (float)Math.Round((world_position.y / 1.5f), 0, MidpointRounding.AwayFromZero) * 1.5f;
        float z = (float)Math.Round((world_position.z / 1.5f), 0, MidpointRounding.AwayFromZero) * 1.5f;
        world_position.x = 0;
        world_position.y = y;
        world_position.z = z;
        return world_position;
    }
}
