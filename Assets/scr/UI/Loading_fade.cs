using System.Collections;
using UnityEngine;

//フェード画面
public class Loading_fade : MonoBehaviour
{

    //動作中か
    [SerializeField] bool fade_move;
    //NowLoadingのあれ
    [SerializeField] GameObject loadingObj;

    //0-1でフェイドインやアウトを制御
    private float fade;
    //UIのトランスフォーム
    RectTransform rectTransform;
    //UIの上下左右指定用
    float top, bottom, left, right;

    //時間入れる用
    float t = 0f;

    void Awake()
    {
        //コンポーネントを取得
        rectTransform = GetComponent<RectTransform>();
        //上下左右の初期値を指定（暗い状態）
        top = -5f;
        bottom = -5f;
        left = -5f;
        right = -5f;
        //割り当てる
        rectTransform.offsetMin = new Vector2(left, bottom);
        rectTransform.offsetMax = new Vector2(-right, -top);
        //NowLoadingを消す
        loadingObj.SetActive(false);
    }

    private void FixedUpdate()
    {
        //もしフェードがずっと止まってしまったら（無いと思うけど）
        if (Fade_move)
        {
            //経過時間加算
            t += Time.fixedDeltaTime;
            //10秒以上止まってしまった
            if (t >= 10)
            {
                //行ってしまおう
                Fade_move = false;
                Open();
                t = 0;
            }
        }
    }

    //動きなしで開けておきたい場合に使用
    public void Open()
    {
        //上下左は変わらず
        top = -5f;
        bottom = -5f;
        left = -5f;
        //右だけ画面サイズにする（開き切るため）
        //ただ1920より小さいとなぜか開き切らないので別指定
        if (Screen.width < 1920) right = 1930;
        else right = Screen.width;
        //割り当てる
        rectTransform.offsetMin = new Vector2(left, bottom);
        rectTransform.offsetMax = new Vector2(-right, -top);
        //NowLoadingを消す
        loadingObj.SetActive(false);
    }

    //動きなしで閉めておきたい場合に使用
    public void Close()
    {
        //上下左右指定
        top = -5f;
        bottom = -5f;
        left = -5f;
        right = -5;
        //割り当て
        rectTransform.offsetMin = new Vector2(left, bottom);
        rectTransform.offsetMax = new Vector2(-right, -top);
        //NowLoadingを表示する
        loadingObj.SetActive(true);
    }

    //フェードイン（明転）
    public void Fadein()
    {
        Debug.Log("フェードイン");
        //閉めておく
        Close();
        //動かす
        StartCoroutine("fadein_move");
    }
    //実際にfadeoutを動かしているのはコチラ
    IEnumerator fadein_move()
    {
        //アニメーション位置
        fade = 0f;
        //動いているとする
        Fade_move = true;
        //横を画面サイズにする
        float width = Screen.width;
        //1920より小さいとなぜか開き切らないので別指定
        if (width <= 1920) width = 1930;

        //アニメーション開始
        while (fade <= 1)
        {
            //右から左へ滑らかに変化させる
            right = fade * width - 5;
            //逐一割り当てていく
            rectTransform.offsetMax = new Vector2(-right, -top);
            //アニメーションを進ませる
            fade += 0.025f;
            //時間単位で管理
            yield return new WaitForSecondsRealtime(0.01f);
        }
        //一応終わったとは思うけど開けておく
        Open();
        //待つ
        yield return null;
        //動き完了
        Debug.Log("width:" + width + ",right:" + rectTransform.offsetMax);
        //NowLoadingを消す
        loadingObj.SetActive(false);
        Fade_move = false;
    }

    //フェードアウト
    public void Fadeout()
    {
        Debug.Log("フェードアウト");
        //NowLoadingを表示にする
        loadingObj.SetActive(true);
        StartCoroutine("fadeout_move");
    }
    //実際にfadeoutを動かしているのはコチラ
    IEnumerator fadeout_move()
    {
        //アニメーションを1にする
        fade = 1f;
        //動いている
        Fade_move = true;
        //横幅を取得
        float width = Screen.width;
        //1920より小さいとバグるので別指定
        if (width <= 1920) width = 1930;
        //アニメーション開始
        while (fade >= 0)
        {
            //右端からの大きさを決定
            right = fade * width - 5;
            //逐一代入
            rectTransform.offsetMax = new Vector2(-right, -top);
            //段階を進める
            fade -= 0.025f;
            //時間単位で管理（フレームだと性能に左右され過ぎる）
            yield return new WaitForSecondsRealtime(0.01f);
        }
        //一応閉じる
        Close();
        //待つ
        yield return null;
        //完了
        Debug.Log("width:" + width + ",right:" + rectTransform.offsetMax);
        Fade_move = false;
    }

    //プロパティ
    //アニメーション再生中か
    public bool Fade_move
    {
        get { return fade_move; }
        set { fade_move = value; }
    }
}
