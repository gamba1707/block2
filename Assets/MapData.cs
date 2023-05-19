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

    [Header("�ۑ�����ꏊ")]
    [SerializeField] private string path;

    //��΂Ɏc���Ăق����̂�static
    [Header("�X�e�[�W�f�[�^")]
    private static MapData_scrobj mapData_Scrobj;



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
        StreamWriter writer;//�������ޏ���
        writer = new StreamWriter(Application.dataPath + "/" + path+ "/"+stagename+".json", false);
        Debug.Log(stagename+".json���Z�[�u���܂����B");
        Debug.Log("clearnum:"+data.clearnum+"\nfloornum:"+ data.floornum);
        writer.Write(jsonstr);
        writer.Flush();
        writer.Close();
    }

    //���낢�����������ăX�e�[�W�n�`��������ScriptableObject���쐬����
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
            //���W�����蓖�ĂĂ���
            obj.floorpos[i] = floor_parent.GetChild(i).position;
            //��Ԓ[�̍��W��T��
            if (obj.floorpos[i].z < xmin) xmin = obj.floorpos[i].z;
            else if (obj.floorpos[i].z > xmax) xmax = obj.floorpos[i].z;
            if (obj.floorpos[i].y < ymin) ymin = obj.floorpos[i].y;
            else if (obj.floorpos[i].y > ymax) ymax = obj.floorpos[i].y;
        }
        for (int i = 0; i < fall_parent.childCount; i++)
        {
            obj.fallpos[i] = fall_parent.GetChild(i).position;
            //��Ԓ[�̍��W��T��
            if (obj.fallpos[i].z < xmin) xmin = obj.fallpos[i].z;
            else if (obj.fallpos[i].z > xmax) xmax = obj.fallpos[i].z;
            if (obj.fallpos[i].y < ymin) ymin = obj.fallpos[i].y;
            else if (obj.fallpos[i].y > ymax) ymax = obj.fallpos[i].y;
        }
        //�[�ƒ[�̐�Βl�𑫂���2�Ŋ���ƒ��S���킩�邩��J�����̈ʒu�ɂ���
        obj.stage_vcampos = new Vector3(0, (Mathf.Abs(ymax) + Mathf.Abs(ymin)) / 2, (Mathf.Abs(xmax)+ Mathf.Abs(xmin))/2);
        obj.goalpos = goalpos;

        //�t�H���_�[�����݂��Ȃ��Ȃ���
        if(!System.IO.Directory.Exists("Assets/" + path)) System.IO.Directory.CreateDirectory(Application.dataPath + "/" + path);
        //ScriptableObject���쐬
        AssetDatabase.CreateAsset(obj, Path.Combine("Assets/"+path, stagename+ ".asset"));
    }

    public void LoadMapData(MapData_scrobj stagedata)
    {
            if (GameManager.I.Editmode) mapData_Scrobj = stagedata;
            Debug.Log(mapData_Scrobj.name);
            GameManager.I.Add_Blocknum_goal=mapData_Scrobj.clearnum;//�ڕW���ݒ�
            GameManager.I.SetFloorblock(mapData_Scrobj.floorpos);//�����𑗂��Ĕz�u
            GameManager.I.SetFallblock(mapData_Scrobj.fallpos);//�ޗ��ʒu���𑗂��Ĕz�u
            GameManager.I.SetGoalblock(mapData_Scrobj.goalpos);//�S�[���ꏊ�𑗂��Ĕz�u
        Camera.main.gameObject.GetComponent<CameraManager>().SetStageCamera(mapData_Scrobj.stage_vcampos);
    }

    public void setMapData(MapData_scrobj stagedata)
    {
        mapData_Scrobj = stagedata;
        Debug.Log("��M�F"+mapData_Scrobj.name);
    }

    public string mapname()
    {
        return mapData_Scrobj.name;
    }

}
