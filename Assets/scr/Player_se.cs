using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�����ł̓A�j���[�V��������Ăяo�����Ƒ�����炵�܂�
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
