using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;


public class MapData : MonoBehaviour
{
    [Serializable]
    public class map
    {
        public int clearnum;
        public int floornum;
        public Vector3[] floorpos;
        public int fallnum;
        public Vector3[] fallpos;
        public Vector3 goalpos;
    }

    public static MapData mapinstance;

    [Header("保存する場所")]
    [SerializeField] private string path;

    //絶対に残してほしいのでstatic
    [Header("ステージデータ")]
    private static MapData_scrobj mapData_Scrobj;



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

    public void OnMapSave_json(string stagename,int clearnum,Transform floor_parent,Transform fall_parent)
    {
        map data = new map();
        data.clearnum = clearnum;
        data.floornum = floor_parent.childCount;
        data.fallnum = fall_parent.childCount;
        data.floorpos =new Vector3[data.floornum];
        data.fallpos = new Vector3[data.fallnum];
        for (int i=0;i<data.floornum;i++)
        {
            data.floorpos[i]=floor_parent.GetChild(i).position;
        }
        for(int i = 0; i < fall_parent.childCount; i++)
        {
            data.fallpos[i]=fall_parent.GetChild(i).position;
        }
        string jsonstr = JsonUtility.ToJson(data);
        StreamWriter writer;//書き込む準備
        writer = new StreamWriter(Application.dataPath + "/" + path+ "/"+stagename+".json", false);
        Debug.Log(stagename+".jsonをセーブしました。");
        Debug.Log("clearnum:"+data.clearnum+"\nfloornum:"+ data.floornum);
        writer.Write(jsonstr);
        writer.Flush();
        writer.Close();
    }

    //いろいろ情報をもらってステージ地形を持ったScriptableObjectを作成する
    public void OnMapSave_scrobj(string stagename, int clearnum, Transform floor_parent, Transform fall_parent,Vector3 goalpos)
    {
        var obj = ScriptableObject.CreateInstance<MapData_scrobj>();
        float xmin=50f, xmax=0f, ymin=50f, ymax=0f;
        obj.clearnum = clearnum;
        obj.floornum = floor_parent.childCount;
        obj.fallnum = fall_parent.childCount;
        obj.floorpos = new Vector3[obj.floornum];
        obj.fallpos = new Vector3[obj.fallnum];
        for (int i = 0; i < obj.floornum; i++)
        {
            //座標を割り当てていく
            obj.floorpos[i] = floor_parent.GetChild(i).position;
            //一番端の座標を探す
            if (obj.floorpos[i].z < xmin) xmin = obj.floorpos[i].z;
            else if (obj.floorpos[i].z > xmax) xmax = obj.floorpos[i].z;
            if (obj.floorpos[i].y < ymin) ymin = obj.floorpos[i].y;
            else if (obj.floorpos[i].y > ymax) ymax = obj.floorpos[i].y;
        }
        for (int i = 0; i < fall_parent.childCount; i++)
        {
            obj.fallpos[i] = fall_parent.GetChild(i).position;
            //一番端の座標を探す
            if (obj.fallpos[i].z < xmin) xmin = obj.fallpos[i].z;
            else if (obj.fallpos[i].z > xmax) xmax = obj.fallpos[i].z;
            if (obj.fallpos[i].y < ymin) ymin = obj.fallpos[i].y;
            else if (obj.fallpos[i].y > ymax) ymax = obj.fallpos[i].y;
        }
        //端と端の絶対値を足して2で割ると中心がわかるからカメラの位置にする
        obj.stage_vcampos = new Vector3(0, (Mathf.Abs(ymax) + Mathf.Abs(ymin)) / 2, (Mathf.Abs(xmax)+ Mathf.Abs(xmin))/2);
        obj.goalpos = goalpos;

        //フォルダーが存在しないなら作る
        if(!System.IO.Directory.Exists("Assets/" + path)) System.IO.Directory.CreateDirectory(Application.dataPath + "/" + path);
        //ScriptableObjectを作成
        AssetDatabase.CreateAsset(obj, Path.Combine("Assets/"+path, stagename+ ".asset"));
    }

    public void LoadMapData(MapData_scrobj stagedata)
    {
            if (GameManager.I.Editmode) mapData_Scrobj = stagedata;
            Debug.Log(mapData_Scrobj.name);
            GameManager.I.Add_Blocknum_goal=mapData_Scrobj.clearnum;//目標数設定
            GameManager.I.SetFloorblock(mapData_Scrobj.floorpos);//床情報を送って配置
            GameManager.I.SetFallblock(mapData_Scrobj.fallpos);//奈落位置情報を送って配置
            GameManager.I.SetGoalblock(mapData_Scrobj.goalpos);//ゴール場所を送って配置
        Camera.main.gameObject.GetComponent<CameraManager>().SetStageCamera(mapData_Scrobj.stage_vcampos);
    }

    public void setMapData(MapData_scrobj stagedata)
    {
        mapData_Scrobj = stagedata;
        Debug.Log("受信："+mapData_Scrobj.name);
    }

    public string mapname()
    {
        return mapData_Scrobj.name;
    }

}
