using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Clear_act : MonoBehaviour
{
    [SerializeField] GameObject effect;

    void OnEffect()
    {
        effect.SetActive(true);
    }

    //プレイヤーのゴール演出が終わると呼び出されます
    void Onmove_End()
    {
        //if(SceneManager.GetActiveScene().)
        GameManager.I.OnClear_end();
    }
}
