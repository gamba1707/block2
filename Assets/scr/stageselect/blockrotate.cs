using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blockrotate : MonoBehaviour
{
    [Header("‰ñ“]”")]
    [SerializeField]float x, y, z;

    void FixedUpdate()
    {
        transform.Rotate(x, y, z);
    }
}
