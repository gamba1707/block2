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

    //�Q�[���ɖ߂�
    public void OnReturnGame()
    {
        GameManager.I.OnPouseback();
    }
    //���Z�b�g����������ꂽ��Ă΂��
    public void OnResetGame()
    {
        GameManager.I.GameReset();
    }
    //�Z���N�g�ɖ߂���������ꍇ�ɌĂ΂��
    public void OnReturnSelect()
    {

    }
}
