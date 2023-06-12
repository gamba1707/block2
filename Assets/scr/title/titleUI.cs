using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class titleUI : MonoBehaviour
{
    [SerializeField] GameObject panel1, modepanel, SaveLoadPanel, CreatePanel, OptionPanel;
    [SerializeField] private Loading_fade LoadUI;
    AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        SaveManager.instance.LoadSaveData_Setting();
        Screen.SetResolution(SaveManager.instance.Width, SaveManager.instance.Height, SaveManager.instance.FullScreen);

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
        if (Input.anyKey && panel1.activeInHierarchy)
        {
            panel1.SetActive(false);
            modepanel.SetActive(true);
            audioSource.PlayOneShot(audioSource.clip);
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

    public void OnReturnModePanel_option()
    {
        modepanel.SetActive(true);
        OptionPanel.SetActive(false);
        SaveManager.instance.SaveData_Setting();
    }
}
