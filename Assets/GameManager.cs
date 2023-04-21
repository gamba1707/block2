using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour
{
    public static GameManager I;

    [Header("�u���b�N�������Ă�")]
    [SerializeField] private bool block_move;
    [Header("����")]
    [SerializeField] private string selectname;



    //�Q�[���J�n����ɏ������s��
    private void Awake()
    {
        //�ϐ�I��null�Ȃ��
        if (I == null)
        {
            //I�Ɏ��g�iGameManager�j����
            I = this;
        }
    }

    public bool Block_move // �v���p�e�B
    {
        get { return block_move; }  // �ʏ̃Q�b�^�[�B�Ăяo��������score���Q�Ƃł���
        set { block_move = value; } // �ʏ̃Z�b�^�[�Bvalue �̓Z�b�g���鑤�̐����Ȃǂ𔽉f����
    }

    public string Selectname
    {
        get { return selectname; }
        set { selectname = value; }
    }

}
