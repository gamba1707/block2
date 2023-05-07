using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pouse_UI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //ゲームに戻る
    public void OnReturnGame()
    {
        GameManager.I.OnPouseback();
    }
    //リセットするを押されたら呼ばれる
    public void OnResetGame()
    {
        GameManager.I.GameReset();
    }
    //セレクトに戻るを押した場合に呼ばれる
    public void OnReturnSelect()
    {

    }
}
