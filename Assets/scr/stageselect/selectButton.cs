using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//�����ł̓X�e�[�W�Z���N�g��ʂ��قڊǗ����Ă��܂�
public class selectButton : MonoBehaviour
{
    [Header("���[�h���")]
    [SerializeField] private Loading_fade LoadUI;
    //�X�e�[�W�f�[�^�i�{�^���������ꂽ�Ƃ��ɂ��ꂼ��ɃA�^�b�`����Ă���f�[�^��n���ׁj
    [SerializeField] MapData mapData;

    private void Start()
    {
        //�t�F�[�h�C����������
        LoadUI.Fadein();
    }

    //���ꂼ��̃X�e�[�W�̃{�^���������ꂽ��Ă΂��
    public void OnClickButton(MapData_scrobj stagedata)
    {
        //�t�F�[�h�A�E�g������
        LoadUI.Fadeout();
        //���ꂼ��̃{�^���ɃA�^�b�`����Ă���X�e�[�W�f�[�^��n��
        mapData.setMapData(stagedata);
        //���܂�ɑ����ƕs�C���Ȃ̂ŃX�e�[�W���[�h��0.5�b�҂�
        //Invoke("LoadStage",0.5f);
        StartCoroutine(LoadStageScene());
        //SceneManager.LoadScene("Stage");
    }
    private IEnumerator LoadStageScene()
    {
        var async = SceneManager.LoadSceneAsync("Stage");

        async.allowSceneActivation = false;
        while (LoadUI.Fade_move) yield return null;
        async.allowSceneActivation = true;
    }
}
