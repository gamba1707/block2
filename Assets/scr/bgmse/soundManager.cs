using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//設定での音量を適応させるスクリプト
public class soundManager : MonoBehaviour
{
    //BGMかSEか
    [SerializeField] bool bgm;
    //音
    private AudioSource audioSource;
    //初期音量
    private float first_vol;

    void Start()
    {
        //コンポーネントを取得
        audioSource = gameObject.GetComponent<AudioSource>();

        //最初に設定されている音量
        first_vol = audioSource.volume;

        //BGMならBGMの音量設定
        //SEならSEの音量設定を適応する
        if (bgm) audioSource.volume = first_vol * SaveManager.instance.BGMVolume;
        else audioSource.volume = first_vol * SaveManager.instance.SEVolume;
    }

    //タイトル画面でリアルタイムに音量を適応させる
    //スライダーから値が変わると呼ばれる
    public void OnChengeVolume()
    {
        //BGMならBGMの音量設定
        //SEならSEの音量設定を適応する
        if (bgm) audioSource.volume = first_vol * SaveManager.instance.BGMVolume;
        else audioSource.volume = first_vol * SaveManager.instance.SEVolume;
    }
}
