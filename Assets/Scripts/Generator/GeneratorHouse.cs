using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorHouse : Generator
{
    private void Start()
    {
        for(int i=0;i<startNum;i++)
        {
            House house = PoolingManager.Instance.getHouse();
            house.transform.position = getSpawnPos(Vector2.zero);
        }   

        // StartCoroutine(generateRepeater());
    }

    IEnumerator generateRepeater()
    {
        while(true)
        {
            yield return new WaitForSeconds(5f);
            Camera cam = Camera.main;
            House house = PoolingManager.Instance.getHouse();
            house.transform.position = getSpawnPos(PlayerMover.Instance.transform.position);
        }
    }
}
