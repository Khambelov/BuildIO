using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMover : MonoBehaviour
{
    public static PlayerMover Instance;

    List<Human> brigada = new List<Human>();


    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        // if((int)(Time.time)%2!=0)
        //     return;

        // foreach(Human human in brigada)
        // {
        //     human.transform.position = transform.position +  new Vector3(Random.Range(-3f,3f),Random.Range(-3f,3f),0);
        // }

        float va = Input.GetAxis("Vertical");
        float xa = Input.GetAxis("Horizontal");

        // transform.Translate(new Vector3(xa,0,va)*-0.1f);
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        Human human = other.GetComponent<Human>();
        if(human)
        {
            // PoolingManager.Instance.release(human);
            brigada.Add(human);
        }    
    }
}
