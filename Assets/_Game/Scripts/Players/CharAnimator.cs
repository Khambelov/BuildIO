using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharAnimator : MonoBehaviour
{
    public IndieMarc.TopDown.Character character;
    public Animator animator;

    void Update()
    {
        animator.SetFloat("Speed", character.GetMove().magnitude);
        animator.SetInteger("Side", character.GetSideAnim());
    }
}

