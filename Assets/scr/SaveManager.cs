using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    [System.Serializable]
    public class PlayerData
    {
        public string dateTime;
        public List<string> stagescore;
    }

    [Serializable]
    public class Setting
    {
        public bool fullScreen;
        public int screen_w;
        public int screen_h;
        public float bgmvolume;
        public float sevolume;
    }

    public static SaveManager instance;

    // 初期化ベクトル"<半角16文字（1byte=8bit, 8bit*16=128bit>"
    private const string AES_IV_128 = @"pf69DL6GrWFyZcMK";
    // 暗号化鍵<半角32文字（8bit*32文字=256bit）>
    private const string AES_Key_256 = @"5TGB&YHN7UJM(IK<5TGB&YHN7UJM(IK<";

    //読み込むべきセーブデータ名
    public static string dataname;
    //セーブデータ
    private string dateTime;//セーブされた日付
    private List<string> stagescore = new List<string>();//クリアしたステージ名とスコア
    //設定数値
    private bool fullScreen;
    private int screen_w;
    private int screen_h;
    private float bgmvolume;
    private float sevolume;

    StatusText statustext;

    void Awake()
    {
        //重複して増えていかないようにない場合はそのままである場合は削除する
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
        statustext= GameObject.Find("statusText").GetComponent<StatusText>();
    }

    public bool Clearstage(MapData_scrobj mapdata)
    {
        
        if (stagescore != null && stagescore.Contains(mapdata.name))
            return mapdata.clearnum < int.Parse(stagescore[stagescore.IndexOf(mapdata.name) + 1]);
        return false;
    }
    public bool exClearstage(MapData_scrobj mapdata)
    {
        if (stagescore != null && stagescore.Contains(mapdata.name))
            return mapdata.clearnum >= int.Parse(stagescore[stagescore.IndexOf(mapdata.name) + 1]);
        return false;
    }
    public int clearnum()
    {
        if (stagescore.Count >= 2) return stagescore.Count / 2;
        return 0;
    }
    public string clearscore(string name)
    {
        if (stagescore.Contains(name)) return stagescore[stagescore.IndexOf(name) + 1];
        return "-";
    }
    public string getDateTime()
    {
        return this.dateTime;
    }

    public bool FullScreen
    {
        get { return this.fullScreen; }
        set { this.fullScreen = value; }
    }
    public int Width
    {
        get { return this.screen_w; }
        set { this.screen_w= value; }
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

    public void init_Setting()
    {
        FullScreen = true;
        Width = Screen.resolutions[Screen.resolutions.Length-1].width;
        Height = Screen.resolutions[Screen.resolutions.Length-1].height;
        BGMVolume = 1.0f;
        SEVolume = 1.0f;
    }

    public void SaveData_Setting()
    {
        Debug.Log("設定ファイル保存");
        Setting setting = new Setting();
        StreamWriter writer;
        setting.fullScreen = FullScreen;
        setting.screen_w = Width;
        setting.screen_h = Height;
        setting.bgmvolume = BGMVolume;
        setting.sevolume = SEVolume;
        //Json形式に変換する
        string jsonstr = JsonUtility.ToJson(setting);
        //ファイルに書き込む処理
        writer = new StreamWriter(Application.dataPath + "/Setting.json", false);
        writer.Write(jsonstr);
        writer.Flush();
        writer.Close();
        statustext.SetStatusText("設定ファイル保存完了");
        Debug.Log("設定ファイル完了");
    }

    public void LoadSaveData_Setting()
    {
        StreamReader reader;
        string datastr = "";
        reader = new StreamReader(Application.dataPath + "/Setting.json");
        datastr = reader.ReadToEnd();
        reader.Close();
        Setting data = JsonUtility.FromJson<Setting>(datastr);
        FullScreen=data.fullScreen;
        Width=data.screen_w;
        Height=data.screen_h;
        BGMVolume = data.bgmvolume;
        SEVolume = data.sevolume;
    }

    public void SaveData(string stagename, int add_blocknum, int add_blocknum_goal)
    {
        Debug.Log(dataname);
        PlayerData data = new PlayerData();
        StreamWriter writer;
        data.dateTime = DateTime.Now.ToString("yyyy年M月d日 HH:mm:ss");
        //一旦自分のリストに登録してからPlayerDataの方にリストごと渡してしまう
        //もしもうそのステージ名を持っていれば、スコアを更新するだけにする
        if (stagescore.Contains(stagename))
        {
            if (int.Parse(stagescore[stagescore.IndexOf(stagename) + 1]) > add_blocknum)
                stagescore[stagescore.IndexOf(stagename) + 1] = add_blocknum.ToString();
        }
        else
        {
            stagescore.Add(stagename);
            stagescore.Add(add_blocknum.ToString());
        }

        data.stagescore = stagescore;
        //Json形式に変換する
        string jsonstr = JsonUtility.ToJson(data);
        Debug.Log(jsonstr);
        //AESで暗号化する
        jsonstr = EncryptAES(jsonstr);
        //ファイルに書き込む処理
        writer = new StreamWriter(Application.dataPath + "/" + dataname + ".json", false);
        writer.Write(jsonstr);
        writer.Flush();
        writer.Close();
        Debug.Log("セーブが終了しました");
    }

    public void LoadSaveData(string name)
    {
        dataname = name;
        Debug.Log(dataname);

        string datastr = "";
        if (File.Exists(Application.dataPath + "/" + dataname + ".json"))
        {
            StreamReader reader;
            reader = new StreamReader(Application.dataPath + "/" + dataname + ".json");
            datastr = reader.ReadToEnd();
            reader.Close();
            datastr = DecryptAES(datastr);
            PlayerData data = JsonUtility.FromJson<PlayerData>(datastr);
            this.dateTime = data.dateTime;
            this.stagescore = data.stagescore;
        }
        else
        {
            this.dateTime = null;
            this.stagescore.Clear();
        }
    }


    public void OnDeleteSaveData(string savename)
    {
        string dataname = Application.dataPath + "/" + savename + ".json";
        if (File.Exists(dataname)) File.Delete(dataname);
    }


    //参考サイト：https://yuyu-code.com/programming-languages/c-sharp/aes-encryption-decryption/
    //                  https://magnaga.com/2016/08/29/save-encryption/
    //AES(CBCモード)で暗号化がしたかった
    //ここは暗号化処理
    public string EncryptAES(string text)
    {
        Aes aes = Aes.Create();
        //AESの設定
        aes.BlockSize = 128;
        aes.KeySize = 256;
        aes.Padding = PaddingMode.PKCS7;
        aes.Mode = CipherMode.CBC;

        aes.IV = Encoding.UTF8.GetBytes(AES_IV_128);
        aes.Key = Encoding.UTF8.GetBytes(AES_Key_256);

        ICryptoTransform encrypt = aes.CreateEncryptor();
        MemoryStream memoryStream = new MemoryStream();
        CryptoStream cryptStream = new CryptoStream(memoryStream, encrypt, CryptoStreamMode.Write);

        byte[] text_bytes = Encoding.UTF8.GetBytes(text);

        cryptStream.Write(text_bytes, 0, text_bytes.Length);
        cryptStream.FlushFinalBlock();

        byte[] encrypted = memoryStream.ToArray();

        return (Convert.ToBase64String(encrypted));
    }

    //復号処理
    public static string DecryptAES(string text)
    {
        Aes aes = Aes.Create();
        //AESの設定(暗号と同じ)
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

        cryptStream.Read(planeText, 0, planeText.Length);

        return (Encoding.UTF8.GetString(planeText));
    }

}
