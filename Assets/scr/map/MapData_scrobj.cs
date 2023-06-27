using UnityEngine;

//ストーリーモードの地形データ
public class MapData_scrobj : ScriptableObject
{
    public bool laststage;
    public bool bossstage;//ボスステージか？
    public bool bossstage_reverse;//ボスステージ反転
    public int clearnum;//目標クリア数
    public Vector3 stage_vcampos;//全体を映すカメラ
    public Vector3[] floorpos;//床ブロックの位置
    public Vector3[] fallpos;//奈落ブロックの位置
    public Vector3[] Trampolinepos;//地形飛べるブロックの位置
    public Vector3[] Downpos;//地形下がるブロックの位置
    public Vector3 goalpos;//ゴール位置
    public float deadline;//自動的に計算するデッドライン
}
