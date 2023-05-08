using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pouse_UI : MonoBehaviour
{
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
}
