using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ここではアニメーションから呼び出されると足音を鳴らします
public class Player_se : MonoBehaviour
{
    AudioSource source;
    [SerializeField] AudioClip walkse;
    // Start is called before the first frame update
    void Start()
    {
        source= GetComponent<AudioSource>();
    }

    void Onfoot()
    {
        source.PlayOneShot(walkse);
    }
}
