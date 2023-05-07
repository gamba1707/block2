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
        //左クリック押した瞬間かつUIを押したときではないとき
        if (Input.GetMouseButtonDown(0)&&!EventSystem.current.IsPointerOverGameObject())
        {
            Debug.Log("UI?:"+EventSystem.current.IsPointerOverGameObject());
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Physics.Raycast(ray, out hit, Camera.main.transform.position.x+2.5f);
            //何もないところに生成する場合
            //生成位置がプレイヤーの位置ではない
            if (!(GameManager.I.Playerpos == point()))
            {
                //生成しようとしている場所に何もない（一応床ではないかも判定する）
                if (hit.collider==null)
                {
                    //現在のセレクトされているブロックを生成させる
                    switch (GameManager.I.Selectname)
                    {
                        case "普通のブロック":
                            poolm.GetNomalObject(point());//poolManagerに位置を渡して生成させる
                            GameManager.I.Add_Blocknum++;//GameManagerに加算する
                            Debug.Log("生成したよ");
                            break;
                        case "飛べるブロック":
                            Debug.Log("生成したよ");
                            poolm.GetTranpolineObject(point());//poolManagerに位置を渡してトランポリンを生成させる
                            GameManager.I.Add_Blocknum++;//GameManagerに加算する
                            break;
                    }
                }
                else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("addBlock"))
                {
                    if (GameManager.I.Selectname.Equals("けしごむ"))
                    {
                        Debug.Log("消せる対象:" + hit.collider.gameObject.name);
                        poolm.EraserObject(hit.collider.gameObject);
                    }
                }
            }
        }
    }

    Vector3 point()
    {
        // マウスのポインタがあるスクリーン座標を取得
        Vector3 screen_point = Input.mousePosition;
        // z に正しいカメラの距離（このゲームではX座標）を入れないと正しく変換できない
        screen_point.z = Camera.main.transform.position.x;
        // スクリーン座標をワールド座標に変換
        Vector3 world_position = Camera.main.ScreenToWorldPoint(screen_point);
        //ボックスは1.5刻みなので1.5の倍数の位置に変換
        float y = (float)Math.Round((world_position.y / 1.5f), 0, MidpointRounding.AwayFromZero) * 1.5f;
        float z = (float)Math.Round((world_position.z / 1.5f), 0, MidpointRounding.AwayFromZero) * 1.5f;
        world_position.x = 0;
        world_position.y = y;
        world_position.z = z;
        return world_position;
    }

}
