﻿using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace IndieMarc.TopDown
{
    class CharacterPlayer : Character
    {
        public static CharacterPlayer Instance;

        public PlayerControls controls;

        private void Start()
        {
            Instance = this;

            score = 0;
            buildCount = 0;
            teamColor = Color.red;

            CharacterContainer.Instance.AddNewCharacter(this);
        }

        //Handle physics
        void FixedUpdate()
        {
            //Movement velocity
            float desiredSpeedX = Mathf.Abs(move_input.x) > 0.1f ? move_input.x * move_max : 0f;
            float accelerationX = Mathf.Abs(move_input.x) > 0.1f ? move_accel : move_deccel;
            move.x = Mathf.MoveTowards(move.x, desiredSpeedX, accelerationX * Time.fixedDeltaTime);
            float desiredSpeedY = Mathf.Abs(move_input.y) > 0.1f ? move_input.y * move_max : 0f;
            float accelerationY = Mathf.Abs(move_input.y) > 0.1f ? move_accel : move_deccel;
            move.y = Mathf.MoveTowards(move.y, desiredSpeedY, accelerationY * Time.fixedDeltaTime);

            //Move
            if (Input.GetMouseButton(0))
            {
                rigid.velocity = move;
            }
            else
            {
                rigid.velocity = Vector2.zero;
                move = Vector2.zero;
            }

            state = move != Vector2.zero ? State.moving : state != State.working ? State.idle : State.working;
        }



        private  void OnCollisionStay2D(Collision2D collision)
        {
            HouseBuildCollider hbcollider = collision.gameObject.GetComponent<HouseBuildCollider>();
            if (hbcollider != null && controls.IsStay)
            {
                currentHouse = hbcollider.house;
                currentHouse.currentBuilder = this;
            }
        }    

        //Handle render and controls
        void Update()
        {
            hit_timer += Time.deltaTime;
            move_input = Vector2.zero;

            //Controls
            if (!disable_controls)
            {
                PlayerControls controls = PlayerControls.Get(player_id);
                move_input = controls.GetMove();
            }

            //Update lookat side
            if (move.magnitude > 0.1f)
                lookat = move.normalized;
            if (Mathf.Abs(lookat.x) > 0.02)
                side = Mathf.Sign(lookat.x);

        }
    }
}