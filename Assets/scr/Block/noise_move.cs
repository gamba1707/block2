using UnityEngine;

//ボスステージで追いかけてくるノイズ
public class noise_move : MonoBehaviour
{
    //初期値に戻すための格納場所
    private Vector3 firstpos;

    void Start()
    {
        //ボスステージじゃなければこのオブジェクトは消しておく
        if (!MapData.mapinstance.Boss) this.gameObject.SetActive(false);

        //初期値登録
        firstpos = transform.position;

        //もし反転設定されていたら
        if (MapData.mapinstance.Boss_Reverse)
        {
            //反転なら再度オンに
            this.gameObject.SetActive(true);
            //逆の位置に設定
            transform.position = new Vector3(firstpos.x, firstpos.y, -1 * firstpos.z);
            //初期値登録
            firstpos = transform.position;
        }
    }

    void FixedUpdate()
    {
        //プレイ中なら
        if (GameManager.I.gamestate("Play"))
        {
            if (MapData.mapinstance.Boss_Reverse)
            {
                //左方向に移動させる
                transform.position -= new Vector3(0, 0, 0.025f);
            }
            else
            {
                //右方向に移動させる
                transform.position += new Vector3(0, 0, 0.025f);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //プレイ中で、Playerに当たったら
        if (GameManager.I.gamestate("Play") && other.gameObject.name.Equals("Player"))
        {
            //ゲームオーバーにする
            GameManager.I.OnGameOver();
        }
    }

    //リセットで呼ばれる
    public void noise_reset()
    {
        //初期値に戻す
        transform.position = firstpos;
    }
}
