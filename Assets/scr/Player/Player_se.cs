using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ここではアニメーションから呼び出されると足音を鳴らします
public class Player_se : MonoBehaviour
{
    //オーディオソース
    AudioSource source;
    //歩く効果音
    [SerializeField] AudioClip walkse;

    void Start()
    {
        //コンポーネントを取得
        source= GetComponent<AudioSource>();
    }

    //アニメーションから床につくタイミングで呼ばれる
    void Onfoot()
    {
        //鳴らす
        source.PlayOneShot(walkse);
    }
}
