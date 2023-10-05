using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
[CreateAssetMenu(fileName="Chara",menuName="CreatChara")]

public class Chara : ScriptableObject{
    public int id;
    public string name;
    public float defense;
    public float speed;
    public float jumppow;
    public string info;
    public Sprite sprite;

     public Chara(Chara chara){
        this.info=chara.info;
        this.name=chara.name;
        this.id=chara.id;
        this.sprite=chara.sprite;
        this.defense=chara.defense;
        this.jumppow=chara.jumppow;
        this.speed=chara.speed;


    }
   



    
    
}
