using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebGL_invisible : MonoBehaviour
{
    [SerializeField] bool WebGL_visible;
    // Start is called before the first frame update
    void Start()
    {
        if (Application.platform == RuntimePlatform.WebGLPlayer)
            this.gameObject.SetActive(WebGL_visible);
    }
}
