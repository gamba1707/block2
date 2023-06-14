using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageBGMManager : MonoBehaviour
{

    AudioSource audioSource;
    [Header("�X�e�[�WBGM")]
    [SerializeField] AudioClip BOSSBGM;
    [SerializeField] AudioClip stage0BGM;
    [SerializeField] AudioClip stage1BGM;
    [SerializeField] AudioClip stage2BGM;
    [SerializeField] AudioClip stage3BGM;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        Debug.Log(MapData.mapinstance.mapname().Substring(0, 1));
        Debug.Log("BOSS:"+ MapData.mapinstance.Boss);
        if (MapData.mapinstance.Boss)
        {

            audioSource.clip = BOSSBGM;
        }
        else
        {
            switch (MapData.mapinstance.mapname().Substring(0, 1))
            {
                case "0":
                    audioSource.clip = stage0BGM;
                    break;
                case "1":
                    audioSource.clip = stage1BGM;
                    break;
                case "2":
                    audioSource.clip = stage2BGM;
                    break;
                case "3":
                    audioSource.clip = stage3BGM;
                    break;
            }
        }
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.I.gamestate("GameClear"))
        {
            audioSource.Stop();
        }
    }
}
