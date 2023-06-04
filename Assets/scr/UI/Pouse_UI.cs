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
    //�Q�[���ɖ߂�
    public void OnReturnGame()
    {
        GameManager.I.OnPouseback();
    }
    //���Z�b�g����������ꂽ��Ă΂��
    public void OnResetGame()
    {
        GameManager.I.OnGameReset();
    }
    //�Z���N�g�ɖ߂���������ꍇ�ɌĂ΂��
    public void OnReturnSelect()
    {
        GameManager.I.OnStageSelect();
    }
    public void OnTitleButton()
    {
        GameManager.I.OnTitleBack();
    }
}
