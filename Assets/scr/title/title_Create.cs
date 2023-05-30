using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class title_Create : MonoBehaviour
{
    [SerializeField] Transform Content;
    [SerializeField] List<MapData_scrobj>mapdataList;
    // Start is called before the first frame update
    void Start()
    {
        string path = Application.dataPath + "/StageData_Create";
        string[] files = Directory.GetFiles(path, "*.json", SearchOption.AllDirectories);
        foreach (string file in files)
        {
            Debug.Log(file);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
