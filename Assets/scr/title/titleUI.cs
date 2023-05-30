using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class titleUI : MonoBehaviour
{
    [SerializeField]GameObject panel1,modepanel,SaveLoadPanel;
    [SerializeField] int state;
    // Start is called before the first frame update
    void Awake()
    {
        if(File.Exists(Application.dataPath + "/Setting.json"))
        {
            SaveManager.instance.LoadSaveData_Setting();
            Screen.SetResolution(SaveManager.instance.getWidth(), SaveManager.instance.getHeight(), SaveManager.instance.getFullscreen());
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey&&state==0)
        {
            panel1.SetActive(false);
            modepanel.SetActive(true);
            state = 1;
            Invoke("interval_off", 0.5f);
        }
    }

    public void OnStoryMode()
    {
        modepanel.SetActive(false);
        SaveLoadPanel.SetActive(true);
    }

    public void OnReturnModePanel()
    {
        modepanel.SetActive(true);
        SaveLoadPanel.SetActive(false);
    }
}
