using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapData_scrobj : ScriptableObject
{
    public int clearnum;
    public Vector3 stage_vcampos;
    public int floornum;
    public Vector3[] floorpos;
    public int fallnum;
    public Vector3[] fallpos;
    public Vector3 goalpos;
    public float deadline;
}
