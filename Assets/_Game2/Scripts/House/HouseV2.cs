using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class HouseV2 : MonoBehaviour
{
    public static Color baseColor = Color.white;
    public static Color transpColor = new Color(1,1,1,0.75f);
    public static Vector3 baseRot = new Vector3(-30,0,0);

    public SpriteRenderer sprite;
    public float zShift;

    private void Start() {
        resetPos();
    }

    public void resetPos()
    {
        sprite.sortingOrder = (int)transform.position.y*-1;
        
        Vector3 pos = transform.position;
        pos.z = zShift;
        transform.position = pos;

        transform.rotation = Quaternion.Euler(baseRot);
    }

    public void setInvis(bool invis)
    {
        sprite.color = invis ? transpColor : baseColor;
    }
}
