using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ボスが出すブロックそれぞれ
public class boss_block : MonoBehaviour
{
    //位置
    Vector3 startpos;//初期位置
    Vector3 targetpos;//飛ばす位置

    //2重に判定をしないように
    bool interval;

    //リサイクルするのでOnEnable
    void OnEnable()
    {
        //初期位置
        startpos = transform.position;

        //最初だけブロックの当たり判定を消す
        this.GetComponent<SphereCollider>().enabled = false;

        //再帰用にfalseにする
        interval = false;

        //最初のコルーチン
        StartCoroutine(start_move());
    }

    //最初のコルーチン
    IEnumerator start_move()
    {
        float t = 0;
        //初期位置からプレイヤー上空まで滑らかに補間移動させる
        while (t <= 1)
        {
            //1になるまで滑らかに移動させる
            transform.position = Vector3.Slerp(startpos, targetpos, t);
            //待つ
            yield return new WaitForSecondsRealtime(0.01f);
            //アニメーション足す
            t += 0.01f;
        }
        //上空に行ったら当たり判定をオンにする
        this.GetComponent<SphereCollider>().enabled = true;
    }

    //処理能力に大きく左右されないためにFixed
    void FixedUpdate()
    {
        //設定されているデッドラインまで有効
        if (transform.position.y >= MapData.mapinstance.Deadline)
        {
            //下へ移動
            transform.position += new Vector3(0, -0.05f, 0);
        }
        else
        {
            //デッドラインに到達したら非表示化（また呼ばれるのを待つ）
            this.gameObject.SetActive(false);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        //プレイ中なら
        if (GameManager.I.gamestate("Play"))
        {
            Debug.Log(other.gameObject.tag);
            //プレイヤーに当たった
            if (other.gameObject.CompareTag("Player"))
            {
                //プレイヤーのHPを減らす
                if (!interval) GameManager.I.OnBossBlock();
                //インターバルを置く
                interval = true;
            }
            else if (other.gameObject.CompareTag("cube"))
            {//どれかのブロックに当たった
                //下がるブロックを置く
                GameManager.I.SetDownblock(other.transform.position);
                //当たったオブジェクトは非表示にする
                other.gameObject.SetActive(false);
            }
            else if (other.gameObject.CompareTag("trampoline"))
            {//トランポリンブロックに当たった
                //アニメーションのためコルーチンへ
                StartCoroutine(trampoline_move());
                //トランポリンブロックも下がるブロックに変換
                GameManager.I.SetDownblock(other.transform.position);
                other.gameObject.SetActive(false);
            }
        }
    }

    //プレイヤーの攻撃
    IEnumerator trampoline_move()
    {
        float t = 0;
        //現在の位置
        Vector3 pos = transform.position;

        //ボスの位置まで補間移動する
        while (t <= 1)
        {
            //補間
            transform.position = Vector3.Slerp(pos, startpos, t);
            //待つ
            yield return new WaitForSecondsRealtime(0.01f);
            //ちょっとアニメーションを進ませる
            t += 0.01f;
        }
    }

    //狙う位置（プレイヤーの上空）
    public void SetTargetPos_z(float z)
    {
        targetpos = new Vector3(GameManager.I.Playerpos.x, GameManager.I.Playerpos.y + 10f, GameManager.I.Playerpos.z+z);
    }
}
