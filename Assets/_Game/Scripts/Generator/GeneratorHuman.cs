using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorHuman : Generator
{
    Vector2 vec = new Vector2(15,40);

    private void Start()
    {
        for(int i=0;i<startNum;i++)
        {
            Human human = PoolingManager.Instance.getHuman();
            human.transform.position = getSpawnPos(vec);
             ContainerEmploy.instance.addNewHuman(human);
        }   

        StartCoroutine(generateRepeater());
    }

    IEnumerator generateRepeater()
    {
        while(true)
        {
            yield return new WaitForSeconds(3f);
            Camera cam = Camera.main;
            Human human = PoolingManager.Instance.getHuman();
            // human.transform.position = getSpawnPos(PlayerMover.Instance.transform.position);
            human.transform.position = getSpawnPos(vec);
            ContainerEmploy.instance.addNewHuman(human);
        }
    }
}
