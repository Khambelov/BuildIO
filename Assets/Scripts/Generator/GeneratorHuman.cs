using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorHuman : Generator
{
    private void Start()
    {
        for(int i=0;i<startNum;i++)
        {
            Human human = PoolingManager.Instance.getHuman();
            human.transform.position = getSpawnPos(Vector2.zero);
        }   

        StartCoroutine(generateRepeater());
    }

    IEnumerator generateRepeater()
    {
        while(true)
        {
            yield return new WaitForSeconds(5f);
            Camera cam = Camera.main;
            Human human = PoolingManager.Instance.getHuman();
            human.transform.position = getSpawnPos(PlayerMover.Instance.transform.position);
        }
    }
}
