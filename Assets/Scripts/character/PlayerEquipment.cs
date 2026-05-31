using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 提供对每一个着色器的引用，提供更换的方法
/// </summary>
public class PlayerEquipment : MonoBehaviour
{
    List<SpriteRenderer> Renderers = new List<SpriteRenderer>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //change Rendering Mode of a material
   public void ChangeRender()
    {
        
    }
}

public enum EquipmentType
{
    None,
    Hat,
    Cloth,
    Pants,
    Shoes,
    Accessory
}
