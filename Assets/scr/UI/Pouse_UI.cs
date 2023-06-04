using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pouse_UI : MonoBehaviour
{
    [SerializeField] GameObject selectbutton, titlebutton;

    private void Start()
    {
        if(MapData.mapinstance.Createmode) selectbutton.SetActive(false);
        else titlebutton.SetActive(false);
    }
    //ゲームに戻る
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
    public void OnTitleButton()
    {
        GameManager.I.OnTitleBack();
    }
}
