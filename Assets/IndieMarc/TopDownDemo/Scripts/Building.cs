using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

/// <summary>
/// Lever script
/// Author: Indie Marc (Marc-Antoine Desbiens)
/// </summary>
/// 

namespace IndieMarc.TopDown
{

    public enum BuildingState
    {
        broken, inprogress, repaired, disabled
    }

    public class Building : MonoBehaviour
    {
        public Sprite building_broken;
        public Sprite building_inprogress;
        public Sprite building_repaired;
        public Sprite building_disabled;

        public float health = 0;
        public int healthMax;

        public bool can_be_center;
        public BuildingState state;
        public int door_value = 1;
        public bool no_return = false;
        public bool reset_on_dead = true;

        private SpriteRenderer render;
        private BuildingState start_state;
        private BuildingState prev_state;
        private float timer = 0f;

        public UnityAction OnTriggerLever;

        private static List<Building> buildings = new List<Building>();

        private void Awake()
        {
            buildings.Add(this);
            render = GetComponent<SpriteRenderer>();
        }

        void Start()
        {
            start_state = state;
            prev_state = state;
            ChangeSprite();
        }

        void Update()
        {

            timer += Time.deltaTime;

            if (health >= healthMax) state = BuildingState.repaired;
            else if (health > healthMax / 2) state = BuildingState.inprogress;

            if (state != prev_state)
            {
                ChangeSprite();
                prev_state = state;
            }
        }

        private void OnDestroy()
        {
            buildings.Remove(this);
        }

        void OnTriggerStay2D(Collider2D coll)
        {
            Character character = coll.gameObject.GetComponent<Character>();
            if (character)
            {
                if (state == BuildingState.disabled)
                    return;
                
                Activate(character);
            }
        }

        public void Activate(Character character)
        {
            if (character.state == Character.State.working) return;

            Debug.Log("StartRepair()");
            character.StartRepair(this);

            ////Can't activate twice very fast
            //if (timer < 0f)
            //    return;

            //if (!no_return || state == start_state)
            //{
            //    timer = -0.8f;

            //    //Change state
            //    if (state == BuildingState.broken)
            //    {
            //        state = (can_be_center) ? BuildingState.broken : BuildingState.repaired;
            //    }
            //    else if (state == BuildingState.inprogress)
            //    {
            //        state = BuildingState.repaired;
            //    }
            //    else if (state == BuildingState.repaired)
            //    {
            //        state = BuildingState.broken;
            //    }

            //    //Audio
            //    GetComponent<AudioSource>().Play();

            //    //Trigger
            //    if (OnTriggerLever != null)
            //        OnTriggerLever.Invoke();
            //}
        }

        private void ChangeSprite()
        {
            if (state == BuildingState.broken)
            {
                render.sprite = building_broken;
            }
            if (state == BuildingState.inprogress)
            {
                render.sprite = building_inprogress;
            }
            if (state == BuildingState.repaired)
            {
                render.sprite = building_repaired;
            }
            if (state == BuildingState.disabled)
            {
                render.sprite = building_disabled;
            }

            if (no_return && state != start_state)
            {
                render.sprite = building_disabled;
            }
        }
        
        public void ResetOne()
        {
            if (reset_on_dead)
            {
                state = start_state;
            }
        }

        public static void ResetAll()
        {
            foreach (Building lever in buildings)
            {
                lever.ResetOne();
            }
        }
    }
}