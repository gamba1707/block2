using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//WebGLで非表示にしなければならない物に付けると消えてくれる
public class WebGL_invisible : MonoBehaviour
{
    [SerializeField] bool WebGL_visible;

    void Start()
    {
        //WebGLの場合消す
        if (Application.platform == RuntimePlatform.WebGLPlayer)
            this.gameObject.SetActive(WebGL_visible);
    }
}
