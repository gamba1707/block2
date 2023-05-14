using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//�����ł̓X�e�[�W�Z���N�g��ʂ��قڊǗ����Ă��܂�
public class selectUI : MonoBehaviour
{
    [Header("���[�h���")]
    [SerializeField] private Loading_fade LoadUI;
    [Header("�{�^���̐e�I�u�W�F�N�g")]
    [SerializeField] private Transform selectbutton_parent;
    //�X�e�[�W�f�[�^�i�{�^���������ꂽ�Ƃ��ɂ��ꂼ��ɃA�^�b�`����Ă���f�[�^��n���ׁj
    [SerializeField] MapData mapData;

    private void Start()
    {
        //�t�F�[�h�C����������
        LoadUI.Fadein();
        StartCoroutine(open_move());
    }

    IEnumerator open_move()
    {
        yield return null;
        int num = SaveManager.instance.clearnum();
        var button = selectbutton_parent.GetChild(num).gameObject.GetComponent<SelectButton>();
        button.buttonFalse();
        while (LoadUI.Fade_move)yield return null;
        
        Debug.Log(num);
        button.StageOpen();
    }

    //���ꂼ��̃X�e�[�W�̃{�^���������ꂽ��Ă΂��
    public void OnClickButton(MapData_scrobj stagedata)
    {
        //�t�F�[�h�A�E�g������
        LoadUI.Fadeout();
        //���ꂼ��̃{�^���ɃA�^�b�`����Ă���X�e�[�W�f�[�^��n��
        mapData.setMapData(stagedata);
        StartCoroutine(LoadStageScene());
    }
    private IEnumerator LoadStageScene()
    {
        var async = SceneManager.LoadSceneAsync("Stage");

        async.allowSceneActivation = false;
        while (LoadUI.Fade_move) yield return null;
        async.allowSceneActivation = true;
    }
}
