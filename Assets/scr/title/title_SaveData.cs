using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

//タイトル画面のセーブ画面を管理
public class title_SaveData : MonoBehaviour
{
    //新しいデータの場合はすぐにステージへ飛ばすため、そのステージデータ
    [Header("最初のステージデータ")]
    [SerializeField] MapData_scrobj firststagedata;

    //削除モード
    [Header("削除モード")]
    [SerializeField] bool deleteMode;

    //選んだセーブデータ
    [Header("選んだデータ名")]
    [SerializeField] int selectdata;

    //削除して大丈夫か聞くパネル
    [Header("削除して大丈夫か聞くパネル")]
    [SerializeField] GameObject confPanel;

    //それぞれのボタンなどのテキスト
    [Header("一番上のタイトルテキスト")]
    [SerializeField] TextMeshProUGUI header;

    [Header("それぞれのボタンなどのテキスト")]
    [SerializeField] TextMeshProUGUI SaveData1;
    [SerializeField] TextMeshProUGUI SaveData2;
    [SerializeField] TextMeshProUGUI SaveData3;
    [SerializeField] TextMeshProUGUI deleteButton;

    [Header("スタッフクレジットボタン")]
    [SerializeField] GameObject CreditButton;
    bool All_Crear;

    //フェードパネル
    [Header("フェードパネル")]
    [SerializeField] private Loading_fade LoadUI;

    //表示されたら
    private void OnEnable()
    {
        //それぞれのボタンの内容を書き換える
        settextdata(1);
        settextdata(2);
        settextdata(3);

        //全クリの場合スタッフクレジットを見れるように
        if (All_Crear) CreditButton.SetActive(true);
        else CreditButton.SetActive(false);
    }

    //番号を受け取るとそれに合うtextmeshproを返す
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
                //全クリの場合色を変える
                if (SaveManager.instance.clearnum() >= 27)
                {
                    All_Crear = true;
                    //あったら日付とクリア数を表示させる
                    text.color = new Color(0.7924528f, 0.7023308f, 0.0f);//黄金色
                    text.text = "★"+SaveManager.instance.getDateTime() + "\nクリア数：" + SaveManager.instance.clearnum();
                }
                else
                {
                    text.color = Color.black;
                    //あったら日付とクリア数を表示させる
                    text.text = SaveManager.instance.getDateTime() + "\nクリア数：" + SaveManager.instance.clearnum();
                }
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
                {
                    //全クリの場合色を変える
                    if (SaveManager.instance.clearnum() >= 27)
                    {
                        All_Crear = true;
                        //あったら日付とクリア数を表示させる
                        text.color = new Color(0.7924528f, 0.7023308f, 0.0f);//黄金色
                        text.text = "★" + SaveManager.instance.getDateTime() + "\nクリア数：" + SaveManager.instance.clearnum();
                    }
                    else
                    {
                        text.color = Color.black;
                        //あったら日付とクリア数を表示させる
                        text.text = SaveManager.instance.getDateTime() + "\nクリア数：" + SaveManager.instance.clearnum();
                    }
                }
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
                        StartCoroutine(LoadScene("Stage"));
                    }
                    else
                    {
                        //ステージセレクトに飛ばす
                        StartCoroutine(LoadScene("Select"));
                    }
                }
                else
                {
                    //WebGLじゃない処理
                    //セーブデータが存在する
                    if (System.IO.File.Exists(Application.dataPath + "/SaveData" + selectdata + ".json"))
                    {
                        //ステージセレクトに飛ばす
                        StartCoroutine(LoadScene("Select"));
                    }
                    else
                    {
                        //セーブデータが存在しない
                        //はじめのステージを読み込む
                        MapData.mapinstance.setMapData(firststagedata);
                        //飛ばす
                        StartCoroutine(LoadScene("Stage"));
                    }
                }
            }
        }
    }

    //シーン読み込み演出
    private IEnumerator LoadScene(string scenename)
    {
        LoadUI.Fadeout();
        //暗転するまで待つ
        while (LoadUI.Fade_move) yield return null;

        //シーン読み込み
        var async = SceneManager.LoadSceneAsync(scenename);

        while (!async.isDone)
        {
            Debug.Log(async.progress);
            yield return null;
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
            deleteButton.color = Color.red;
            deleteButton.text = "削除";
        }
        else
        {
            //削除モードにする
            deleteMode = true;
            //テキストも変更
            header.text = "セーブデータ　<color=red>削除モード";
            deleteButton.color= Color.black;
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

    //クリア後に押すともう一度見れる
    public void OnCredit()
    {
        StartCoroutine(Scene_move("Ending"));
    }

    //シーン移動時にフェードもしつつシーン読み込みもしたかった
    IEnumerator Scene_move(string scenename)
    {
        //動きをもとに戻す
        Time.timeScale = 1;
        if (MapData.mapinstance.Last) yield return new WaitForSecondsRealtime(3f);
        LoadUI.Fadeout();
        //暗転するまで待つ
        while (LoadUI.Fade_move) yield return null;

        //シーン読み込み
        var async = SceneManager.LoadSceneAsync(scenename);

        while (!async.isDone)
        {
            Debug.Log(async.progress);
            yield return null;
        }
    }
}
