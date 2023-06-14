using System;
using System.IO;
using UnityEngine;

//�ǂ̃V�[���ɂ�����}�b�v�f�[�^��ǂ񂾂������肷��X�N���v�g
public class MapData : MonoBehaviour
{
    //�}�b�v�{�́i�N���G�C�g���[�h�j
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

    //�C���X�^���X
    public static MapData mapinstance;

    //��΂Ɏc���Ăق����̂�static
    [Header("�X�e�[�W�f�[�^")]
    private static MapData_scrobj mapData_Scrobj;
    //�N���G�C�g���[�h�̎��ɂ��̃p�X���i�[����
    private static string jsonpath;

    [Header("Create���[�h�i������̂�V��ł���j")]
    [SerializeField] private bool createmode;

    //���������ǂݏo���Ǝ�ԂȂ̂Ŏ����Ă����p
    //�{�X�X�e�[�W��
    private bool boss;
    //�f�b�h���C��
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

    //�N���G�C�g���[�h�̕ҏW�ŃZ�[�u�{�^�����������ƌĂ΂��
    //�u���Ă���u���b�N�̍��W��Json�Ɋi�[����
    public void OnMapSave_json(string stagename, int clearnum, Transform floor_parent, Transform fall_parent, Transform trampoline_parent, Transform down_parent, Vector3 goalpos)
    {
        //����p��
        map data = new map();

        //���ꂼ��i�[���Ă���
        //���͂��ꂽ�ڕW��
        data.clearnum = clearnum;
        //�q�̐��̑����Ŕz���p�ӂ���
        data.floorpos = new Vector3[floor_parent.childCount];
        data.fallpos = new Vector3[fall_parent.childCount];
        data.trampolinepos = new Vector3[trampoline_parent.childCount];
        data.downpos = new Vector3[down_parent.childCount];

        //�f�b�h���C�������l
        float deadline = 0.0f;

        //�q�I�u�W�F�N�g�i�u���b�N�����j�����ꂼ��i�[����
        //���u���b�N
        for (int i = 0; i < floor_parent.childCount; i++)
        {
            //���W�����蓖�ĂĂ���
            data.floorpos[i] = floor_parent.GetChild(i).position;
            //�i�[����Ƃ��Ɉ�ԉ��̃u���b�N�̍��W������΃f�b�h���C�����X�V����
            if (data.floorpos[i].y < deadline) deadline = data.floorpos[i].y;
        }
        //�ޗ��u���b�N
        for (int i = 0; i < fall_parent.childCount; i++)
        {
            //���W�i�[
            data.fallpos[i] = fall_parent.GetChild(i).position;
            //�i�[����Ƃ��Ɉ�ԉ��̃u���b�N�̍��W������΃f�b�h���C�����X�V����
            if (data.fallpos[i].y < deadline) deadline = data.fallpos[i].y;
        }
        //�n�`��ׂ�u���b�N
        for (int i = 0; i < trampoline_parent.childCount; i++)
        {
            //���W�����蓖�ĂĂ���
            data.trampolinepos[i] = trampoline_parent.GetChild(i).position;
            //�i�[����Ƃ��Ɉ�ԉ��̃u���b�N�̍��W������΃f�b�h���C�����X�V����
            if (data.trampolinepos[i].y < deadline) deadline = data.trampolinepos[i].y;
        }
        //�n�`������u���b�N
        for (int i = 0; i < down_parent.childCount; i++)
        {
            //���W�i�[
            data.downpos[i] = down_parent.GetChild(i).position;
            //�i�[����Ƃ��Ɉ�ԉ��̃u���b�N�̍��W������΃f�b�h���C�����X�V����
            if (data.downpos[i].y < deadline) deadline = data.downpos[i].y;
        }

        //�ŏI�u���b�N��-10�̈ʒu�Ƀf�b�h���C����݂���
        data.deadline = deadline - 10;
        //�S�̃J�����̈ʒu�͍��f���Ă���J�����̈ʒu�ɂ���
        data.stage_vcampos = Camera.main.transform.position;
        //�S�[���̍��W���i�[
        data.goalpos = goalpos;

        //Json�ɕϊ�
        string jsonstr = JsonUtility.ToJson(data);
        //��������
        StreamWriter writer;
        writer = new StreamWriter(Application.dataPath + "/StageData_Create/" + stagename + ".json", false);
        writer.Write(jsonstr);
        writer.Flush();
        writer.Close();
        //�Z�[�u�o������\��
        Debug.Log(Application.dataPath + "/StageData_Create/" + stagename + ".json���Z�[�u���܂����B");
    }

