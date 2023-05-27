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
    [SerializeField] float deadline;
    [SerializeField] Vector3 goalpos;
    [SerializeField] Vector3 stage_vcampos;
    [SerializeField] List<Vector3> floorpos = new List<Vector3>();
    [SerializeField] List<Vector3> fallpos = new List<Vector3>();

    public override void OnInspectorGUI()
    {
        //アタッチされた情報を受け取る為
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
        
        //読み込みボタン
        if (GUILayout.Button("シート情報の読み込み"))
        {
            Debug.Log("シート情報の読み込みを開始しています");
            try
            {
                //シートの情報を読み取る
                OnLoadSheet(excelImpoter);
                //シートを読み込んだということで追加で情報を表示させる
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
        //シートを読み取った後表示する
        if (OnSheetLoad)
        {
            //隙間
            EditorGUILayout.Space(20);
            //タイトルラベル
            EditorGUILayout.LabelField("シートの指定");
            //シートをプルダウンで選べるやつ（プルダウンは数字で管理されているので代入）
            //（ラベル名、番号、その番号に対応するシート名）
            excelImpoter.selectsheet = EditorGUILayout.Popup("シート名", excelImpoter.selectsheet, excelImpoter.sheetNameList.ToArray());
            //ステージ情報の読み込みボタンを押したら
            if (GUILayout.Button("ステージ情報の読み込み"))
            {
                try
                {
                    //実際に中のデータを読み取ってデータにする
                    OnLoadData(excelImpoter);
                    OnDataLoad = true;
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
        //シートを指定して地形を読み込んだら表示
        if(OnDataLoad)
        {
            //空白
            EditorGUILayout.Space(20);
            //これを取得
            var so = new SerializedObject(this);
            //読み取ったデータを確認のため表示
            EditorGUILayout.IntField("目標クリア数",clearnum);
            EditorGUILayout.Vector3Field("ゴール地点",goalpos);
            EditorGUILayout.Vector3Field("全体カメラ座標",stage_vcampos);
            EditorGUILayout.PropertyField(so.FindProperty("floorpos"), true);
            EditorGUILayout.PropertyField(so.FindProperty("fallpos"), true);
            //ステージ情報の保存ボタン
            if (GUILayout.Button("ステージ情報の保存"))
            {
                //読み取ったデータをまとめてScriptableObjectにする（名前はExcelのシート名で）
                Debug.Log(excelImpoter.sheetNameList[excelImpoter.selectsheet]);
                OnSaveObj(excelImpoter.sheetNameList[excelImpoter.selectsheet]);
            }
        }

    }

    //データを受け取ってExcelのシート名を全てListに格納する
    void OnLoadSheet(ExcelImporter excel)
    {
        //押したときの並び順に変更したいため、初期化する
        excel.sheetNameList.Clear();
        //指定されたパスにあるExcelを取得
        IWorkbook book = WorkbookFactory.Create(@excel.path);
        //for文で回ってシート名を全てListに格納
        for (int i = 0; i < book.NumberOfSheets; i++)
        {
            excel.sheetNameList.Add(book.GetSheetAt(i).SheetName);
        }
    }

    //データをもらって選択されたシートにある地形データを座標などに変換して格納しておく
    void OnLoadData(ExcelImporter excel)
    {
        //指定されたパスにあるExcelを取得
        IWorkbook book = WorkbookFactory.Create(@excel.path);
        //指定されたシートを取得
        ISheet sheet=book.GetSheetAt(excel.selectsheet);
        //sheet.GetRow(行番号-1).GetCell(列番号-1);
        //各シートの左上にあるEの位置情報を取得
        //B1にある横方向の終わり位置を取得
        int x_end= (int)sheet.GetRow(0).GetCell(1).NumericCellValue-1;
        //A2にある縦方向の終わり位置を取得
        int y_end= (int)sheet.GetRow(1).GetCell(0).NumericCellValue-1;
        float y_start = 0;
        Debug.Log(x_end+","+y_end);
        //fallオブジェクトを通り抜けてしまった時用の座標（一番下の+3の位置）
        deadline = (y_end * 1.5f)+3;
        //縦の終わり位置のすぐ下にある目標数を取得
        clearnum= (int)sheet.GetRow(y_end+1).GetCell(2).NumericCellValue;
        //スタート地点の一つ下を(0,0,0)にするためにスタート位置を取得
        Vector2 startpoint=new Vector2((int)sheet.GetRow(y_end + 2).GetCell(3).NumericCellValue, (int)sheet.GetRow(y_end + 2).GetCell(2).NumericCellValue);
        //ゴール位置を取得（ゴールだけはちょっと位置がずれるので2段階に分けて変換して格納）
        goalpos = new Vector3(0, (int)sheet.GetRow(y_end + 3).GetCell(2).NumericCellValue, (int)sheet.GetRow(y_end + 3).GetCell(3).NumericCellValue);
        goalpos = new Vector3(0,((startpoint.y-goalpos.y+1)*1.5f)-0.75f, (goalpos.z- startpoint.x) * 1.5f);
        Debug.Log(startpoint);
        
        //2度押した場合用に一応Listを初期化しておく
        floorpos.Clear();
        fallpos.Clear();

        //ここから地形データを読み取り
        //左上から横方向に順番に読み取っていく
        for (int i = 0;i<y_end;i++)
        {
            for(int j = 0; j < x_end; j++)
            {
                //一行何もないとエラーになるのでi行に何かある&&そのi行とj列のセルに何かある
                if(sheet.GetRow(i)!=null&& sheet.GetRow(i).GetCell(j) != null)
                {
                    //そのセルのタイプは文字列？
                    if (sheet.GetRow(i).GetCell(j).CellType == CellType.String)
                    {
                        //その文字がT（普通の床）の場合
                        if (sheet.GetRow(i).GetCell(j).StringCellValue.Equals("T"))
                        {
                            //床Listに座標を計算して格納
                            floorpos.Add(new Vector3(0, (startpoint.y - i) * 1.5f, (j - startpoint.x + 1) * 1.5f));
                            if ((startpoint.y - i) * 1.5f > y_start) y_start = (startpoint.y - i) * 1.5f;
                        }
                        //その文字がF（奈落）
                        else if (sheet.GetRow(i).GetCell(j).StringCellValue.Equals("F"))
                        {
                            //奈落Listに座標を計算して格納
                            fallpos.Add(new Vector3(0, (startpoint.y - i) * 1.5f, (j-startpoint.x+1) * 1.5f));
                        }
                        
                        Debug.Log(sheet.GetRow(i).GetCell(j).Address + "     " + sheet.GetRow(i).GetCell(j).StringCellValue+ new Vector3(0, (startpoint.y - i) * 1.5f, (j - startpoint.x+1) * 1.5f));
                    }
                }
            }
        }
        float x = Mathf.Abs(goalpos.z / 2) + 10f;
        if (x < 20) x += 10;
        stage_vcampos = new Vector3(x, (float)(startpoint.y-(((y_end*1.5)-y_start) / 2)), goalpos.z / 2 );
        Debug.Log("y_start:"+y_start+"  "+y_end);
    }

    //ステージ情報の保存を押したら読み取ったデータをシートの名前でScriptableObjectにする
    void OnSaveObj(string name)
    {
        //地形データの元が入っているMapData_scrobjのインスタンスを作る
        var obj = ScriptableObject.CreateInstance<MapData_scrobj>();
        //保持していたデータを割り当てる
        obj.clearnum = clearnum;
        obj.floornum = fallpos.Count;
        obj.fallnum = fallpos.Count;
        obj.floorpos = floorpos.ToArray();
        obj.fallpos = fallpos.ToArray();
        obj.goalpos = goalpos;
        obj.stage_vcampos = stage_vcampos;
        obj.deadline= deadline;
        //フォルダーが存在しないなら作る
        if (!System.IO.Directory.Exists("Assets/StageData")) System.IO.Directory.CreateDirectory(Application.dataPath + "/StageData");
        //ScriptableObjectを作成
        AssetDatabase.CreateAsset(obj, Path.Combine("Assets/StageData", name + ".asset"));
    }
}
