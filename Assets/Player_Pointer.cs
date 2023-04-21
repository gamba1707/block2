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
        
        //左クリック押した瞬間
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
                    case "普通のブロック":
                        Instantiate(block[0], new Vector3(0, clickpos.y, clickpos.z), Quaternion.identity);
                        Debug.Log("生成したよ");
                        break;
                }
            }

            
        }
        //左クリック
        if (Input.GetMouseButton(0))
        {

        }
    }

    

}