    //�X�g�[���[���[�h�̒n�`��ǂݍ���
    public void LoadMapData(MapData_scrobj stagedata)
    {
        //�f�o�b�O���[�h�Ȃ�GameManager�ɂ��Ă�t�@�C����ǂݍ���
        if (GameManager.I.Editmode) mapData_Scrobj = stagedata;
        //
        Debug.Log(mapData_Scrobj.name);
        //��{�f�[�^���󂯎��
        Boss = mapData_Scrobj.bossstage;
        Deadline = mapData_Scrobj.deadline;
        //�n�`��GameManager�ɑ����Đݒ肵�Ă��炤
        GameManager.I.Add_Blocknum_goal = mapData_Scrobj.clearnum;//�ڕW���ݒ�
        GameManager.I.SetFloorblock(mapData_Scrobj.floorpos);//�����𑗂��Ĕz�u
        GameManager.I.SetFallblock(mapData_Scrobj.fallpos);//�ޗ��ʒu���𑗂��Ĕz�u
        GameManager.I.SetBeforeTrampolineblock(mapData_Scrobj.Trampolinepos);//�n�`��ׂ�u���b�N
        GameManager.I.SetBeforeDownblock(mapData_Scrobj.Downpos);//�n�`������u���b�N
        GameManager.I.SetGoalblock(mapData_Scrobj.goalpos);//�S�[���ꏊ�𑗂��Ĕz�u
        //�S�̃J�����̈ʒu�𑗂�
        Camera.main.gameObject.GetComponent<CameraManager>().SetStageCamera(mapData_Scrobj.stage_vcampos);
    }

    //Json�ō��ꂽ�X�e�[�W�i�N���G�C�g���[�h�j���\�z����
    public void LoadMapData_Create()
    {
        //�f�[�^������ꏊ
        string datastr = "";
        //�t�@�C����ǂݍ��ށi�p�X�͊��ɓn����Ă���j
        StreamReader reader;
        reader = new StreamReader(jsonpath);
        datastr = reader.ReadToEnd();
        reader.Close();
        //�ϊ�����
        map data = JsonUtility.FromJson<map>(datastr);
        //���蓖�ĂĂ���
        Deadline = data.deadline;
        GameManager.I.Add_Blocknum_goal = data.clearnum;//�ڕW���ݒ�
        GameManager.I.SetFloorblock(data.floorpos);//�����𑗂��Ĕz�u
        GameManager.I.SetFallblock(data.fallpos);//�ޗ��ʒu���𑗂��Ĕz�u
        GameManager.I.SetBeforeTrampolineblock(data.trampolinepos);//�n�`��ׂ�u���b�N
        GameManager.I.SetBeforeDownblock(data.downpos);//�n�`������u���b�N
        GameManager.I.SetGoalblock(data.goalpos);//�S�[���ꏊ�𑗂��Ĕz�u
        //�S�̃J�����̈ʒu
        Camera.main.gameObject.GetComponent<CameraManager>().SetStageCamera(data.stage_vcampos);
    }

    //�Z���N�g��ʂ���n�`�f�[�^��������
    //���̃f�[�^�������ăV�[����n��
    public void setMapData(MapData_scrobj stagedata)
    {
        //�n�`�f�[�^��ێ�����
        mapData_Scrobj = stagedata;
        Debug.Log("��M�F" + mapData_Scrobj.name);
    }

    //�N���G�C�g���[�h�̎��ɑ��݂���f�[�^�ŗV�񂾂������肷��Ƃ��ɌĂ΂��
    public void setMapData_Create(string path)
    {
        //��΃p�X�����蓖�Ă�
        jsonpath = path;
        Debug.Log("��M�F" + jsonpath);
    }

    //�X�e�[�W�������H�Ȃ��ŕԂ�
    public string mapname()
    {
        //���O�����p
        string name = "";
        //�N���G�C�g���[�h�̏ꍇ�͂��̃t�@�C������Ԃ�
        //�X�g�[���[���[�h�̎��͒n�`�̖��O��Ԃ�
        if (Application.platform != RuntimePlatform.WebGLPlayer && Createmode && jsonpath_enable()) name = System.IO.Path.GetFileNameWithoutExtension(jsonpath);
        else if (mapData_Scrobj != null) name = mapData_Scrobj.name;
        return name;
    }

    //�X�e�[�W����Ԃ�
    //�t�@�C�����ɉ��s���g���Ȃ��̂Łu�v������Ή��s����p
    public string mapname_text()
    {
        //���O�����p
        string name = "";
        //�N���G�C�g���[�h�̏ꍇ�͂��̃t�@�C������Ԃ�
        //�X�g�[���[���[�h�̎��͒n�`�̖��O��Ԃ�
        if (Application.platform != RuntimePlatform.WebGLPlayer && Createmode && jsonpath_enable()) name = System.IO.Path.GetFileNameWithoutExtension(jsonpath);
        else if (mapData_Scrobj != null) name = mapData_Scrobj.name;
        //�����J�M�J�b�R������΂���1�O�ɉ��s�R�[�h��}������
        if (name.IndexOf('�u') != -1) name = name.Insert(name.IndexOf('�u'), "\n");
        return name;
    }

    //�����ɃN���G�C�g���[�h��Json�t�@�C���̃p�X�����邩�ǂ�����Ԃ�
    public bool jsonpath_enable()
    {
        if (jsonpath == null) return false;
        return true;
    }


    //��������v���p�e�B
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
