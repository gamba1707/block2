using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blockrotate : MonoBehaviour
{
    [Header("��]��")]
    [SerializeField]float x, y, z;

    void FixedUpdate()
    {
        transform.Rotate(x, y, z);
    }
}
