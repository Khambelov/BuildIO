using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialShadow : MonoBehaviour
{
    SpriteRenderer parentSprte;
    SpriteRenderer currentSprte;
    Color shadowColor = Color.black;

    void Start()
    {
        parentSprte = transform.parent.GetComponent<SpriteRenderer>();
        currentSprte = GetComponent<SpriteRenderer>();

        currentSprte.sprite = parentSprte.sprite;


        shadowColor.a = 0.3f;
        currentSprte.color = shadowColor;

        transform.SetParent(ShadowsContainer.instance.transform,true);
    }
}
