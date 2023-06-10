using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class clime : MonoBehaviour
{
    private Player_move player_move;
    private Animator anim;
    int rotate;

    private Vector3 blockpos;
    // Start is called before the first frame update
    void Start()
    {
        player_move= transform.root.gameObject.GetComponent<Player_move>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(new Vector3(transform.position.x, transform.position.y + 1.7f, transform.position.z), transform.forward * 1.0f, Color.red);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.I.gamestate("Play")&&other.gameObject.CompareTag("cube"))
        {
            Ray ray = new Ray(new Vector3(transform.position.x, transform.position.y + 1.7f, transform.position.z), transform.forward);
            Debug.DrawRay(new Vector3(transform.position.x, transform.position.y + 1.7f, transform.position.z), transform.forward * 1.0f, Color.red);
            RaycastHit hit;
            Physics.Raycast(ray, out hit, 1.0f);
            Debug.Log(hit.collider == null);
           if (hit.collider == null)
           {//何もなければ登る
                    Debug.Log("OK");
                blockpos = other.transform.position;
                anim.SetTrigger("clime");
            }
            else
            {
                Debug.Log("登れない場所のはず");
            }
        }
    }

    //アニメーションのイベントから呼び出される
    //色々な兼ね合いから無理やり座標を上げて駆け上がったように見せてる
    IEnumerator clime_move()
    {
        float f=0f;
        Debug.Log(blockpos);
        Vector3 startpos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        Vector3 endpos = new Vector3(blockpos.x, blockpos.y + 1f, blockpos.z);
        Debug.Log(startpos);
        Debug.Log(endpos);
        //駆け上がるまで繰り返す（補完待ち）
        while (f <= 1.0f)
        {
            transform.root.position = Vector3.Slerp(startpos, endpos, f);
            f += 0.05f;
            Debug.Log(f);
            yield return new WaitForSecondsRealtime(0.005f);  
        }
    }

}
