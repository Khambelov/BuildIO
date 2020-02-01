using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IndieMarc.TopDown
{
    [RequireComponent(typeof(Character))]
    [RequireComponent(typeof(Animator))]
    public class CharacterAnim : MonoBehaviour
    {
        private Character character;
        private CharacterHoldItem character_item;
        private Animator animator;

        void Awake()
        {
            character = GetComponent<Character>();
            character_item = GetComponent<CharacterHoldItem>();
            animator = GetComponent<Animator>();
        }

        void Update()
        {
            //Anims
            animator.SetFloat("Speed", character.GetMove().magnitude);
            animator.SetInteger("Side", character.GetSideAnim());
            if(character_item != null)
                animator.SetBool("Hold", character_item.GetHeldItem() != null);
        }
        
    }

}