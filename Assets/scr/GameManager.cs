using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using TMPro;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;
using System.IO;

public class GameManager : MonoBehaviour
{
    public static GameManager I;

    [Header("Editerモード(編集している)")]
    [SerializeField] private bool editmode;
    [SerializeField] private MapData_scrobj edit_mapdata;


    [Header("ゲームの状態")]
    [SerializeField] private GAME_STATUS game_status;
    private enum GAME_STATUS { Play, GameClear, Pause, GameOver}

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



    //ゲーム開始直後に処理を行う
    private void Awake()
    {
        //変数Iがnullならば
        if (I == null)
        {
            //Iに自身（GameManager）を代入
            I = this;
        }
        Time.timeScale = 1;
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
        game_status = GAME_STATUS.Pause;
        mapdata = GameObject.Find("MapData").GetComponent<MapData>();
        if (Editmode)
        {
            Debug.Log("<color=cyan>EditMode</color>");
            if (mapdata.jsonpath_enable())
            {
                mapdata.LoadMapData_Create();
            }
            else
            {
                mapdata.LoadMapData(edit_mapdata);
            }
            game_status = GAME_STATUS.Play;
        }
        else if (MapData.mapinstance.Createmode)
        {
            mapdata.LoadMapData_Create();
        }
        else
        {
            mapdata.LoadMapData(edit_mapdata);
        }
        SetStagename(mapdata.mapname_text());
        LoadUI.Fadein();//ロード画面を開ける
        
    }
    public void SetFloorblock(Vector3[] pos)
    {
        for(int i = 0;i< pos.Length; i++)
        {
            pManager.GetFloorObject(pos[i]);
        }
    }
    public void SetFallblock(Vector3[] pos)
    {
        for(int i = 0;i< pos.Length; i++)
        {
            pManager.GetFallObject(pos[i]);
        }
    }
    public void SetBeforeTrampolineblock(Vector3[] pos)
    {
        for(int i = 0;i< pos.Length; i++)
        {
            pManager.GetTrampolineObject_before(pos[i]);
        }
    }
    public void SetBeforeDownblock(Vector3[] pos)
    {
        for(int i = 0;i< pos.Length; i++)
        {
            pManager.GetDownObject_before(pos[i]);
        }
    }

    public void SetGoalblock(Vector3 pos)
    {
        pManager.GetGoalObject(pos);
    }

    public void SetStagename(string s)
    {
        StagenameText.text= s;
        if (s.Equals(""))
        {
            StagenameText.enabled = false;
            game_status = GAME_STATUS.Play;
        }
    }


    private void Update()
    {

        //tabキーを押すとポーズ画面を出す
        if (Input.GetKeyDown(KeyCode.Tab)&&gamestate("Play"))
        {
            game_status = GAME_STATUS.Pause;
            PousePanel.SetActive(true);
            Time.timeScale = 0;
        }
        if (MapData.mapinstance.Deadline >= Playerpos.y) OnGameReset();
    }

    //===============================
    //クリエイトモードセーブボタン用
    public void Save(TMP_InputField inputField)
    {
        Debug.Log(inputField.text);
        Debug.Log(inputField.text.Equals(""));
        
        if (inputField.text.Equals(""))
        {//もし空欄で押された場合はエラー的な文で返す
            statustext.SetStatusText("<color=red>ステージ名を入れてください</color>");
        }
        else
        {//大丈夫そうならステージデータを保存してしまう
            if (Directory.Exists(Application.dataPath + "/StageData_Create"))
            {
                mapdata.OnMapSave_json(inputField.text, Add_Blocknum, pManager.Floor_parent, pManager.Fall_parent, pManager.Trampoline_parent_before, pManager.Down_parent_before, pManager.Goalpos);
            }
            else
            {
                Directory.CreateDirectory(Application.dataPath + "/StageData_Create");
                mapdata.OnMapSave_json(inputField.text, Add_Blocknum, pManager.Floor_parent, pManager.Fall_parent, pManager.Trampoline_parent_before, pManager.Down_parent_before, pManager.Goalpos);
            }
                
            statustext.SetStatusText("Stage_Create/" + inputField.text + ".jsonに保存しました");
        }
    }

    //クリエイトモードでセーブボタンを押した時
    //入力された目標数を取得する
    public void SetAdd_Blocknum_Create(TMP_InputField inputField)
    {
        int n;
        //変換を試みる
        bool isInt=int.TryParse(inputField.text.ToString(),out n);
        //ちゃんと半角数字なら適応する
        //違いそうならエラー文で返す
        if (isInt)Add_Blocknum = n;
        else statustext.SetStatusText("<color=red>目標数に半角数字を入力してください</color>");
    }
    //===============================

    //名前を与えると現在ゲームがどれの状態なのか教えてくれる
    public bool gamestate(string s)
    {
        if (s.Equals(game_status.ToString())) return true;
        return false;
    }
    //クリアするとGoalから呼ばれる
    public void OnClear()
    {
        game_status = GAME_STATUS.GameClear;
        statustext.SetStatusText("セーブ中");
        pmove.Clear_move();
    }

    public void OnClear_end()
    {
        if (MapData.mapinstance.Createmode)
        {
            LoadUI.Fadeout();
            StartCoroutine(Scene_move("title"));
        }
        else
        {
            Debug.Log("save"+mapdata.mapname());
            SaveManager.instance.SaveData(mapdata.mapname(), Add_Blocknum, Add_Blocknum_goal);
            OnStageSelect();
        }
        
    }

    //ポーズ画面をやめる
    public void OnPouseback()
    {
        PousePanel.SetActive(false);
        game_status = GAME_STATUS.Play;
        Time.timeScale = 1;
    }
    public void OnGameOver()
    {
        game_status = GAME_STATUS.GameOver;
        pmove.GameOver_move();
        GameOverPanel.SetActive(true);
    }

    public void OnGameReset()
    {
        StartCoroutine(GameReset_move());
    }
    IEnumerator GameReset_move()
    {
        LoadUI.Fadeout();
        while(LoadUI.Fade_move) yield return null;
        Time.timeScale = 1;
        if (!Editmode) pmove.Reset_move();//プレイヤー関連の初期化
        Add_Blocknum = 0;//置いたブロック数を初期化
        pManager.Reset_box();//置いたブロックを初期化
        noise.noise_reset();
        if(GameOverPanel.activeInHierarchy)GameOverPanel.SetActive(false);//表示しているゲームオーバー画面を消す
        else if(PousePanel.activeInHierarchy)PousePanel.SetActive(false);
        LoadUI.Fadein();
        while (LoadUI.Fade_move) yield return null;
        if (!Editmode) pmove.Reset_move();//プレイヤー関連の初期化
        game_status = GAME_STATUS.Play;
    }

    public void OnStageSelect()
    {
        LoadUI.Fadeout();
        StartCoroutine(Scene_move("Select"));
    }
    public void OnTitleBack()
    {
        LoadUI.Fadeout();
        StartCoroutine(Scene_move("title"));
    }
    IEnumerator Scene_move(string scenename)
    {
        var async = SceneManager.LoadSceneAsync(scenename);
        async.allowSceneActivation = false;
        while (LoadUI.Fade_move)
        {
            yield return null;
        }
        yield return null;
        Time.timeScale = 1;
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
            float y = (float)(Math.Round((value.y / 1.5f), 0, MidpointRounding.AwayFromZero) * 1.5f)+1.5f;
            float z = (float)(Math.Round((value.z / 1.5f), 0, MidpointRounding.AwayFromZero) * 1.5f);
            playerpos = new Vector3(x,y, z); 
        }
    }
}
