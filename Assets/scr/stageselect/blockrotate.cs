using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ステージセレクト画面でブロックを回転させる
public class blockrotate : MonoBehaviour
{
    //回転数を任意に定められるように
    [Header("回転数")]
    [SerializeField]float x, y, z;

    void FixedUpdate()
    {
        //もらった数値で回転させる
        transform.Rotate(x, y, z);
    }
}
