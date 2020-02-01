using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        // StartCoroutine(ystroyDestroy());
    }

    IEnumerator ystroyDestroy()
    {
        yield return new WaitForSeconds(1);
        PoolingManager.Instance.release(this);
    }
}
