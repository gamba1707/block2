using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using TMPro;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager I;

    [Header("Editerモード")]
    [SerializeField] private bool editmode;
    [SerializeField] private MapData_scrobj edit_mapdata;

    [Header("ゲームの状態")]
    [SerializeField] private GAME_STATUS game_status;
    private enum GAME_STATUS { Play, GameClear, Pause, GameOver }

    [Header("セーブ")]
    [SerializeField] private MapData mapdata;

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

    [Header("現在までに出現させたブロック数")]
    [SerializeField] private int add_blocknum;
    [Header("目標のブロック数")]
    [SerializeField] private int add_blocknum_goal;

    [Header("このステージで使うブロック")]
    [SerializeField] private bool nomal;
    [SerializeField] private bool trampoline;
    [SerializeField] private bool down;

    [Header("通常のブロック（初期化時に控えておく数）")]
    [SerializeField] private int nomalnum;
    [Header("その他のブロック（初期化時に控えておく数）")]
    [SerializeField] private int blocknum;

    [Header("セレクトしているブロック名")]
    [SerializeField] private string selectname;

    [Header("プレイヤーの位置（一マス単位）")]
    [SerializeField] private Vector3 playerpos;

    string stagename;


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
        if(Editmode)
        {
            GUIStyle style = new GUIStyle();
            style.fontSize = 32;
            style.normal.textColor = Color.white;
            string s = "現在、デバッグモードが有効です\nステージセレクトのデータは読み取られていません";
            string s2 = "\n現在のステージ："+edit_mapdata.name;
            GUI.Label(new Rect(200, 0, Screen.width, Screen.height), s+s2, style);
        }
    }


    private void Start()
    {
        if (Editmode)
        {
            Debug.Log("<color=cyan>EditMode</color>");
            mapdata.LoadMapData(edit_mapdata);
        }
        else
        {
            mapdata = GameObject.Find("MapData").GetComponent<MapData>();
            mapdata.LoadMapData(edit_mapdata);
        }
        LoadUI.Fadein();//ロード画面を開ける
        game_status = GAME_STATUS.Play;
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
    public void SetGoalblock(Vector3 pos)
    {
        pManager.GetGoalObject(pos);
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
    }

    public void Save(TMP_InputField inputField)
    {
        Debug.Log(inputField.text);
        //mapdata.OnMapSave_json(text.text,Add_Blocknum,pManager.Floor_parent,pManager.Fall_parent);
        mapdata.OnMapSave_json(inputField.text, Add_Blocknum, pManager.Floor_parent, pManager.Fall_parent,pManager.Goalpos);
    }
    public void SetAdd_Blocknum_Create(TMP_InputField inputField)
    {
        int n;
        bool isInt=int.TryParse(inputField.text.ToString(),out n);
        if (isInt)Add_Blocknum = n;
    }


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
        pmove.Clear_move();
    }

    public void OnClear_end()
    {
        SaveManager.instance.SaveData(mapdata.mapname(),Add_Blocknum,Add_Blocknum_goal);
        OnStageSelect();
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
        if(!Editmode) pmove.Reset_move();//プレイヤー関連の初期化
        Add_Blocknum = 0;//置いたブロック数を初期化
        pManager.Reset_box();//置いたブロックを初期化
        if(GameOverPanel.activeInHierarchy)GameOverPanel.SetActive(false);//表示しているゲームオーバー画面を消す
        else if(PousePanel.activeInHierarchy)PousePanel.SetActive(false);
        LoadUI.Fadein();
        while (LoadUI.Fade_move) yield return null;
        Time.timeScale = 1;
        game_status = GAME_STATUS.Play;
    }

    public void OnStageSelect()
    {
        LoadUI.Fadeout();
        StartCoroutine(StageSelect_move());
    }
    IEnumerator StageSelect_move()
    {
        var async = SceneManager.LoadSceneAsync("Select");
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

    public bool Nomal
    {
        get { return nomal; }
    }
    public bool Trampoline
    {
        get { return trampoline; }
    }
    public bool Down
    {
        get { return down; }
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
