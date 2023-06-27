using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ブロックのプール管理をしているような...
public class PoolManager : MonoBehaviour
{
    //ブロックのプレハブ（控えがなかった時用）
    [Header("ブロックのプレハブ")]
    [SerializeField] GameObject nomalblock;
    [SerializeField] GameObject trampolineblock;
    [SerializeField] GameObject downblock;
    [Header("特殊ブロックのプレハブ")]
    [SerializeField] GameObject floorblock;
    [SerializeField] GameObject fallblock;
    [SerializeField] GameObject trampolineblock_before;
    [SerializeField] GameObject downblock_before;

    [Header("生成するブロックの親オブジェクト先")]
    [SerializeField] Transform nomal_parent;//親として配置先
    [SerializeField] Transform trampoline_parent;//親として配置先
    [SerializeField] Transform down_parent;//親として配置先

    [Header("生成するブロックの親オブジェクト先(特殊オブジェクト)")]
    [SerializeField] Transform floor_parent;//親として配置先
    [SerializeField] Transform fall_parent;//親として配置先
    [SerializeField] Transform trampoline_p_before;//親として配置先
    [SerializeField] Transform down_p_before;//親として配置先

    [Header("それぞれのシーンにあるゴールオブジェクト")]
    [SerializeField] GameObject goalblock;

    [Header("生成効果音")]
    AudioSource audioSource;
    [SerializeField] AudioClip nomal_se, eraser_se;

    //何個非表示で控えているか
    private int nomalnum, blocknum, trampolinenum, downnum;

    void Start()
    {
        //コンポーネントを取得
        audioSource = GetComponent<AudioSource>();

        //GameManagerの方で設定された数値で初期化する
        nomalnum = GameManager.I.Nomalnum;
        blocknum = GameManager.I.Blocknum;
        trampolinenum = blocknum;
        downnum = blocknum;

        //指定されているものを指定された数生成する
        //普通のブロック
        for (int i = 0; i < nomalnum; i++)
        {
            Instantiate(nomalblock, new Vector3(0, 0, -5), Quaternion.identity, nomal_parent);
        }
        //それ以外のブロック
        for (int i = 0; i < blocknum; i++)
        {
            Instantiate(trampolineblock, new Vector3(0, 0, -5), Quaternion.identity, trampoline_parent);
            Instantiate(downblock, new Vector3(0, 0, -5), Quaternion.identity, down_parent);
        }

        //生成したオブジェクトを非表示にする
        //普通のブロック
        foreach (Transform child in nomal_parent)
        {
            GameObject childObject = child.gameObject;
            child.gameObject.SetActive(false);
        }
        //トランポリンブロック
        foreach (Transform child in trampoline_parent)
        {
            GameObject childObject = child.gameObject;
            child.gameObject.SetActive(false);
        }
        //下がるブロック
        foreach (Transform child in down_parent)
        {
            GameObject childObject = child.gameObject;
            child.gameObject.SetActive(false);
        }
    }

    //座標をもらうと床ブロックを生成する
    public void GetFloorObject(Vector3 pos)
    {
        Instantiate(floorblock, pos, Quaternion.identity, floor_parent);
    }

    //座標をもらうと奈落ブロックを生成する
    public void GetFallObject(Vector3 pos)
    {
        Instantiate(fallblock, pos, Quaternion.identity, fall_parent);
    }

    //座標をもらうと地形トランポリンブロックを生成する
    public void GetTrampolineObject_before(Vector3 pos)
    {
        Instantiate(trampolineblock_before, pos, Quaternion.identity, trampoline_p_before);
    }

    //座標をもらうと地形下がるブロックを生成すr
    public void GetDownObject_before(Vector3 pos)
    {
        Instantiate(downblock_before, pos, Quaternion.identity, down_p_before);
    }

    //座標をもらうとゴール位置を変更する（クリエイトモードの時）
    public void GetGoalObject_edit(Vector3 pos)
    {
        goalblock.gameObject.transform.position = new Vector3(pos.x, pos.y - 0.75f, pos.z);
    }

    //座標をもらうとゴール位置を変更する
    public void GetGoalObject(Vector3 pos)
    {
        goalblock.gameObject.transform.position = pos;
    }

