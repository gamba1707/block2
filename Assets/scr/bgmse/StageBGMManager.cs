using UnityEngine;

//ステージBGMを管理する
public class StageBGMManager : MonoBehaviour
{
    //オーディオソース
    AudioSource audioSource;
    //それぞれのステージで流すBGM
    [Header("ステージBGM")]
    [SerializeField] AudioClip BOSSBGM;
    [SerializeField] AudioClip FINALBGM;
    [SerializeField] AudioClip stage0BGM;
    [SerializeField] AudioClip stage1BGM;
    [SerializeField] AudioClip stage2BGM;
    [SerializeField] AudioClip stage3BGM;

    void Start()
    {
        //コンポーネントを取得
        audioSource = GetComponent<AudioSource>();

        Debug.Log(MapData.mapinstance.mapname().Substring(0, 1));
        Debug.Log("BOSS:" + MapData.mapinstance.Boss);
        //ボスステージなら
        if (MapData.mapinstance.Boss ||MapData.mapinstance.Boss_Reverse)
        {
            //ボスステージのBGMにする
            audioSource.clip = BOSSBGM;
        }
        else if (MapData.mapinstance.Last)
        {
            //ラスボスステージのBGMにする
            audioSource.clip = FINALBGM;
        }
        else
        {
            //ボスステージじゃない場合
            //ステージ名頭文字の数字で分ける
            switch (MapData.mapinstance.mapname().Substring(0, 1))
            {
                case "0":
                    audioSource.clip = stage0BGM;
                    break;
                case "1":
                    audioSource.clip = stage1BGM;
                    break;
                case "2":
                    audioSource.clip = stage2BGM;
                    break;
                case "3":
                    audioSource.clip = stage3BGM;
                    break;
            }
        }
        //再生する
        audioSource.Play();
    }

    void Update()
    {
        //ゲームクリアしたとき
        if (GameManager.I.gamestate("GameClear"))
        {
            //BGMを止める
            audioSource.Stop();
        }
    }
}
