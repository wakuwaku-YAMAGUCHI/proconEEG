using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ca : MonoBehaviour
{
    // 変数の定義
    public Transform target;
 
    // シーン開始時に一度だけ呼ばれる関数
    void Start(){
        // 変数にPlayerオブジェクトのtransformコンポーネントを代入
        
    }
 
    // シーン中にフレーム毎に呼ばれる関数
    void Update () {
        // カメラのx座標をPlayerオブジェクトのx座標から取得y座標とz座標は現在の状態を維持
        if(target!=null)
            transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);
    }
}
