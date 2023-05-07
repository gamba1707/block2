using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Map_pointer : MonoBehaviour
{
    [SerializeField] PoolManager poolm;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Physics.Raycast(ray, out hit, 10.0f);
            if(hit.collider == null)
            {
                //現在のセレクトされているブロックを生成させる
                switch (GameManager.I.Selectname)
                {
                    case "床":
                        poolm.GetFloorObject(point());//poolManagerに位置を渡して生成させる
                        Debug.Log("生成したよ");
                        break;
                    case "奈落":
                        poolm.GetFallObject(point());//poolManagerに位置を渡して生成させる
                        Debug.Log("生成したよ");
                        break;
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

    Vector3 point()
    {
        // マウスのポインタがあるスクリーン座標を取得
        Vector3 screen_point = Input.mousePosition;
        // z に 1 を入れないと正しく変換できない
        screen_point.z = 1.0f;
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
