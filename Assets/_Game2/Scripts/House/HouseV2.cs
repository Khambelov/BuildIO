using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class HouseV2 : MonoBehaviour
{
    public static Color baseColor = Color.white;
    public static Color transpColor = new Color(1,1,1,0.75f);

    public SpriteRenderer sprite;

    private void Start() {
        resetPos();
    }

    //temp
    private void Update() {
        resetPos();
    }

    public void resetPos()
    {
        sprite.sortingOrder = (int)transform.position.y*-1;
    }

    public void setInvis(bool invis)
    {
        sprite.color = invis ? transpColor : baseColor;
    }
}
