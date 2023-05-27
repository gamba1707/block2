using Org.BouncyCastle.Asn1.Pkcs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    // �������x�N�g��"<���p16�����i1byte=8bit, 8bit*16=128bit>"
    private const string AES_IV_128 = @"pf69DL6GrWFyZcMK";
    // �Í�����<���p32�����i8bit*32����=256bit�j>
    private const string AES_Key_256 = @"5TGB&YHN7UJM(IK<5TGB&YHN7UJM(IK<";

    //�ǂݍ��ނׂ��Z�[�u�f�[�^��
    public static string dataname;
    //
    public string dateTime;
    //Dictionary<string, int> stagescore = new Dictionary<string, int>();
    public List<string> stagescore = new List<string>();
    public List<string> exclearStagename = new List<string>();
    public List<string> clearStagename = new List<string>();

    [System.Serializable]
    public class PlayerData
    {
        public string dateTime;
        //public Dictionary<string, int> stagescore = new Dictionary<string, int>();
        public List<string> stagescore;
    }

    public static SaveManager instance;

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
        return stagescore.Count/2;
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

    public void SaveData(string stagename, int add_blocknum, int add_blocknum_goal)
    {
        Debug.Log(dataname);
        PlayerData data = new PlayerData();
        StreamWriter writer;
        data.dateTime = DateTime.Now.ToString("yyyy�NM��d�� HH:mm:ss");
        //��U�����̃��X�g�ɓo�^���Ă���PlayerData�̕��Ƀ��X�g���Ɠn���Ă��܂�
        //�����ڕW�Ɠ����������葁���N���A������ex�ɓo�^����//�X�R�A���L�^����
        //�����������̃X�e�[�W���������Ă���΁A�X�R�A���X�V���邾���ɂ���
        if (stagescore.Contains(stagename)) stagescore[stagescore.IndexOf(stagename) + 1] = add_blocknum.ToString();
        else
        {
            stagescore.Add(stagename);
            stagescore.Add(add_blocknum.ToString());
        }

        data.stagescore = stagescore;
        //Json�`���ɕϊ�����
        string jsonstr = JsonUtility.ToJson(data);
        Debug.Log(jsonstr);
        //AES�ňÍ�������
        jsonstr = EncryptAES(jsonstr);
        //�t�@�C���ɏ������ޏ���
        writer = new StreamWriter(Application.dataPath + "/" + dataname + ".json", false);
        writer.Write(jsonstr);
        writer.Flush();
        writer.Close();
        Debug.Log("�Z�[�u���I�����܂���");
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
    }


    public void OnDeleteSaveData(string savename)
    {
        string dataname = Application.dataPath + "/" + savename + ".json";
        if (File.Exists(dataname)) File.Delete(dataname);
        
    }


    //�Q�l�T�C�g�Fhttps://yuyu-code.com/programming-languages/c-sharp/aes-encryption-decryption/
    //                  https://magnaga.com/2016/08/29/save-encryption/
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

    //��������
    public static string DecryptAES(string text)
    {
        Aes aes = Aes.Create();
        //AES�̐ݒ�(�Í��Ɠ���)
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
