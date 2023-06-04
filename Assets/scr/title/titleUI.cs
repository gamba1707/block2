using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class titleUI : MonoBehaviour
{
    [SerializeField]GameObject panel1,modepanel,SaveLoadPanel,CreatePanel,OptionPanel;
    [SerializeField] private Loading_fade LoadUI;

    // Start is called before the first frame update
    void Awake()
    {
        if(File.Exists(Application.dataPath + "/Setting.json"))
        {
            SaveManager.instance.LoadSaveData_Setting();
            Screen.SetResolution(SaveManager.instance.getWidth(), SaveManager.instance.getHeight(), SaveManager.instance.getFullscreen());
        }

        
    }

    private void Start()
    {
        if (MapData.mapinstance.Createmode)
        {
            MapData.mapinstance.setMapData_Create(null);
            panel1.SetActive(false);
            CreatePanel.SetActive(true);
            MapData.mapinstance.Createmode = false;
        }
        LoadUI.Open();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey&& panel1.activeInHierarchy)
        {
            panel1.SetActive(false);
            modepanel.SetActive(true);
        }
    }

    public void OnStoryMode()
    {
        modepanel.SetActive(false);
        SaveLoadPanel.SetActive(true);
    }

    public void OnCreateMode()
    {
        modepanel.SetActive(false);
        CreatePanel.SetActive(true);
    }

    public void OnOptionMode()
    {
        modepanel.SetActive(false);
        OptionPanel.SetActive(true);
    }

    public void OnReturnModePanel()
    {
        modepanel.SetActive(true);
        if (SaveLoadPanel.activeInHierarchy) SaveLoadPanel.SetActive(false);
        else if (CreatePanel.activeInHierarchy) CreatePanel.SetActive(false);
        else if (OptionPanel.activeInHierarchy) OptionPanel.SetActive(false);
    }
}
