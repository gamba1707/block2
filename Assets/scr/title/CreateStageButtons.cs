using System.Collections;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;

//プレハブのボタンにそれぞれつけられている
public class CreateStageButtons : MonoBehaviour
{
    //ステージ名を入れるテキスト
    [SerializeField] TextMeshProUGUI titleText;
    //日時を入れるテキスト
    [SerializeField] TextMeshProUGUI dateText;
    //割り当てられたJsonへのパスを入れる用
    [SerializeField] private string path;
    //フェード
    [SerializeField] private Loading_fade LoadUI;

    // Start is called before the first frame update
    void Start()
    {
        //フェードをさがして取得（処理的に微妙）
        LoadUI = transform.root.Find("LoadPanel").GetComponent<Loading_fade>();
        //解像度を変更されるとなぜか意味不明な数値にスケールを変更されるため
        transform.localScale = new Vector3(1, 1, 1);
    }

    //編集するを押した時
    public void OnEditCreateStage()
    {
        //フェードアウトさせ
        LoadUI.Fadeout();
        //Jsonファイルのパスを渡す
        MapData.mapinstance.setMapData_Create(path);
        //編集モードをオンにする
        MapData.mapinstance.Createmode = true;
        //シーンを読み込む
        StartCoroutine(LoadStageScene("MapEditer"));
    }

    //遊ぶボタンを押したときにStageシーンへ飛ばす
    public void OnPlayCreateStage()
    {
        //フェードアウトさせる
        LoadUI.Fadeout();
        //Jsonファイルのパスを渡す
        MapData.mapinstance.setMapData_Create(path);
        //編集モードをオンにする
        MapData.mapinstance.Createmode = true;
        //シーンを読み込む
        StartCoroutine(LoadStageScene("Stage"));
    }

    //シーンを読み込む
    private IEnumerator LoadStageScene(string scenename)
    {
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

    //プロパティ
    public string Path
    {
        get { return path; }
        set
        {
            //パスを渡されたらテキストも設定してしまう
            //パス設定
            path = value;
            //ステージ名設定
            titleText.text = System.IO.Path.GetFileNameWithoutExtension(path);
            //ファイルの作成日を取得して設定
            DateTime dt = System.IO.File.GetCreationTime(path);
            dateText.text = dt.ToString("D");
        }
    }
}
