using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class testShot : MonoBehaviour
{
   
    [SerializeField] private MagicDataBase magicDataBase;//データベース呼び出し
    public GameObject shot;
    private Vector2 a; //自分の座標
    float x;            //x座標
    float y;            //y座標
    Magic magic;        //保存用


    // Start is called before the first frame update
    void Start()
    {
         magic =magicDataBase.magics[0];//魔法の種類
    }

    // Update is called once per frame
    void Update()
    {
        /*
        a=this.transform.position;
        x=a.x;
        y=a.y;
        
        if(Input.GetMouseButtonDown(1)&&magic.lostmp<=UnityChan2DController.mp){
            UnityChan2DController.mp=UnityChan2DController.mp-magic.lostmp;
            Instantiate(magic.Hantei,new Vector2(x,y),Quaternion.identity);
             
            
        }
        */
    }
}
