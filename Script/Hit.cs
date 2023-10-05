using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Hit : MonoBehaviourPunCallbacks
{
    public int MyId { get; private set; }
    public  bool flag=false;
    public  bool Cflag=false;
    float pow=10f;
    
    private Collider2D MyCollider;
    public Collider2D otherCollider=null;

    public GameObject Pearent= default;
     
    
    // Start is called before the first frame update
    void Start()
    {
         MyCollider = GetComponent<Collider2D>();
        
         Debug.Log(MyCollider);
        
    }

    // Update is called once per frame
    void Update()
    {
         
    }

    void OnTriggerEnter2D(Collider2D other) {
   
        otherCollider=other;
        if (photonView.IsMine) {
            if(other.TryGetComponent<MagicDamage>(out var MagicDamage)){
                     
                if (MagicDamage.IDA!= PhotonNetwork.LocalPlayer.ActorNumber) {
                    if(Shot.caun==true){
                        Cflag=true;
                    }
                    else{
                        MagicDamage.Damage(MyCollider,pow);
                        flag=true;
                       
                    }
                    
                 }   
            }
           
        }
         
    }


     
}
