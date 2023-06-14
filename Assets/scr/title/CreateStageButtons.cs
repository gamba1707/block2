using System.Collections;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;

//�v���n�u�̃{�^���ɂ��ꂼ������Ă���
public class CreateStageButtons : MonoBehaviour
{
    //�X�e�[�W��������e�L�X�g
    [SerializeField] TextMeshProUGUI titleText;
    //����������e�L�X�g
    [SerializeField] TextMeshProUGUI dateText;
    //���蓖�Ă�ꂽJson�ւ̃p�X������p
    [SerializeField] private string path;
    //�t�F�[�h
    [SerializeField] private Loading_fade LoadUI;

    // Start is called before the first frame update
    void Start()
    {
        //�t�F�[�h���������Ď擾�i�����I�ɔ����j
        LoadUI = transform.root.Find("LoadPanel").GetComponent<Loading_fade>();
        //�𑜓x��ύX�����ƂȂ����Ӗ��s���Ȑ��l�ɃX�P�[����ύX����邽��
        transform.localScale = new Vector3(1, 1, 1);
    }

    //�ҏW�������������
    public void OnEditCreateStage()
    {
        //�t�F�[�h�A�E�g����
        LoadUI.Fadeout();
        //Json�t�@�C���̃p�X��n��
        MapData.mapinstance.setMapData_Create(path);
        //�ҏW���[�h���I���ɂ���
        MapData.mapinstance.Createmode = true;
        //�V�[����ǂݍ���
        StartCoroutine(LoadStageScene("MapEditer"));
    }

    //�V�ԃ{�^�����������Ƃ���Stage�V�[���֔�΂�
    public void OnPlayCreateStage()
    {
        //�t�F�[�h�A�E�g������
        LoadUI.Fadeout();
        //Json�t�@�C���̃p�X��n��
        MapData.mapinstance.setMapData_Create(path);
        //�ҏW���[�h���I���ɂ���
        MapData.mapinstance.Createmode = true;
        //�V�[����ǂݍ���
        StartCoroutine(LoadStageScene("Stage"));
    }

    //�V�[����ǂݍ���
    private IEnumerator LoadStageScene(string scenename)
    {
        var async = SceneManager.LoadSceneAsync(scenename);

        async.allowSceneActivation = false;
        while (LoadUI.Fade_move) yield return null;
        async.allowSceneActivation = true;
    }

    //�v���p�e�B
    public string Path
    {
        get { return path; }
        set
        {
            //�p�X��n���ꂽ��e�L�X�g���ݒ肵�Ă��܂�
            //�p�X�ݒ�
            path = value;
            //�X�e�[�W���ݒ�
            titleText.text = System.IO.Path.GetFileNameWithoutExtension(path);
            //�t�@�C���̍쐬�����擾���Đݒ�
            DateTime dt = System.IO.File.GetCreationTime(path);
            dateText.text = dt.ToString("D");
        }
    }
}
