using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�����ł͉E�N���b�N���������Ƃ��ɃX�e�[�W�S�̂��f���悤�ɂ�����̂ł�
public class CameraManager : MonoBehaviour
{
    [SerializeField] GameObject Player_vcam;
    [SerializeField] GameObject stage_vcam;
    [SerializeField] GameObject clear_vcam;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.I.gamestate("Play"))
        {
            //�E�N���b�N��������
            if (Input.GetMouseButtonDown(1))
            {
                stage_vcam.SetActive(true);
            }
            //�E�N���b�N�𗣂�����
            else if (Input.GetMouseButtonUp(1))
            {
                stage_vcam.SetActive(false);
            }
        }
    }

    public void SetStageCamera(Vector3 pos)
    {
        stage_vcam.transform.position = pos;
    }
}
