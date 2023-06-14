using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    //セーブデータ本体
    [System.Serializable]
    public class PlayerData
    {
        public string dateTime;
        public List<string> stagescore;
    }

    //設定ファイル本体
    [Serializable]
    public class Setting
    {
        public bool fullScreen;
        public int screen_w;
        public int screen_h;
        public float bgmvolume;
        public float sevolume;
    }

    //インスタンス
    public static SaveManager instance;

    // 初期化ベクトル"<半角16文字（1byte=8bit, 8bit*16=128bit>"
    private const string AES_IV_128 = @"hG9PtAuatfJbdgBm";
    // 暗号化鍵<半角32文字（8bit*32文字=256bit）>
    private const string AES_Key_256 = @"AMJVgHnRNdUez5yYKJiVbwCdj8MyZUfZ";


    //データを都度呼び出すのではなく持っておく
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
    //ステータス文
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
        //ゲーム起動時のみここでステータス文のオブジェクトを取得する
        statustext = GameObject.Find("statusText").GetComponent<StatusText>();
    }

    private void OnLevelWasLoaded()
    {
        //シーンごとにステータス文オブジェクトを取得
        statustext = GameObject.Find("statusText").GetComponent<StatusText>();
    }

    //そのステージは目標よりも多くクリアした場合（悪い）にTrueを返すメソッド
    public bool Clearstage(MapData_scrobj mapdata)
    {
        //セーブデータが存在して、そのステージのクリア状況がある場合
        //そのステージの目標より多いクリア→True
        if (stagescore != null && stagescore.Contains(mapdata.name))
            return mapdata.clearnum < int.Parse(stagescore[stagescore.IndexOf(mapdata.name) + 1]);
        return false;
    }

    //目標以上でクリアしていた場合にTrueで返すメソッド
    public bool exClearstage(MapData_scrobj mapdata)
    {
        //セーブデータが存在して、そのステージのクリア状況がある場合
        //そのステージの目標より少ないか同じクリア→True
        if (stagescore != null && stagescore.Contains(mapdata.name))
            return mapdata.clearnum >= int.Parse(stagescore[stagescore.IndexOf(mapdata.name) + 1]);
        return false;
    }

    //現在のクリア数を返す
    public int clearnum()
    {
        //2つ以上あればクリアしているとしてその数値を返す
        //ステージ名とスコアが並んで登録されているので2で割った数を返す
        if (stagescore.Count >= 2) return stagescore.Count / 2;
        return 0;
    }

    //ステージ名を渡すとクリアしたブロック数を返す
    public string clearscore(string name)
    {
        //登録があればその次に登録されているスコアを返す
        if (stagescore.Contains(name)) return stagescore[stagescore.IndexOf(name) + 1];
        //無ければ-を返す
        return "-";
    }

    //セーブデータの日付を返す
    public string getDateTime()
    {
        return this.dateTime;
    }


    //設定ファイルがない場合の初期値設定場所
    public void init_Setting()
    {
        //フルスクリーンにする
        FullScreen = true;
        //一番いい解像度にする（逆順に格納されているので配列の一番最後）
        Width = Screen.resolutions[Screen.resolutions.Length - 1].width;
        Height = Screen.resolutions[Screen.resolutions.Length - 1].height;
        //BGMとSEはマックス
        BGMVolume = 1.0f;
        SEVolume = 1.0f;
    }

    //設定ファイル保存メソッド
    public void SaveData_Setting()
    {
        Debug.Log("設定ファイル保存");
        //インスタンス生成
        Setting setting = new Setting();
        //外部ファイル書き込む用
        StreamWriter writer;
        //それぞれの設定されたデータを割り当てていく
        setting.fullScreen = FullScreen;
        setting.screen_w = Width;
        setting.screen_h = Height;
        setting.bgmvolume = BGMVolume;
        setting.sevolume = SEVolume;
        //Json形式に変換する
        string jsonstr = JsonUtility.ToJson(setting);
        //WebGL版とそれ以外で分ける
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            Debug.Log("WebGL版処理");
            //WebGLだと外部ファイルは無理なのでindexDBに保存
            PlayerPrefs.SetString("Setting", jsonstr);
            PlayerPrefs.Save();
        }
        else
        {
            //ファイルに書き込む処理
            writer = new StreamWriter(Application.dataPath + "/Setting.json", false);
            writer.Write(jsonstr);
            writer.Flush();
            writer.Close();
        }

        statustext.SetStatusText("設定ファイル保存完了");
        Debug.Log("設定ファイル完了");
    }

    //設定ファイルを読み込む
    public void LoadSaveData_Setting()
    {
        //外部ファイル読み込み準備
        StreamReader reader;
        //データ格納用
        string datastr = "";
        //WebGL版とそれ以外で分ける
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            Debug.Log("WebGL版処理");
            //データを指定して取ってくる
            datastr = PlayerPrefs.GetString("Setting", "");
            if (datastr.Equals(""))
            {
                //データが空っぽだったら初期値を割り当てておく
                init_Setting();
            }
            else
            {
                //データがあったら変換して設定していく
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
            //WebGL以外の処理
            if (File.Exists(Application.dataPath + "/Setting.json"))
            {
                //Jsonファイルがあれば読み取る
                reader = new StreamReader(Application.dataPath + "/Setting.json");
                datastr = reader.ReadToEnd();
                reader.Close();
                //読み終わったら変換して設定していく
                Setting data = JsonUtility.FromJson<Setting>(datastr);
                FullScreen = data.fullScreen;
                Width = data.screen_w;
                Height = data.screen_h;
                BGMVolume = data.bgmvolume;
                SEVolume = data.sevolume;
            }
            else
            {
                //データがなかった場合は初期値割り当て
                init_Setting();
            }
        }
    }

    //セーブデータを上書きしたり保存したりする
    public void SaveData(string stagename, int add_blocknum)
    {
        Debug.Log(dataname);
        //準備
        PlayerData data = new PlayerData();
        StreamWriter writer;
        //今の時間を型指定して格納
        data.dateTime = DateTime.Now.ToString("yyyy年M月d日 HH:mm:ss");
        //もしもうそのステージ名を持っていれば、スコアを更新するだけにする
        if (stagescore.Contains(stagename))
        {
            //スコアが更新していれば上書きする
            if (int.Parse(stagescore[stagescore.IndexOf(stagename) + 1]) > add_blocknum)
                stagescore[stagescore.IndexOf(stagename) + 1] = add_blocknum.ToString();
        }
        else
        {
            //特にデータがなければ普通にデータを格納
            //必ずステージ名、スコアの順番で登録する
            stagescore.Add(stagename);
            stagescore.Add(add_blocknum.ToString());
        }
        //登録したスコアデータを格納する
        data.stagescore = stagescore;
        //Json形式に変換する
        string jsonstr = JsonUtility.ToJson(data);
        Debug.Log(jsonstr);
        //AESで暗号化する
        jsonstr = EncryptAES(jsonstr);
        //WebGL版とそれ以外で分ける
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            //WebGL版の処理
            Debug.Log("WebGL版処理");
            //indexDBにセーブデータを登録
            PlayerPrefs.SetString(dataname, jsonstr);
            PlayerPrefs.Save();
        }
        else
        {
            //WebGL版以外の処理
            //ファイルに書き込む処理
            writer = new StreamWriter(Application.dataPath + "/" + dataname + ".json", false);
            writer.Write(jsonstr);
            writer.Flush();
            writer.Close();
        }

        Debug.Log("セーブが終了しました");
    }

    //セーブデータを読み取って保持する
    public void LoadSaveData(string name)
    {
        //どのセーブデータを選択して始めたかを登録する
        dataname = name;
        Debug.Log(dataname);

        //データ置き場
        string datastr = "";
        //WebGL版とそれ以外で分ける
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            Debug.Log("WebGL版処理");
            //文字列を取得してくる（無ければ空文字で）
            datastr = PlayerPrefs.GetString(dataname, "");
            //もしデータがなければ
            if (datastr.Equals(""))
            {
                //セーブ日時なし
                this.dateTime = null;
                //残っているかもしれないのでListを空にする
                this.stagescore.Clear();
            }
            else
            {
                //データがあれば
                //AESで暗号化されたデータを復号する
                datastr = DecryptAES(datastr);
                //データを変換する
                PlayerData data = JsonUtility.FromJson<PlayerData>(datastr);
                //自身の変数に格納
                this.dateTime = data.dateTime;
                this.stagescore = data.stagescore;
            }
        }
        else
        {
            //WebGL版以外の処理
            //ファイルが存在している
            if (File.Exists(Application.dataPath + "/" + dataname + ".json"))
            {
                //ファイルを読み取る
                StreamReader reader;
                reader = new StreamReader(Application.dataPath + "/" + dataname + ".json");
                datastr = reader.ReadToEnd();
                reader.Close();
                //読み込んだデータを復号する
                datastr = DecryptAES(datastr);
                //もしデータが空っぽだったら
                if (datastr.Equals(""))
                {
                    //初期化する
                    this.dateTime = null;
                    this.stagescore.Clear();
                }
                else
                {
                    //データがある場合
                    //変換する
                    PlayerData data = JsonUtility.FromJson<PlayerData>(datastr);
                    //自身に格納する
                    this.dateTime = data.dateTime;
                    this.stagescore = data.stagescore;
                }

            }
            else
            {
                //ファイルがないようなら初期化する
                this.dateTime = null;
                this.stagescore.Clear();
            }
        }

    }

    //実際にセーブデータを消す処理
    public void OnDeleteSaveData(string savename)
    {
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            // WebGL版の処理
            Debug.Log("WebGL版処理");
            //データを削除する
            PlayerPrefs.DeleteKey(savename);
        }
        else
        {
            // WebGL版以外の処理
            //パスの場所を絶対参照で作る
            string dataname = Application.dataPath + "/" + savename + ".json";
            //ファイルが存在していそうなら削除する
            if (File.Exists(dataname)) File.Delete(dataname);
        }
    }

    /*
    //参考サイト：https://yuyu-code.com/programming-languages/c-sharp/aes-encryption-decryption/
    //                  https://magnaga.com/2016/08/29/save-encryption/
    */
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

        //初期値と暗号化鍵
        aes.IV = Encoding.UTF8.GetBytes(AES_IV_128);
        aes.Key = Encoding.UTF8.GetBytes(AES_Key_256);

        ICryptoTransform encrypt = aes.CreateEncryptor();
        MemoryStream memoryStream = new MemoryStream();
        CryptoStream cryptStream = new CryptoStream(memoryStream, encrypt, CryptoStreamMode.Write);

        //データをbyte配列に変換
        byte[] text_bytes = Encoding.UTF8.GetBytes(text);

        cryptStream.Write(text_bytes, 0, text_bytes.Length);
        cryptStream.FlushFinalBlock();

        byte[] encrypted = memoryStream.ToArray();

        return (Convert.ToBase64String(encrypted));
    }

    //復号処理
    public string DecryptAES(string text)
    {
        Aes aes = Aes.Create();
        //AESの設定(暗号化と同じ)
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

        //復号を試みる
        try
        {
            //大丈夫そうなら復号された文を返す
            cryptStream.Read(planeText, 0, planeText.Length);
            return (Encoding.UTF8.GetString(planeText));
        }
        catch (Exception ex)
        {
            //ダメそうなら壊れたということにする
            Debug.LogError(ex.Message);
            statustext.SetStatusText("ファイルが破損しています...削除してください...");
        }
        return "";
    }


    //ここからプロパティ
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
