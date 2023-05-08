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
        Debug.Log(rb.velocity);
        if(rb.velocity.y<-1f)rb.velocity = new Vector3(0, -1f, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.tag);
        if (other.gameObject.name.Equals("Player"))
        {
            meshRenderer.material= downmaterial;

            Invoke("down_move",2f);
        }
    }

    void down_move()
    {
        rb.useGravity = true;
        
        //Y‚¾‚¯‰ðœ‚µ‚Ä—Ž‰º‚³‚¹‚é
        rb.constraints = RigidbodyConstraints.FreezeRotation| RigidbodyConstraints.FreezePositionX;
        rb.velocity = new Vector3(0, -1f, 0);
    }

    //@ƒJƒƒ‰‚©‚çŠO‚ê‚½
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
