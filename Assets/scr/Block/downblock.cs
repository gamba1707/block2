using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//下がるブロック
public class downblock : MonoBehaviour
{
    //通常時と下がっているときのマテリアル
    [SerializeField] Material nomalmaterial,downmaterial;
    //初期値用
    Vector3 firstpos;
    //マテリアル変える用
    MeshRenderer meshRenderer;
    //下がっているときに使用
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        //初期値登録
        firstpos = transform.position;
        //コンポーネントを取得
        meshRenderer = GetComponent<MeshRenderer>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //速度が最大で-1で下がり続けるようにする
        if(rb.velocity.y<-1f||rb.velocity.y>0)rb.velocity = new Vector3(0, -1f, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        //プレイヤーが触れたら
        if (other.gameObject.name.Equals("Player"))
        {
            //下がっている色にする
            meshRenderer.material= downmaterial;
            //2秒後に下がり始める
            Invoke("down_move",2f);
        }
    }

    //動き出す命令
    void down_move()
    {
        //RiditBodyの重力を使う
        rb.useGravity = true;
        
        //Yだけ解除して落下させる
        rb.constraints = RigidbodyConstraints.FreezeRotation| RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
        //落下速度を指定
        rb.velocity = new Vector3(0, -1f, 0);
    }

    //カメラから外れたら呼ばれる
    private void OnBecameInvisible()
    {
        //一旦このオブジェクトを消す
        this.gameObject.SetActive(false);
        //初期値に戻す
        transform.position = firstpos;
        //重力を切る
        rb.useGravity = false;
        //全部固定する
        rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePosition;
        //通常の色に戻す
        meshRenderer.material= nomalmaterial;
        //完了したのでオブジェクトを表示する
        this.gameObject.SetActive(true);
    }
}
