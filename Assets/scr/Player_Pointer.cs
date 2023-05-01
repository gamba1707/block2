using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player_Pointer : MonoBehaviour
{
    [SerializeField] GameObject nomalblock, trampolineblock;
    [SerializeField] GameObject addBlock;
    PoolManager poolm;
    Vector3 clickpos = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        poolm = addBlock.GetComponent<PoolManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //���N���b�N�������u�Ԃ���UI���������Ƃ��ł͂Ȃ��Ƃ�
        if (Input.GetMouseButtonDown(0)&&!EventSystem.current.IsPointerOverGameObject())
        {
            Debug.Log("UI?:"+EventSystem.current.IsPointerOverGameObject());
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Physics.Raycast(ray, out hit, 10.0f);
            Debug.Log(Input.mousePosition);
            //�����Ȃ��Ƃ���ɐ�������ꍇ
            //�����ʒu���v���C���[�̈ʒu�ł͂Ȃ�
            if (!(GameManager.I.Playerpos == hit.transform.position))
            {
                //�������悤�Ƃ��Ă���ꏊ�ɉ����Ȃ��i�ꉞ���ł͂Ȃ��������肷��j
                if (hit.collider.gameObject.CompareTag("Untagged") && hit.collider.gameObject.layer != LayerMask.NameToLayer("floor"))
                {
                    Debug.Log(hit.transform.position);
                    clickpos = hit.transform.position;
                    //���݂̃Z���N�g����Ă���u���b�N�𐶐�������
                    switch (GameManager.I.Selectname)
                    {
                        case "���ʂ̃u���b�N":
                            //Instantiate(nomalblock, clickpos, Quaternion.identity,addBlock);
                            poolm.GetNomalObject(clickpos);
                            Debug.Log("����������");
                            break;
                        case "��ׂ�u���b�N":
                            //Instantiate(trampolineblock, clickpos, Quaternion.identity,addBlock);
                            Debug.Log("����������");
                            poolm.GetTranpolineObject(clickpos);
                            break;
                    }
                }
                else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("addBlock"))
                {
                    if (GameManager.I.Selectname.Equals("��������"))
                    {
                        Debug.Log("������Ώ�:" + hit.collider.gameObject.name);
                        poolm.EraserObject(hit.collider.gameObject);
                    }
                }
            }
        }
    }



}
