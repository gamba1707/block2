using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SelectButton : MonoBehaviour
{
    //ステージ名変更用
    [SerializeField] TextMeshProUGUI stagetext;
    [SerializeField] MapData_scrobj stagedata;
    RectTransform rectTransform;
    Vector2 buttonsize;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        buttonsize = rectTransform.sizeDelta;
        stagetext.text=stagedata.name;
        stagetext.color = Color.white;
        //もし目標数<=でクリアしていたら文字をシアン色にする
        //普通にクリアしていたら黄色にする
        //もしクリアデータに入っていなければ非表示にする
        if (SaveManager.instance.exClearstage(stagedata.name))
        {
            stagetext.enabled = true;
            GetComponent<Button>().enabled = true;
            stagetext.color = Color.cyan;
        }
        else if (SaveManager.instance.Clearstage(stagedata.name))
        {
            stagetext.enabled = true;
            GetComponent<Button>().enabled = true;
            stagetext.color = Color.red;
        }
        else
        {
            rectTransform.sizeDelta = new Vector2(-10f, buttonsize.y);
            GetComponent<Button>().enabled = false;
            stagetext.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //ボタンが押されたら一番上にあるselectUIに持っているステージデータを渡す
    public void OnSelectButton()
    {
        transform.root.gameObject.GetComponent<selectUI>().OnClickButton(stagedata);
    }

    //ステージ開放演出
    public void StageOpen()
    {
        StartCoroutine(StageOpen_move());
    }
    IEnumerator StageOpen_move()
    {
        stagetext.enabled = true;
        GetComponent<Button>().enabled = true;
        rectTransform.sizeDelta = buttonsize;
        transform.rotation = Quaternion.Euler(0f, 90f, 0f);
        while (transform.localEulerAngles.y<=90) 
        {
            transform.Rotate(new Vector3(0, -0.1f, 0));
            yield return null;
        }
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
    }
    public void buttonFalse()
    {
        rectTransform.sizeDelta = new Vector2(-10f, buttonsize.y);
        GetComponent<Button>().enabled = false;
        stagetext.enabled = false;
    }
   
}
