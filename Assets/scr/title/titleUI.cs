using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class titleUI : MonoBehaviour
{
    [SerializeField]GameObject panel1,modepanel,SaveLoadPanel;
    // Start is called before the first frame update
    void Start()
    {
        if (Input.anyKey)
        {
            panel1.SetActive(false);
            modepanel.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
