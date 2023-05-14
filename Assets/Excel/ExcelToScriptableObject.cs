using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.Util;
using UnityEditor;
using NPOI.XWPF.UserModel;
using System;
using System.Security.Cryptography.X509Certificates;

[CustomEditor(typeof(ExcelImporter))]
public class ExcelToScriptableObject : Editor
{
    bool OnSheetLoad,OnDataLoad;
    [SerializeField] int clearnum;
    [SerializeField] Vector3 goalpos;
    [SerializeField] List<Vector3> floorpos = new List<Vector3>();
    [SerializeField] List<Vector3> fallpos = new List<Vector3>();

    public override void OnInspectorGUI()
    {
        var excelImpoter = target as ExcelImporter;
        
        DrawDefaultInspector();
        EditorGUILayout.LabelField("シート読み込み");
        //インスペクターにD&Dするとパスを出してくれる
        //引用サイトhttps://warapuri.com/post/24185933889/unity-inspector-editor%E4%B8%8A%E3%81%ABdragdrop%E3%81%97%E3%81%9F%E3%81%84
        var evt = Event.current;

        var dropArea = GUILayoutUtility.GetRect(0.0f, 50.0f, GUILayout.ExpandWidth(true));
        GUI.Box(dropArea, "Drag & Drop");
        int id = GUIUtility.GetControlID(FocusType.Passive);
        switch (evt.type)
        {
            case EventType.DragUpdated:
            case EventType.DragPerform:
                if (!dropArea.Contains(evt.mousePosition)) break;

                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                DragAndDrop.activeControlID = id;

                if (evt.type == EventType.DragPerform)
                {
                    DragAndDrop.AcceptDrag();

                    foreach (var draggedObject in DragAndDrop.objectReferences)
                    {
                        Debug.Log("Drag Object:" + AssetDatabase.GetAssetPath(draggedObject));
                        excelImpoter.path = AssetDatabase.GetAssetPath(draggedObject);
                    }
                    DragAndDrop.activeControlID = 0;
                }
                Event.current.Use();
                break;
        }
        
        if (GUILayout.Button("シート情報の読み込み"))
        {
            Debug.Log("シート情報の読み込みを開始しています");
            try
            {
                OnLoadSheet(excelImpoter);
                OnSheetLoad = true;
            }
            catch (FileNotFoundException e)
            {
                Debug.LogError("読み込むファイルがありません。\n"+e);
            }
            catch (IOException e)
            {
                Debug.LogError("おそらくファイルが開いたままになっています。\n" + e);
            }
            
        }
        if (OnSheetLoad)
        {
            EditorGUILayout.Space(20);
            EditorGUILayout.LabelField("シートの指定");
            excelImpoter.selectsheet = EditorGUILayout.Popup("シート名", excelImpoter.selectsheet, excelImpoter.sheetNameList.ToArray());
            if (GUILayout.Button("ステージ情報の読み込み"))
            {
                try
                {
                    OnLoadData(excelImpoter);
                }
                catch (FileNotFoundException e)
                {
                    Debug.LogError("読み込むファイルがありません。\n" + e);
                }
                catch (IOException e)
                {
                    Debug.LogError("おそらくファイルが開いたままになっています。\n" + e);
                }
            }
        }
        if(OnSheetLoad)
        {
            EditorGUILayout.Space(20);
            var so = new SerializedObject(this);
            EditorGUILayout.IntField("目標クリア数",clearnum);
            EditorGUILayout.Vector3Field("ゴール地点",goalpos);
            EditorGUILayout.PropertyField(so.FindProperty("floorpos"), true);
            EditorGUILayout.PropertyField(so.FindProperty("fallpos"), true);
            if (GUILayout.Button("ステージ情報の保存"))
            {
                Debug.Log(excelImpoter.sheetNameList[excelImpoter.selectsheet]);
                OnSaveObj(excelImpoter.sheetNameList[excelImpoter.selectsheet]);
            }
        }

    }

    void OnLoadSheet(ExcelImporter excel)
    {
        if (excel == null)
        {

        }
        //押したときの並び順に変更したいため、初期化する
        excel.sheetNameList.Clear();
        IWorkbook book = WorkbookFactory.Create(@excel.path);
        string[] sheetname = new string[book.NumberOfSheets];
        for (int i = 0; i < sheetname.Length; i++)
        {
            excel.sheetNameList.Add(book.GetSheetAt(i).SheetName);
        }
    }

    void OnLoadData(ExcelImporter excel)
    {
        IWorkbook book = WorkbookFactory.Create(@excel.path);
        ISheet sheet=book.GetSheetAt(excel.selectsheet);
        //sheet.GetRow(行番号-1).GetCell(列番号-1);
        int x_end= (int)sheet.GetRow(0).GetCell(1).NumericCellValue-1;
        int y_end= (int)sheet.GetRow(1).GetCell(0).NumericCellValue-1;
        Debug.Log(x_end+","+y_end);
        clearnum= (int)sheet.GetRow(y_end+1).GetCell(2).NumericCellValue;
        Vector2 startpoint=new Vector2((int)sheet.GetRow(y_end + 2).GetCell(3).NumericCellValue, (int)sheet.GetRow(y_end + 2).GetCell(2).NumericCellValue);
        goalpos = new Vector3(0, (int)sheet.GetRow(y_end + 3).GetCell(2).NumericCellValue, (int)sheet.GetRow(y_end + 3).GetCell(3).NumericCellValue);
        goalpos = new Vector3(0,((startpoint.y-goalpos.y+1)*1.5f)-0.75f, (goalpos.z- startpoint.x) * 1.5f);
        Debug.Log(startpoint);

        floorpos.Clear();
        fallpos.Clear();

        for (int i = 0;i<y_end;i++)
        {
            for(int j = 0; j < x_end; j++)
            {
                if(sheet.GetRow(i)!=null&& sheet.GetRow(i).GetCell(j) != null)
                {
                    if (sheet.GetRow(i).GetCell(j).CellType == CellType.String)
                    {
                        if (sheet.GetRow(i).GetCell(j).StringCellValue.Equals("T"))
                        {
                            floorpos.Add(new Vector3(0, (startpoint.y - i) * 1.5f, (j - startpoint.x + 1) * 1.5f));
                        }
                        else if (sheet.GetRow(i).GetCell(j).StringCellValue.Equals("F"))
                        {
                            fallpos.Add(new Vector3(0, (startpoint.y - i) * 1.5f, (j-startpoint.x+1) * 1.5f));
                        }
                        Debug.Log(sheet.GetRow(i).GetCell(j).Address + "     " + sheet.GetRow(i).GetCell(j).StringCellValue+ new Vector3(0, (startpoint.y - i) * 1.5f, (j - startpoint.x+1) * 1.5f));
                    }
                }
            }
        }

    }

    void OnSaveObj(string name)
    {
        var obj = ScriptableObject.CreateInstance<MapData_scrobj>();
        obj.clearnum = clearnum;
        obj.floornum = fallpos.Count;
        obj.fallnum = fallpos.Count;
        obj.floorpos = floorpos.ToArray();
        obj.fallpos = fallpos.ToArray();
        obj.goalpos = goalpos;
        //フォルダーが存在しないなら作る
        if (!System.IO.Directory.Exists("Assets/StageData")) System.IO.Directory.CreateDirectory(Application.dataPath + "/StageData");
        //ScriptableObjectを作成
        AssetDatabase.CreateAsset(obj, Path.Combine("Assets/StageData", name + ".asset"));
    }
}
