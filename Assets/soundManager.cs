using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundManager : MonoBehaviour
{
    [SerializeField] bool bgm;
    private AudioSource audioSource;
    private float first_vol;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        first_vol=audioSource.volume;
        if(bgm)audioSource.volume = first_vol * SaveManager.instance.BGMVolume;
        else audioSource.volume = first_vol * SaveManager.instance.SEVolume;
    }

    public void OnChengeVolume()
    {
        if (bgm) audioSource.volume = first_vol * SaveManager.instance.BGMVolume;
        else audioSource.volume = first_vol * SaveManager.instance.SEVolume;
    }
}
