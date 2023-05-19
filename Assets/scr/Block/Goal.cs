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
            Debug.Log("<color=red>�S�[�����܂���</color>");
            if(other.gameObject.name.Equals("Player"))
                GameManager.I.OnClear();
            clear_vcam.SetActive(true);
        }
    }
}
