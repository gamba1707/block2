using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

//タイトル画面のセーブ画面を管理
public class title_SaveData : MonoBehaviour
{
    //新しいデータの場合はすぐにステージへ飛ばすため、そのステージデータ
    [SerializeField] MapData_scrobj firststagedata;
    //削除モード
    [SerializeField] bool deleteMode;
    //選んだセーブデータ
    [SerializeField] int selectdata;
    //削除して大丈夫か聞くパネル
    [SerializeField] GameObject confPanel;
    //それぞれのボタンなどのテキスト
    [SerializeField] TextMeshProUGUI header, SaveData1, SaveData2, SaveData3, deleteButton;

    //表示されたら
    private void OnEnable()
    {
        //それぞれのボタンの内容を書き換える
        settextdata(1);
        settextdata(2);
        settextdata(3);
    }

    TextMeshProUGUI num2textdata(int num)
    {
        switch (num)
        {
            case 1:
                return SaveData1;
            case 2:
                return SaveData2;
            case 3:
                return SaveData3;
            default:
                Debug.LogError("数字割り当てを見直してみてください");
                return null;
        }
    }

    //数字をもらってそのセーブデータの内容に書き換える
    void settextdata(int num)
    {
        Debug.Log(Application.dataPath + "/SaveData" + num + ".json");
        //数字でどれを編集するのか割り当てる
        TextMeshProUGUI text = num2textdata(num);
        //内容を見るために一旦読み込む
        SaveManager.instance.LoadSaveData("SaveData" + num);
        //WebGLとで分ける
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            //WebGLの処理
            if (PlayerPrefs.GetString("SaveData" + num, "").Equals(""))
            {
                //セーブデータがなさそうの場合、新しいデータとする
                text.text = "新しいデータ";
            }
            else
            {
                //あったら日付とクリア数を表示させる
                text.text = SaveManager.instance.getDateTime() + "\nクリア数：" + SaveManager.instance.clearnum();
            }
        }
        else
        {
            //WebGL以外の処理
            if (System.IO.File.Exists(Application.dataPath + "/SaveData" + num + ".json"))
            {
                //日付が取得できなければ壊れてるとする
                //あったら日付とクリア数を表示させる
                if (SaveManager.instance.getDateTime() == null) text.text = "<color=red>ファイル破損";
                else
                    text.text = SaveManager.instance.getDateTime() + "\nクリア数：" + SaveManager.instance.clearnum();
            }
            else
            {
                //ファイルがなかったら新しいデータとする
                text.text = "新しいデータ";
            }
        }
    }

    //それぞれのボタンについていてデータを読んでセレクトなどに移動する
    public void LoadSaveData(int datanum)
    {
        //なんのデータかを記録する
        selectdata = datanum;

        //もし削除モードだったら
        if (deleteMode)
        {
            //ほんとにいいか尋ねるパネルを出す
            confPanel.SetActive(true);
        }
        else
        {
            //削除モードじゃない
            //ファイルが壊れていなければ読み込む
            if (!num2textdata(selectdata).text.Equals("<color=red>ファイル破損"))
            {
                //データを読み込む
                SaveManager.instance.LoadSaveData("SaveData" + selectdata);
                //またWebGLと分ける
                if (Application.platform == RuntimePlatform.WebGLPlayer)
                {
                    //データがない場合
                    if (PlayerPrefs.GetString("SaveData" + selectdata, "").Equals(""))
                    {
                        //いきなりはじめのステージを読み込む
                        MapData.mapinstance.setMapData(firststagedata);
                        //飛ばす
                        SceneManager.LoadScene("Stage");
                    }
                    else
                    {
                        //ステージセレクトに飛ばす
                        SceneManager.LoadScene("Select");
                    }
                }
                else
                {
                    //WebGLじゃない処理
                    //セーブデータが存在する
                    if (System.IO.File.Exists(Application.dataPath + "/SaveData" + selectdata + ".json"))
                    {
                        //ステージセレクトに飛ばす
                        SceneManager.LoadScene("Select");
                    }
                    else
                    {
                        //セーブデータが存在しない
                        //はじめのステージを読み込む
                        MapData.mapinstance.setMapData(firststagedata);
                        //飛ばす
                        SceneManager.LoadScene("Stage");
                    }
                }
            }
        }
    }

    //削除ボタンを押したら呼ばれる
    public void OnDeleteMode()
    {
        //削除モードONの場合
        if (deleteMode)
        {
            //削除モードオフにする
            deleteMode = false;
            //テキストも変更
            header.text = "セーブデータ";
            deleteButton.text = "<color=red>削除";
        }
        else
        {
            //削除モードにする
            deleteMode = true;
            //テキストも変更
            header.text = "セーブデータ　<color=red>削除モード";
            deleteButton.text = "戻る";
        }
    }

    //データを消す命令を出す
    public void OnDeleteData()
    {
        //選ばれたデータを消す
        SaveManager.instance.OnDeleteSaveData("SaveData" + selectdata);
        //確認パネルを消す
        confPanel.SetActive(false);
        //ボタンの内容を表示しなおす
        settextdata(selectdata);
    }

    //消す画面でやめるを押したときに呼ばれる
    public void OnReturnDelete()
    {
        //確認画面を非表示にする
        confPanel.SetActive(false);
    }
}
