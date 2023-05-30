using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//ここでは右クリックを押したときにステージ全体を映すようにするものです
public class CameraManager : MonoBehaviour
{
    [SerializeField] GameObject Player_vcam;
    [SerializeField] GameObject stage_vcam;
    [SerializeField] GameObject clear_vcam;

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
                    stage_vcam.SetActive(true);
                }
                //右クリックを離したら
                else if (Input.GetMouseButtonUp(1))
                {
                    stage_vcam.SetActive(false);
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
