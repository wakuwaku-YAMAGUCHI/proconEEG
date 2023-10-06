using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
[CreateAssetMenu(fileName="Magic",menuName="CreatMagic")]


public class Magic : ScriptableObject
{
    public int id;
    public string name;
    public string info;
    public Type type;
    public Sprite sprite;
    public float forse;
    public float lostmp;
    public float speed;
    public GameObject Hantei;


    public enum Type{
        Fire,

    }

    public Magic(Magic magic){
        this.type=magic.type;
        this.info=magic.info;
        this.name=magic.name;
        this.id=magic.id;
        this.sprite=magic.sprite;
        this.Hantei=magic.Hantei;
        this.lostmp=magic.lostmp;

    }
    
}
