using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class titleUI : MonoBehaviour
{
    [SerializeField]GameObject panel1,modepanel,SaveLoadPanel;
    [SerializeField] int state;
    bool interval;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey&&state==0)
        {
            panel1.SetActive(false);
            modepanel.SetActive(true);
            state = 1;
            interval = true;
            Invoke("interval_off", 0.5f);
        }
    }

    void interval_off()
    {
        interval = false;
    }

    public void OnStoryMode()
    {
        modepanel.SetActive(false);
        SaveLoadPanel.SetActive(true);
    }

    public void OnData1()
    {

    }
}
