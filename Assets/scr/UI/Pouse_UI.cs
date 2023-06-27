using UnityEngine;

//ポーズ画面
public class Pouse_UI : MonoBehaviour
{
    //クリエイトモード時にセレクトかタイトルかを切り替えるため
    [SerializeField] GameObject selectbutton, titlebutton;

    private void Start()
    {
        //クリエイトモードならセレクトへ戻るを消す
        //ストーリーモードのならタイトルへ戻るを消す
        if (MapData.mapinstance.Createmode) selectbutton.SetActive(false);
        else titlebutton.SetActive(false);
    }

    //ゲームに戻るボタン
    public void OnReturnGame()
    {
        GameManager.I.OnPouseback();
    }

    //リセットするを押されたら呼ばれる
    public void OnResetGame()
    {
        GameManager.I.OnGameReset();
    }

    //セレクトに戻るを押した場合に呼ばれる
    public void OnReturnSelect()
    {
        GameManager.I.OnStageSelect();
    }

    //タイトルに戻るボタン
    public void OnTitleButton()
    {
        GameManager.I.OnTitleBack();
    }
}
