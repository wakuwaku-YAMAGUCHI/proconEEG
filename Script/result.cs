using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;


public class result : MonoBehaviour
{

     
     private float con;
     private float Restime;
     private float ResCon;
     private float Reslir;
     public TextMeshProUGUI timestr;
     public TextMeshProUGUI constr;
     public TextMeshProUGUI lirstr;

    
    // Start is called before the first frame update
    void Start()
    {
        Restime=testkansu.Gametime;
        con=testkansu.con;
    }

    // Update is called once per frame
    void Update()
    {

        ResCon=con/Restime*100;
        Reslir=100-ResCon;
        timestr.SetText("ゲームの参加時間 : {0:1}", Restime); 
        constr.SetText("集中の割合: {0:1}", ResCon); 
        lirstr.SetText("リラックスの割合 : {0:1}", Reslir); 
       

            

    }
}
