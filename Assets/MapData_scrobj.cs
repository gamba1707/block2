using UnityEngine;

//�X�g�[���[���[�h�̒n�`�f�[�^
public class MapData_scrobj : ScriptableObject
{
    public bool bossstage;//�{�X�X�e�[�W���H
    public int clearnum;//�ڕW�N���A��
    public Vector3 stage_vcampos;//�S�̂��f���J����
    public Vector3[] floorpos;//���u���b�N�̈ʒu
    public Vector3[] fallpos;//�ޗ��u���b�N�̈ʒu
    public Vector3[] Trampolinepos;//�n�`��ׂ�u���b�N�̈ʒu
    public Vector3[] Downpos;//�n�`������u���b�N�̈ʒu
    public Vector3 goalpos;//�S�[���ʒu
    public float deadline;//�����I�Ɍv�Z����f�b�h���C��
}
