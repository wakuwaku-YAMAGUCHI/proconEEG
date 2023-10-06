using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using UnityEngine.InputSystem;

public class title : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
         if(Input.GetKeyDown ("joystick button 2")&&Gamepad.current == null){
            
           SceneManager.LoadScene("Title");
        }

        if(Input.GetKey(KeyCode.E)){
            
           SceneManager.LoadScene("Title");

        }
        if (Gamepad.current == null) 
            return;            

        if (Gamepad.current.aButton.wasPressedThisFrame) {
            SceneManager.LoadScene("Title");

        }
    }
}
