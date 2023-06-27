using System.Collections.Generic;
using UnityEngine;
using System.IO;
using NPOI.SS.UserModel;
using UnityEditor;

//Excelからステージデータを構築するEditor拡張
[CustomEditor(typeof(ExcelImporter))]
public class ExcelToScriptableObject : Editor
{
    //シートを読み込んだ、データを読み込んだ
    bool OnSheetLoad, OnDataLoad;

    //読み取ったステージデータたち
    [SerializeField] int clearnum;//クリア数
    [SerializeField] float deadline;//デッドライン（自動計算）
    [SerializeField] Vector3 goalpos;//ゴール位置
    [SerializeField] Vector3 stage_vcampos;//全体カメラの位置
    [SerializeField] List<Vector3> floorpos = new List<Vector3>();//床ブロック
    [SerializeField] List<Vector3> fallpos = new List<Vector3>();//奈落ブロック
    [SerializeField] List<Vector3> Trampolinepos = new List<Vector3>();//地形トランポリンブロック
    [SerializeField] List<Vector3> Downpos = new List<Vector3>();//地形下がるブロック

    //インスペクターで表示するやつ
    public override void OnInspectorGUI()
    {
        //アタッチされた情報を受け取る為
        var excelImpoter = target as ExcelImporter;

        //パスはここで表示する
        DrawDefaultInspector();
        //ラベル表示
        EditorGUILayout.LabelField("シート読み込み");

        //====================================
        //インスペクターにD&Dするとパスを出してくれる
        //引用サイト　https://warapuri.com/post/24185933889/unity-inspector-editor%E4%B8%8A%E3%81%ABdragdrop%E3%81%97%E3%81%9F%E3%81%84
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
        //====================================

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
                Debug.LogError("読み込むファイルがありません。\n" + e);
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
                    //読み込んだことにする
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
        if (OnDataLoad)
        {
            //空白
            EditorGUILayout.Space(20);
            //これを取得
            var so = new SerializedObject(this);
            //読み取ったデータを確認のため表示
            EditorGUILayout.IntField("目標クリア数", clearnum);
            EditorGUILayout.Vector3Field("ゴール地点", goalpos);
            EditorGUILayout.FloatField("デッドライン（最終ブロックの-10地点）", deadline);
            EditorGUILayout.Vector3Field("全体カメラ座標", stage_vcampos);
            EditorGUILayout.PropertyField(so.FindProperty("floorpos"), true);
            EditorGUILayout.PropertyField(so.FindProperty("fallpos"), true);
            EditorGUILayout.PropertyField(so.FindProperty("Trampolinepos"), true);
            EditorGUILayout.PropertyField(so.FindProperty("Downpos"), true);
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
        ISheet sheet = book.GetSheetAt(excel.selectsheet);
        //sheet.GetRow(行番号-1).GetCell(列番号-1);
        //各シートの左上にあるEの位置情報を取得
        //B1にある横方向の読み取り終わり位置を取得
        int x_end = (int)sheet.GetRow(0).GetCell(1).NumericCellValue - 1;
        //A2にある縦方向の読み取り終わり位置を取得
        int y_end = (int)sheet.GetRow(1).GetCell(0).NumericCellValue - 1;
        Debug.Log(x_end + "," + y_end);

        //縦の終わり位置のすぐ下にある目標数を取得
        clearnum = (int)sheet.GetRow(y_end + 1).GetCell(2).NumericCellValue;
        //スタート地点の一つ下を(0,0,0)にするためにスタート位置を取得
        Vector2 startpoint = new Vector2((int)sheet.GetRow(y_end + 2).GetCell(3).NumericCellValue, (int)sheet.GetRow(y_end + 2).GetCell(2).NumericCellValue);
        //fallオブジェクトを通り抜けてしまった時用の座標（一番下の+3の位置）
        deadline = -1 * Mathf.Abs((y_end * 1.5f) - startpoint.y + 3);
        //ゴール位置を取得（ゴールだけはちょっと位置がずれるので2段階に分けて変換して格納）
        goalpos = new Vector3(0, (int)sheet.GetRow(y_end + 3).GetCell(2).NumericCellValue, (int)sheet.GetRow(y_end + 3).GetCell(3).NumericCellValue);
        goalpos = new Vector3(0, ((startpoint.y - goalpos.y + 1) * 1.5f) - 0.75f, (goalpos.z - startpoint.x) * 1.5f);
        Debug.Log(startpoint);

        //2度押した場合用に一応Listを初期化しておく
        floorpos.Clear();
        fallpos.Clear();
        Trampolinepos.Clear();
        Downpos.Clear();

        //ここから地形データを読み取り
        //左上から横方向に順番に読み取っていく
        for (int i = 0; i < y_end; i++)
        {
            for (int j = 0; j < x_end; j++)
            {
                //一行何もないとエラーになるのでi行に何かある&&そのi行とj列のセルに何かある
                if (sheet.GetRow(i) != null && sheet.GetRow(i).GetCell(j) != null)
                {
                    //そのセルのタイプは文字列？
                    if (sheet.GetRow(i).GetCell(j).CellType == CellType.String)
                    {
                        switch (sheet.GetRow(i).GetCell(j).StringCellValue)
                        {
                            case "T"://その文字がT（普通の床）の場合
                                //床Listに座標を計算して格納
                                floorpos.Add(new Vector3(0, (startpoint.y - i) * 1.5f, (j - startpoint.x + 1) * 1.5f));
                                break;
                            case "F"://その文字がF（奈落）
                                //奈落Listに座標を計算して格納
                                fallpos.Add(new Vector3(0, (startpoint.y - i) * 1.5f, (j - startpoint.x + 1) * 1.5f));
                                break;
                            case "TT"://地形トランポリンブロック
                                //Listに座標を計算して格納
                                Trampolinepos.Add(new Vector3(0, (startpoint.y - i) * 1.5f, (j - startpoint.x + 1) * 1.5f));
                                break;
                            case "TD"://地形下がる
                                //Listに座標を計算して格納
                                Downpos.Add(new Vector3(0, (startpoint.y - i) * 1.5f, (j - startpoint.x + 1) * 1.5f));
                                break;
                        }

                        Debug.Log(sheet.GetRow(i).GetCell(j).Address + "     " + sheet.GetRow(i).GetCell(j).StringCellValue + new Vector3(0, (startpoint.y - i) * 1.5f, (j - startpoint.x + 1) * 1.5f));
                    }
                }
            }
        }

        //全体カメラの位置計算
        //全体を移すには（奥行き,縦方向の位置,横方向の位置）を計算する
        //その時に多分直角三角形の1:1:√2みたいなやつになれば奥行きの位置を計算できると思った
        //一応横方向の√2と縦方向の√2を計算して大きかった方を奥行きの位置として採用する
        float x = Mathf.Abs(x_end / 2) * 1.5f * 1.414f;
        float y = Mathf.Abs(y_end / 2) * 1.5f * 1.414f;
        //横長ステージなら上の命令、縦長ステージなら下の命令
        if (x >= y) stage_vcampos = new Vector3(x, (startpoint.y - (y_end / 2)) * 1.5f, ((x_end / 2) - startpoint.x) * 1.5f);
        else stage_vcampos = new Vector3(y, (startpoint.y - (y_end / 2)) * 1.5f, ((x_end / 2) - startpoint.x) * 1.5f);
        Debug.Log("検討x" + x + "    検討y" + y);
    }

    //ステージ情報の保存を押したら読み取ったデータをシートの名前でScriptableObjectにする
    void OnSaveObj(string name)
    {
        //地形データの元が入っているMapData_scrobjのインスタンスを作る
        var obj = ScriptableObject.CreateInstance<MapData_scrobj>();
        //保持していたデータを割り当てる
        obj.clearnum = clearnum;
        obj.floorpos = floorpos.ToArray();
        obj.fallpos = fallpos.ToArray();
        obj.Trampolinepos = Trampolinepos.ToArray();
        obj.Downpos = Downpos.ToArray();
        obj.goalpos = goalpos;
        obj.stage_vcampos = stage_vcampos;
        obj.deadline = deadline;
        //フォルダーが存在しないなら作る
        if (!Directory.Exists("Assets/StageData")) Directory.CreateDirectory(Application.dataPath + "/StageData");
        //ScriptableObjectを作成
        AssetDatabase.CreateAsset(obj, Path.Combine("Assets/StageData", name + ".asset"));
    }
}
