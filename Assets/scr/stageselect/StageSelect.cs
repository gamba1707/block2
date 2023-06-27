using UnityEngine;
using TMPro;

//ステージセレクトを大きく管理している
public class StageSelect : MonoBehaviour
{
    [Header("ブロック関連")]
    [SerializeField] int stagenum;//何番目のステージに注目しているか
    [SerializeField] MeshRenderer centercube, leftcube, rightcube;//下で回転しているブロック
    [SerializeField] Material blue, yellow, green, clear, gray;//下のブロックの色を変える用
    [SerializeField] private MapData_scrobj[] stagedata;//全てのステージデータを順番に格納
    [SerializeField] bool EditMode;//クリアしてないときに動作確認

    [Header("文字関連")]
    [SerializeField] TextMeshProUGUI StageText;//中央にあるステージ名
    [SerializeField] TextMeshProUGUI ScoreText;//中央にあるスコア

    //音
    AudioSource audioSource;

    //瞬間で端にいかないように左右押してもインターバルを置く
    bool interval;

    // Start is called before the first frame update
    void Start()
    {
        //もしすべてのステージをクリアしていたらそれ以上行き過ぎないようにする
        //それ以外は次のステージをフォーカスする
        if (SaveManager.instance.clearnum() >= stagedata.Length) stagenum = SaveManager.instance.clearnum() - 1;
        else stagenum = SaveManager.instance.clearnum();

        //コンポーネント取得
        audioSource = GetComponent<AudioSource>();
        //ブロックの色とステージ情報を更新
        setstageinfo();
    }

    // Update is called once per frame
    void Update()
    {
        //動かしてからある程度たっていたら
        if (!interval)
        {
            //==========================
            //ここから左右押したときの色設定
            //Editモードをオンにするとインスペクターでいじくりまわせるはず
            if (EditMode)
            {
                //ブロックの色とステージ情報を更新
                setstageinfo();
            }
            //左に押した場合、さらに左にステージがある場合
            if (Input.GetAxis("Horizontal") < 0 && stagenum > 0)
            {
                //左のひとつ前のステージにフォーカスを変える
                stagenum--;
                //音
                audioSource.PlayOneShot(audioSource.clip);
                //ブロックの色とステージ情報を更新
                setstageinfo();
            }
            //右に押した場合、現在の位置がクリア数より小さい場合
            if (Input.GetAxis("Horizontal") > 0 && stagenum < SaveManager.instance.clearnum() && stagenum + 1 < stagedata.Length)
            {
                //次のステージにフォーカスする
                stagenum++;
                //音
                audioSource.PlayOneShot(audioSource.clip);
                //ブロックの色とステージ情報を更新
                setstageinfo();
            }
            //==========================

            //決定ボタンを押した
            if (Input.GetButtonDown("Submit"))
            {
                Debug.Log("押した");
                //音
                audioSource.PlayOneShot(audioSource.clip);
                //どうしてもUIの関係上透明にしないと文字が見切れるため右ブロックを透明にする
                rightcube.material = clear;
                //上のSelectUIにデータを送る
                transform.root.gameObject.GetComponent<selectUI>().OnClickButton(stagedata[stagenum]);
            }
        }
    }

    //番号を与えるとセーブデータの情報から色を返してくれる
    //目標以上のクリア：青色、とりあえずクリア：黄色、新しいステージ：緑色、まだ：灰色
    Material colorBlock(int num)
    {
        if (num < 0 || num >= stagedata.Length) return clear;
        else if (num > SaveManager.instance.clearnum()) return gray;
        else if (SaveManager.instance.exClearstage(stagedata[num])) return blue;
        else if (SaveManager.instance.Clearstage(stagedata[num])) return yellow;
        else if (num == SaveManager.instance.clearnum()) return green;
        return gray;
    }

    //テキストやブロックの色などを更新する
    void setstageinfo()
    {
        //操作間隔をあける
        interval = true;
        //下のブロックの色を変える
        leftcube.material = colorBlock(stagenum - 1);
        centercube.material = colorBlock(stagenum);
        rightcube.material = colorBlock(stagenum + 1);
        //ステージ名に変更する
        StageText.text = stagedata[stagenum].name;
        //スコアを変更
        ScoreText.text = SaveManager.instance.clearscore(stagedata[stagenum].name) + "/" + stagedata[stagenum].clearnum;
        //0.5秒後にまた動けるようにする
        Invoke("interval_reset", 0.5f);
    }

    //インターバルをオフに
    void interval_reset()
    {
        interval = false;
    }
}
