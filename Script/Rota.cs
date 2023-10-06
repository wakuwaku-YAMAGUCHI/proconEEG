using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Rota : MonoBehaviour
{

    public static float playerRotate;
   
    public int comID;

     public  void getRotate(float rot){
          playerRotate=rot;
     }


   
     

    // Start is called before the first frame update
    void Start()
    {
        
           
        
    }

     private void OnTriggerEnter2D(Collider2D other) {
        
          
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
