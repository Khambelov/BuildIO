using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace IndieMarc.TopDown
{
    public class CharacterEmployee : Character
    {
        [Header("Owner")]
        public Character owner;
        public Vector3 movePosition;

        void Start()
        {
            movePosition = transform.position;
        }

        public void setColor(Color color)
        {
            GetComponent<SpriteRenderer>().color = color;
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
            rigid.velocity = move;

        }

        //Handle render and controls
        void Update()
        {
            hit_timer += Time.deltaTime;
            move_input = Vector2.zero;

            if (Vector2.Distance(owner.transform.position, transform.position) > 1 || UnityEngine.Random.value < 0.01f)
            {
                Vector2 moveCircle = UnityEngine.Random.insideUnitCircle * 2;
                movePosition = new Vector3(owner.transform.position.x + moveCircle.x, owner.transform.position.y + moveCircle.y, 0);
            }
            move_input = (movePosition - transform.position).normalized;

            //Update lookat side
            if (move.magnitude > 0.1f)
                lookat = move.normalized;
            if (Mathf.Abs(lookat.x) > 0.02)
                side = Mathf.Sign(lookat.x);

        }

            
        private void OnTriggerEnter2D(Collider2D other) {
            CharacterEmployee enemy = other.GetComponent<CharacterEmployee>();
            if(enemy && enemy.owner!=owner && enemy.owner!=null)
            {
                if(owner.getEmployeesCount()>enemy.owner.getEmployeesCount())
                {
                    enemy.owner.EmployeeRemove();
                }
            }
        }
    }

}