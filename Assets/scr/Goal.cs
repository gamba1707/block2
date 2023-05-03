using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    [SerializeField] GameObject clear_vcam;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("ÉSÅ[ÉãÇµÇ‹ÇµÇΩ");
            if(other.gameObject.name.Equals("Player"))
                GameManager.I.OnClear(other.gameObject.GetComponent<Player_move>());
            clear_vcam.SetActive(true);
        }
    }
}
