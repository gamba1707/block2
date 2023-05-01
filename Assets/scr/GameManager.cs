using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour
{
    public static GameManager I;

    [Header("このステージで使うブロック")]
    [SerializeField] private bool nomal;
    [SerializeField] private bool trampoline;

    [Header("通常のブロック（初期化時に控えておく数）")]
    [SerializeField] private int nomalnum;
    [Header("その他のブロック（初期化時に控えておく数）")]
    [SerializeField] private int blocknum;

    [Header("歩数")]
    [SerializeField] private string selectname;

    [Header("プレイヤーの位置（一マス単位）")]
    [SerializeField] private Vector3 playerpos;



    //ゲーム開始直後に処理を行う
    private void Awake()
    {
        //変数Iがnullならば
        if (I == null)
        {
            //Iに自身（GameManager）を代入
            I = this;
        }
    }

    public bool Nomal
    {
        get { return nomal; }
    }
    public bool Trampoline
    {
        get { return trampoline; }
    }

    public int Nomalnum
    {
        get { return nomalnum; }
    }
    public int Blocknum
    {
        get { return blocknum; }
    }

    public string Selectname
    {
        get { return selectname; }
        set { selectname = value; }
    }

    public Vector3 Playerpos
    {
        get { return playerpos; }
        set { playerpos = value; }
    }
}
