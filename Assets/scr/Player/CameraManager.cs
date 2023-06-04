using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//ここでは右クリックを押したときにステージ全体を映すようにするものです
public class CameraManager : MonoBehaviour
{
    [SerializeField] CinemachineBrain brain;
    [SerializeField] CinemachineVirtualCamera Player_vcam;
    [SerializeField] CinemachineVirtualCamera stage_vcam;
    [SerializeField] CinemachineVirtualCamera clear_vcam;
    [SerializeField] CinemachineVirtualCamera startmovie_vcam;

    [Header("ロード画面")]
    [SerializeField] private Loading_fade LoadUI;

    private void Start()
    {
        StartCoroutine(startmovie_move());
        stage_vcam.enabled = false;
    }

    IEnumerator startmovie_move()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        while (LoadUI.Fade_move)yield return null;
        startmovie_vcam.enabled=false;
        yield return null;
        while (brain.ActiveBlend!=null)yield return null;
        GameManager.I.SetStagename("");
    }


    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name.Equals("Stage"))
        {
            if (GameManager.I.gamestate("Play"))
            {
                //右クリック押したら
                if (Input.GetMouseButtonDown(1))
                {
                    stage_vcam.enabled = true;
                }
                //右クリックを離したら
                else if (Input.GetMouseButtonUp(1))
                {
                    stage_vcam.enabled = false;
                }
            }
        }
        
    }

    public void SetStageCamera(Vector3 pos)
    {
        stage_vcam.transform.position = pos;
    }

    public void OnMoveCamera_Y(float move)
    {
        stage_vcam.transform.position += new Vector3(0,move,0);
    }
    public void OnMoveCamera_Z(float move)
    {
        stage_vcam.transform.position += new Vector3(0,0,move);
    }
}
