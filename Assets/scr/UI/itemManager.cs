using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//左側にあるアイテム管理
public class itemManager : MonoBehaviour
{
    //トグルグループで管理しているので持っておく
    [SerializeField] private ToggleGroup toggleGroup;
    //アイテムトグルグループ配列
    private Toggle[] toggle;
    //アイテムの画像
    [SerializeField] Sprite[] itemtex;

    void Start()
    {
        //とりあえずグループの数分用意する
        toggle = new Toggle[transform.childCount];

        //割り当てられている画像分回る
        for (int i = 0; i < itemtex.Length; i++)
        {
            //その番のトグルを取得する
            toggle[i] = transform.GetChild(i).GetComponent<Toggle>();
            //名前を画像名に変更
            toggle[i].name = itemtex[i].name;
            //画像を書き換える
            toggle[i].targetGraphic.GetComponent<Image>().sprite = itemtex[i];
            //画像名にテキストを設定
            transform.GetChild(i).Find("Label＿TMP").GetComponent<TextMeshProUGUI>().text = itemtex[i].name;
        }
        //最初に選択されているブロックとして一番上の物にする
        GameManager.I.Selectname = toggleGroup.ActiveToggles().First().name;
        Debug.Log("今選択されている：" + GameManager.I.Selectname);
    }


    //値が変化すると呼ばれる
    //選択されているものを更新する
    public void OnSelectChenge()
    {
        //新しいものを通知する
        GameManager.I.Selectname = toggleGroup.ActiveToggles().First().name;
        Debug.Log("今選択されている：" + GameManager.I.Selectname);
    }
}
