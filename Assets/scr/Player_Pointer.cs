using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player_Pointer : MonoBehaviour
{
    [SerializeField] GameObject nomalblock, trampolineblock;
    [SerializeField] GameObject addBlock;
    PoolManager poolm;
    Vector3 clickpos = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        poolm = addBlock.GetComponent<PoolManager>();
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
            Physics.Raycast(ray, out hit, 10.0f);
            Debug.Log(Input.mousePosition);
            //何もないところに生成する場合
            //生成位置がプレイヤーの位置ではない
            if (!(GameManager.I.Playerpos == hit.transform.position))
            {
                //生成しようとしている場所に何もない（一応床ではないかも判定する）
                if (hit.collider.gameObject.CompareTag("Untagged") && hit.collider.gameObject.layer != LayerMask.NameToLayer("floor"))
                {
                    Debug.Log(hit.transform.position);
                    clickpos = hit.transform.position;
                    //現在のセレクトされているブロックを生成させる
                    switch (GameManager.I.Selectname)
                    {
                        case "普通のブロック":
                            //Instantiate(nomalblock, clickpos, Quaternion.identity,addBlock);
                            poolm.GetNomalObject(clickpos);
                            Debug.Log("生成したよ");
                            break;
                        case "飛べるブロック":
                            //Instantiate(trampolineblock, clickpos, Quaternion.identity,addBlock);
                            Debug.Log("生成したよ");
                            poolm.GetTranpolineObject(clickpos);
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



}
