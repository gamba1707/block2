using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class title_SaveData : MonoBehaviour
{
    [SerializeField] MapData_scrobj firststagedata;
    [SerializeField] bool deleteMode;
    [SerializeField] int selectdata;
    [SerializeField] GameObject confPanel;
    [SerializeField] TextMeshProUGUI header, SaveData1, SaveData2, SaveData3,deleteButton;
    private void OnEnable()
    {
        settextdata(1);
        settextdata(2);
        settextdata(3);
    }

    void settextdata(int num)
    {
        TextMeshProUGUI text=null;
        switch (num)
        {
            case 1:
                text = SaveData1;
                break;
            case 2:
                text = SaveData2;
                break;
            case 3:
                text = SaveData3;
                break;
            default:
                Debug.LogError("数字割り当てを見直してみてください");
                break;
        }
        Debug.Log(Application.dataPath + "/SaveData" + num + ".json");
        if (System.IO.File.Exists(Application.dataPath + "/SaveData" + num + ".json"))
        {
            SaveManager.instance.LoadSaveData("SaveData" + num);
            text.text = SaveManager.instance.getDateTime()+"\nクリア数："+SaveManager.instance.clearnum();
        }
        else
        {
            text.text = "新しいデータ";
        }
    }

    public void LoadSaveData(int datanum)
    {
        selectdata = datanum;
        if (deleteMode)
        {
            confPanel.SetActive(true);
        }
        else
        {
            SaveManager.instance.LoadSaveData("SaveData" + selectdata);
            if (System.IO.File.Exists(Application.dataPath + "/SaveData" + selectdata + ".json"))
            {
                
                SceneManager.LoadScene("Select");
            }
            else
            {
                MapData.mapinstance.setMapData(firststagedata);
                SceneManager.LoadScene("Stage");
            }
            
        }
    }


    public void OnDeleteMode()
    {
        if (deleteMode)
        {
            deleteMode = false;
            header.text = "セーブデータ";
            deleteButton.text = "<color=red>削除";
        }
        else
        {
            deleteMode = true;
            header.text = "セーブデータ　<color=red>削除モード";
            deleteButton.text= "戻る";
        }
    }

    public void OnDeleteData()
    {
        SaveManager.instance.OnDeleteSaveData("SaveData" + selectdata);
        confPanel.SetActive(false);
        settextdata(selectdata);
    }

    public void OnReturnDelete()
    {
        confPanel.SetActive(false);
    }
}
