using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//ラスボスの制御
public class BossManager : MonoBehaviour
{
    //ボス用ブロック生成先
    [SerializeField] Transform boss_block_p;
    //ボス用ブロックプレハブ
    [SerializeField] GameObject boss_block;

    //最初にしゃべるのでテキスト
    [SerializeField] TextMeshProUGUI bosstext;
    //しゃべる内容配列
    [SerializeField] string[] bosstextlist;

    //マテリアル変える用
    MeshRenderer meshRenderer;

    //普通とダメージの色
    [SerializeField] Material nomal;
    [SerializeField] Material damage;

    //ボスのライフ
    [SerializeField] int boss_life;

    [SerializeField] GameObject effect;

    // Start is called before the first frame update
    void Start()
    {
        //最終ステージじゃなければオブジェクトを消す
        if (!MapData.mapinstance.Last) this.gameObject.SetActive(false);

        //ボスの初期体力値
        boss_life = 75;

        //テキストの演出をするため、最初にテキストを消す
        bosstext.gameObject.SetActive(false);

        //コンポーネントを取得
        meshRenderer = GetComponent<MeshRenderer>();

        //しゃべり演出
        StartCoroutine(start_text());
    }

    //しゃべり演出
    IEnumerator start_text()
    {
        //ずっとループさせる
        while (true)
        {
            //ステージ名の演出が終わった後スタート
            if (GameManager.I.gamestate("Perform"))
            {
                //配列用
                int i = 0;

            LOOP://LOOP駅
                //文字列を適応させる
                bosstext.text = bosstextlist[i];
                yield return null;
                //オブジェクトを表示にしてテキスト演出を開始する
                bosstext.gameObject.SetActive(true);

                while (true)
                {
                    //もし何かキーを押してテキストが表示状態なら
                    if (Input.anyKey && bosstext.gameObject.activeInHierarchy)
                    {
                        //次のテキストへ
                        i++;
                        //0.2秒待つ
                        yield return new WaitForSecondsRealtime(0.2f);
                        //テキストオブジェクトを非表示にする
                        bosstext.gameObject.SetActive(false);

                        //もしまだ文字列配列に文字列があればLOOP駅へワープする
                        //無ければ攻撃の動きへ
                        if (i < bosstextlist.Length) goto LOOP;
                        else StartCoroutine(boss_move());
                    }
                    yield return null;
                }
            }
            else
            {
                //プレイ中などになればこのコルーチンは止める
                yield return null;
                StopCoroutine(start_text());
            }
        }
    }

    //ボスの攻撃
    IEnumerator boss_move()
    {
        //テキストを空に
        bosstext.text = "";
        //ボスのしゃべりが終わった事を知らせ、ゲームを開始にする
        GameManager.I.OnPerform_end();

        while (true)
        {
            //プレイ中なら
            if (GameManager.I.gamestate("Play"))
            {
                //ランダムな時間攻撃待機する（1秒から5秒）
                yield return new WaitForSeconds(Random.Range(1, 6));

                //攻撃パターンRandom.Range(1, 3)
                //通常攻撃80％　特殊全体10％＋10％のはず
                switch (Random.Range(1, 11))
                {
                    case 1:
                        setBossBlock(5);
                        yield return new WaitForSecondsRealtime(0.5f);
                        setBossBlock(4);
                        yield return new WaitForSecondsRealtime(0.5f);
                        setBossBlock(3);
                        yield return new WaitForSecondsRealtime(0.5f);
                        setBossBlock(2);
                        yield return new WaitForSecondsRealtime(0.5f);
                        setBossBlock(1);
                        yield return new WaitForSecondsRealtime(0.5f);
                        setBossBlock(0);
                        break;
                    case 2:
                        setBossBlock(-5);
                        yield return new WaitForSecondsRealtime(0.5f);
                        setBossBlock(-4);
                        yield return new WaitForSecondsRealtime(0.5f);
                        setBossBlock(-3);
                        yield return new WaitForSecondsRealtime(0.5f);
                        setBossBlock(-2);
                        yield return new WaitForSecondsRealtime(0.5f);
                        setBossBlock(-1);
                        yield return new WaitForSecondsRealtime(0.5f);
                        setBossBlock(0);
                        break;
                    default:
                        setBossBlock(0);
                        break;
                }
                yield return null;
            }
            //プレイ中以外は何もしない
            else { yield return null; }
        }
    }

    void setBossBlock(float z)
    {
        //攻撃
        //重くなるので再利用できるものがあるか探す
        foreach (Transform t in boss_block_p)
        {
            //非表示の待機オブジェクトがあったら
            if (!t.gameObject.activeInHierarchy)
            {
                //ポジションをボスの場所へ
                t.transform.position = transform.position;
                t.gameObject.GetComponent<boss_block>().SetTargetPos_z(z);
                //表示状態にする
                t.gameObject.SetActive(true);
                //次の攻撃に移るためにBOSS_BLOCK_END駅へ
                goto BOSS_BLOCK_END;
            }
        }
        //もし再利用できるオブジェクトがなく、ここまで来てしまったら新たに生成する
        GameObject obj = Instantiate(boss_block, transform.position, Quaternion.identity, boss_block_p);
        obj.GetComponent<boss_block>().SetTargetPos_z(z);

    //BOSS_BLOCK_END駅
    BOSS_BLOCK_END:;
    }


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.tag);
        //もし、攻撃が帰ってきたら
        if (other.gameObject.CompareTag("boss_block"))
        {
            //ボスのライフが2つ減る
            boss_life -= 2;
            //右上の表示に体力分加算する
            GameManager.I.Add_Blocknum += 2;

            //もし倒したらラストムービーへ
            if (boss_life <= 0)
            {
                //GameManagerに通知する
                GameManager.I.OnClear();
                StartCoroutine(boss_down());
            }


            //ダメージを受けたとして赤色にする
            meshRenderer.material = damage;
            //0.5秒後に色を戻す
            Invoke("color_reset", 0.5f);
        }
    }

    IEnumerator boss_down()
    {
        for (int i = 0; i < 5; i++)
        {
            effect.gameObject.SetActive(true);
            yield return new WaitForSecondsRealtime(0.5f);
            effect.gameObject.SetActive(false);
        }

    }

    //色を戻す
    void color_reset()
    {
        //普通の色へ
        meshRenderer.material = nomal;
    }

    //リセットボタンを押すと呼ばれる
    public void OnReset()
    {
        //ボスのブロックを全て回って非表示にする
        foreach (Transform t in boss_block_p)
        {
            //オブジェクトが表示状態なら
            if (t.gameObject.activeInHierarchy)
            {
                //非表示にする
                t.gameObject.SetActive(false);
            }
        }

        //ボスのライフを初期値へ
        boss_life = 75;
    }
}
