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
        //��U�����̃��X�g�ɓo�^���Ă���PlayerData�̕��Ƀ��X�g���Ɠn���Ă��܂�
        //�����ڕW�Ɠ����������葁���N���A������ex�ɓo�^����
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
        //Json�`���ɕϊ�����
        string jsonstr = JsonUtility.ToJson(data);
        Debug.Log(jsonstr);
        //AES�ňÍ�������
        jsonstr=EncryptAES(jsonstr);
        //�t�@�C���ɏ������ޏ���
        writer = new StreamWriter(Application.dataPath + "/savedata.json", false);
        writer.Write(jsonstr);
        writer.Flush();
        writer.Close();
        Debug.Log("�Z�[�u���I�����܂���");
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
