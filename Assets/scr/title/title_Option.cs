using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

//タイトルのオプション画面
public class title_Option : MonoBehaviour
{
    //どんな項目が編集できるかのテキスト
    [SerializeField] TextMeshProUGUI Option_text;
    //解像度のドロップダウン
    [SerializeField] TMP_Dropdown dropdown;
    //フルスクリーンとウィンドウのボタンテキスト
    [SerializeField] TextMeshProUGUI Button_Windowtext;
    //音量のスライダー
    [SerializeField] Slider bgmslider, seslider;

    //ドロップダウンの何番目が選ばれたか
    int selectnum;
    //解像度一覧
    Resolution[] resolutions;
    //解像度一覧の文字列
    List<string> resolutionlist = new List<string>();

    // Start is called before the first frame update
    void Start()
    {
        //WebGL以外の処理
        if (Application.platform != RuntimePlatform.WebGLPlayer)
        {
            //対応している解像度一覧を取得
            resolutions = Screen.resolutions;
            //重複を消す（あまり機能していないみたい）
            resolutions = resolutions.Distinct().ToArray();
            //一番良い解像度が下に入っているので逆順にする
            Array.Reverse(resolutions);
            //一応見る
            Debug.Log(resolutions);
            Debug.Log(SaveManager.instance.Width + " x " + SaveManager.instance.Height);

            //対応している解像度を回る
            for (int i = 0; i < resolutions.Length; i++)
            {
                //文字列に加えていく
                resolutionlist.Add(resolutions[i].width + " x " + resolutions[i].height);
                Debug.Log(resolutions[i].width + " x " + resolutions[i].height);
                //もしセーブされている解像度と同じものがあればその場所を覚えておく
                if (SaveManager.instance.Width == resolutions[i].width && SaveManager.instance.Height == resolutions[i].height)
                    selectnum = i;
            }
            //一応ドロップダウンの中身を消しておく
            dropdown.ClearOptions();
            //解像度を代入する
            dropdown.AddOptions(resolutionlist);
            //ある場合もない場合も最初に選んでいるものを選択状態にしておく
            dropdown.value = selectnum;

            //もしフルスクリーンならボタンにフルスクリーン
            //ウィンドウならウィンドウと表示する
            if (Screen.fullScreen) Button_Windowtext.text = "フルスクリーン";
            else Button_Windowtext.text = "ウィンドウ";
        }
        else
        {
            //WebGL版なら解像度もウィンドウかも対応していないので消しておく
            Option_text.text = "\r\n\r\n\r\n\r\n音量\r\n　BGM\r\n　SE";
        }

        //BGMとSEのスライダーを設定する（初期値が0のため）
        bgmslider.value = SaveManager.instance.BGMVolume * 10;
        seslider.value = SaveManager.instance.SEVolume * 10;
    }

    //スクリーンモードのボタンから呼ばれる
    public void OnScreenMode()
    {
        //フルスクリーンなら
        if (Screen.fullScreen)
        {
            //テキストをウィンドウにして
            Button_Windowtext.text = "ウィンドウ";
            //ウィンドウ化して
            Screen.fullScreen = false;
            //フルスクリーンでは無いとセーブ側に通知する
            SaveManager.instance.FullScreen = false;
        }
        else
        {
            //ウィンドウの場合は
            //テキストをフルスクリーンにして
            Button_Windowtext.text = "フルスクリーン";
            //フルスクリーンにして
            Screen.fullScreen = true;
            //セーブ側に通知する
            SaveManager.instance.FullScreen = true;
        }
    }

    //解像度を変えたら呼ばれる
    public void OnScreenSize()
    {
        //選ばれている解像度を取得
        int width = resolutions[dropdown.value].width;
        int height = resolutions[dropdown.value].height;
        //セーブ側に通知する
        SaveManager.instance.Height = height;
        SaveManager.instance.Width = width;
        //解像度を変更する
        Screen.SetResolution(width, height, Screen.fullScreen);
    }

    //スライダーを動かすと呼ばれる
    //BGMスライダー
    public void OnSetBGMVolume(Slider slider)
    {
        //スライダーは整数値なので0から1に変換する（0.1倍する）
        Debug.Log(slider.value);
        SaveManager.instance.BGMVolume = slider.value * 0.1f;
    }
    //SEスライダー
    public void OnSetSEVolume(Slider slider)
    {
        Debug.Log(slider.value);
        SaveManager.instance.SEVolume = slider.value * 0.1f;
    }
}
