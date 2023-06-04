using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class noise_move : MonoBehaviour
{
    Vector3 firstpos;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Start_Check", 0.5f);
        firstpos = transform.position;
    }
    void Start_Check()
    {
        if (!MapData.mapinstance.Boss)this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (GameManager.I.gamestate("Play"))
        {
            transform.position += new Vector3(0, 0, 0.025f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.I.gamestate("Play") && other.gameObject.name.Equals("Player"))
        {
            GameManager.I.OnGameOver();
        }
    }

    public void noise_reset()
    {
        transform.position = firstpos;
    }
}
