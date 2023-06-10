using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatusText : MonoBehaviour
{
    TextMeshProUGUI text;
    // Start is called before the first frame update
    void Awake()
    {
        text=GetComponent<TextMeshProUGUI>();
        text.text = "";
    }

    public void SetStatusText(string s)
    {
        text.text = s;
        Invoke("ResetText",3f);
    }

    void ResetText()
    {
        text.text = "";
    }
}
