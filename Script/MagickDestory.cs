using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class MagickDestory : MonoBehaviour
{
     void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "attack"){
               
               Destroy(other.gameObject);
        }
           
     }
}
