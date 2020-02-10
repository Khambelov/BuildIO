using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingManager2 : MonoBehaviour
{
    public GameObject[] prfabs1;

    public Transform spawnerPoints1;
    public Transform houseParent1;


    public GameObject[] prfabs2;

    public Transform spawnerPoints2;
    public Transform houseParent2;

    private void Start() {
        generate(prfabs1,spawnerPoints1,houseParent1,5,0);
        generate(prfabs2,spawnerPoints2,houseParent2,0,-30);
    }

    void generate(GameObject[] prefabs, Transform spawnerPoints,Transform parent,float shift = 0,float angle=0)
    {
        foreach(Transform tf in spawnerPoints)
        {
            int rnd = Random.Range(0,prefabs.Length);
            GameObject house = Instantiate(prefabs[rnd],tf.transform.position,Quaternion.identity);
            Vector3 pos =  house.transform.position;
            pos.z = 0;

            house.transform.position = pos;
            house.transform.rotation = Quaternion.Euler(new Vector3(angle,0,0));
            house.transform.SetParent(parent);
        }

        foreach(Transform tf in spawnerPoints)
        {
            Destroy(tf.gameObject);
        }
    }
}
