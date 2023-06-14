using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System;
using System.IO;


//ステージシーンで全てを持っているスクリプト
public class GameManager : MonoBehaviour
{
    //インスタンス
    public static GameManager I;

    [Header("Editerモード(編集している)")]
    [SerializeField] private bool editmode;
    [SerializeField] private MapData_scrobj edit_mapdata;

    [Header("ゲームの状態")]
    [SerializeField] private GAME_STATUS game_status;
    private enum GAME_STATUS { Play, GameClear, Pause, GameOver }

    [Header("マップデータ")]
    private MapData mapdata;

    [Header("最初に表示するステージ名テキスト")]
    [SerializeField] private TextMeshProUGUI StagenameText;

    [Header("ステータステキスト")]
    [SerializeField] StatusText statustext;

    [Header("Player_move(Playerに付いてる)")]
    [SerializeField] private Player_move pmove;

    [Header("PoolManager(addBlock)")]
    [SerializeField] private PoolManager pManager;

    [Header("ポーズ画面")]
    [SerializeField] private GameObject PousePanel;

    [Header("ゲームオーバー画面")]
    [SerializeField] private GameObject GameOverPanel;

    [Header("ロード画面")]
    [SerializeField] private Loading_fade LoadUI;

    [Header("ロード画面")]
    [SerializeField] private noise_move noise;

    [Header("現在までに出現させたブロック数")]
    [SerializeField] private int add_blocknum;
    [Header("目標のブロック数")]
    [SerializeField] private int add_blocknum_goal;


    [Header("通常のブロック（初期化時に控えておく数）")]
    [SerializeField] private int nomalnum;
    [Header("その他のブロック（初期化時に控えておく数）")]
    [SerializeField] private int blocknum;

    [Header("セレクトしているブロック名")]
    [SerializeField] private string selectname;

    [Header("プレイヤーの位置（一マス単位）")]
    [SerializeField] private Vector3 playerpos;


    private void Awake()
    {
        //変数Iがnullならば
        if (I == null)
        {
            //Iに自身（GameManager）を代入
            I = this;
        }

        //止まっている場合困るので一応1にしておく
        Time.timeScale = 1;

        //マップデータを読み込む
        //ゲームを一時停止状態にする
        game_status = GAME_STATUS.Pause;
        //マップデータを持っているオブジェクトを取得する
        mapdata = GameObject.Find("MapData").GetComponent<MapData>();
        //もし編集モード（デバッグやクリエイトモード）なら
        if (Editmode)
        {
            Debug.Log("<color=cyan>EditMode</color>");
            //Json形式のマップデータを持っているとき（クリエイトの編集）
            if (mapdata.jsonpath_enable())
            {
                //Jsonを読み込んで構築する
                mapdata.LoadMapData_Create();
            }
            else
            {
                //デバッグとして割り当てたステージファイルを読み込む
                mapdata.LoadMapData(edit_mapdata);
            }
            //この場合はスタート演出が要らないのでスタートする
            game_status = GAME_STATUS.Play;
        }
        else if (MapData.mapinstance.Createmode)
        {
            //クリエイトモードで遊ぶを押した場合にJson形式のデータを読んで構築する
            mapdata.LoadMapData_Create();
        }
        else
        {
            //ストーリーモードのステージを読み取る
            //前のシーンで既にステージデータを持ってきているのでnullを送る
            mapdata.LoadMapData(null);
        }
    }

    private void OnGUI()
    {
        /*
        if(Editmode)
        {
            GUIStyle style = new GUIStyle();
            style.fontSize = 32;
            style.normal.textColor = Color.white;
            string s = "現在、デバッグモードが有効です\nステージセレクトのデータは読み取られていません";
            string s2 = "\n現在のステージ："+edit_mapdata.name;
            GUI.Label(new Rect(200, 0, Screen.width, Screen.height), s+s2, style);
        }
        */
    }


    private void Start()
    {
        //開けたときにステージ名を表示する
        SetStagename(mapdata.mapname_text());
        //ロード画面を開ける
        LoadUI.Fadein();

    }

