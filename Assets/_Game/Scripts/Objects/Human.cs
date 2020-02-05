using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : MonoBehaviour
{
    public bool used = false;
    public bool busy = false;

    // Start is called before the first frame update
    void Start()
    {
        // StartCoroutine(ystroyDestroy());
    }

    IEnumerator ystroyDestroy()
    {
        yield return new WaitForSeconds(20);
        PoolingManager.Instance.release(this);
    }
}
