using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class MagicDamage :  MonoBehaviourPunCallbacks
{
      public int MyID { get; private set; }
      public int IDA;
     
      public  void getID(int ID){
         
          MyID=ID;
          IDA=MyID;
          Debug.Log(IDA);
     }

      
     

    public void Damage(Collider2D other,float pow)
    {
          
          IDdamage damageable = other.gameObject.GetComponent<IDdamage>();//プレイヤーのダメージ計算式を取得
          if(damageable != null)
          {
               damageable.Damagea(pow);
               
          }

          
          
               
          
          

    }
     
    
}
