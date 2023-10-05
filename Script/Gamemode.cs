using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using UnityEngine.InputSystem;


public class Gamemode : MonoBehaviour
{

    public void OnClickStartButton()
{
    PhotonNetwork.Disconnect();
    SceneManager.LoadScene("Game scene");
     
}
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        
        if(Input.GetKeyDown ("joystick button 2")&&Gamepad.current == null){
            PhotonNetwork.Disconnect();
            SceneManager.LoadScene("Game scene");

        }

        if(Input.GetKey(KeyCode.E)){
            PhotonNetwork.Disconnect();
            SceneManager.LoadScene("Game scene");

        }
        if (Gamepad.current == null) 
            return;            

        if (Gamepad.current.aButton.wasPressedThisFrame) {
             PhotonNetwork.Disconnect();
            SceneManager.LoadScene("Game scene");

        }
                
    }
}
