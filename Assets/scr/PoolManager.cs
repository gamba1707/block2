using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [Header("�u���b�N�̃v���n�u")]
    [SerializeField] GameObject floorblock,nomalblock, trampolineblock,fallblock,downblock;//�u���b�N�̃v���n�u�i�T�����Ȃ��������p�j
    [Header("��������u���b�N�̐e�I�u�W�F�N�g��")]
    [SerializeField] Transform floor_parent,nomal_parent, trampoline_parent,fall_parent,down_parent;//�e�Ƃ��Ĕz�u��
    [Header("���ꂼ��̃V�[���ɂ���S�[���I�u�W�F�N�g")]
    [SerializeField] GameObject goalblock;

    private int nomalnum, blocknum,trampolinenum,downnum;//����\���ōT���Ă��邩
    void Start()
    {
        //GameManager�̕��Őݒ肳�ꂽ���l�ŏ���������
        nomalnum = GameManager.I.Nomalnum;
        blocknum = GameManager.I.Blocknum;
        if (GameManager.I.Trampoline) trampolinenum = blocknum;
        if(GameManager.I.Down) downnum = blocknum;
        //�w�肳��Ă�����̂��w�肳�ꂽ����������
        for (int i = 0;i<nomalnum;i++)
        {
            if(GameManager.I.Nomal)Instantiate(nomalblock, new Vector3(0,0,-5), Quaternion.identity, nomal_parent);
        }
        for(int i = 0; i < blocknum; i++)
        {
            if (GameManager.I.Trampoline) Instantiate(trampolineblock, new Vector3(0, 0, -5), Quaternion.identity, trampoline_parent);
            if (GameManager.I.Down) Instantiate(downblock, new Vector3(0, 0, -5), Quaternion.identity, down_parent);
        
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
    public void GetGoalObject_edit(Vector3 pos)
    {
        goalblock.gameObject.transform.position= new Vector3(pos.x,pos.y-0.75f,pos.z);
    }
    public void GetGoalObject(Vector3 pos)
    {
        goalblock.gameObject.transform.position= pos;
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
                    nomalnum--;
                    goto END;//��o����������I��点��
                }
            }
        }
        else//�����X�g�b�N���Ȃ��Ȃ�V���ɐ������đΉ����邵���Ȃ�...
        {
            Instantiate(nomalblock, pos, Quaternion.identity, nomal_parent);
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
                    trampolinenum--;
                    goto END;//��o����������I��点��
                }
            }
        }
        else//�����X�g�b�N���Ȃ��Ȃ�V���ɐ������đΉ����邵���Ȃ�...
        {
            Instantiate(trampolineblock, pos, Quaternion.identity, trampoline_parent);
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
                    downnum--;
                    goto END;//��o����������I��点��
                }
            }
        }
        else//�����X�g�b�N���Ȃ��Ȃ�V���ɐ������đΉ����邵���Ȃ�...
        {
            Instantiate(downblock, pos, Quaternion.identity, down_parent);
        }
    END:;//�I���]����
    }

    //�����S���@�\
    public void EraserObject(GameObject obj)
    {
        //�ė��p�o���鐔���X�V
        if(obj.CompareTag("cube"))nomalnum++;
        else if(obj.CompareTag("trampoline"))trampolinenum++;
        else if (obj.CompareTag("down")) downnum++;
        else Destroy(obj);
        //�I�u�W�F�N�g���\���ɂ���
        if(obj!=null)obj.SetActive(false);
    }

    public void Reset_box()
    {
        //�T���Ă��鐔�����������Ȃ���
        nomalnum = nomal_parent.childCount;
        trampolinenum=trampoline_parent.childCount;
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

        Debug.Log("<color=#0000ffff>�u���b�N������</color>\nnomalnum:"+nomalnum+ "\ntrampolinenum:"+trampolinenum + "\ndownnum:" + downnum);

    }

    public Transform Floor_parent
    {
        get { return floor_parent; }
    }
    public Transform Fall_parent
    {
        get { return fall_parent; }
    }
    public Vector3 Goalpos
    {
        get { return goalblock.gameObject.transform.position; }
    }
}

