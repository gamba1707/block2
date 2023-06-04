using System;
using System.Data;
using System.IO;
using UnityEditor;
using UnityEngine;


public class MapData : MonoBehaviour
{
    [Serializable]
    public class map
    {
        public int clearnum;
        public Vector3[] floorpos;
        public Vector3[] fallpos;
        public Vector3 stage_vcampos;
        public Vector3 goalpos;
    }

    public static MapData mapinstance;


    //絶対に残してほしいのでstatic
    [Header("ステージデータ")]
    private static MapData_scrobj mapData_Scrobj;

    private static string jsonpath;

    [Header("Createモード（作ったのを遊んでいる）")]
    [SerializeField] private bool createmode;

    bool boss;



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

    public void OnMapSave_json(string stagename, int clearnum, Transform floor_parent, Transform fall_parent, Vector3 goalpos)
    {
        map data = new map();
        data.clearnum = clearnum;
        data.floorpos = new Vector3[floor_parent.childCount];
        data.fallpos = new Vector3[fall_parent.childCount];
        for (int i = 0; i < floor_parent.childCount; i++)
        {
            //座標を割り当てていく
            data.floorpos[i] = floor_parent.GetChild(i).position;
        }
        for (int i = 0; i < fall_parent.childCount; i++)
        {
            data.fallpos[i] = fall_parent.GetChild(i).position;
        }
        data.stage_vcampos = Camera.main.transform.position;
        data.goalpos = goalpos;
        string jsonstr = JsonUtility.ToJson(data);
        StreamWriter writer;//書き込む準備
        writer = new StreamWriter(Application.dataPath + "/StageData_Create/" + stagename + ".json", false);
        Debug.Log(Application.dataPath + "/StageData_Create/" + stagename + ".jsonをセーブしました。");
        writer.Write(jsonstr);
        writer.Flush();
        writer.Close();
    }

    //いろいろ情報をもらってステージ地形を持ったScriptableObjectを作成する
    public void OnMapSave_scrobj(string stagename, int clearnum, Transform floor_parent, Transform fall_parent, Vector3 goalpos)
    {
        var obj = ScriptableObject.CreateInstance<MapData_scrobj>();
        obj.clearnum = clearnum;
        obj.floorpos = new Vector3[floor_parent.childCount];
        obj.fallpos = new Vector3[fall_parent.childCount];
        for (int i = 0; i < floor_parent.childCount; i++)
        {
            //座標を割り当てていく
            obj.floorpos[i] = floor_parent.GetChild(i).position;
        }
        for (int i = 0; i < fall_parent.childCount; i++)
        {
            obj.fallpos[i] = fall_parent.GetChild(i).position;
        }
        obj.stage_vcampos = Camera.main.transform.position;
        obj.goalpos = goalpos;

        //フォルダーが存在しないなら作る
        if (!Directory.Exists(Application.dataPath + "/StageData_Create")) Directory.CreateDirectory(Application.dataPath + "/StageData_Create");
        //ScriptableObjectを作成
        AssetDatabase.CreateAsset(obj, Path.Combine(Application.dataPath + "/StageData_Create" + stagename + ".asset"));
    }

    public void LoadMapData(MapData_scrobj stagedata)
    {
        if (GameManager.I.Editmode) mapData_Scrobj = stagedata;
        Debug.Log(mapData_Scrobj.name);
        Boss = mapData_Scrobj.bossstage;
        GameManager.I.Add_Blocknum_goal = mapData_Scrobj.clearnum;//目標数設定
        GameManager.I.SetFloorblock(mapData_Scrobj.floorpos);//床情報を送って配置
        GameManager.I.SetFallblock(mapData_Scrobj.fallpos);//奈落位置情報を送って配置
        GameManager.I.SetGoalblock(mapData_Scrobj.goalpos);//ゴール場所を送って配置
        Camera.main.gameObject.GetComponent<CameraManager>().SetStageCamera(mapData_Scrobj.stage_vcampos);
    }

    public void LoadMapData_Create()
    {
        string datastr = "";
        StreamReader reader;
        reader = new StreamReader(jsonpath);
        datastr = reader.ReadToEnd();
        reader.Close();
        map data = JsonUtility.FromJson<map>(datastr);
        GameManager.I.Add_Blocknum_goal = data.clearnum;//目標数設定
        GameManager.I.SetFloorblock(data.floorpos);//床情報を送って配置
        GameManager.I.SetFallblock(data.fallpos);//奈落位置情報を送って配置
        GameManager.I.SetGoalblock(data.goalpos);//ゴール場所を送って配置
        Camera.main.gameObject.GetComponent<CameraManager>().SetStageCamera(data.stage_vcampos);
    }

    public void setMapData(MapData_scrobj stagedata)
    {
        mapData_Scrobj = stagedata;
        Debug.Log("受信：" + mapData_Scrobj.name);
    }

    public void setMapData_Create(string path)
    {
        jsonpath = path;
        Debug.Log("受信：" + jsonpath);
    }

    public string mapname()
    {
        if(Createmode&&jsonpath_enable()) return System.IO.Path.GetFileNameWithoutExtension(jsonpath);
        else if (mapData_Scrobj != null) return mapData_Scrobj.name;
        return "";
    }

    public bool jsonpath_enable()
    {
        if(jsonpath == null)return false;
        return true;
    }

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

}
