using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    // 初期化ベクトル"<半角16文字（1byte=8bit, 8bit*16=128bit>"
    private const string AES_IV_128 = @"pf69DL6GrWFyZcMK";
    // 暗号化鍵<半角32文字（8bit*32文字=256bit）>
    private const string AES_Key_256 = @"5TGB&YHN7UJM(IK<5TGB&YHN7UJM(IK<";

    [SerializeField] List<string> exclearStagename=new List<string>();
    [SerializeField] List<string> clearStagename=new List<string>();

    [System.Serializable]
    public class PlayerData
    {
        public List<string> exclearStagename;
        public List<string> clearStagename;
    }

    public static SaveManager instance;

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

    public bool Clearstage(string name)
    {
        return clearStagename.Contains(name);
    }
    public bool exClearstage(string name)
    {
        return exclearStagename.Contains(name);
    }
    public int clearnum()
    {
        return clearStagename.Count+exclearStagename.Count;
    }

    public void SaveData(string stage,bool ex)
    {
        PlayerData data = new PlayerData();
        StreamWriter writer;
        //一旦自分のリストに登録してからPlayerDataの方にリストごと渡してしまう
        //もし目標と同じかそれより早くクリアしたらexに登録する
        if (ex)
        {
            exclearStagename.Add(stage);
            data.exclearStagename = exclearStagename;
        }
        else
        {
            clearStagename.Add(stage);
            data.clearStagename = clearStagename;
        }
        //Json形式に変換する
        string jsonstr = JsonUtility.ToJson(data);
        Debug.Log(jsonstr);
        //AESで暗号化する
        jsonstr=EncryptAES(jsonstr);
        //ファイルに書き込む処理
        writer = new StreamWriter(Application.dataPath + "/savedata.json", false);
        writer.Write(jsonstr);
        writer.Flush();
        writer.Close();
        Debug.Log("セーブが終了しました");
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

        Debug.Log(Encoding.UTF8.GetBytes(AES_Key_256).Length);
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
