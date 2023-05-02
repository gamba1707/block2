using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [SerializeField] GameObject nomalblock, trampolineblock;
    [SerializeField] Transform nomal_parent, trampoline_parent;
    private int nomalnum, blocknum,trampolinenum;
    void Start()
    {
        //GameManagerの方で設定された数値で初期化する
        nomalnum = GameManager.I.Nomalnum;
        blocknum = GameManager.I.Blocknum;
        if (GameManager.I.Trampoline) trampolinenum = blocknum;
        //指定されているものを指定された数生成する
        for (int i = 0;i<nomalnum;i++)
        {
            if(GameManager.I.Nomal)Instantiate(nomalblock, new Vector3(0,0,-5), Quaternion.identity, nomal_parent);
        }
        for(int i = 0; i < blocknum; i++)
        {
            if (GameManager.I.Trampoline) Instantiate(trampolineblock, new Vector3(0, 0, -5), Quaternion.identity, trampoline_parent);
        }

        //生成したオブジェクトを非表示にする
        foreach(Transform child in nomal_parent)
        {
            GameObject childObject = child.gameObject;
            child.gameObject.SetActive(false);
        }
        foreach (Transform child in trampoline_parent)
        {
            GameObject childObject = child.gameObject;
            child.gameObject.SetActive(false);
        }
    }

    //ノーマルのブロックを生成する
    public void GetNomalObject(Vector3 pos)
    {
        if (nomalnum > 0)
        {
            foreach (Transform child in nomal_parent)
            {
                if (!child.gameObject.activeInHierarchy)
                {
                    child.transform.position = pos;
                    child.gameObject.SetActive(true);
                    nomalnum--;
                    goto END;//一つ出現させたら終わらせる
                }
            }
        }
        else//もうストックがないなら新たに生成して対応するしかない...
        {
            Instantiate(nomalblock, pos, Quaternion.identity, nomal_parent);
        }
    END:;//終わり転送先
    }

    //トランポリン的なブロックを生成する
    public void GetTranpolineObject(Vector3 pos)
    {
        if (trampolinenum > 0)
        {
            foreach (Transform child in trampoline_parent)
            {
                if (!child.gameObject.activeInHierarchy)
                {
                    child.transform.position = pos;
                    child.gameObject.SetActive(true);
                    trampolinenum--;
                    goto END;//一つ出現させたら終わらせる
                }
            }
        }
        else//もうストックがないなら新たに生成して対応するしかない...
        {
            Instantiate(trampolineblock, pos, Quaternion.identity, trampoline_parent);
        }
    END:;//終わり転送先
    }

    //消しゴム機能
    public void EraserObject(GameObject obj)
    {
        //再利用出来る数を更新
        if(obj.CompareTag("cube"))nomalnum++;
        else if(obj.CompareTag("trampoline"))trampolinenum++;
        //オブジェクトを非表示にする
        obj.SetActive(false);
    }
}
