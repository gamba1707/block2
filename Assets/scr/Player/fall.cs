using UnityEngine;

//奈落ブロック用
public class fall : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        //プレイ中にプレイヤーに当たったら
        if (GameManager.I.gamestate("Play") && other.gameObject.name.Equals("Player"))
        {
            //ゲームオーバーにさせる
            GameManager.I.OnGameOver();
        }
    }
}