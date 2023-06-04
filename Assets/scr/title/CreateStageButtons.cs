using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class CreateStageButtons : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI titleText;
    [SerializeField] TextMeshProUGUI dateText;
    [SerializeField] private string path;
    [SerializeField] private Loading_fade LoadUI;

    // Start is called before the first frame update
    void Start()
    {
        LoadUI = transform.root.Find("LoadPanel").GetComponent<Loading_fade>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnnewCreate()
    {
        //フェードアウトさせる
        LoadUI.Fadeout();
        MapData.mapinstance.setMapData_Create(null);
        MapData.mapinstance.Createmode = true;
        StartCoroutine(LoadStageScene("MapEditer"));
    }

    public void OnEditCreateStage()
    {
        //フェードアウトさせ
        LoadUI.Fadeout();
        MapData.mapinstance.setMapData_Create(path);
        MapData.mapinstance.Createmode = true;
        StartCoroutine(LoadStageScene("MapEditer"));
    }

    public void OnPlayCreateStage()
    {
        //フェードアウトさせる
        LoadUI.Fadeout();
        MapData.mapinstance.setMapData_Create(path);
        MapData.mapinstance.Createmode = true;
        StartCoroutine(LoadStageScene("Stage"));
    }
    private IEnumerator LoadStageScene(string scenename)
    {
        var async = SceneManager.LoadSceneAsync(scenename);

        async.allowSceneActivation = false;
        while (LoadUI.Fade_move) yield return null;
        async.allowSceneActivation = true;
    }

    public string Path
    {
        get { return path; }
        set 
        {
            path = value;
            titleText.text = System.IO.Path.GetFileNameWithoutExtension(path);
            DateTime dt = System.IO.File.GetCreationTime(path);
            dateText.text = dt.ToString("D");
        }
    }

}
