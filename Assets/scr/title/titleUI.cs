using UnityEngine;

//タイトル画面を総括している
public class titleUI : MonoBehaviour
{
    //それぞれのパネル達
    [SerializeField] GameObject panel1, modepanel, SaveLoadPanel, CreatePanel, OptionPanel;
    //フェードパネル
    [SerializeField] private Loading_fade LoadUI;
    //オーディオソース
    AudioSource audioSource;

    private void Awake()
    {
        //コンポーネントを取得
        audioSource = GetComponent<AudioSource>();
        //設定値を読み込む
        SaveManager.instance.LoadSaveData_Setting();
        //解像度を反映させる（無かったら一番いい奴になる）
        Screen.SetResolution(SaveManager.instance.Width, SaveManager.instance.Height, SaveManager.instance.FullScreen);
    }

    private void Start()
    {
        //クリエイトモードから帰ってきた場合
        if (MapData.mapinstance.Createmode)
        {
            //マップデータをなくしておく
            MapData.mapinstance.setMapData_Create(null);
            //ANYBUTTON画面は消す
            panel1.SetActive(false);
            //クリエイトモードを表示
            CreatePanel.SetActive(true);
            //クリエイトモードをオフにする
            MapData.mapinstance.Createmode = false;
            //明転させる
            LoadUI.Fadein();
        }
        else
        {
            //普通に起動時の場合はタイトルなので開けておく
            LoadUI.Open();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //最初の画面で何か押したら
        if (Input.anyKey && panel1.activeInHierarchy)
        {
            //最初の画面をオフに
            panel1.SetActive(false);
            //色々なアレを表示
            modepanel.SetActive(true);
            //音
            audioSource.PlayOneShot(audioSource.clip);
        }
    }

    //ストーリーボタン
    public void OnStoryMode()
    {
        //モードセレクト非表示
        modepanel.SetActive(false);
        //セーブ画面表示
        SaveLoadPanel.SetActive(true);
    }

    //クリエイトボタン
    public void OnCreateMode()
    {
        //モードセレクト非表示
        modepanel.SetActive(false);
        //クリエイトモードパネル表示
        CreatePanel.SetActive(true);
    }

    //オプションボタン
    public void OnOptionMode()
    {
        //モードセレクト非表示
        modepanel.SetActive(false);
        //オプション画面表示
        OptionPanel.SetActive(true);
    }

    //ゲーム終了ボタン
    public void OnExitButton()
    {
        //エディターなら再生ボタン終了
        //UnityEditor.EditorApplication.isPlaying = false;
        //それ以外ならアプリケーション終了
        Application.Quit();
    }

    //モードセレクトに戻るボタン
    public void OnReturnModePanel()
    {
        //モードセレクト表示
        modepanel.SetActive(true);
        //それぞれのパネルが表示の場合は非表示へ
        if (SaveLoadPanel.activeInHierarchy) SaveLoadPanel.SetActive(false);
        else if (CreatePanel.activeInHierarchy) CreatePanel.SetActive(false);
        else if (OptionPanel.activeInHierarchy) OptionPanel.SetActive(false);
    }

    //オプション画面からモードセレクトに戻るとき
    public void OnReturnModePanel_option()
    {
        //モードセレクト表示
        modepanel.SetActive(true);
        //オプション画面非表示
        OptionPanel.SetActive(false);
        //設定値をセーブ
        SaveManager.instance.SaveData_Setting();
    }
}
