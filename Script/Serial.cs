using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Serial : MonoBehaviour
{
    public SerialHandler serialHandler;
    private float time;

  void Start()
    {
    
    }
    //受信した信号(message)に対する処理
       void Update() 
    {
       
            serialHandler.Write(testkansu.flag);
            time=0;
            Debug.Log(testkansu.flag);
        

        


    }

    
     private void OnApplicationQuit()
    {
      //serialHandler.Close();
    }
    
}