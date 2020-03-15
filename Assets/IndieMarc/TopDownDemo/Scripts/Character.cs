using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

/// <summary>
/// Top down character movement
/// Author: Indie Marc (Marc-Antoine Desbiens)
/// </summary>

namespace IndieMarc.TopDown
{
    public class Character : MonoBehaviour
    {
        public int player_id;

        [Header("Stats")]
        public float max_hp = 100f;
        public State state;
        public enum State { idle, moving, working }

        [Header("Status")]
        public bool invulnerable = false;

        [Header("Movement")]
        public float move_accel = 1f;
        public float move_deccel = 1f;
        public float move_max = 1f;

        [Header("For Scores")]
        public Color teamColor;
        public int score;
        public int buildCount;

        public UnityAction onDeath;
        public UnityAction onHit;

        protected Rigidbody2D rigid;
        protected Animator animator;
        protected AutoOrderLayer auto_order;
        protected ContactFilter2D contact_filter;

        protected float hp;
        protected bool is_dead = false;
        protected Vector2 move;
        protected Vector2 move_input;
        protected Vector2 lookat = Vector2.zero;
        protected float side = 1f;
        protected bool disable_controls = false;
        protected float hit_timer = 0f;

        private static Dictionary<int, Character> character_list = new Dictionary<int, Character>();

        [Header("Employees")]
        protected List<IndieMarc.TopDown.CharacterEmployee> employeesList = new List<CharacterEmployee>();

        public SpriteRenderer spr;
        public Color playerColor;

        void Awake()
        {
            character_list[player_id] = this;
            rigid = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            auto_order = GetComponent<AutoOrderLayer>();
            hp = max_hp;

            if (spr == null)
            {
                GetComponent<SpriteRenderer>().color = playerColor;
            }
            else
            {
                spr.color = playerColor;
            }
        }

        public virtual int getEmployeesCount()
        {
            return employeesList.Count+1;
        }

        public void EmployeeAdd(Vector3 spawnPosition)
        {
            if (spawnPosition == Vector3.zero)
            {
                Vector2 spawnCircle = UnityEngine.Random.insideUnitCircle * 5;
                spawnPosition = new Vector3(transform.position.x + spawnCircle.x, transform.position.y + spawnCircle.y, transform.position.z);
            }

            CharacterEmployee newEmployee = ContainerEmploy.instance.getEmploy();
            newEmployee.setColor(playerColor);
            newEmployee.move_max = PlayerMover.Instance.GetComponent<Character>().move_max;
            newEmployee.move_accel = PlayerMover.Instance.GetComponent<Character>().move_accel;
            newEmployee.move_deccel = PlayerMover.Instance.GetComponent<Character>().move_deccel;
            newEmployee.transform.position =spawnPosition;
            newEmployee.transform.rotation = transform.rotation;            
            


            newEmployee.owner = this;
            employeesList.Add(newEmployee);
            // if (employeesCount < employeesList.Count) employeesCount = employeesList.Count;

            AudioManager.Instance.PlaySound("Join");
        }

        private void OnCollisionEnter2D(Collision2D other) {
            Human human = other.gameObject.GetComponent<Human>();
            if(human)
            {
                Debug.Log(gameObject.name  + " getHuman");
                EmployeeAdd(other.gameObject.transform.position);
                PoolingManager.Instance.release(human);
            }
        }

        
       protected  House currentHouse;

        private  void OnCollisionStay2D(Collision2D collision)
        {
            HouseBuildCollider hbcollider = collision.gameObject.GetComponent<HouseBuildCollider>();
            if (hbcollider != null)
            {
                currentHouse = hbcollider.house;
                currentHouse.currentBuilder = this;
                // currentHouse.StartBuilding(GetBuildSpeed(hbcollider.house.RequiredWorkers), employeesCount + 1, gameObject);
                // StartCoroutine(buildToBuild());
            }
        }    

        private float GetBuildSpeed(int requiredWorkers)
        {
            float workers = (float)requiredWorkers;
            float employee = (float)getEmployeesCount();
            float result = ((workers / 120f) / workers) * employee * 100f;

            if (result > 14.3f)
                result = 14.3f;

            return getEmployeesCount();
            // return result;
        }

        void OnDestroy()
        {
            character_list.Remove(player_id);
        }

        public void EmployeeRemove()
        {
            if(employeesList.Count<=0)
                return;
                
            Destroy(employeesList.Last().gameObject);
            employeesList.Remove(employeesList.Last());
        }

        public void StartRepair(Building building)
        {
            if (state == State.idle) StartCoroutine(Repairing(building));
        }

        IEnumerator Repairing(Building building)
        {
            state = State.working;
            animator.SetBool("Working", true);
            while (building.health < building.healthMax && state == State.working)
            {
                building.health++;
                yield return new WaitForSeconds(0.25f);
            }
            animator.SetBool("Working", false);
            state = State.moving;
            yield return null;
        }

        public void HealDamage(float heal)
        {
            if (!is_dead)
            {
                hp += heal;
                hp = Mathf.Min(hp, max_hp);
            }
        }

        public void TakeDamage(float damage)
        {
            if (!is_dead && !invulnerable && hit_timer > 0f)
            {
                hp -= damage;
                hit_timer = -1f;

                if (hp <= 0f)
                {
                    Kill();
                }
                else
                {
                    if (onHit != null)
                        onHit.Invoke();
                }
            }
        }

        public void Kill()
        {
            if (!is_dead)
            {
                is_dead = true;
                rigid.velocity = Vector2.zero;
                move = Vector2.zero;
                move_input = Vector2.zero;

                if (onDeath != null)
                    onDeath.Invoke();
            }
        }
        
        public void Teleport(Vector3 pos)
        {
            transform.position = pos;
            move = Vector2.zero;
        }

        public Vector2 GetMove()
        {
            return move;
        }

        public Vector2 GetFacing()
        {
            return lookat;
        }

        public int GetSortOrder()
        {
            return auto_order.GetSortOrder();
        }

        //Get Character side
        public float GetSide()
        {
            return side; //Return 1 frame before to let anim do transitions
        }

        public int GetSideAnim()
        {
            return (side >= 0) ? 1 : 3;
        }

        public bool IsDead()
        {
            return is_dead;
        }

        public void DisableControls() { disable_controls = true; }
        public void EnableControls() { disable_controls = false; }
        
        public static Character GetNearest(Vector3 pos, float range = 999f, bool alive_only=true)
        {
            Character nearest = null;
            float min_dist = range;
            foreach (Character character in character_list.Values)
            {
                if (!alive_only || !character.IsDead())
                {
                    float dist = (pos - character.transform.position).magnitude;
                    if (dist < min_dist)
                    {
                        min_dist = dist;
                        nearest = character;
                    }
                }
            }
            return nearest;
        }

        public static Character Get(int player_id)
        {
            foreach (Character character in character_list.Values)
            {
                if (character.player_id == player_id)
                {
                    return character;
                }
            }
            return null;
        }

        public static Character[] GetAll()
        {
            Character[] list = new Character[character_list.Count];
            character_list.Values.CopyTo(list, 0);
            return list;
        }
    }
}
