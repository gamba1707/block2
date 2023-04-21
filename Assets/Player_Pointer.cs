using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player_Pointer : MonoBehaviour
{
    [SerializeField] GameObject[] block;
    Vector3 clickpos = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        //���N���b�N�������u��
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 10.0f))
            {
                Debug.Log(hit.transform.position);
                clickpos = hit.transform.position;
                switch (GameManager.I.Selectname)
                {
                    case "���ʂ̃u���b�N":
                        Instantiate(block[0], new Vector3(0, clickpos.y, clickpos.z), Quaternion.identity);
                        Debug.Log("����������");
                        break;
                }
            }

            
        }
        //���N���b�N
        if (Input.GetMouseButton(0))
        {

        }
    }

    

}
