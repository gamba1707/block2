using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//ここではステージセレクト画面をほぼ管理しています
public class selectUI : MonoBehaviour
{
    [Header("ロード画面")]
    [SerializeField] private Loading_fade LoadUI;
    [Header("ボタンの親オブジェクト")]
    [SerializeField] private Transform selectbutton_parent;

    private bool load_scene;

    //ステージデータ（ボタンが押されたときにそれぞれにアタッチされているデータを渡す為）
    //MapDataスクリプト
    [SerializeField] MapData mapData;

    private void Start()
    {
        //フェードインを見せる
        LoadUI.Fadein();
    }

    //タイトルへ戻るを押した
    public void OnTitleBack()
    {
        //フェードアウト（暗転する）
        LoadUI.Fadeout();
        //シーンを読み込む
        StartCoroutine(LoadScene("title"));
    }

    //それぞれのステージのボタンが押されたら呼ばれる
    public void OnClickButton(MapData_scrobj stagedata)
    {
        //フェードアウトさせる
        LoadUI.Fadeout();
        //それぞれのボタンにアタッチされているステージデータを渡す
        mapData.setMapData(stagedata);
        //シーンを読み込む
        if(!load_scene)StartCoroutine(LoadScene("Stage"));
        //２回目以降は受け付けない
        load_scene = true;
    }

    //シーン読み込み演出
    private IEnumerator LoadScene(string scenename)
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
}
