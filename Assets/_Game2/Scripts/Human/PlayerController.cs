using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool humanControlled;

    // Start is called before the first frame update
    void Start()
    {
        if(humanControlled)
            Camera.main.transform.SetParent(transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate() {
        float hor = Input.GetAxis("Horizontal");
        float ver = Input.GetAxis("Vertical");

        transform.Translate(new Vector3(hor,ver,0),Space.World);
    }
}
