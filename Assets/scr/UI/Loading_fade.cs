using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Loading_fade : MonoBehaviour
{
    private float fade;//0-1�Ńt�F�C�h�C����A�E�g�𐧌�
    [SerializeField] bool fade_move;//���쒆��
    RectTransform rectTransform;
    float top, bottom, left, right;
    // Start is called before the first frame update
    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        top = -5f;
        bottom = -5f;
        left = -5f;
        right = -5f;
        rectTransform.offsetMin = new Vector2(left, bottom);
        rectTransform.offsetMax = new Vector2(-right, -top);
    }

    // Update is called once per frame
    void Update()
    {
        /*
        right = fade * Screen.width - 5;
        rectTransform.offsetMin = new Vector2(left, bottom);
        rectTransform.offsetMax = new Vector2(-right, -top);
        */
    }

    public bool Fade_move
    {
        get { return fade_move; }
        set { fade_move = value; }
    }

    public void Fadein()
    {
        Debug.Log("�t�F�[�h�C��");
        StartCoroutine("fadein_move");
    }
    //���ۂ�fadeout�𓮂����Ă���̂̓R�`��
    IEnumerator fadein_move()
    {
        fade = 0f;
        Fade_move = true;
        while (fade <= 1)
        {
            right = fade * Screen.width - 5;
            rectTransform.offsetMax = new Vector2(-right, -top);
            fade += 0.005f;
            yield return null;
        }
        yield return new WaitForSecondsRealtime(0.25f);
        Fade_move = false;
    }

    public void Fadeout()
    {
        Debug.Log("�t�F�[�h�A�E�g");
        StartCoroutine("fadeout_move");
    }
    //���ۂ�fadeout�𓮂����Ă���̂̓R�`��
    IEnumerator fadeout_move()
    {
        fade = 1f;//1�ɂ���
        Fade_move= true;
        
        while (fade >= 0)
        {
            right = fade * Screen.width - 5;//�E�[����̑傫��������
            rectTransform.offsetMax = new Vector2(-right, -top);//������
            fade -= 0.005f;//�i�K��i�߂�
            yield return null;//�t���[�������
        }
        yield return new WaitForSecondsRealtime(0.25f);
        Fade_move = false;
    }
}
