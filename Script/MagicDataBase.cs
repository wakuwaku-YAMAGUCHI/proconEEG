using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "MagicDataBase", menuName = "CreateMagicDataBase")]
public class MagicDataBase : ScriptableObject
{
    public List<Magic> magics=new  List<Magic>();
}
