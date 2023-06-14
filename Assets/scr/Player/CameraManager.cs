using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

//�J�����Ȃǂ̋@�\���Ǘ�
public class CameraManager : MonoBehaviour
{
    //�J�����֘A
    [SerializeField] CinemachineBrain brain;//�J�����Ǘ��̒���
    [SerializeField] CinemachineVirtualCamera Player_vcam;//Player��ǂ��Ă�J����
    [SerializeField] CinemachineVirtualCamera stage_vcam;//�X�e�[�W�S�̂̃J����
    [SerializeField] CinemachineVirtualCamera clear_vcam;//�S�[���̃J����
    [SerializeField] CinemachineVirtualCamera startmovie_vcam;//�X�^�[�g���̓����p�J����

    [Header("���[�h���")]
    [SerializeField] private Loading_fade LoadUI;

    private void Start()
    {
        //�X�e�[�W���ŏ��ɉf���Ă�������𓮂���
        StartCoroutine(startmovie_move());
        //�S�̃J�������I�t�ɂ���
        stage_vcam.enabled = false;
    }

    //�X�^�[�g���̉��o
    IEnumerator startmovie_move()
    {
        //������Ƒ҂�
        yield return new WaitForSecondsRealtime(0.5f);
        //�܂��t�F�[�h���I����ĂȂ���Α҂�
        while (LoadUI.Fade_move) yield return null;
        //�S�[���t�߂ɐݒu�����J�������\���ɂ��āA�����Ńv���C���[�̃J�����Ɉڂ�ς��
        startmovie_vcam.enabled = false;
        //��U�҂�
        yield return null;
        //�J�����̈ړ����I���܂ő҂�
        while (brain.ActiveBlend != null) yield return null;
        //�X�e�[�W���������i�Q�[���X�^�[�g�����˂�j
        GameManager.I.SetStagename("");
    }


    // Update is called once per frame
    void Update()
    {
        //�V�[�������X�e�[�W
        if (SceneManager.GetActiveScene().name.Equals("Stage"))
        {
            //�Q�[���v���C��
            if (GameManager.I.gamestate("Play"))
            {
                //�E�N���b�N��������
                if (Input.GetMouseButtonDown(1))
                {
                    //�S�̃J�������I���ɂ���
                    stage_vcam.enabled = true;
                }
                //�E�N���b�N�𗣂�����
                else if (Input.GetMouseButtonUp(1))
                {
                    //�S�̃J�������I�t�ɂ���
                    stage_vcam.enabled = false;
                }
            }
        }
    }

    //�S�̃J�����̈ʒu��ݒ�
    public void SetStageCamera(Vector3 pos)
    {
        stage_vcam.transform.position = pos;
    }

    //�N���G�C�g���[�h�̏c�ړ��{�^��
    public void OnMoveCamera_Y(float move)
    {
        stage_vcam.transform.position += new Vector3(0, move, 0);
    }
    //�N���G�C�g���[�h�̉��ړ��{�^��
    public void OnMoveCamera_Z(float move)
    {
        stage_vcam.transform.position += new Vector3(0, 0, move);
    }
}
