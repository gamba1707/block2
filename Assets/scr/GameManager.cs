using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using TMPro;

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
        //mapdata.RoadMapData("Stage1-1​");
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

    public void Save(TextMeshProUGUI text)
    {
        Debug.Log(text.text);
        //mapdata.OnMapSave_json(text.text,Add_Blocknum,pManager.Floor_parent,pManager.Fall_parent);
        mapdata.OnMapSave_scrobj(text.text, Add_Blocknum, pManager.Floor_parent, pManager.Fall_parent);
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
    //ポーズ画面をやめる
    public void OnPouseback()
    {
        game_status = GAME_STATUS.Play;
        PousePanel.SetActive(false);
        Time.timeScale = 1;
    }
    public void OnGameOver()
    {
        game_status = GAME_STATUS.GameOver;
        pmove.GameOver_move();
        GameOverPanel.SetActive(true);
    }

    public void GameReset()
    {

        StartCoroutine(GameReset_move());
    }
    IEnumerator GameReset_move()
    {
        LoadUI.Fadeout();

        while(LoadUI.Fade_move) yield return null;

        Add_Blocknum = 0;//置いたブロック数を初期化
        pmove.Reset_move();//プレイヤー関連の初期化
        pManager.Reset_box();//置いたブロックを初期化
        GameOverPanel.SetActive(false);//表示しているゲームオーバー画面を消す
        yield return new WaitForSeconds(0.5f);//あまりに速いと不気味なので0.5秒待つ
        LoadUI.Fadein();
        while (LoadUI.Fade_move) yield return null;
        game_status = GAME_STATUS.Play;
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
        set { playerpos = value; }
    }
}
