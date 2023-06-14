using System.Collections;
using UnityEngine;

//プレイヤーの登る機能
public class clime : MonoBehaviour
{
    //アニメーション
    private Animator anim;
    //当たったブロックの位置
    private Vector3 blockpos;

    // Start is called before the first frame update
    void Start()
    {
        //コンポーネント取得
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //一応どんなふうに確認しているのか確認
        //プレイヤーの斜め上に何もないか確認しています
        Debug.DrawRay(new Vector3(transform.position.x, transform.position.y + 1.7f, transform.position.z), transform.forward * 1.0f, Color.red);
    }

    private void OnTriggerEnter(Collider other)
    {
        //プレイ中にキューブに当たった
        if (GameManager.I.gamestate("Play") && other.gameObject.CompareTag("cube"))
        {
            //光線を頭ぐらいの位置に出して
            Ray ray = new Ray(new Vector3(transform.position.x, transform.position.y + 1.7f, transform.position.z), transform.forward);
            Debug.DrawRay(new Vector3(transform.position.x, transform.position.y + 1.7f, transform.position.z), transform.forward * 1.0f, Color.red);

            //斜め上に何があるか見る
            RaycastHit hit;
            Physics.Raycast(ray, out hit, 1.0f);
            Debug.Log(hit.collider == null);

            //何もなければ登る
            if (hit.collider == null)
            {
                Debug.Log("OK");
                //目の前のブロックの位置を登録する
                blockpos = other.transform.position;
                //登るアニメーションをする
                anim.SetTrigger("clime");
            }
            else
            {
                Debug.Log("登れない場所のはず");
            }
        }
    }

    //アニメーションのイベントから呼び出される
    //色々な兼ね合いから無理やり座標を上げて駆け上がったように見せてる
    IEnumerator clime_move()
    {
        float f = 0f;
        Debug.Log(blockpos);
        //現在のプレイヤーの位置
        Vector3 startpos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        //当たったブロックの一つ上の位置
        Vector3 endpos = new Vector3(blockpos.x, blockpos.y + 1f, blockpos.z);
        Debug.Log(startpos);
        Debug.Log(endpos);

        //駆け上がるまで繰り返す（補完待ち）
        while (f <= 1.0f)
        {
            //補完しながら上に行く
            transform.root.position = Vector3.Slerp(startpos, endpos, f);
            //状況加算
            f += 0.05f;
            //時間単位で待つ
            yield return new WaitForSecondsRealtime(0.005f);
        }
    }
}
