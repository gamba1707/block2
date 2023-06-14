using System;
using UnityEngine;

//プレイヤーの移動に関するプログラム
public class Player_move : MonoBehaviour
{
    CharacterController controller;//キャラクターコントローラー
    private Camera maincamera;//メインカメラ入れておく用
    private GameObject Player_t;//Playerをすぐその方向に歩かせるため
    private Animator anim;//動きのアニメーション

    private Vector3 moveDirection = Vector3.zero;//動きを入れておく用
    private Vector3 gravityDirection = Vector3.zero;//浮遊しているときにその状況を加算する用
    private Vector3 cameraForward = Vector3.zero;//本当は3D空間を歩き回ろうとしていたため用意した

    private float x, y;//キー入力値
    [SerializeField] private Vector3 playerpos_first;//初期位置
    [SerializeField] private float speed = 5F;//移動速度
    [SerializeField] private bool falling;//落ちているか

    AudioSource audioSource;//音（跳ねる音）

    void Start()
    {
        //コンポーネントを取得
        controller = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
        audioSource = GetComponent<AudioSource>();
        //メインカメラを入れておく
        maincamera = Camera.main;
        //子オブジェクトを入れる（移動と向きの回転を分けるため）
        Player_t = transform.GetChild(0).gameObject;
        //初期位置
        playerpos_first = transform.position;
    }

    void Update()
    {
        //プレイ中なら
        if (GameManager.I.gamestate("Play"))
        {
            //ここはプレイヤーの位置をGameManagerに伝える所
            GameManager.I.Playerpos = transform.position;

            //入力値
            x = Input.GetAxis("Horizontal");    //左右矢印キーの値(-1.0~1.0)

            //アニメーション
            //移動中なら歩くモーションを入れる
            if (x != 0 || y != 0) anim.SetBool("walk", true);
            else anim.SetBool("walk", false);

            //着地時（たぶん）
            if (controller.isGrounded)
            {
                //落ちてない
                falling = false;
                //落ちていないので重力値は無にする
                gravityDirection = new Vector3(0, 0, 0);
                // カメラの方向から、X-Z平面の単位ベクトルを取得
                cameraForward = Vector3.Scale(maincamera.transform.forward, new Vector3(1, 0, 1)).normalized;
                // 方向キーの入力値とカメラの向きから、移動方向を決定
                moveDirection = cameraForward * y * speed + maincamera.transform.right * x * speed;
                //ワールドに変換する
                moveDirection = transform.TransformDirection(moveDirection);
            }
            else//空中にいる場合
            {
                //落ちていることにして処理能力に応じないようにFixedUpdateで処理する
                falling = true;
            }

            //動いているときは常に押されている方向を向いてほしい
            if (x != 0 || y != 0) Player_t.transform.localRotation = Quaternion.LookRotation(cameraForward * y + maincamera.transform.right * x);

            //最終的に動かす
            controller.Move(moveDirection * Time.deltaTime);
        }
    }

    //落下中の時のみ使用（処理能力に左右されないため）
    private void FixedUpdate()
    {
        //プレイ中で落ちている時
        if (GameManager.I.gamestate("Play") && falling)
        {
            //右方向と奥方向を足し合わせたものを移動量とする
            Vector3 Direction = ((maincamera.transform.right * x * 3f) + (maincamera.transform.forward * y * 3f));
            //重力を加算していく
            gravityDirection.y -= 0.3f * Time.deltaTime;
            //xとzには移動量、yには移動量と重力を与える
            moveDirection = new Vector3(Direction.x, gravityDirection.y + moveDirection.y, Direction.z);
            //ワールドに変換
            moveDirection = transform.TransformDirection(moveDirection);
        }
    }


    private void OnTriggerStay(Collider other)
    {
        //トランポリンブロックに当たった時
        if (other.gameObject.CompareTag("trampoline"))
        {
            //キャラクターコントローラーの接地精度が低いのであるけどここで初期化
            gravityDirection = new Vector3(0, 0, 0);
            //上方向に上げる
            moveDirection.y = 6.2f;
            //跳ねる音を鳴らす
            audioSource.PlayOneShot(audioSource.clip);
        }
    }

    //ゲームクリアしたときにGameManagerから呼び出される
    public void Clear_move()
    {
        //プレイヤーをカメラ向きにする
        Player_t.transform.rotation = Quaternion.Euler(0f, 90f, 0f);
        //位置を補正する
        clear_warp(transform.position);
        //アニメーションする
        anim.SetTrigger("clear");
    }

    //当たり判定的に入り口でゴールポーズされるとかっこ悪いのでエフェクトの中心に立たせる
    void clear_warp(Vector3 start)
    {
        //横方向の位置を1.5の倍数の位置にする
        float z = (float)Math.Round((transform.position.z / 1.5f), 0, MidpointRounding.AwayFromZero);
        //1.5かけて位置を決定
        z *= 1.5f;

        Debug.Log("変換前：" + start.z + "  変換後：" + z);
        //移動する位置
        Vector3 end = new Vector3(transform.position.x, transform.position.y, z);
        //滑らかにゴール位置に補正する
        for (float i = 0; i <= 1; i += 0.01f)
        {
            transform.position = Vector3.Lerp(start, end, i);
        }
    }

    //GameManagerからゲームオーバー時に呼ばれる
    public void GameOver_move()
    {
        //飛び降りた風の向きにする
        Player_t.transform.rotation = Quaternion.Euler(20f, Player_t.transform.localEulerAngles.y, 0f);
        //落ちたアニメーション
        anim.SetBool("fall", true);
    }

    //リセットされたとき初期化する
    public void Reset_move()
    {
        //初期位置に戻す
        this.transform.position = playerpos_first;
        //演出準備
        anim.SetTrigger("reset");
        anim.SetBool("walk", false);
        anim.SetBool("fall", true);
        //向きを戻す
        Player_t.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        //移動量初期化
        x = 0;
        moveDirection = Vector3.zero;
        //最後に落ちたのを戻す
        anim.SetBool("fall", false);
        Debug.Log("<color=#0000ffff>プレイヤー初期化</color>\nPlayerpos:" + transform.position);
    }

    //明転したときに呼ぶ
    public void start_move()
    {
        //出現した風のアニメーションを流す
        anim.SetTrigger("start");
    }
}
