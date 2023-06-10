using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [Header("�u���b�N�̃v���n�u")]
    //�u���b�N�̃v���n�u�i�T�����Ȃ��������p�j
    [SerializeField] GameObject nomalblock;
    [SerializeField] GameObject trampolineblock;
    [SerializeField] GameObject downblock;
    [Header("����u���b�N�̃v���n�u")]
    [SerializeField] GameObject floorblock;
    [SerializeField] GameObject fallblock;
    [SerializeField] GameObject trampolineblock_before;
    [SerializeField] GameObject downblock_before;

    [Header("��������u���b�N�̐e�I�u�W�F�N�g��")]
    [SerializeField] Transform nomal_parent;//�e�Ƃ��Ĕz�u��
    [SerializeField] Transform trampoline_parent;//�e�Ƃ��Ĕz�u��
    [SerializeField] Transform down_parent;//�e�Ƃ��Ĕz�u��

    [Header("��������u���b�N�̐e�I�u�W�F�N�g��(����I�u�W�F�N�g)")]
    [SerializeField] Transform floor_parent;//�e�Ƃ��Ĕz�u��
    [SerializeField] Transform fall_parent;//�e�Ƃ��Ĕz�u��
    [SerializeField] Transform trampoline_p_before;//�e�Ƃ��Ĕz�u��
    [SerializeField] Transform down_p_before;//�e�Ƃ��Ĕz�u��

    [Header("���ꂼ��̃V�[���ɂ���S�[���I�u�W�F�N�g")]
    [SerializeField] GameObject goalblock;

    [Header("�������ʉ�")]
    AudioSource audioSource;
    [SerializeField] AudioClip nomal_se, eraser_se;

    private int nomalnum, blocknum, trampolinenum, downnum;//����\���ōT���Ă��邩
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        //GameManager�̕��Őݒ肳�ꂽ���l�ŏ���������
        nomalnum = GameManager.I.Nomalnum;
        blocknum = GameManager.I.Blocknum;
        trampolinenum = blocknum;
        downnum = blocknum;
        //�w�肳��Ă�����̂��w�肳�ꂽ����������
        for (int i = 0; i < nomalnum; i++)
        {
            Instantiate(nomalblock, new Vector3(0, 0, -5), Quaternion.identity, nomal_parent);
        }
        for (int i = 0; i < blocknum; i++)
        {
            Instantiate(trampolineblock, new Vector3(0, 0, -5), Quaternion.identity, trampoline_parent);
            Instantiate(downblock, new Vector3(0, 0, -5), Quaternion.identity, down_parent);

        }

        //���������I�u�W�F�N�g���\���ɂ���
        foreach (Transform child in nomal_parent)
        {
            GameObject childObject = child.gameObject;
            child.gameObject.SetActive(false);
        }
        foreach (Transform child in trampoline_parent)
        {
            GameObject childObject = child.gameObject;
            child.gameObject.SetActive(false);
        }
        foreach (Transform child in down_parent)
        {
            GameObject childObject = child.gameObject;
            child.gameObject.SetActive(false);
        }
    }

    public void GetFloorObject(Vector3 pos)
    {
        Instantiate(floorblock, pos, Quaternion.identity, floor_parent);
    }

    public void GetFallObject(Vector3 pos)
    {
        Instantiate(fallblock, pos, Quaternion.identity, fall_parent);
    }
    public void GetTrampolineObject_before(Vector3 pos)
    {
        Instantiate(trampolineblock_before, pos, Quaternion.identity, trampoline_p_before);
    }
    public void GetDownObject_before(Vector3 pos)
    {
        Instantiate(downblock_before, pos, Quaternion.identity, down_p_before);
    }
    public void GetGoalObject_edit(Vector3 pos)
    {
        goalblock.gameObject.transform.position = new Vector3(pos.x, pos.y - 0.75f, pos.z);
    }
    public void GetGoalObject(Vector3 pos)
    {
        goalblock.gameObject.transform.position = pos;
    }

    //�m�[�}���̃u���b�N�𐶐�����
    public void GetNomalObject(Vector3 pos)
    {
        if (nomalnum > 0)
        {
            foreach (Transform child in nomal_parent)
            {
                if (!child.gameObject.activeInHierarchy)
                {
                    child.transform.position = pos;
                    child.gameObject.SetActive(true);
                    audioSource.PlayOneShot(nomal_se);
                    nomalnum--;
                    goto END;//��o����������I��点��
                }
            }
        }
        else//�����X�g�b�N���Ȃ��Ȃ�V���ɐ������đΉ����邵���Ȃ�...
        {
            Instantiate(nomalblock, pos, Quaternion.identity, nomal_parent);
            audioSource.PlayOneShot(nomal_se);
        }
    END:;//�I���]����
    }

    //�g�����|�����I�ȃu���b�N�𐶐�����
    public void GetTranpolineObject(Vector3 pos)
    {
        if (trampolinenum > 0)
        {
            foreach (Transform child in trampoline_parent)
            {
                if (!child.gameObject.activeInHierarchy)
                {
                    child.transform.position = pos;
                    child.gameObject.SetActive(true);
                    audioSource.PlayOneShot(nomal_se);
                    trampolinenum--;
                    goto END;//��o����������I��点��
                }
            }
        }
        else//�����X�g�b�N���Ȃ��Ȃ�V���ɐ������đΉ����邵���Ȃ�...
        {
            Instantiate(trampolineblock, pos, Quaternion.identity, trampoline_parent);
            audioSource.PlayOneShot(nomal_se);
        }
    END:;//�I���]����
    }

    //������u���b�N�𐶐�����
    public void GetDownObject(Vector3 pos)
    {
        if (downnum > 0)
        {
            foreach (Transform child in down_parent)
            {
                if (!child.gameObject.activeInHierarchy)
                {
                    child.transform.position = pos;
                    child.gameObject.SetActive(true);
                    audioSource.PlayOneShot(nomal_se);
                    downnum--;
                    goto END;//��o����������I��点��
                }
            }
        }
        else//�����X�g�b�N���Ȃ��Ȃ�V���ɐ������đΉ����邵���Ȃ�...
        {
            Instantiate(downblock, pos, Quaternion.identity, down_parent);
            audioSource.PlayOneShot(nomal_se);
        }
    END:;//�I���]����
    }

    //�����S���@�\
    public void EraserObject(GameObject obj)
    {
        audioSource.PlayOneShot(eraser_se);
        //�ė��p�o���鐔���X�V
        if (obj.CompareTag("cube")) nomalnum++;
        else if (obj.CompareTag("trampoline")) trampolinenum++;
        else if (obj.CompareTag("down")) downnum++;
        else Destroy(obj);
        //�I�u�W�F�N�g���\���ɂ���
        if (obj != null) obj.SetActive(false);
    }

    public void Reset_box()
    {
        //�T���Ă��鐔�����������Ȃ���
        nomalnum = nomal_parent.childCount;
        trampolinenum = trampoline_parent.childCount;
        //�m�[�}���u���b�N��S�Ĕ�\����
        foreach (Transform child in nomal_parent)
        {
            if (child.gameObject.activeInHierarchy)
            {
                child.gameObject.SetActive(false);
            }
        }
        //�g�����|�����u���b�N��S�Ĕ�\����
        foreach (Transform child in trampoline_parent)
        {
            if (child.gameObject.activeInHierarchy)
            {
                child.gameObject.SetActive(false);
            }
        }
        //������u���b�N��S�Ĕ�\����
        foreach (Transform child in down_parent)
        {
            if (child.gameObject.activeInHierarchy)
            {
                child.gameObject.SetActive(false);
            }
        }

        Debug.Log("<color=#0000ffff>�u���b�N������</color>\nnomalnum:" + nomalnum + "\ntrampolinenum:" + trampolinenum + "\ndownnum:" + downnum);

    }

    public Transform Floor_parent
    {
        get { return floor_parent; }
    }
    public Transform Fall_parent
    {
        get { return fall_parent; }
    }
    public Transform Trampoline_parent_before
    {
        get { return trampoline_p_before; }
    }
    public Transform Down_parent_before
    {
        get { return down_p_before; }
    }
    public Vector3 Goalpos
    {
        get { return goalblock.gameObject.transform.position; }
    }
}

