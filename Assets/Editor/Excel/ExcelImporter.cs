using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Excelからステージデータを構築する元のやつ
[CreateAssetMenu(menuName = "ScriptableObject/Create Excel Impoter")]
public class ExcelImporter : ScriptableObject
{
    //Excelの絶対パス
    public string path;
    //選んだシート
    [System.NonSerialized] public int selectsheet;
    //シート一覧
    [System.NonSerialized] public List<string> sheetNameList = new List<string>();
}
