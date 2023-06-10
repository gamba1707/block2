using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class downblock : MonoBehaviour
{
    [SerializeField] Material nomalmaterial,downmaterial;
    Vector3 firstpos;
    MeshRenderer meshRenderer;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        firstpos = transform.position;
        meshRenderer = GetComponent<MeshRenderer>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(rb.velocity.y<-1f||rb.velocity.y>0)rb.velocity = new Vector3(0, -1f, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Equals("Player"))
        {
            meshRenderer.material= downmaterial;

            Invoke("down_move",2f);
        }
    }

    void down_move()
    {
        rb.useGravity = true;
        
        //YÇæÇØâèúÇµÇƒóéâ∫Ç≥ÇπÇÈ
        rb.constraints = RigidbodyConstraints.FreezeRotation| RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
        rb.velocity = new Vector3(0, -1f, 0);
    }

    //Å@ÉJÉÅÉâÇ©ÇÁäOÇÍÇΩ
    private void OnBecameInvisible()
    {
        this.gameObject.SetActive(false);
        transform.position = firstpos;
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePosition;
        meshRenderer.material= nomalmaterial;
        this.gameObject.SetActive(true);
    }
}
