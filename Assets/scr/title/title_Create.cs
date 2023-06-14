using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

//タイトル画面のクリエイトモードを管理する
public class title_Create : MonoBehaviour
{
    //フェイド画面
    [SerializeField] private Loading_fade LoadUI;
    //スクロールビューの実際に物を配置していくオブジェクト
    [SerializeField] Transform Content;
    //ボタンのプレハブ
    [SerializeField] GameObject CreateLoadButton;
    //Jsonファイルのパス全て
    [SerializeField] string[] files;


    void Start()
    {
        //クリエイトモードのフォルダー位置
        string path = Application.dataPath + "/StageData_Create";
        //フォルダーが存在するか
        if (Directory.Exists(path))
        {
            Debug.Log(path + "フォルダーありました。");
            //そのフォルダー内にあるJsonファイルのパスを全て格納する
            files = Directory.GetFiles(path, "*.json", SearchOption.AllDirectories);
            //ファイルをそれぞれ読む
            foreach (string file in files)
            {
                Debug.Log(file);
                //プレハブを配置する
                GameObject listButton = Instantiate(CreateLoadButton);
                //そのオブジェクトの親位置を決める
                listButton.transform.SetParent(Content);
                //そのスクリプトにパスを割り当てていく
                listButton.GetComponent<CreateStageButtons>().Path = file;
            }
        }
        else
        {
            Debug.Log(path + "フォルダー無かったので作っておきました。");
            //フォルダーがなかったら作る
            Directory.CreateDirectory(path);
        }
    }

    //新しく作るボタンを押したら
    public void OnnewCreate()
    {
        //フェードアウトさせる
        LoadUI.Fadeout();
        //サンプルのマップデータを読み込む
        MapData.mapinstance.setMapData_Create(null);
        //MapEditerシーンへ
        StartCoroutine(LoadStageScene("MapEditer"));
    }

    //シーンを読み込む
    private IEnumerator LoadStageScene(string scenename)
    {
        var async = SceneManager.LoadSceneAsync(scenename);

        async.allowSceneActivation = false;
        while (LoadUI.Fade_move) yield return null;
        async.allowSceneActivation = true;
    }
}
