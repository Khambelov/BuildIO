using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanLayer : MonoBehaviour
{
    public SpriteRenderer sprite;
    public int shift;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // sprite.sortingOrder = (int)transform.position.y*-1 + shift;
    }

    private void OnTriggerStay2D(Collider2D other) {
        if(other.CompareTag("HouseBack"))
        {
            sprite.sortingOrder = (int)other.transform.parent.position.y*-1-1;
            other.GetComponent<BackCollider>().house.setInvis(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.CompareTag("HouseBack"))
        {
            sprite.sortingOrder = 2000;
            other.GetComponent<BackCollider>().house.setInvis(false);
        }
    }
}
