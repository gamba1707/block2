using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [SerializeField] GameObject floorblock,nomalblock, trampolineblock,fallblock;//ブロックのプレハブ（控えがなかった時用）
    [SerializeField] Transform floor_parent,nomal_parent, trampoline_parent,fall_parent;//親として配置先
    private int floornum,nomalnum, blocknum,trampolinenum;//何個非表示で控えているか
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

    public void GetFloorObject(Vector3 pos)
    {
        Instantiate(nomalblock, pos, Quaternion.identity, floor_parent);
    }

    public void GetFallObject(Vector3 pos)
    {
        Instantiate(fallblock, pos, Quaternion.identity, fall_parent);
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
        else Destroy(obj);
        //オブジェクトを非表示にする
        if(obj!=null)obj.SetActive(false);
    }

    public void Reset_box()
    {
        //控えている数を初期化しなおし
        nomalnum = nomal_parent.childCount;
        trampolinenum=trampoline_parent.childCount;
        //ノーマルブロックを全て非表示化
        foreach (Transform child in nomal_parent)
        {
            if (child.gameObject.activeInHierarchy)
            {
                child.gameObject.SetActive(false);
            }
        }
        //トランポリンブロックを全て非表示化
        foreach (Transform child in trampoline_parent)
        {
            if (child.gameObject.activeInHierarchy)
            {
                child.gameObject.SetActive(false);
            }
        }

        Debug.Log("<color=#0000ffff>ブロック初期化</color>\nnomalnum:"+nomalnum+ "\ntrampolinenum:"+trampolinenum);

    }

    public Transform Floor_parent
    {
        get { return floor_parent; }
    }
    public Transform Fall_parent
    {
        get { return fall_parent; }
    }
}

