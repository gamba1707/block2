using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StageSelect : MonoBehaviour
{
    [Header("�u���b�N�֘A")]
    [SerializeField] int stagenum;
    [SerializeField] MeshRenderer centercube,leftcube,rightcube;
    [SerializeField] Material blue,yellow,green,clear,gray;
    [SerializeField] private MapData_scrobj[] stagedata;
    [SerializeField] bool EditMode;

    [Header("�����֘A")]
    [SerializeField] TextMeshProUGUI StageText;
    [SerializeField] TextMeshProUGUI ScoreText;

    AudioSource audioSource;

    bool interval;

    // Start is called before the first frame update
    void Start()
    {
        if(SaveManager.instance.clearnum()>= stagedata.Length) stagenum = SaveManager.instance.clearnum()-1;
        else stagenum = SaveManager.instance.clearnum();
        audioSource = GetComponent<AudioSource>();
        setstageinfo();
    }

    // Update is called once per frame
    void Update()
    {
        if (!interval)
        {
            //==========================
            //�������獶�E�������Ƃ��̐F�ݒ�
            //Edit���[�h���I���ɂ���ƃC���X�y�N�^�[�ł�������܂킹��͂�
            if (EditMode)
            {
                setstageinfo();
            }
            //���ɉ������ꍇ�A����ɍ��ɃX�e�[�W������ꍇ
            if (Input.GetAxis("Horizontal") < 0 && stagenum > 0)
            {
                stagenum--;
                audioSource.PlayOneShot(audioSource.clip);
                setstageinfo();
            }
            //�E�ɉ������ꍇ�A���݂̈ʒu���N���A����菬�����ꍇ
            if (Input.GetAxis("Horizontal") > 0 && stagenum < SaveManager.instance.clearnum() && stagenum+1 < stagedata.Length)
            {
                stagenum++;
                audioSource.PlayOneShot(audioSource.clip);
                setstageinfo();
            }
            //==========================

            if (Input.GetButtonDown("Submit"))
            {
                Debug.Log("������");
                audioSource.PlayOneShot(audioSource.clip);
                //�ǂ����Ă�UI�̊֌W�㓧���ɂ��Ȃ��ƕ��������؂�邽��
                rightcube.material = clear;
                transform.root.gameObject.GetComponent<selectUI>().OnClickButton(stagedata[stagenum]);
            }
        }
        
    }

    void interval_reset()
    {
        interval = false;
    }

    //�ԍ���^����ƃZ�[�u�f�[�^�̏�񂩂�F��Ԃ��Ă����
    //�ڕW�ȏ�̃N���A�F�F�A�Ƃ肠�����N���A�F���F�A�V�����X�e�[�W�F�ΐF�A�܂��F�D�F
    Material colorBlock(int num)
    {
        if (num < 0 || num >= stagedata.Length) return clear;
        else if (num > SaveManager.instance.clearnum()) return gray;
        else if (SaveManager.instance.exClearstage(stagedata[num])) return blue;
        else if (SaveManager.instance.Clearstage(stagedata[num])) return yellow;
        else if(num==SaveManager.instance.clearnum()) return green;
        return gray;
    }

    void setstageinfo()
    {
        interval = true;
        leftcube.material = colorBlock(stagenum - 1);
        centercube.material = colorBlock(stagenum);
        rightcube.material = colorBlock(stagenum + 1);
        StageText.text = stagedata[stagenum].name;
        ScoreText.text = SaveManager.instance.clearscore(stagedata[stagenum].name) + "/" + stagedata[stagenum].clearnum;
        Invoke("interval_reset", 0.5f);
    }
}
