using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fall : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.I.gamestate("Play")&&other.gameObject.name.Equals("Player"))
        {
            GameManager.I.OnGameOver();
        }
        
    }
}
