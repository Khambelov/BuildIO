using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : HumanController
{
    public float speed;

    void Start()
    {
        Camera.main.transform.SetParent(transform);
    }

    private void FixedUpdate() {
        float hor = Input.GetAxis("Horizontal");
        float ver = Input.GetAxis("Vertical");

        transform.Translate(new Vector3(hor*speed,ver*speed,0),Space.World);
    }
}
