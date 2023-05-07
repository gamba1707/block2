using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.TextCore.Text;

[CustomEditor(typeof(jsonconvertor))]
public class json2scrobj : Editor
{
    public override void OnInspectorGUI()
    {
        var jsonconvertor = target as jsonconvertor;
        DrawDefaultInspector();

        if (GUILayout.Button("�X�e�[�W�f�[�^�̍쐬"))
        {
            Debug.Log("�}�X�^�[�̃X�e�[�W�f�[�^���쐬");
            Onjson2scrobj(jsonconvertor);
        }
    }

    void Onjson2scrobj(jsonconvertor json)
    {
        if (json.jsonAsset == null)
        {
            Debug.LogWarning("�ǂݍ���json�t�@�C�����A�^�b�`����Ă��܂���B");
            return;
        }
        string jsonText = json.jsonAsset.ToString();
        Debug.Log(jsonText);
        MapData.map jsondata = JsonUtility.FromJson<MapData.map>(jsonText);
        
    }
}
