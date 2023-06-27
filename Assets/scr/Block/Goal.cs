using UnityEngine;

//ゴールのもやもやにつけるスクリプト
public class Goal : MonoBehaviour
{
    //ゴールした時に寄るカメラ
    [SerializeField] GameObject clear_vcam;
    //ゴール用のオーディオ
    AudioSource audioSource;
    //SEをすでに鳴らしているか
    bool SE_IsDone;

    private void Start()
    {
        if (MapData.mapinstance.Last) this.gameObject.SetActive(false);
        //自身にあるaudioSourceを取得
        audioSource = GetComponent<AudioSource>();
    }

    //ゴール判定に当たったら呼ばれる
    private void OnTriggerEnter(Collider other)
    {
        if (!MapData.mapinstance.Last)
        {
            //当たった相手がPlayerだった場合
            if (other.gameObject.CompareTag("Player"))
            {
                //ゴール処理をする
                Debug.Log("<color=red>ゴールしました</color>");
                //GameManagerに通知する
                GameManager.I.OnClear();
                //カメラを寄らせる
                clear_vcam.SetActive(true);
                //効果音を鳴らす
                if (!SE_IsDone) audioSource.PlayOneShot(audioSource.clip);
                //音が二重にならないようにする
                SE_IsDone = true;
            }
        }
    }
}
