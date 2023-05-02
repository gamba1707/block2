using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [SerializeField] GameObject nomalblock, trampolineblock;
    [SerializeField] Transform nomal_parent, trampoline_parent;
    private int nomalnum, blocknum,trampolinenum;
    void Start()
    {
        //GameManager�̕��Őݒ肳�ꂽ���l�ŏ���������
        nomalnum = GameManager.I.Nomalnum;
        blocknum = GameManager.I.Blocknum;
        if (GameManager.I.Trampoline) trampolinenum = blocknum;
        //�w�肳��Ă�����̂��w�肳�ꂽ����������
        for (int i = 0;i<nomalnum;i++)
        {
            if(GameManager.I.Nomal)Instantiate(nomalblock, new Vector3(0,0,-5), Quaternion.identity, nomal_parent);
        }
        for(int i = 0; i < blocknum; i++)
        {
            if (GameManager.I.Trampoline) Instantiate(trampolineblock, new Vector3(0, 0, -5), Quaternion.identity, trampoline_parent);
        }

        //���������I�u�W�F�N�g���\���ɂ���
        foreach(Transform child in nomal_parent)
        {
            GameObject childObject = child.gameObject;
            child.gameObject.SetActive(false);
        }
        foreach (Transform child in trampoline_parent)
        {
            GameObject childObject = child.gameObject;
            child.gameObject.SetActive(false);
        }
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

    //�����S���@�\
    public void EraserObject(GameObject obj)
    {
        //�ė��p�o���鐔���X�V
        if(obj.CompareTag("cube"))nomalnum++;
        else if(obj.CompareTag("trampoline"))trampolinenum++;
        //�I�u�W�F�N�g���\���ɂ���
        obj.SetActive(false);
    }
}
