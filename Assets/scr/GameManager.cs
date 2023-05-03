using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour
{
    public static GameManager I;

    [Header("�Q�[���̏��")]
    [SerializeField] private GAME_STATUS game_status;
    private enum GAME_STATUS { Play, GameClear, Pause, GameOver }

    [Header("���̃X�e�[�W�Ŏg���u���b�N")]
    [SerializeField] private bool nomal;
    [SerializeField] private bool trampoline;

    [Header("�ʏ�̃u���b�N�i���������ɍT���Ă������j")]
    [SerializeField] private int nomalnum;
    [Header("���̑��̃u���b�N�i���������ɍT���Ă������j")]
    [SerializeField] private int blocknum;

    [Header("�Z���N�g���Ă���u���b�N��")]
    [SerializeField] private string selectname;

    [Header("�v���C���[�̈ʒu�i��}�X�P�ʁj")]
    [SerializeField] private Vector3 playerpos;



    //�Q�[���J�n����ɏ������s��
    private void Awake()
    {
        //�ϐ�I��null�Ȃ��
        if (I == null)
        {
            //I�Ɏ��g�iGameManager�j����
            I = this;
        }
        game_status = GAME_STATUS.Play;
    }

    public bool gamestate(string s)
    {
        if (s.Equals(game_status.ToString())) return true;
        return false;
    }

    public void OnClear(Player_move pmove)
    {
        game_status = GAME_STATUS.GameClear;
        pmove.Clear_move();
    }


    //�ȉ��v���p�e�B
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
