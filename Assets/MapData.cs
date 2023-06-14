using System;
using System.IO;
using UnityEngine;

//どのシーンにもいるマップデータを読んだり作ったりするスクリプト
public class MapData : MonoBehaviour
{
    //マップ本体（クリエイトモード）
    [Serializable]
    public class map
    {
        public int clearnum;
        public float deadline;
        public Vector3[] floorpos;
        public Vector3[] fallpos;
        public Vector3[] trampolinepos;
        public Vector3[] downpos;
        public Vector3 stage_vcampos;
        public Vector3 goalpos;
    }

    //インスタンス
    public static MapData mapinstance;

    //絶対に残してほしいのでstatic
    [Header("ステージデータ")]
    private static MapData_scrobj mapData_Scrobj;
    //クリエイトモードの時にそのパスを格納する
    private static string jsonpath;

    [Header("Createモード（作ったのを遊んでいる）")]
    [SerializeField] private bool createmode;

    //いちいち読み出すと手間なので持っておく用
    //ボスステージか
    private bool boss;
    //デッドライン
    private float deadline;

    void Awake()
    {
        //重複して増えていかないようにない場合はそのままである場合は削除する
        if (mapinstance == null)
        {
            mapinstance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this);
    }

    //クリエイトモードの編集でセーブボタンが押されると呼ばれる
    //置いてあるブロックの座標をJsonに格納する
    public void OnMapSave_json(string stagename, int clearnum, Transform floor_parent, Transform fall_parent, Transform trampoline_parent, Transform down_parent, Vector3 goalpos)
    {
        //箱を用意
        map data = new map();

        //それぞれ格納していく
        //入力された目標数
        data.clearnum = clearnum;
        //子の数の多さで配列を用意する
        data.floorpos = new Vector3[floor_parent.childCount];
        data.fallpos = new Vector3[fall_parent.childCount];
        data.trampolinepos = new Vector3[trampoline_parent.childCount];
        data.downpos = new Vector3[down_parent.childCount];

        //デッドライン初期値
        float deadline = 0.0f;

        //子オブジェクト（ブロックたち）をそれぞれ格納する
        //床ブロック
        for (int i = 0; i < floor_parent.childCount; i++)
        {
            //座標を割り当てていく
            data.floorpos[i] = floor_parent.GetChild(i).position;
            //格納するときに一番下のブロックの座標があればデッドラインを更新する
            if (data.floorpos[i].y < deadline) deadline = data.floorpos[i].y;
        }
        //奈落ブロック
        for (int i = 0; i < fall_parent.childCount; i++)
        {
            //座標格納
            data.fallpos[i] = fall_parent.GetChild(i).position;
            //格納するときに一番下のブロックの座標があればデッドラインを更新する
            if (data.fallpos[i].y < deadline) deadline = data.fallpos[i].y;
        }
        //地形飛べるブロック
        for (int i = 0; i < trampoline_parent.childCount; i++)
        {
            //座標を割り当てていく
            data.trampolinepos[i] = trampoline_parent.GetChild(i).position;
            //格納するときに一番下のブロックの座標があればデッドラインを更新する
            if (data.trampolinepos[i].y < deadline) deadline = data.trampolinepos[i].y;
        }
        //地形下がるブロック
        for (int i = 0; i < down_parent.childCount; i++)
        {
            //座標格納
            data.downpos[i] = down_parent.GetChild(i).position;
            //格納するときに一番下のブロックの座標があればデッドラインを更新する
            if (data.downpos[i].y < deadline) deadline = data.downpos[i].y;
        }

        //最終ブロックの-10の位置にデッドラインを設ける
        data.deadline = deadline - 10;
        //全体カメラの位置は今映しているカメラの位置にする
        data.stage_vcampos = Camera.main.transform.position;
        //ゴールの座標を格納
        data.goalpos = goalpos;

        //Jsonに変換
        string jsonstr = JsonUtility.ToJson(data);
        //書き込む
        StreamWriter writer;
        writer = new StreamWriter(Application.dataPath + "/StageData_Create/" + stagename + ".json", false);
        writer.Write(jsonstr);
        writer.Flush();
        writer.Close();
        //セーブ出来たら表示
        Debug.Log(Application.dataPath + "/StageData_Create/" + stagename + ".jsonをセーブしました。");
    }

