using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//このスクリプトではそれぞれのマス内にプレイヤーが来た時にプレイヤー位置を更新する
//そこにはブロックが生成出来ないようにする
public class Block_onpleyer : MonoBehaviour
{
        private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("プレイヤーがいる地点" + this.transform.position);
            GameManager.I.Playerpos = this.transform.position;
        }
    }
}
