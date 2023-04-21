using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class itemManager : MonoBehaviour
{
    
    private ToggleGroup toggleGroup;
    //アイテムトグルグループ配列
    private Toggle[] toggle;
    //アイテムの画像
    [SerializeField] Sprite[]itemtex;
    // Start is called before the first frame update
    void Start()
    {
        toggleGroup = GetComponent<ToggleGroup>();
        //とりあえずグループの数分取得して格納
        toggle = new Toggle[transform.childCount];
        
        for (int i=0; i<itemtex.Length; i++)
        {
            toggle[i]=transform.GetChild(i).GetComponent<Toggle>();
            toggle[i].name= itemtex[i].name;//名前を画像名に変更
            toggle[i].targetGraphic.GetComponent<Image>().sprite = itemtex[i];//画像を書き換える
            transform.GetChild(i).Find("Label").GetComponent<Text>().text = itemtex[i].name;//画像名にテキストを設定
        }
        GameManager.I.Selectname = toggleGroup.ActiveToggles().First().name;
        Debug.Log("今選択されている：" + GameManager.I.Selectname);
    }

 
    //値が変化すると呼ばれる
    //選択されているものを更新する
    public void OnSelectChenge()
    {
        GameManager.I.Selectname =toggleGroup.ActiveToggles().First().name;
        Debug.Log("今選択されている："+GameManager.I.Selectname);
    }
}
