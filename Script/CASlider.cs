using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CASlider : MonoBehaviour
{

     [SerializeField]
    private Slider Counterlider;
    public static float Gage;


    // Update is called once per frame
    void Update()
    {
        if(Gage!=null){
            
            Counterlider.value=Gage;
        }
    }
}
