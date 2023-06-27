using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

//カメラなどの機能を管理
public class CameraManager : MonoBehaviour
{
    //カメラ関連
    [SerializeField] CinemachineBrain brain;//カメラ管理の中枢
    [SerializeField] CinemachineVirtualCamera Player_vcam;//Playerを追ってるカメラ
    [SerializeField] CinemachineVirtualCamera stage_vcam;//ステージ全体のカメラ
    [SerializeField] CinemachineVirtualCamera clear_vcam;//ゴールのカメラ
    [SerializeField] CinemachineVirtualCamera startmovie_vcam;//スタート時の動き用カメラ

    [Header("ロード画面")]
    [SerializeField] private Loading_fade LoadUI;

    private void Start()
    {
        //ステージを最初に映していくあれを動かす
        StartCoroutine(startmovie_move());
        //全体カメラをオフにする
        stage_vcam.enabled = false;
    }

    //スタート時の演出
    IEnumerator startmovie_move()
    {
        //ちょっと待つ
        yield return new WaitForSecondsRealtime(0.5f);
        //まだフェードが終わってなければ待つ
        while (LoadUI.Fade_move) yield return null;
        //ゴール付近に設置したカメラを非表示にして、自動でプレイヤーのカメラに移り変わる
        startmovie_vcam.enabled = false;
        //一旦待つ
        yield return null;
        //カメラの移動が終わるまで待つ
        while (brain.ActiveBlend != null) yield return null;
        //ステージ名を消す（ゲームスタートも兼ねる）
        GameManager.I.SetStagename("");
    }


    // Update is called once per frame
    void Update()
    {
        //シーン名がステージ
        if (SceneManager.GetActiveScene().name.Equals("Stage"))
        {
            //ゲームプレイ中
            if (GameManager.I.gamestate("Play"))
            {
                //右クリック押したら
                if (Input.GetMouseButtonDown(1))
                {
                    //全体カメラをオンにする
                    stage_vcam.enabled = true;
                }
                //右クリックを離したら
                else if (Input.GetMouseButtonUp(1))
                {
                    //全体カメラをオフにする
                    stage_vcam.enabled = false;
                }
            }
        }
    }

    //全体カメラの位置を設定
    public void SetStageCamera(Vector3 pos)
    {
        stage_vcam.transform.position = pos;
    }

    //クリエイトモードの縦移動ボタン
    public void OnMoveCamera_Y(float move)
    {
        stage_vcam.transform.position += new Vector3(0, move, 0);
    }
    //クリエイトモードの横移動ボタン
    public void OnMoveCamera_Z(float move)
    {
        stage_vcam.transform.position += new Vector3(0, 0, move);
    }
}
