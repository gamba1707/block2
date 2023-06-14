using UnityEngine;

//ゴールした時やスタート時にエフェクトを出す
public class Clear_act : MonoBehaviour
{
    //エフェクト
    [SerializeField] GameObject effect;

    //エフェクトを表示する（アニメーションから呼び出される）
    void OnEffect()
    {
        //エフェクトを表示する
        effect.SetActive(true);
        //良いころ合いで消す
        Invoke("effect_reset", 0.5f);
    }

    //エフェクトを非表示にする
    void effect_reset()
    {
        effect.SetActive(false);
    }

    //プレイヤーのゴール演出が終わるとアニメーションから呼び出されます
    void Onmove_End()
    {
        GameManager.I.OnClear_end();
    }
}
