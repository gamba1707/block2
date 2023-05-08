using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//プレイヤーの移動に関するプログラム
public class Player_move : MonoBehaviour
{
    CharacterController controller;
    private Camera maincamera;
    private GameObject Player_t;//Playerをすぐその方向に歩かせるため
    private Animator anim;

    private Vector3 moveDirection = Vector3.zero;
    private Vector3 gravityDirection = Vector3.zero;
    private Vector3 cameraForward = Vector3.zero;
    private static float x, y;
    [SerializeField] private Vector3 playerpos_first;
    [SerializeField] private float speed=5F;
    [SerializeField] private bool falling;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        maincamera = Camera.main;
        Player_t = transform.GetChild(0).gameObject;
        anim = GetComponentInChildren<Animator>();
        playerpos_first = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.I.gamestate("Play"))
        {
            //ここはプレイヤーの位置をGameManagerに伝える所
            GameManager.I.Playerpos=transform.position;

            //入力値
            x = Input.GetAxis("Horizontal");    //左右矢印キーの値(-1.0~1.0)

            //アニメーション
            if (x != 0 || y != 0) anim.SetBool("walk", true);
            else anim.SetBool("walk", false);

            if (controller.isGrounded)//着地時（たぶん）
            {
                falling = false;
                gravityDirection = new Vector3(0, 0, 0);
                // カメラの方向から、X-Z平面の単位ベクトルを取得
                cameraForward = Vector3.Scale(maincamera.transform.forward, new Vector3(1, 0, 1)).normalized;
                // 方向キーの入力値とカメラの向きから、移動方向を決定
                moveDirection = cameraForward * y * speed + maincamera.transform.right * x * speed;
                moveDirection = transform.TransformDirection(moveDirection);
            }
            else//空中にいる場合
            {
                falling = true;//FixcedUpdateの方で処理
            }

            //動いているときは常に押されている方向を向いてほしい
            if (x != 0 || y != 0) Player_t.transform.localRotation = Quaternion.LookRotation(cameraForward * y + maincamera.transform.right * x);

            //最終的に動かす
            controller.Move(moveDirection * Time.deltaTime);
        }
    }

    //落下中の時のみ使用
    private void FixedUpdate()
    {
        if (falling)
        {
            Vector3 Direction = ((maincamera.transform.right * x * 3f) + (maincamera.transform.forward * y * 3f));
            gravityDirection.y -= 0.3f * Time.deltaTime;
            moveDirection = new Vector3(Direction.x, gravityDirection.y + moveDirection.y, Direction.z);
            moveDirection = transform.TransformDirection(moveDirection);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("trampoline"))
        {
            //キャラクターコントローラーの接地精度が低いのであるけどここで初期化
            gravityDirection = new Vector3(0, 0, 0);
            moveDirection.y = 6.2f;
            Debug.Log("kiffnsofo"+ moveDirection.y);
        }
    }

    //ゲームクリアしたときにGameManagerから呼び出される
    //
    public void Clear_move()
    {
        Player_t.transform.rotation = Quaternion.Euler(0f, 90f, 0f);
        clear_warp(transform.position);
        anim.SetTrigger("clear");
    }
    //当たり判定的に入り口でゴールポーズされるとかっこ悪いのでエフェクトの中心に立たせる
    void clear_warp(Vector3 start)
    {
        float z = (float)Math.Round((transform.position.z/1.5f), 0, MidpointRounding.AwayFromZero);
        z *= 1.5f;
        Debug.Log("変換前："+start.z+"  変換後："+z);
        Vector3 end=new Vector3(transform.position.x, transform.position.y, z);
        for(float i = 0; i <= 1; i += 0.01f)
        {
            transform.position = Vector3.Lerp(start, end, i);
        }
    }
    public void GameOver_move()
    {
        //飛び降りた風の向きにする
        Player_t.transform.rotation = Quaternion.Euler(20f, Player_t.transform.localEulerAngles.y, 0f);
        //落ちたアニメーション
        anim.SetBool("fall",true);
    }

    public void Reset_move()
    {
        Player_t.transform.rotation = Quaternion.Euler(0f, 0, 0f);
        transform.position = playerpos_first;
        anim.SetBool("walk", false);
        anim.SetBool("fall", false);
        Debug.Log("<color=#0000ffff>プレイヤー初期化</color>\nPlayerpos:" + transform.position);
    }
}
