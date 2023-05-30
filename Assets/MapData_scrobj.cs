using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapData_scrobj : ScriptableObject
{
    public int clearnum;
    public Vector3 stage_vcampos;
    public Vector3[] floorpos;
    public Vector3[] fallpos;
    public Vector3 goalpos;
    public float deadline;
}
