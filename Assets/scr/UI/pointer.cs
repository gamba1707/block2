using System;
using UnityEngine;

//青いあのポインターオブジェクトの位置を動かす用
public class pointer : MonoBehaviour
{
    //通常色とプレイヤーの位置の時の色
    [SerializeField] Material redmaterial, bluematerial;
    //色変更用
    MeshRenderer mr;
    //メインカメラ格納用
    Camera maincamera;

    void Start()
    {
        //コンポーネントを取得
        mr = GetComponent<MeshRenderer>();
        //メインカメラを格納
        maincamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        //ゲームプレイ中なら
        if (GameManager.I.gamestate("Play"))
        {
            //ついていなければ存在をオンにする
            if (!mr.enabled) mr.enabled = true;
            //位置をポインターの位置にする
            transform.position = point();

            //プレイヤーの位置と重なっている場合
            if (GameManager.I.Playerpos == point())
            {
                //赤色にする
                mr.material = redmaterial;
            }
            else
            {
                //特に何もなければ青色にする
                mr.material = bluematerial;
            }
        }
        else
        {
            //プレイ中でなければ消しておく
            if (mr.enabled) mr.enabled = false;
        }

    }

    //マウス座標からワールドに変換する
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