    //ストーリーモードの地形を読み込む
    public void LoadMapData(MapData_scrobj stagedata)
    {
        //デバッグモードならGameManagerについてるファイルを読み込む
        if (GameManager.I.Editmode) mapData_Scrobj = stagedata;
        //
        Debug.Log(mapData_Scrobj.name);
        //基本データを受け取る
        Boss = mapData_Scrobj.bossstage;
        Deadline = mapData_Scrobj.deadline;
        //地形をGameManagerに送って設定してもらう
        GameManager.I.Add_Blocknum_goal = mapData_Scrobj.clearnum;//目標数設定
        GameManager.I.SetFloorblock(mapData_Scrobj.floorpos);//床情報を送って配置
        GameManager.I.SetFallblock(mapData_Scrobj.fallpos);//奈落位置情報を送って配置
        GameManager.I.SetBeforeTrampolineblock(mapData_Scrobj.Trampolinepos);//地形飛べるブロック
        GameManager.I.SetBeforeDownblock(mapData_Scrobj.Downpos);//地形下がるブロック
        GameManager.I.SetGoalblock(mapData_Scrobj.goalpos);//ゴール場所を送って配置
        //全体カメラの位置を送る
        Camera.main.gameObject.GetComponent<CameraManager>().SetStageCamera(mapData_Scrobj.stage_vcampos);
    }

    //Jsonで作られたステージ（クリエイトモード）を構築する
    public void LoadMapData_Create()
    {
        //データを入れる場所
        string datastr = "";
        //ファイルを読み込む（パスは既に渡されている）
        StreamReader reader;
        reader = new StreamReader(jsonpath);
        datastr = reader.ReadToEnd();
        reader.Close();
        //変換する
        map data = JsonUtility.FromJson<map>(datastr);
        //割り当てていく
        Deadline = data.deadline;
        GameManager.I.Add_Blocknum_goal = data.clearnum;//目標数設定
        GameManager.I.SetFloorblock(data.floorpos);//床情報を送って配置
        GameManager.I.SetFallblock(data.fallpos);//奈落位置情報を送って配置
        GameManager.I.SetBeforeTrampolineblock(data.trampolinepos);//地形飛べるブロック
        GameManager.I.SetBeforeDownblock(data.downpos);//地形下がるブロック
        GameManager.I.SetGoalblock(data.goalpos);//ゴール場所を送って配置
        //全体カメラの位置
        Camera.main.gameObject.GetComponent<CameraManager>().SetStageCamera(data.stage_vcampos);
    }

    //セレクト画面から地形データが送られる
    //そのデータをもってシーンを渡る
    public void setMapData(MapData_scrobj stagedata)
    {
        //地形データを保持する
        mapData_Scrobj = stagedata;
        Debug.Log("受信：" + mapData_Scrobj.name);
    }

    //クリエイトモードの時に存在するデータで遊んだり作ったりするときに呼ばれる
    public void setMapData_Create(string path)
    {
        //絶対パスを割り当てる
        jsonpath = path;
        Debug.Log("受信：" + jsonpath);
    }

    //ステージ名を加工なしで返す
    public string mapname()
    {
        //名前入れる用
        string name = "";
        //クリエイトモードの場合はそのファイル名を返す
        //ストーリーモードの時は地形の名前を返す
        if (Application.platform != RuntimePlatform.WebGLPlayer && Createmode && jsonpath_enable()) name = System.IO.Path.GetFileNameWithoutExtension(jsonpath);
        else if (mapData_Scrobj != null) name = mapData_Scrobj.name;
        return name;
    }

    //ステージ名を返す
    //ファイル名に改行が使えないので「」があれば改行する用
    public string mapname_text()
    {
        //名前入れる用
        string name = "";
        //クリエイトモードの場合はそのファイル名を返す
        //ストーリーモードの時は地形の名前を返す
        if (Application.platform != RuntimePlatform.WebGLPlayer && Createmode && jsonpath_enable()) name = System.IO.Path.GetFileNameWithoutExtension(jsonpath);
        else if (mapData_Scrobj != null) name = mapData_Scrobj.name;
        //もしカギカッコがあればその1つ前に改行コードを挿入する
        if (name.IndexOf('「') != -1) name = name.Insert(name.IndexOf('「'), "\n");
        return name;
    }

    //ここにクリエイトモードのJsonファイルのパスがあるかどうかを返す
    public bool jsonpath_enable()
    {
        if (jsonpath == null) return false;
        return true;
    }


    //ここからプロパティ
    public bool Createmode
    {
        get { return createmode; }
        set { createmode = value; }
    }

    public bool Boss
    {
        get { return boss; }
        set { boss = value; }
    }

    public float Deadline
    {
        get { return deadline; }
        set { deadline = value; }
    }

}
