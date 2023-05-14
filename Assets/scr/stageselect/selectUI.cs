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

    private void Start()
    {
        //フェードインを見せる
        LoadUI.Fadein();
        StartCoroutine(open_move());
    }

    IEnumerator open_move()
    {
        yield return null;
        int num = SaveManager.instance.clearnum();
        var button = selectbutton_parent.GetChild(num).gameObject.GetComponent<SelectButton>();
        button.buttonFalse();
        while (LoadUI.Fade_move)yield return null;
        
        Debug.Log(num);
        button.StageOpen();
    }

    //それぞれのステージのボタンが押されたら呼ばれる
    public void OnClickButton(MapData_scrobj stagedata)
    {
        //フェードアウトさせる
        LoadUI.Fadeout();
        //それぞれのボタンにアタッチされているステージデータを渡す
        mapData.setMapData(stagedata);
        StartCoroutine(LoadStageScene());
    }
    private IEnumerator LoadStageScene()
    {
        var async = SceneManager.LoadSceneAsync("Stage");

        async.allowSceneActivation = false;
        while (LoadUI.Fade_move) yield return null;
        async.allowSceneActivation = true;
    }
}
