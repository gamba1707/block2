using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MyScriptable/Create CSV Impoter")]
public class ExcelImporter : ScriptableObject
{
    public string path;
    [System.NonSerialized] public int selectsheet;
    [System.NonSerialized] public List<string> sheetNameList = new List<string>();
}
