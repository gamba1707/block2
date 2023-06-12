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
        public float deadline;
        public Vector3[] floorpos;
        public Vector3[] fallpos;
        public Vector3[] trampolinepos;
        public Vector3[] downpos;
        public Vector3 stage_vcampos;
        public Vector3 goalpos;
    }

    public static MapData mapinstance;


    //��΂Ɏc���Ăق����̂�static
    [Header("�X�e�[�W�f�[�^")]
    private static MapData_scrobj mapData_Scrobj;

    private static string jsonpath;

    [Header("Create���[�h�i������̂�V��ł���j")]
    [SerializeField] private bool createmode;

    private bool boss;
    private float deadline;



    void Awake()
    {
        //�d�����đ����Ă����Ȃ��悤�ɂȂ��ꍇ�͂��̂܂܂ł���ꍇ�͍폜����
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

    public void OnMapSave_json(string stagename, int clearnum, Transform floor_parent, Transform fall_parent, Transform trampoline_parent, Transform down_parent, Vector3 goalpos)
    {
        map data = new map();
        data.clearnum = clearnum;
        data.floorpos = new Vector3[floor_parent.childCount];
        data.fallpos = new Vector3[fall_parent.childCount];
        data.trampolinepos = new Vector3[trampoline_parent.childCount];
        data.downpos = new Vector3[down_parent.childCount];
        float deadline=0.0f;
        for (int i = 0; i < floor_parent.childCount; i++)
        {
            //���W�����蓖�ĂĂ���
            data.floorpos[i] = floor_parent.GetChild(i).position;
            if (data.floorpos[i].y<deadline)deadline = data.floorpos[i].y;
        }
        for (int i = 0; i < fall_parent.childCount; i++)
        {
            data.fallpos[i] = fall_parent.GetChild(i).position;
            if (data.fallpos[i].y < deadline) deadline = data.fallpos[i].y;
        }
        for (int i = 0; i < trampoline_parent.childCount; i++)
        {
            //���W�����蓖�ĂĂ���
            data.trampolinepos[i] = trampoline_parent.GetChild(i).position;
            if (data.trampolinepos[i].y < deadline) deadline = data.trampolinepos[i].y;
        }
        for (int i = 0; i < down_parent.childCount; i++)
        {
            Debug.Log(down_parent.childCount);
            data.downpos[i] = down_parent.GetChild(i).position;
            if (data.downpos[i].y < deadline) deadline = data.downpos[i].y;
        }
        data.deadline = deadline-10;
        data.stage_vcampos = Camera.main.transform.position;
        data.goalpos = goalpos;
        string jsonstr = JsonUtility.ToJson(data);
        StreamWriter writer;//�������ޏ���
        writer = new StreamWriter(Application.dataPath + "/StageData_Create/" + stagename + ".json", false);
        Debug.Log(Application.dataPath + "/StageData_Create/" + stagename + ".json���Z�[�u���܂����B");
        writer.Write(jsonstr);
        writer.Flush();
        writer.Close();
    }

    
    public void LoadMapData(MapData_scrobj stagedata)
    {
        if (GameManager.I.Editmode) mapData_Scrobj = stagedata;
        Debug.Log(mapData_Scrobj.name);
        Boss = mapData_Scrobj.bossstage;
        Deadline = mapData_Scrobj.deadline;
        GameManager.I.Add_Blocknum_goal = mapData_Scrobj.clearnum;//�ڕW���ݒ�
        GameManager.I.SetFloorblock(mapData_Scrobj.floorpos);//�����𑗂��Ĕz�u
        GameManager.I.SetFallblock(mapData_Scrobj.fallpos);//�ޗ��ʒu���𑗂��Ĕz�u
        GameManager.I.SetBeforeTrampolineblock(mapData_Scrobj.Trampolinepos);
        GameManager.I.SetBeforeDownblock(mapData_Scrobj.Downpos);
        GameManager.I.SetGoalblock(mapData_Scrobj.goalpos);//�S�[���ꏊ�𑗂��Ĕz�u
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
        Deadline = data.deadline;
        GameManager.I.Add_Blocknum_goal = data.clearnum;//�ڕW���ݒ�
        GameManager.I.SetFloorblock(data.floorpos);//�����𑗂��Ĕz�u
        GameManager.I.SetFallblock(data.fallpos);//�ޗ��ʒu���𑗂��Ĕz�u
        GameManager.I.SetBeforeTrampolineblock(data.trampolinepos);
        GameManager.I.SetBeforeDownblock(data.downpos);
        GameManager.I.SetGoalblock(data.goalpos);//�S�[���ꏊ�𑗂��Ĕz�u
        Camera.main.gameObject.GetComponent<CameraManager>().SetStageCamera(data.stage_vcampos);
    }

    public void setMapData(MapData_scrobj stagedata)
    {
        mapData_Scrobj = stagedata;
        Debug.Log("��M�F" + mapData_Scrobj.name);
    }

    public void setMapData_Create(string path)
    {
        jsonpath = path;
        Debug.Log("��M�F" + jsonpath);
    }

    public string mapname()
    {
        string name = "";
        if(Application.platform != RuntimePlatform.WebGLPlayer && Createmode&&jsonpath_enable()) name=System.IO.Path.GetFileNameWithoutExtension(jsonpath);
        else if (mapData_Scrobj != null) name= mapData_Scrobj.name;
        return name;
    }
    public string mapname_text()
    {
        string name = "";
        if(Application.platform != RuntimePlatform.WebGLPlayer && Createmode &&jsonpath_enable()) name=System.IO.Path.GetFileNameWithoutExtension(jsonpath);
        else if (mapData_Scrobj != null) name=mapData_Scrobj.name;
        if (name.IndexOf('�u') != -1) name=name.Insert(name.IndexOf('�u'),"\n");
        return name;
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

    public float Deadline
    {
        get { return deadline; }
        set { deadline = value; }
    }

}
