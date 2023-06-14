using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StageSelect : MonoBehaviour
{
    [Header("ブロック関連")]
    [SerializeField] int stagenum;
    [SerializeField] MeshRenderer centercube,leftcube,rightcube;
    [SerializeField] Material blue,yellow,green,clear,gray;
    [SerializeField] private MapData_scrobj[] stagedata;
    [SerializeField] bool EditMode;

    [Header("文字関連")]
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
            //ここから左右押したときの色設定
            //Editモードをオンにするとインスペクターでいじくりまわせるはず
            if (EditMode)
            {
                setstageinfo();
            }
            //左に押した場合、さらに左にステージがある場合
            if (Input.GetAxis("Horizontal") < 0 && stagenum > 0)
            {
                stagenum--;
                audioSource.PlayOneShot(audioSource.clip);
                setstageinfo();
            }
            //右に押した場合、現在の位置がクリア数より小さい場合
            if (Input.GetAxis("Horizontal") > 0 && stagenum < SaveManager.instance.clearnum() && stagenum+1 < stagedata.Length)
            {
                stagenum++;
                audioSource.PlayOneShot(audioSource.clip);
                setstageinfo();
            }
            //==========================

            if (Input.GetButtonDown("Submit"))
            {
                Debug.Log("押した");
                audioSource.PlayOneShot(audioSource.clip);
                //どうしてもUIの関係上透明にしないと文字が見切れるため
                rightcube.material = clear;
                transform.root.gameObject.GetComponent<selectUI>().OnClickButton(stagedata[stagenum]);
            }
        }
        
    }

    void interval_reset()
    {
        interval = false;
    }

    //番号を与えるとセーブデータの情報から色を返してくれる
    //目標以上のクリア：青色、とりあえずクリア：黄色、新しいステージ：緑色、まだ：灰色
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
