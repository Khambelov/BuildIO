using System;
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
        [Header("Employees")]
        public int employeesCount;
        public List<CharacterEmployee> employeesList;
        public GameObject employePrefab;
        public PlayerControls controls;

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
            state = move != Vector2.zero ? State.moving : state != State.working ? State.idle : State.working;

            if (employeesCount != employeesList.Count)
                if (employeesCount > employeesList.Count) EmployeeAdd(Vector3.zero);
                else EmployeeRemove();
        }

        public override int getEmployeesCount()
        {
            return employeesCount+1;
        }

        public void EmployeeAdd(Vector3 spawnPosition)
        {
            if (spawnPosition == Vector3.zero)
            {
                Vector2 spawnCircle = UnityEngine.Random.insideUnitCircle * 5;
                spawnPosition = new Vector3(transform.position.x + spawnCircle.x, transform.position.y + spawnCircle.y, transform.position.z);
            }
            CharacterEmployee newEmployee = Instantiate(employePrefab, spawnPosition, transform.rotation).GetComponent<CharacterEmployee>();
            
            //if (employeesList.Count > 1) newEmployee.owner = employeesList[employeesList.Count - 2].gameObject;
            //else newEmployee.owner = gameObject;
            newEmployee.owner = gameObject;
            employeesList.Add(newEmployee);
            if (employeesCount < employeesList.Count) employeesCount = employeesList.Count;

            AudioManager.Instance.PlaySound("Join");
        }

        private void EmployeeRemove()
        {
            Destroy(employeesList.Last().gameObject);
            employeesList.Remove(employeesList.Last());
        }

        House currentHouse;

        private void OnCollisionStay2D(Collision2D collision)
        {
            HouseBuildCollider hbcollider = collision.gameObject.GetComponent<HouseBuildCollider>();
            if (hbcollider != null && controls.IsStay)
            {
                currentHouse = hbcollider.house;
                currentHouse.currentBuilder = this;
                // currentHouse.StartBuilding(GetBuildSpeed(hbcollider.house.RequiredWorkers), employeesCount + 1, gameObject);
                // StartCoroutine(buildToBuild());
            }
        }

        // IEnumerator buildToBuild()
        // {
        //     while(currentHouse && Vector2.Distance(transform.position,currentHouse.transform.position)<=3)
        //     {
        //         Debug.Log("dist "+Vector2.Distance(transform.position,currentHouse.transform.position));
        //         yield return null;
        //     }

        //     if(currentHouse)
        //     {
        //         currentHouse.CancelBuildings();
        //         currentHouse = null;
        //     }
        // }       

        private float GetBuildSpeed(int requiredWorkers)
        {
            float workers = (float)requiredWorkers;
            float employee = (float)employeesCount + 1f;
            float result = ((workers / 120f) / workers) * employee * 100f;

            if (result > 14.3f)
                result = 14.3f;

            return employeesCount+1;
            // return result;
        }

        //Handle render and controls
        void Update()
        {
            hit_timer += Time.deltaTime;
            move_input = Vector2.zero;

            //Controls
            if (!disable_controls)
            {
                //Controls
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