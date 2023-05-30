using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class title_Option : MonoBehaviour
{

    [SerializeField] TMP_Dropdown dropdown;
    [SerializeField] TextMeshProUGUI Button_Windowtext;
    int selectnum;
    UnityEngine.Resolution[] resolutions;
    List<string> resolutionlist = new List<string>();
    // Start is called before the first frame update
    void Start()
    {
        resolutions = Screen.resolutions;
        Array.Reverse(resolutions);
        Debug.Log(SaveManager.instance.getWidth() + " x " + SaveManager.instance.getHeight());
        for (int i = 0; i < resolutions.Length; i++)
        {
            resolutionlist.Add(resolutions[i].width + " x " + resolutions[i].height);
            Debug.Log(resolutions[i].width + " x " + resolutions[i].height);
            if (SaveManager.instance.getWidth() == resolutions[i].width && SaveManager.instance.getHeight() == resolutions[i].height)
                selectnum = i;
        }
        
        dropdown.ClearOptions();
        dropdown.AddOptions(resolutionlist);
        dropdown.value = selectnum;
    }

    public void OnScreenMode()
    {

        if (Screen.fullScreen)
        {
            Button_Windowtext.text = "フルスクリーン";
            Screen.fullScreen = true;
        }
        else
        {
            Button_Windowtext.text = "ウィンドウ";
            Screen.fullScreen = false;
        }
        SaveManager.instance.SaveData_Setting(Screen.fullScreen, Screen.width, Screen.height);
    }
    public void OnScreenSize()
    {
        int width = resolutions[dropdown.value].width;
        int height = resolutions[dropdown.value].height;
        Screen.SetResolution(width, height, Screen.fullScreen);
        SaveManager.instance.SaveData_Setting(Screen.fullScreen, Screen.width, Screen.height);

    }
}
