using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class itemManager : MonoBehaviour
{
    
    private ToggleGroup toggleGroup;
    //�A�C�e���g�O���O���[�v�z��
    private Toggle[] toggle;
    //�A�C�e���̉摜
    [SerializeField] Sprite[]itemtex;
    // Start is called before the first frame update
    void Start()
    {
        toggleGroup = GetComponent<ToggleGroup>();
        //�Ƃ肠�����O���[�v�̐����擾���Ċi�[
        toggle = new Toggle[transform.childCount];
        
        for (int i=0; i<itemtex.Length; i++)
        {
            toggle[i]=transform.GetChild(i).GetComponent<Toggle>();
            toggle[i].name= itemtex[i].name;//���O���摜���ɕύX
            toggle[i].targetGraphic.GetComponent<Image>().sprite = itemtex[i];//�摜������������
            transform.GetChild(i).Find("Label").GetComponent<Text>().text = itemtex[i].name;//�摜���Ƀe�L�X�g��ݒ�
        }
        GameManager.I.Selectname = toggleGroup.ActiveToggles().First().name;
        Debug.Log("���I������Ă���F" + GameManager.I.Selectname);
    }

 
    //�l���ω�����ƌĂ΂��
    //�I������Ă�����̂��X�V����
    public void OnSelectChenge()
    {
        GameManager.I.Selectname =toggleGroup.ActiveToggles().First().name;
        Debug.Log("���I������Ă���F"+GameManager.I.Selectname);
    }
}
