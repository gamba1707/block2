using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

//�^�C�g����ʂ̃N���G�C�g���[�h���Ǘ�����
public class title_Create : MonoBehaviour
{
    //�t�F�C�h���
    [SerializeField] private Loading_fade LoadUI;
    //�X�N���[���r���[�̎��ۂɕ���z�u���Ă����I�u�W�F�N�g
    [SerializeField] Transform Content;
    //�{�^���̃v���n�u
    [SerializeField] GameObject CreateLoadButton;
    //Json�t�@�C���̃p�X�S��
    [SerializeField] string[] files;


    void Start()
    {
        //�N���G�C�g���[�h�̃t�H���_�[�ʒu
        string path = Application.dataPath + "/StageData_Create";
        //�t�H���_�[�����݂��邩
        if (Directory.Exists(path))
        {
            Debug.Log(path + "�t�H���_�[����܂����B");
            //���̃t�H���_�[���ɂ���Json�t�@�C���̃p�X��S�Ċi�[����
            files = Directory.GetFiles(path, "*.json", SearchOption.AllDirectories);
            //�t�@�C�������ꂼ��ǂ�
            foreach (string file in files)
            {
                Debug.Log(file);
                //�v���n�u��z�u����
                GameObject listButton = Instantiate(CreateLoadButton);
                //���̃I�u�W�F�N�g�̐e�ʒu�����߂�
                listButton.transform.SetParent(Content);
                //���̃X�N���v�g�Ƀp�X�����蓖�ĂĂ���
                listButton.GetComponent<CreateStageButtons>().Path = file;
            }
        }
        else
        {
            Debug.Log(path + "�t�H���_�[���������̂ō���Ă����܂����B");
            //�t�H���_�[���Ȃ���������
            Directory.CreateDirectory(path);
        }
    }

    //�V�������{�^������������
    public void OnnewCreate()
    {
        //�t�F�[�h�A�E�g������
        LoadUI.Fadeout();
        //�T���v���̃}�b�v�f�[�^��ǂݍ���
        MapData.mapinstance.setMapData_Create(null);
        //MapEditer�V�[����
        StartCoroutine(LoadStageScene("MapEditer"));
    }

    //�V�[����ǂݍ���
    private IEnumerator LoadStageScene(string scenename)
    {
        var async = SceneManager.LoadSceneAsync(scenename);

        async.allowSceneActivation = false;
        while (LoadUI.Fade_move) yield return null;
        async.allowSceneActivation = true;
    }
}
