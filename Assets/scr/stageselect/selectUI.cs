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
    //ステージデータ（ボタンが押されたときにそれぞれにアタッチされているデータを渡す為）
    [SerializeField] MapData mapData;
    [SerializeField] GameObject LoadingUI;

    private void Start()
    {
        LoadingUI.SetActive(false);
        //フェードインを見せる
        LoadUI.Fadein();
    }

    public void OnTitleBack()
    {
        LoadUI.Fadeout();
        StartCoroutine(LoadScene("title"));
    }

    //それぞれのステージのボタンが押されたら呼ばれる
    public void OnClickButton(MapData_scrobj stagedata)
    {
        //フェードアウトさせる
        LoadUI.Fadeout();
        //それぞれのボタンにアタッチされているステージデータを渡す
        mapData.setMapData(stagedata);
        StartCoroutine(LoadScene("Stage"));
    }
    private IEnumerator LoadScene(string scenename)
    {
        LoadingUI.SetActive(true);
        while (LoadUI.Fade_move) yield return null;
        
        var async = SceneManager.LoadSceneAsync(scenename);
        
        
        while (!async.isDone)
        {
            Debug.Log(async.progress);
            yield return null;
        }
    }
}