    //ここから初期にMapDataからPoolManagerに座標を送って構築する
    //床ブロック
    public void SetFloorblock(Vector3[] pos)
    {
        for (int i = 0; i < pos.Length; i++)
        {
            pManager.GetFloorObject(pos[i]);
        }
    }
    //奈落ブロック
    public void SetFallblock(Vector3[] pos)
    {
        for (int i = 0; i < pos.Length; i++)
        {
            pManager.GetFallObject(pos[i]);
        }
    }
    //地形飛べるブロック
    public void SetBeforeTrampolineblock(Vector3[] pos)
    {
        for (int i = 0; i < pos.Length; i++)
        {
            pManager.GetTrampolineObject_before(pos[i]);
        }
    }
    //地形下がるブロック
    public void SetBeforeDownblock(Vector3[] pos)
    {
        for (int i = 0; i < pos.Length; i++)
        {
            pManager.GetDownObject_before(pos[i]);
        }
    }
    //ゴール場所
    public void SetGoalblock(Vector3 pos)
    {
        pManager.GetGoalObject(pos);
    }

    //最初に表示される文字（ステージ名）を設定できる
    public void SetStagename(string s)
    {
        //もらった文字列を設定する
        StagenameText.text = s;
        //もし空文字列なら
        if (s.Equals(""))
        {
            //消す（ブロックの生成に影響するため）
            StagenameText.enabled = false;
            //始める
            game_status = GAME_STATUS.Play;
            //出現する演出をする
            pmove.start_move();
        }
    }


    private void Update()
    {
        //tabキーを押すとポーズ画面
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (PousePanel.activeInHierarchy)
            {
                //もしもうポーズ画面が出ていたら消す
                OnPouseback();
            }
            else if (gamestate("Play"))
            {
                //プレイ中にTABを押したらポーズにする
                game_status = GAME_STATUS.Pause;
                //ポーズ画面を出す
                PousePanel.SetActive(true);
                //動きがそのまま止まってた方がかっこよさそう
                Time.timeScale = 0;
            }
        }

