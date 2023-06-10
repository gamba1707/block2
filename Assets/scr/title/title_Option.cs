using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class title_Option : MonoBehaviour
{

    [SerializeField] TMP_Dropdown dropdown;
    [SerializeField] TextMeshProUGUI Button_Windowtext;
    [SerializeField] Slider bgmslider,seslider;
    int selectnum;
    UnityEngine.Resolution[] resolutions;
    List<string> resolutionlist = new List<string>();

    // Start is called before the first frame update
    void Start()
    {
        resolutions = Screen.resolutions;
        resolutions=resolutions.Distinct().ToArray();
        Array.Reverse(resolutions);
        Debug.Log(resolutions);
        Debug.Log(SaveManager.instance.Width + " x " + SaveManager.instance.Height);
        for (int i = 0; i < resolutions.Length; i++)
        {
            resolutionlist.Add(resolutions[i].width + " x " + resolutions[i].height);
            Debug.Log(resolutions[i].width + " x " + resolutions[i].height);
            if (SaveManager.instance.Width == resolutions[i].width && SaveManager.instance.Height == resolutions[i].height)
                selectnum = i;
        }
        
        dropdown.ClearOptions();
        dropdown.AddOptions(resolutionlist);
        dropdown.value = selectnum;

        bgmslider.value = SaveManager.instance.BGMVolume*10;
        seslider.value = SaveManager.instance.SEVolume*10;

        if (Screen.fullScreen) Button_Windowtext.text = "フルスクリーン";
        else Button_Windowtext.text = "ウィンドウ";

    }

    public void OnScreenMode()
    {
        if (Screen.fullScreen)
        {
            Button_Windowtext.text = "ウィンドウ";
            Screen.fullScreen = false;
            SaveManager.instance.FullScreen = false;
        }
        else
        {
            Button_Windowtext.text = "フルスクリーン";
            Screen.fullScreen = true;
            SaveManager.instance.FullScreen = true;
        }
    }
    public void OnScreenSize()
    {
        int width = resolutions[dropdown.value].width;
        int height = resolutions[dropdown.value].height;
        SaveManager.instance.Height = height;
        SaveManager.instance.Width = width;
        Screen.SetResolution(width, height, Screen.fullScreen);
    }

    public void OnSetBGMVolume(Slider slider)
    {
        Debug.Log(slider.value);
        SaveManager.instance.BGMVolume = slider.value * 0.1f;
    }
    public void OnSetSEVolume(Slider slider)
    {
        Debug.Log(slider.value);
        SaveManager.instance.SEVolume = slider.value * 0.1f;
    }
}
