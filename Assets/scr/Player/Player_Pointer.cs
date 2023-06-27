using System;
using UnityEngine;
using UnityEngine.EventSystems;

//ストーリーモード限定のオブジェクト生成管理
public class Player_Pointer : MonoBehaviour
{
    //アイテムを実際に管理しているプール管理
    [SerializeField] PoolManager poolm;
    //メインカメラ格納用
    Camera maincamera;

    void Start()
    {
        //メインカメラを格納
        maincamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        //左クリック押した瞬間かつUIを押したときではないとき
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            Debug.Log("UI?:" + EventSystem.current.IsPointerOverGameObject());
            //光線をマウスポインターの先へ出して
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            //光線の長さは奥行き+2.5程度で
            Physics.Raycast(ray, out hit, maincamera.transform.position.x + 2.5f);

            //プレイ中で、生成位置がプレイヤーの位置ではない
            if (GameManager.I.gamestate("Play") && !(GameManager.I.Playerpos == point()))
            {
                //生成しようとしている場所に何もない（一応床ではないかも判定する）
                if (hit.collider == null)
                {
                    //現在のセレクトされているブロックを生成させる
                    switch (GameManager.I.Selectname)
                    {
                        case "普通のブロック":
                            poolm.GetNomalObject(point());//poolManagerに位置を渡して生成させる
                            if (!MapData.mapinstance.Last) GameManager.I.Add_Blocknum++;//GameManagerに加算する
                            Debug.Log("生成したよ");
                            break;
                        case "飛べるブロック":
                            Debug.Log("生成したよ");
                            poolm.GetTranpolineObject(point());//poolManagerに位置を渡してトランポリンを生成させる
                            if (!MapData.mapinstance.Last) GameManager.I.Add_Blocknum += 2;//GameManagerに加算する
                            break;
                        case "下がるブロック":
                            Debug.Log("生成したよ");
                            poolm.GetDownObject(point());//poolManagerに位置を渡してトランポリンを生成させる
                            if (!MapData.mapinstance.Last) GameManager.I.Add_Blocknum += 2;//GameManagerに加算する
                            break;
                    }
                }
                else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("addBlock"))
                {
                    //自分で追加したオブジェクトの場合
                    //消しゴムを選択していたら
                    if (GameManager.I.Selectname.Equals("けしごむ"))
                    {
                        //そのオブジェクトを消す
                        Debug.Log("消せる対象:" + hit.collider.gameObject.name);
                        poolm.EraserObject(hit.collider.gameObject);
                    }
                }
            }
        }
    }

    //ポインターの位置からワールド座標に変換する
    Vector3 point()
    {
        // マウスのポインタがあるスクリーン座標を取得
        Vector3 screen_point = Input.mousePosition;
        // z に正しいカメラの距離（このゲームではX座標）を入れないと正しく変換できない
        screen_point.z = maincamera.transform.position.x;
        // スクリーン座標をワールド座標に変換
        Vector3 world_position = maincamera.ScreenToWorldPoint(screen_point);
        //ボックスは1.5刻みなので1.5の倍数の位置に変換
        float y = (float)Math.Round((world_position.y / 1.5f), 0, MidpointRounding.AwayFromZero) * 1.5f;
        float z = (float)Math.Round((world_position.z / 1.5f), 0, MidpointRounding.AwayFromZero) * 1.5f;

        world_position.x = 0;//奥行きはなし
        world_position.y = y;//高さ
        world_position.z = z;//横方向

        //位置を返す
        return world_position;
    }

}
