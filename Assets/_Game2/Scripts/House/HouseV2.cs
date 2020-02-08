using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseV2 : MonoBehaviour
{
    public static Vector3 baseRot = new Vector3(-30,0,0);

    [Header("Visual")]
    public SpriteRenderer spriteRender;
    public Sprite phase1;
    public Sprite phase2;
    public Animator phase2Animator;

    [Header("state")]
    public EHouseState currentState;
    
    [SerializeField]
    private bool resetPosOnStart = false;

    private void Start() {
        draw();

        if(resetPosOnStart)
            resetPos();
    }

    public void resetPos()
    {
        spriteRender.sortingOrder = (int)transform.position.y*-1;
        
        Vector3 pos = transform.position;
        pos.z = 0;
        transform.position = pos;

        transform.rotation = Quaternion.Euler(baseRot);
    }

    public void toggleTransparency(bool invis)
    {
        spriteRender.color = invis ? GameColors.transpColor : GameColors.baseColor;
    }

    public void draw()
    {
        if(currentState == EHouseState.Destroyed)
        {
            spriteRender.sprite = phase1;
            
            if(phase2Animator)
                phase2Animator.enabled = false;
        }
        else if(currentState==EHouseState.Builded)
        {
            spriteRender.sprite = phase2;

            if(phase2Animator)
                phase2Animator.enabled = true;
        }
    }
}
