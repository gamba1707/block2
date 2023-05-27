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
        //�A�^�b�`���ꂽ�����󂯎���
        var excelImpoter = target as ExcelImporter;
        
        DrawDefaultInspector();
        EditorGUILayout.LabelField("�V�[�g�ǂݍ���");

        //�C���X�y�N�^�[��D&D����ƃp�X���o���Ă����
        //���p�T�C�ghttps://warapuri.com/post/24185933889/unity-inspector-editor%E4%B8%8A%E3%81%ABdragdrop%E3%81%97%E3%81%9F%E3%81%84
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
        
        //�ǂݍ��݃{�^��
        if (GUILayout.Button("�V�[�g���̓ǂݍ���"))
        {
            Debug.Log("�V�[�g���̓ǂݍ��݂��J�n���Ă��܂�");
            try
            {
                //�V�[�g�̏���ǂݎ��
                OnLoadSheet(excelImpoter);
                //�V�[�g��ǂݍ��񂾂Ƃ������ƂŒǉ��ŏ���\��������
                OnSheetLoad = true;
            }
            catch (FileNotFoundException e)
            {
                Debug.LogError("�ǂݍ��ރt�@�C��������܂���B\n"+e);
            }
            catch (IOException e)
            {
                Debug.LogError("�����炭�t�@�C�����J�����܂܂ɂȂ��Ă��܂��B\n" + e);
            }
            
        }
        //�V�[�g��ǂݎ������\������
        if (OnSheetLoad)
        {
            //����
            EditorGUILayout.Space(20);
            //�^�C�g�����x��
            EditorGUILayout.LabelField("�V�[�g�̎w��");
            //�V�[�g���v���_�E���őI�ׂ��i�v���_�E���͐����ŊǗ�����Ă���̂ő���j
            //�i���x�����A�ԍ��A���̔ԍ��ɑΉ�����V�[�g���j
            excelImpoter.selectsheet = EditorGUILayout.Popup("�V�[�g��", excelImpoter.selectsheet, excelImpoter.sheetNameList.ToArray());
            //�X�e�[�W���̓ǂݍ��݃{�^������������
            if (GUILayout.Button("�X�e�[�W���̓ǂݍ���"))
            {
                try
                {
                    //���ۂɒ��̃f�[�^��ǂݎ���ăf�[�^�ɂ���
                    OnLoadData(excelImpoter);
                    OnDataLoad = true;
                }
                catch (FileNotFoundException e)
                {
                    Debug.LogError("�ǂݍ��ރt�@�C��������܂���B\n" + e);
                }
                catch (IOException e)
                {
                    Debug.LogError("�����炭�t�@�C�����J�����܂܂ɂȂ��Ă��܂��B\n" + e);
                }
            }
        }
        //�V�[�g���w�肵�Ēn�`��ǂݍ��񂾂�\��
        if(OnDataLoad)
        {
            //��
            EditorGUILayout.Space(20);
            //������擾
            var so = new SerializedObject(this);
            //�ǂݎ�����f�[�^���m�F�̂��ߕ\��
            EditorGUILayout.IntField("�ڕW�N���A��",clearnum);
            EditorGUILayout.Vector3Field("�S�[���n�_",goalpos);
            EditorGUILayout.Vector3Field("�S�̃J�������W",stage_vcampos);
            EditorGUILayout.PropertyField(so.FindProperty("floorpos"), true);
            EditorGUILayout.PropertyField(so.FindProperty("fallpos"), true);
            //�X�e�[�W���̕ۑ��{�^��
            if (GUILayout.Button("�X�e�[�W���̕ۑ�"))
            {
                //�ǂݎ�����f�[�^���܂Ƃ߂�ScriptableObject�ɂ���i���O��Excel�̃V�[�g���Łj
                Debug.Log(excelImpoter.sheetNameList[excelImpoter.selectsheet]);
                OnSaveObj(excelImpoter.sheetNameList[excelImpoter.selectsheet]);
            }
        }

    }

    //�f�[�^���󂯎����Excel�̃V�[�g����S��List�Ɋi�[����
    void OnLoadSheet(ExcelImporter excel)
    {
        //�������Ƃ��̕��я��ɕύX���������߁A����������
        excel.sheetNameList.Clear();
        //�w�肳�ꂽ�p�X�ɂ���Excel���擾
        IWorkbook book = WorkbookFactory.Create(@excel.path);
        //for���ŉ���ăV�[�g����S��List�Ɋi�[
        for (int i = 0; i < book.NumberOfSheets; i++)
        {
            excel.sheetNameList.Add(book.GetSheetAt(i).SheetName);
        }
    }

    //�f�[�^��������đI�����ꂽ�V�[�g�ɂ���n�`�f�[�^�����W�Ȃǂɕϊ����Ċi�[���Ă���
    void OnLoadData(ExcelImporter excel)
    {
        //�w�肳�ꂽ�p�X�ɂ���Excel���擾
        IWorkbook book = WorkbookFactory.Create(@excel.path);
        //�w�肳�ꂽ�V�[�g���擾
        ISheet sheet=book.GetSheetAt(excel.selectsheet);
        //sheet.GetRow(�s�ԍ�-1).GetCell(��ԍ�-1);
        //�e�V�[�g�̍���ɂ���E�̈ʒu�����擾
        //B1�ɂ��鉡�����̏I���ʒu���擾
        int x_end= (int)sheet.GetRow(0).GetCell(1).NumericCellValue-1;
        //A2�ɂ���c�����̏I���ʒu���擾
        int y_end= (int)sheet.GetRow(1).GetCell(0).NumericCellValue-1;
        float y_start = 0;
        Debug.Log(x_end+","+y_end);
        //fall�I�u�W�F�N�g��ʂ蔲���Ă��܂������p�̍��W�i��ԉ���+3�̈ʒu�j
        deadline = (y_end * 1.5f)+3;
        //�c�̏I���ʒu�̂������ɂ���ڕW�����擾
        clearnum= (int)sheet.GetRow(y_end+1).GetCell(2).NumericCellValue;
        //�X�^�[�g�n�_�̈����(0,0,0)�ɂ��邽�߂ɃX�^�[�g�ʒu���擾
        Vector2 startpoint=new Vector2((int)sheet.GetRow(y_end + 2).GetCell(3).NumericCellValue, (int)sheet.GetRow(y_end + 2).GetCell(2).NumericCellValue);
        //�S�[���ʒu���擾�i�S�[�������͂�����ƈʒu�������̂�2�i�K�ɕ����ĕϊ����Ċi�[�j
        goalpos = new Vector3(0, (int)sheet.GetRow(y_end + 3).GetCell(2).NumericCellValue, (int)sheet.GetRow(y_end + 3).GetCell(3).NumericCellValue);
        goalpos = new Vector3(0,((startpoint.y-goalpos.y+1)*1.5f)-0.75f, (goalpos.z- startpoint.x) * 1.5f);
        Debug.Log(startpoint);
        
        //2�x�������ꍇ�p�ɈꉞList�����������Ă���
        floorpos.Clear();
        fallpos.Clear();

        //��������n�`�f�[�^��ǂݎ��
        //���ォ�牡�����ɏ��Ԃɓǂݎ���Ă���
        for (int i = 0;i<y_end;i++)
        {
            for(int j = 0; j < x_end; j++)
            {
                //��s�����Ȃ��ƃG���[�ɂȂ�̂�i�s�ɉ�������&&����i�s��j��̃Z���ɉ�������
                if(sheet.GetRow(i)!=null&& sheet.GetRow(i).GetCell(j) != null)
                {
                    //���̃Z���̃^�C�v�͕�����H
                    if (sheet.GetRow(i).GetCell(j).CellType == CellType.String)
                    {
                        //���̕�����T�i���ʂ̏��j�̏ꍇ
                        if (sheet.GetRow(i).GetCell(j).StringCellValue.Equals("T"))
                        {
                            //��List�ɍ��W���v�Z���Ċi�[
                            floorpos.Add(new Vector3(0, (startpoint.y - i) * 1.5f, (j - startpoint.x + 1) * 1.5f));
                            if ((startpoint.y - i) * 1.5f > y_start) y_start = (startpoint.y - i) * 1.5f;
                        }
                        //���̕�����F�i�ޗ��j
                        else if (sheet.GetRow(i).GetCell(j).StringCellValue.Equals("F"))
                        {
                            //�ޗ�List�ɍ��W���v�Z���Ċi�[
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

    //�X�e�[�W���̕ۑ�����������ǂݎ�����f�[�^���V�[�g�̖��O��ScriptableObject�ɂ���
    void OnSaveObj(string name)
    {
        //�n�`�f�[�^�̌��������Ă���MapData_scrobj�̃C���X�^���X�����
        var obj = ScriptableObject.CreateInstance<MapData_scrobj>();
        //�ێ����Ă����f�[�^�����蓖�Ă�
        obj.clearnum = clearnum;
        obj.floornum = fallpos.Count;
        obj.fallnum = fallpos.Count;
        obj.floorpos = floorpos.ToArray();
        obj.fallpos = fallpos.ToArray();
        obj.goalpos = goalpos;
        obj.stage_vcampos = stage_vcampos;
        obj.deadline= deadline;
        //�t�H���_�[�����݂��Ȃ��Ȃ���
        if (!System.IO.Directory.Exists("Assets/StageData")) System.IO.Directory.CreateDirectory(Application.dataPath + "/StageData");
        //ScriptableObject���쐬
        AssetDatabase.CreateAsset(obj, Path.Combine("Assets/StageData", name + ".asset"));
    }
}
