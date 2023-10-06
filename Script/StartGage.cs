using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGage : MonoBehaviour
{
    public GameObject CaunterGage= default;
    private bool flag=true;
    public static bool flagc=false;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(CASlider.Gage!=null&&flag==true&&flagc==true){
            CaunterGage.SetActive(true);
            flag=false;
            flagc=false;


        }
    }
}
