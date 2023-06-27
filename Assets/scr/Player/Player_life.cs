using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//ラスボスステージのみ使うスクリプト
public class Player_life : MonoBehaviour
{
    //ライフ表示のスライダー
    [SerializeField] Slider life_slider;
    //プレイヤーのライフ
    [SerializeField] private int life;

    void Start()
    {
        //ラスとステージ以外はオブジェクトを消す
        if (!MapData.mapinstance.Last) this.gameObject.SetActive(false);

        //デバック時のみ挙動確認のためlife1
        //通常時は優秀なクリア数
        if (GameManager.I.Editmode) life = 1;
        else life = SaveManager.instance.ExStage;

        //スライダーを適応する
        //最大数(今までのクリア数)
        life_slider.maxValue =SaveManager.instance.clearnum();
        Debug.Log("maxvalue:"+life_slider.maxValue);
        //現在のライフ数（目標以下でクリアした青ブロックの数）
        life_slider.value = life;
        Debug.Log("value:" + life_slider.value);
    }

    //ボスのブロックに当たったら呼ばれる
    public void OnBossBlock()
    {
        //ライフを減らす
        life--;
        //スライダーに適応する
        life_slider.value = life;

        //もし0になってしまったらゲームオーバー
        if (life <= 0) GameManager.I.OnGameOver();
    }

    //リセットボタンから呼ばれる
    public void OnReset()
    {
        //デバックモードの場合は1
        //それが以外はクリア数にする
        if (GameManager.I.Editmode) life = 1;
        else life = SaveManager.instance.ExStage;
        //スライダーに適応
        life_slider.value = life;
    }
}
