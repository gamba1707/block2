using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class title_Create : MonoBehaviour
{
    [SerializeField] private Loading_fade LoadUI;
    [SerializeField] Transform Content;
    [SerializeField] GameObject CreateLoadButton;
    [SerializeField] string[] files;
    // Start is called before the first frame update
    void Start()
    {
        string path = Application.dataPath + "/StageData_Create";
        files = Directory.GetFiles(path, "*.json", SearchOption.AllDirectories);
        foreach (string file in files)
        {
            Debug.Log(file);
            GameObject listButton = Instantiate(CreateLoadButton) as GameObject;
            listButton.transform.SetParent(Content);
            listButton.GetComponent<CreateStageButtons>().Path = file;
        }
        
    }

    public void OnnewCreate()
    {
        //フェードアウトさせる
        LoadUI.Fadeout();
        MapData.mapinstance.setMapData_Create(null);
        StartCoroutine(LoadStageScene("MapEditer"));
    }
    private IEnumerator LoadStageScene(string scenename)
    {
        var async = SceneManager.LoadSceneAsync(scenename);

        async.allowSceneActivation = false;
        while (LoadUI.Fade_move) yield return null;
        async.allowSceneActivation = true;
    }
}
