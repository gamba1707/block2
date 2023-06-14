using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    //�Z�[�u�f�[�^�{��
    [System.Serializable]
    public class PlayerData
    {
        public string dateTime;
        public List<string> stagescore;
    }

    //�ݒ�t�@�C���{��
    [Serializable]
    public class Setting
    {
        public bool fullScreen;
        public int screen_w;
        public int screen_h;
        public float bgmvolume;
        public float sevolume;
    }

    //�C���X�^���X
    public static SaveManager instance;

    // �������x�N�g��"<���p16�����i1byte=8bit, 8bit*16=128bit>"
    private const string AES_IV_128 = @"hG9PtAuatfJbdgBm";
    // �Í�����<���p32�����i8bit*32����=256bit�j>
    private const string AES_Key_256 = @"AMJVgHnRNdUez5yYKJiVbwCdj8MyZUfZ";


    //�f�[�^��s�x�Ăяo���̂ł͂Ȃ������Ă���
    //�ǂݍ��ނׂ��Z�[�u�f�[�^��
    public static string dataname;
    //�Z�[�u�f�[�^
    private string dateTime;//�Z�[�u���ꂽ���t
    private List<string> stagescore = new List<string>();//�N���A�����X�e�[�W���ƃX�R�A
    //�ݒ萔�l
    private bool fullScreen;
    private int screen_w;
    private int screen_h;
    private float bgmvolume;
    private float sevolume;
    //�X�e�[�^�X��
    StatusText statustext;

    void Awake()
    {
        //�d�����đ����Ă����Ȃ��悤�ɂȂ��ꍇ�͂��̂܂܂ł���ꍇ�͍폜����
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        //�Q�[���N�����݂̂����ŃX�e�[�^�X���̃I�u�W�F�N�g���擾����
        statustext = GameObject.Find("statusText").GetComponent<StatusText>();
    }

    private void OnLevelWasLoaded()
    {
        //�V�[�����ƂɃX�e�[�^�X���I�u�W�F�N�g���擾
        statustext = GameObject.Find("statusText").GetComponent<StatusText>();
    }

    //���̃X�e�[�W�͖ڕW���������N���A�����ꍇ�i�����j��True��Ԃ����\�b�h
    public bool Clearstage(MapData_scrobj mapdata)
    {
        //�Z�[�u�f�[�^�����݂��āA���̃X�e�[�W�̃N���A�󋵂�����ꍇ
        //���̃X�e�[�W�̖ڕW��葽���N���A��True
        if (stagescore != null && stagescore.Contains(mapdata.name))
            return mapdata.clearnum < int.Parse(stagescore[stagescore.IndexOf(mapdata.name) + 1]);
        return false;
    }

    //�ڕW�ȏ�ŃN���A���Ă����ꍇ��True�ŕԂ����\�b�h
    public bool exClearstage(MapData_scrobj mapdata)
    {
        //�Z�[�u�f�[�^�����݂��āA���̃X�e�[�W�̃N���A�󋵂�����ꍇ
        //���̃X�e�[�W�̖ڕW��菭�Ȃ��������N���A��True
        if (stagescore != null && stagescore.Contains(mapdata.name))
            return mapdata.clearnum >= int.Parse(stagescore[stagescore.IndexOf(mapdata.name) + 1]);
        return false;
    }

    //���݂̃N���A����Ԃ�
    public int clearnum()
    {
        //2�ȏ゠��΃N���A���Ă���Ƃ��Ă��̐��l��Ԃ�
        //�X�e�[�W���ƃX�R�A������œo�^����Ă���̂�2�Ŋ���������Ԃ�
        if (stagescore.Count >= 2) return stagescore.Count / 2;
        return 0;
    }

    //�X�e�[�W����n���ƃN���A�����u���b�N����Ԃ�
    public string clearscore(string name)
    {
        //�o�^������΂��̎��ɓo�^����Ă���X�R�A��Ԃ�
        if (stagescore.Contains(name)) return stagescore[stagescore.IndexOf(name) + 1];
        //�������-��Ԃ�
        return "-";
    }

    //�Z�[�u�f�[�^�̓��t��Ԃ�
    public string getDateTime()
    {
        return this.dateTime;
    }


    //�ݒ�t�@�C�����Ȃ��ꍇ�̏����l�ݒ�ꏊ
    public void init_Setting()
    {
        //�t���X�N���[���ɂ���
        FullScreen = true;
        //��Ԃ����𑜓x�ɂ���i�t���Ɋi�[����Ă���̂Ŕz��̈�ԍŌ�j
        Width = Screen.resolutions[Screen.resolutions.Length - 1].width;
        Height = Screen.resolutions[Screen.resolutions.Length - 1].height;
        //BGM��SE�̓}�b�N�X
        BGMVolume = 1.0f;
        SEVolume = 1.0f;
    }

    //�ݒ�t�@�C���ۑ����\�b�h
    public void SaveData_Setting()
    {
        Debug.Log("�ݒ�t�@�C���ۑ�");
        //�C���X�^���X����
        Setting setting = new Setting();
        //�O���t�@�C���������ޗp
        StreamWriter writer;
        //���ꂼ��̐ݒ肳�ꂽ�f�[�^�����蓖�ĂĂ���
        setting.fullScreen = FullScreen;
        setting.screen_w = Width;
        setting.screen_h = Height;
        setting.bgmvolume = BGMVolume;
        setting.sevolume = SEVolume;
        //Json�`���ɕϊ�����
        string jsonstr = JsonUtility.ToJson(setting);
        //WebGL�łƂ���ȊO�ŕ�����
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            Debug.Log("WebGL�ŏ���");
            //WebGL���ƊO���t�@�C���͖����Ȃ̂�indexDB�ɕۑ�
            PlayerPrefs.SetString("Setting", jsonstr);
            PlayerPrefs.Save();
        }
        else
        {
            //�t�@�C���ɏ������ޏ���
            writer = new StreamWriter(Application.dataPath + "/Setting.json", false);
            writer.Write(jsonstr);
            writer.Flush();
            writer.Close();
        }

        statustext.SetStatusText("�ݒ�t�@�C���ۑ�����");
        Debug.Log("�ݒ�t�@�C������");
    }

    //�ݒ�t�@�C����ǂݍ���
    public void LoadSaveData_Setting()
    {
        //�O���t�@�C���ǂݍ��ݏ���
        StreamReader reader;
        //�f�[�^�i�[�p
        string datastr = "";
        //WebGL�łƂ���ȊO�ŕ�����
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            Debug.Log("WebGL�ŏ���");
            //�f�[�^���w�肵�Ď���Ă���
            datastr = PlayerPrefs.GetString("Setting", "");
            if (datastr.Equals(""))
            {
                //�f�[�^������ۂ������珉���l�����蓖�ĂĂ���
                init_Setting();
            }
            else
            {
                //�f�[�^����������ϊ����Đݒ肵�Ă���
                Setting data = JsonUtility.FromJson<Setting>(datastr);
                FullScreen = data.fullScreen;
                Width = data.screen_w;
                Height = data.screen_h;
                BGMVolume = data.bgmvolume;
                SEVolume = data.sevolume;
            }
        }
        else
        {
            //WebGL�ȊO�̏���
            if (File.Exists(Application.dataPath + "/Setting.json"))
            {
                //Json�t�@�C��������Γǂݎ��
                reader = new StreamReader(Application.dataPath + "/Setting.json");
                datastr = reader.ReadToEnd();
                reader.Close();
                //�ǂݏI�������ϊ����Đݒ肵�Ă���
                Setting data = JsonUtility.FromJson<Setting>(datastr);
                FullScreen = data.fullScreen;
                Width = data.screen_w;
                Height = data.screen_h;
                BGMVolume = data.bgmvolume;
                SEVolume = data.sevolume;
            }
            else
            {
                //�f�[�^���Ȃ������ꍇ�͏����l���蓖��
                init_Setting();
            }
        }
    }

    //�Z�[�u�f�[�^���㏑��������ۑ������肷��
    public void SaveData(string stagename, int add_blocknum)
    {
        Debug.Log(dataname);
        //����
        PlayerData data = new PlayerData();
        StreamWriter writer;
        //���̎��Ԃ��^�w�肵�Ċi�[
        data.dateTime = DateTime.Now.ToString("yyyy�NM��d�� HH:mm:ss");
        //�����������̃X�e�[�W���������Ă���΁A�X�R�A���X�V���邾���ɂ���
        if (stagescore.Contains(stagename))
        {
            //�X�R�A���X�V���Ă���Ώ㏑������
            if (int.Parse(stagescore[stagescore.IndexOf(stagename) + 1]) > add_blocknum)
                stagescore[stagescore.IndexOf(stagename) + 1] = add_blocknum.ToString();
        }
        else
        {
            //���Ƀf�[�^���Ȃ���Ε��ʂɃf�[�^���i�[
            //�K���X�e�[�W���A�X�R�A�̏��Ԃœo�^����
            stagescore.Add(stagename);
            stagescore.Add(add_blocknum.ToString());
        }
        //�o�^�����X�R�A�f�[�^���i�[����
        data.stagescore = stagescore;
        //Json�`���ɕϊ�����
        string jsonstr = JsonUtility.ToJson(data);
        Debug.Log(jsonstr);
        //AES�ňÍ�������
        jsonstr = EncryptAES(jsonstr);
        //WebGL�łƂ���ȊO�ŕ�����
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            //WebGL�ł̏���
            Debug.Log("WebGL�ŏ���");
            //indexDB�ɃZ�[�u�f�[�^��o�^
            PlayerPrefs.SetString(dataname, jsonstr);
            PlayerPrefs.Save();
        }
        else
        {
            //WebGL�ňȊO�̏���
            //�t�@�C���ɏ������ޏ���
            writer = new StreamWriter(Application.dataPath + "/" + dataname + ".json", false);
            writer.Write(jsonstr);
            writer.Flush();
            writer.Close();
        }

        Debug.Log("�Z�[�u���I�����܂���");
    }

    //�Z�[�u�f�[�^��ǂݎ���ĕێ�����
    public void LoadSaveData(string name)
    {
        //�ǂ̃Z�[�u�f�[�^��I�����Ďn�߂�����o�^����
        dataname = name;
        Debug.Log(dataname);

        //�f�[�^�u����
        string datastr = "";
        //WebGL�łƂ���ȊO�ŕ�����
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            Debug.Log("WebGL�ŏ���");
            //��������擾���Ă���i������΋󕶎��Łj
            datastr = PlayerPrefs.GetString(dataname, "");
            //�����f�[�^���Ȃ����
            if (datastr.Equals(""))
            {
                //�Z�[�u�����Ȃ�
                this.dateTime = null;
                //�c���Ă��邩������Ȃ��̂�List����ɂ���
                this.stagescore.Clear();
            }
            else
            {
                //�f�[�^�������
                //AES�ňÍ������ꂽ�f�[�^�𕜍�����
                datastr = DecryptAES(datastr);
                //�f�[�^��ϊ�����
                PlayerData data = JsonUtility.FromJson<PlayerData>(datastr);
                //���g�̕ϐ��Ɋi�[
                this.dateTime = data.dateTime;
                this.stagescore = data.stagescore;
            }
        }
        else
        {
            //WebGL�ňȊO�̏���
            //�t�@�C�������݂��Ă���
            if (File.Exists(Application.dataPath + "/" + dataname + ".json"))
            {
                //�t�@�C����ǂݎ��
                StreamReader reader;
                reader = new StreamReader(Application.dataPath + "/" + dataname + ".json");
                datastr = reader.ReadToEnd();
                reader.Close();
                //�ǂݍ��񂾃f�[�^�𕜍�����
                datastr = DecryptAES(datastr);
                //�����f�[�^������ۂ�������
                if (datastr.Equals(""))
                {
                    //����������
                    this.dateTime = null;
                    this.stagescore.Clear();
                }
                else
                {
                    //�f�[�^������ꍇ
                    //�ϊ�����
                    PlayerData data = JsonUtility.FromJson<PlayerData>(datastr);
                    //���g�Ɋi�[����
                    this.dateTime = data.dateTime;
                    this.stagescore = data.stagescore;
                }

            }
            else
            {
                //�t�@�C�����Ȃ��悤�Ȃ珉��������
                this.dateTime = null;
                this.stagescore.Clear();
            }
        }

    }

    //���ۂɃZ�[�u�f�[�^����������
    public void OnDeleteSaveData(string savename)
    {
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            // WebGL�ł̏���
            Debug.Log("WebGL�ŏ���");
            //�f�[�^���폜����
            PlayerPrefs.DeleteKey(savename);
        }
        else
        {
            // WebGL�ňȊO�̏���
            //�p�X�̏ꏊ���ΎQ�Ƃō��
            string dataname = Application.dataPath + "/" + savename + ".json";
            //�t�@�C�������݂��Ă������Ȃ�폜����
            if (File.Exists(dataname)) File.Delete(dataname);
        }
    }

    /*
    //�Q�l�T�C�g�Fhttps://yuyu-code.com/programming-languages/c-sharp/aes-encryption-decryption/
    //                  https://magnaga.com/2016/08/29/save-encryption/
    */
    //AES(CBC���[�h)�ňÍ���������������
    //�����͈Í�������
    public string EncryptAES(string text)
    {
        Aes aes = Aes.Create();
        //AES�̐ݒ�
        aes.BlockSize = 128;
        aes.KeySize = 256;
        aes.Padding = PaddingMode.PKCS7;
        aes.Mode = CipherMode.CBC;

        //�����l�ƈÍ�����
        aes.IV = Encoding.UTF8.GetBytes(AES_IV_128);
        aes.Key = Encoding.UTF8.GetBytes(AES_Key_256);

        ICryptoTransform encrypt = aes.CreateEncryptor();
        MemoryStream memoryStream = new MemoryStream();
        CryptoStream cryptStream = new CryptoStream(memoryStream, encrypt, CryptoStreamMode.Write);

        //�f�[�^��byte�z��ɕϊ�
        byte[] text_bytes = Encoding.UTF8.GetBytes(text);

        cryptStream.Write(text_bytes, 0, text_bytes.Length);
        cryptStream.FlushFinalBlock();

        byte[] encrypted = memoryStream.ToArray();

        return (Convert.ToBase64String(encrypted));
    }

    //��������
    public string DecryptAES(string text)
    {
        Aes aes = Aes.Create();
        //AES�̐ݒ�(�Í����Ɠ���)
        aes.BlockSize = 128;
        aes.KeySize = 256;
        aes.Padding = PaddingMode.PKCS7;
        aes.Mode = CipherMode.CBC;

        aes.IV = Encoding.UTF8.GetBytes(AES_IV_128);
        aes.Key = Encoding.UTF8.GetBytes(AES_Key_256);

        ICryptoTransform decryptor = aes.CreateDecryptor();

        byte[] encrypted = Convert.FromBase64String(text);
        byte[] planeText = new byte[encrypted.Length];

        MemoryStream memoryStream = new MemoryStream(encrypted);
        CryptoStream cryptStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);

        //���������݂�
        try
        {
            //���v�����Ȃ畜�����ꂽ����Ԃ�
            cryptStream.Read(planeText, 0, planeText.Length);
            return (Encoding.UTF8.GetString(planeText));
        }
        catch (Exception ex)
        {
            //�_�������Ȃ��ꂽ�Ƃ������Ƃɂ���
            Debug.LogError(ex.Message);
            statustext.SetStatusText("�t�@�C�����j�����Ă��܂�...�폜���Ă�������...");
        }
        return "";
    }


    //��������v���p�e�B
    public bool FullScreen
    {
        get { return this.fullScreen; }
        set { this.fullScreen = value; }
    }
    public int Width
    {
        get { return this.screen_w; }
        set { this.screen_w = value; }
    }
    public int Height
    {
        get { return this.screen_h; }
        set { this.screen_h = value; }
    }

    public float BGMVolume
    {
        get { return this.bgmvolume; }
        set { this.bgmvolume = value; }
    }
    public float SEVolume
    {
        get { return this.sevolume; }
        set { this.sevolume = value; }
    }

}