    //ノーマルのブロックを生成する
    public void GetNomalObject(Vector3 pos)
    {
        //裏で控えているオブジェクトがありそう
        if (nomalnum > 0)
        {
            //全ての中から探す
            foreach (Transform child in nomal_parent)
            {
                //非表示なやつが見つかった
                if (!child.gameObject.activeInHierarchy)
                {
                    //生成位置に移動させる
                    child.transform.position = pos;
                    //表示にする
                    child.gameObject.SetActive(true);
                    //生成音を鳴らす
                    audioSource.PlayOneShot(nomal_se);
                    //非表示の数を減らす
                    nomalnum--;
                    //一つ出現させたら終わらせる（END駅へ）
                    goto END;
                }
            }
        }
        else//もうストックがないなら新たに生成して対応するしかない...
        {
            //生成
            Instantiate(nomalblock, pos, Quaternion.identity, nomal_parent);
            //音
            audioSource.PlayOneShot(nomal_se);
        }
    END:;//終わり転送先
    }

    //トランポリン的なブロックを生成する
    public void GetTranpolineObject(Vector3 pos)
    {
        //全ての中から探す
        foreach (Transform child in trampoline_parent)
        {
            //非表示なやつが見つかった
            if (!child.gameObject.activeInHierarchy)
            {
                //生成位置に移動させる
                child.transform.position = pos;
                //表示にする
                child.gameObject.SetActive(true);
                //生成音を鳴らす
                audioSource.PlayOneShot(nomal_se);
                //数を減らす
                trampolinenum--;
                //一つ出現させたら終わらせる（END駅へ）
                goto END;
            }
        }

        //もうストックがないなら新たに生成して対応するしかない...
         //生成
        Instantiate(trampolineblock, pos, Quaternion.identity, trampoline_parent);
        //音
        audioSource.PlayOneShot(nomal_se);
    END:;//終わり転送先
    }

    //ちくわブロックを生成する
    public void GetDownObject(Vector3 pos)
    {
        //裏で控えているオブジェクトがありそうなら
        if (downnum > 0)
        {
            //全ての中から探す
            foreach (Transform child in down_parent)
            {
                //非表示なやつが見つかった
                if (!child.gameObject.activeInHierarchy)
                {
                    //生成位置に移動させる
                    child.transform.position = pos;
                    //表示にする
                    child.gameObject.SetActive(true);
                    //生成音を鳴らす
                    audioSource.PlayOneShot(nomal_se);
                    //数を減らす
                    downnum--;
                    //一つ出現させたら終わらせる（END駅へ）
                    goto END;
                }
            }
        }
        else//もうストックがないなら新たに生成して対応するしかない...
        {
            //生成
            Instantiate(downblock, pos, Quaternion.identity, down_parent);
            //音
            audioSource.PlayOneShot(nomal_se);
        }
    END:;//終わり転送先
    }

    //消しゴム機能
    public void EraserObject(GameObject obj)
    {
        //音
        audioSource.PlayOneShot(eraser_se);
        //再利用出来る数を更新
        if (obj.CompareTag("cube")) nomalnum++;
        else if (obj.CompareTag("trampoline")) trampolinenum++;
        else if (obj.CompareTag("down")) downnum++;
        else Destroy(obj);
        //オブジェクトを非表示にする
        if (obj != null) obj.SetActive(false);
    }

    //リセットが呼ばれたら
    public void Reset_box()
    {
        //控えている数を初期化しなおし
        nomalnum = nomal_parent.childCount;
        trampolinenum = trampoline_parent.childCount;
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
        //ちくわブロックを全て非表示化
        foreach (Transform child in down_parent)
        {
            if (child.gameObject.activeInHierarchy)
            {
                child.gameObject.SetActive(false);
            }
        }

        Debug.Log("<color=#0000ffff>ブロック初期化</color>\nnomalnum:" + nomalnum + "\ntrampolinenum:" + trampolinenum + "\ndownnum:" + downnum);
    }

    //ここからプロパティ
    //主にクリエイトモードの位置取得に使う
    public Transform Floor_parent
    {
        get { return floor_parent; }
    }
    public Transform Fall_parent
    {
        get { return fall_parent; }
    }
    public Transform Trampoline_parent_before
    {
        get { return trampoline_p_before; }
    }
    public Transform Down_parent_before
    {
        get { return down_p_before; }
    }
    public Vector3 Goalpos
    {
        get { return goalblock.gameObject.transform.position; }
    }
}