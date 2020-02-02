using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingManager2 : MonoBehaviour
{
    public GameObject[] prfabs;

    public Transform spawnerPoints;
    public Transform houseParent;

    private void Start() {
        foreach(Transform tf in spawnerPoints)
        {
            int rnd = Random.Range(0,prfabs.Length);
            GameObject house = Instantiate(prfabs[rnd],tf.transform.position,Quaternion.identity);
            Vector3 pos =  house.transform.position;
            pos.z = 5;
            house.transform.position = pos;
            house.transform.SetParent(houseParent);
        }

        foreach(Transform tf in spawnerPoints)
        {
            Destroy(tf.gameObject);
        }
    }
}
