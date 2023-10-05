using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using UnityEngine.InputSystem;
using TMPro;

public class GetID : MonoBehaviour
{
    public  TMP_InputField inputField;
    public static string ID;
    // Start is called before the first frame update
    void Start()
    {
        inputField = GameObject.Find("ID").GetComponent< TMP_InputField>();
        ID="";
    }

    public void GetIDs()
    {
        //InputFieldからテキスト情報を取得する
        ID = inputField.text;
        Debug.Log(ID);
         SceneManager.LoadScene("Menu");
 
    }
}
