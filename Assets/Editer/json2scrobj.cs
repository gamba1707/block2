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

        if (GUILayout.Button("ステージデータの作成"))
        {
            Debug.Log("マスターのステージデータを作成");
            Onjson2scrobj(jsonconvertor);
        }
    }

    void Onjson2scrobj(jsonconvertor json)
    {
        if (json.jsonAsset == null)
        {
            Debug.LogWarning("読み込むjsonファイルがアタッチされていません。");
            return;
        }
        string jsonText = json.jsonAsset.ToString();
        Debug.Log(jsonText);
        MapData.map jsondata = JsonUtility.FromJson<MapData.map>(jsonText);
        
    }
}