        //もし奈落をすり抜けて落ちてしまったら強制的にリセットする
        if (MapData.mapinstance.Deadline >= Playerpos.y) OnGameReset();
    }

    //===============================
    //クリエイトモードセーブボタン用
    public void Save(TMP_InputField inputField)
    {
        Debug.Log(inputField.text);
        Debug.Log(inputField.text.Equals(""));

        if (inputField.text.Equals(""))
        {
            //もし空欄で押された場合はエラー的な文で返す
            statustext.SetStatusText("<color=red>ステージ名を入れてください</color>");
        }
        else
        {
            //大丈夫そうならステージデータを保存してしまう
            if (Directory.Exists(Application.dataPath + "/StageData_Create"))
            {
                //StageData_Createフォルダーが既にあればそのまま保存する
                mapdata.OnMapSave_json(inputField.text, Add_Blocknum, pManager.Floor_parent, pManager.Fall_parent, pManager.Trampoline_parent_before, pManager.Down_parent_before, pManager.Goalpos);
            }
            else
            {
                //StageData_Createフォルダーが無ければ作る
                Directory.CreateDirectory(Application.dataPath + "/StageData_Create");
                //ステージを保存する
                mapdata.OnMapSave_json(inputField.text, Add_Blocknum, pManager.Floor_parent, pManager.Fall_parent, pManager.Trampoline_parent_before, pManager.Down_parent_before, pManager.Goalpos);
            }
            //ステータス文にセーブできたと表示する
            statustext.SetStatusText("Stage_Create/" + inputField.text + ".jsonに保存しました");
        }
    }

    //クリエイトモードでセーブボタンを押した時
    //入力された目標数を取得する
    public void SetAdd_Blocknum_Create(TMP_InputField inputField)
    {
        int n;
        //変換を試みる
        bool isInt = int.TryParse(inputField.text.ToString(), out n);
        //ちゃんと半角数字なら適応する
        //違いそうならエラー文で返す
        if (isInt) Add_Blocknum = n;
        else statustext.SetStatusText("<color=red>目標数に半角数字を入力してください</color>");
    }
    //===============================

    //名前を与えると現在ゲームがどれの状態なのか教えてくれる
    public bool gamestate(string s)
    {
        if (s.Equals(game_status.ToString())) return true;
        return false;
    }

    //クリアするとGoal.csから呼ばれる
    public void OnClear()
    {
        //クリアにする
        game_status = GAME_STATUS.GameClear;
        //ステータスにセーブを表示する
        statustext.SetStatusText("セーブ中");
        //クリアした動きをさせる
        pmove.Clear_move();
    }

    //プレイヤーが演出で消えた後呼ばれる
    public void OnClear_end()
    {
        //クリエイトモードの遊ぶの場合はタイトルに戻らせる
        if (MapData.mapinstance.Createmode)
        {
            //ロード画面
            LoadUI.Fadeout();
            //タイトルに飛ばす
            StartCoroutine(Scene_move("title"));
        }
        else
        {
            //ストーリーモード
            Debug.Log("save" + mapdata.mapname());
            //クリアしたことをセーブする
            SaveManager.instance.SaveData(mapdata.mapname(), Add_Blocknum);
            //ステージセレクトに帰る
            OnStageSelect();
        }
    }

    //ポーズ画面をやめる
    public void OnPouseback()
    {
        //ポーズ画面を消す
        PousePanel.SetActive(false);
        //プレイにする
        game_status = GAME_STATUS.Play;
        //止めたのを元に戻す
        Time.timeScale = 1;
    }

    //ゲームオーバーになったら呼ばれる
    public void OnGameOver()
    {
        //状態をゲームオーバーにする
        game_status = GAME_STATUS.GameOver;
        //プレイヤーをゲームオーバー風な見た目にする
        pmove.GameOver_move();
        //ゲームオーバーのパネルを出す
        GameOverPanel.SetActive(true);
    }

    //リセットすると呼ばれる
    public void OnGameReset()
    {
        //下のコルーチンに任せる
        StartCoroutine(GameReset_move());
    }
    IEnumerator GameReset_move()
    {
        //画面を暗転する
        LoadUI.Fadeout();
        //暗転するまで待つ
        while (LoadUI.Fade_move) yield return null;
        //挙動がおかしいので1にする
        Time.timeScale = 1;
        //プレイヤー関連の初期化
        pmove.Reset_move();
        //置いたブロック数を初期化
        Add_Blocknum = 0;
        //置いたブロックを初期化
        pManager.Reset_box();
        //とりあえずボスステージの位置を初期値に戻す
        noise.noise_reset();
        //表示しているゲームオーバー画面を消す
        if (GameOverPanel.activeInHierarchy) GameOverPanel.SetActive(false);
        //ポーズ画面を消す
        else if (PousePanel.activeInHierarchy) PousePanel.SetActive(false);
        //明転する
        LoadUI.Fadein();
        //しきるまで待つ
        while (LoadUI.Fade_move) yield return null;
        //状態をプレイ中にする
        game_status = GAME_STATUS.Play;
        //プレイヤーが出現する演出をする
        pmove.start_move();
    }

    //ステージセレクトに戻る
    public void OnStageSelect()
    {
        LoadUI.Fadeout();
        StartCoroutine(Scene_move("Select"));
    }

    //タイトルに戻る
    public void OnTitleBack()
    {
        LoadUI.Fadeout();
        StartCoroutine(Scene_move("title"));
    }

    //シーン移動時にフェードもしつつシーン読み込みもしたかった
    IEnumerator Scene_move(string scenename)
    {
        //シーン読み込む
        var async = SceneManager.LoadSceneAsync(scenename);
        //ちょっと待ってもらう
        async.allowSceneActivation = false;
        //フェードが終わるまで待つ
        while (LoadUI.Fade_move)
        {
            yield return null;
        }
        yield return null;
        //動きをもとに戻す
        Time.timeScale = 1;
        //移動する
        async.allowSceneActivation = true;
    }


    //以下プロパティ
    public bool Editmode
    {
        get { return editmode; }
    }

    public int Add_Blocknum
    {
        get { return add_blocknum; }
        set { add_blocknum = value; }
    }
    public int Add_Blocknum_goal
    {
        get { return add_blocknum_goal; }
        set { add_blocknum_goal = value; }
    }

    public int Nomalnum
    {
        get { return nomalnum; }
    }
    public int Blocknum
    {
        get { return blocknum; }
    }

    public string Selectname
    {
        get { return selectname; }
        set { selectname = value; }
    }

    public Vector3 Playerpos
    {
        get { return playerpos; }
        set
        {
            //もらった数値を1.5の倍数値に変換してから格納
            //y軸だけはPlayerの足元なので+1.5fする必要性
            float x = (float)(Math.Round((value.x / 1.5f), 0, MidpointRounding.AwayFromZero) * 1.5f);
            float y = (float)(Math.Round((value.y / 1.5f), 0, MidpointRounding.AwayFromZero) * 1.5f) + 1.5f;
            float z = (float)(Math.Round((value.z / 1.5f), 0, MidpointRounding.AwayFromZero) * 1.5f);
            playerpos = new Vector3(x, y, z);
        }
    }
}
