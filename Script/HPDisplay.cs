using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPDisplay : MonoBehaviour
{
    public GameObject Player;
   
    // bool flagA = false;
    // bool flagB = false;
   

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

         Transform transform=this.transform;
         Vector3 pos =transform.position;

        

        if(Player.transform.rotation.y>=-180f){
          //this.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
          this.transform.position = new Vector2(Player.transform.position.x-1.0f, Player.transform.position.y+2f);
        }
        else if(Player.transform.rotation.y<=0f){
          //this.transform.rotation = Quaternion.Euler(0.0f, 180f, 0.0f);
          this.transform.position = new Vector2(Player.transform.position.x+1.0f,Player.transform.position.y+2f);
        }
        
    }
}
