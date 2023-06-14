using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

//�^�C�g����ʂ̃Z�[�u��ʂ��Ǘ�
public class title_SaveData : MonoBehaviour
{
    //�V�����f�[�^�̏ꍇ�͂����ɃX�e�[�W�֔�΂����߁A���̃X�e�[�W�f�[�^
    [SerializeField] MapData_scrobj firststagedata;
    //�폜���[�h
    [SerializeField] bool deleteMode;
    //�I�񂾃Z�[�u�f�[�^
    [SerializeField] int selectdata;
    //�폜���đ��v�������p�l��
    [SerializeField] GameObject confPanel;
    //���ꂼ��̃{�^���Ȃǂ̃e�L�X�g
    [SerializeField] TextMeshProUGUI header, SaveData1, SaveData2, SaveData3, deleteButton;

    //�\�����ꂽ��
    private void OnEnable()
    {
        //���ꂼ��̃{�^���̓��e������������
        settextdata(1);
        settextdata(2);
        settextdata(3);
    }

    TextMeshProUGUI num2textdata(int num)
    {
        switch (num)
        {
            case 1:
                return SaveData1;
            case 2:
                return SaveData2;
            case 3:
                return SaveData3;
            default:
                Debug.LogError("�������蓖�Ă��������Ă݂Ă�������");
                return null;
        }
    }

    //������������Ă��̃Z�[�u�f�[�^�̓��e�ɏ���������
    void settextdata(int num)
    {
        Debug.Log(Application.dataPath + "/SaveData" + num + ".json");
        //�����łǂ��ҏW����̂����蓖�Ă�
        TextMeshProUGUI text = num2textdata(num);
        //���e�����邽�߂Ɉ�U�ǂݍ���
        SaveManager.instance.LoadSaveData("SaveData" + num);
        //WebGL�Ƃŕ�����
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            //WebGL�̏���
            if (PlayerPrefs.GetString("SaveData" + num, "").Equals(""))
            {
                //�Z�[�u�f�[�^���Ȃ������̏ꍇ�A�V�����f�[�^�Ƃ���
                text.text = "�V�����f�[�^";
            }
            else
            {
                //����������t�ƃN���A����\��������
                text.text = SaveManager.instance.getDateTime() + "\n�N���A���F" + SaveManager.instance.clearnum();
            }
        }
        else
        {
            //WebGL�ȊO�̏���
            if (System.IO.File.Exists(Application.dataPath + "/SaveData" + num + ".json"))
            {
                //���t���擾�ł��Ȃ���Ή��Ă�Ƃ���
                //����������t�ƃN���A����\��������
                if (SaveManager.instance.getDateTime() == null) text.text = "<color=red>�t�@�C���j��";
                else
                    text.text = SaveManager.instance.getDateTime() + "\n�N���A���F" + SaveManager.instance.clearnum();
            }
            else
            {
                //�t�@�C�����Ȃ�������V�����f�[�^�Ƃ���
                text.text = "�V�����f�[�^";
            }
        }
    }

    //���ꂼ��̃{�^���ɂ��Ă��ăf�[�^��ǂ�ŃZ���N�g�ȂǂɈړ�����
    public void LoadSaveData(int datanum)
    {
        //�Ȃ�̃f�[�^�����L�^����
        selectdata = datanum;

        //�����폜���[�h��������
        if (deleteMode)
        {
            //�ق�Ƃɂ������q�˂�p�l�����o��
            confPanel.SetActive(true);
        }
        else
        {
            //�폜���[�h����Ȃ�
            //�t�@�C�������Ă��Ȃ���Γǂݍ���
            if (!num2textdata(selectdata).text.Equals("<color=red>�t�@�C���j��"))
            {
                //�f�[�^��ǂݍ���
                SaveManager.instance.LoadSaveData("SaveData" + selectdata);
                //�܂�WebGL�ƕ�����
                if (Application.platform == RuntimePlatform.WebGLPlayer)
                {
                    //�f�[�^���Ȃ��ꍇ
                    if (PlayerPrefs.GetString("SaveData" + selectdata, "").Equals(""))
                    {
                        //�����Ȃ�͂��߂̃X�e�[�W��ǂݍ���
                        MapData.mapinstance.setMapData(firststagedata);
                        //��΂�
                        SceneManager.LoadScene("Stage");
                    }
                    else
                    {
                        //�X�e�[�W�Z���N�g�ɔ�΂�
                        SceneManager.LoadScene("Select");
                    }
                }
                else
                {
                    //WebGL����Ȃ�����
                    //�Z�[�u�f�[�^�����݂���
                    if (System.IO.File.Exists(Application.dataPath + "/SaveData" + selectdata + ".json"))
                    {
                        //�X�e�[�W�Z���N�g�ɔ�΂�
                        SceneManager.LoadScene("Select");
                    }
                    else
                    {
                        //�Z�[�u�f�[�^�����݂��Ȃ�
                        //�͂��߂̃X�e�[�W��ǂݍ���
                        MapData.mapinstance.setMapData(firststagedata);
                        //��΂�
                        SceneManager.LoadScene("Stage");
                    }
                }
            }
        }
    }

    //�폜�{�^������������Ă΂��
    public void OnDeleteMode()
    {
        //�폜���[�hON�̏ꍇ
        if (deleteMode)
        {
            //�폜���[�h�I�t�ɂ���
            deleteMode = false;
            //�e�L�X�g���ύX
            header.text = "�Z�[�u�f�[�^";
            deleteButton.text = "<color=red>�폜";
        }
        else
        {
            //�폜���[�h�ɂ���
            deleteMode = true;
            //�e�L�X�g���ύX
            header.text = "�Z�[�u�f�[�^�@<color=red>�폜���[�h";
            deleteButton.text = "�߂�";
        }
    }

    //�f�[�^���������߂��o��
    public void OnDeleteData()
    {
        //�I�΂ꂽ�f�[�^������
        SaveManager.instance.OnDeleteSaveData("SaveData" + selectdata);
        //�m�F�p�l��������
        confPanel.SetActive(false);
        //�{�^���̓��e��\�����Ȃ���
        settextdata(selectdata);
    }

    //������ʂł�߂���������Ƃ��ɌĂ΂��
    public void OnReturnDelete()
    {
        //�m�F��ʂ��\���ɂ���
        confPanel.SetActive(false);
    }
}
