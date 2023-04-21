using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_se : MonoBehaviour
{
    AudioSource source;
    [SerializeField] AudioClip walkse;
    // Start is called before the first frame update
    void Start()
    {
        source= GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Onfoot()
    {
        source.PlayOneShot(walkse);
    }
}
