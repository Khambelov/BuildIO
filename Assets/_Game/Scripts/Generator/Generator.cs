using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{

    public int startNum = 10;
    public int radiusSpawn = 10;
    public int radiusCheck = 8;
    public LayerMask checkLayers;

    protected Vector2 getSpawnPos(Vector2 centralPosition)
    {
        float posX = Random.Range(-radiusSpawn, radiusSpawn);
        float posZ = Random.Range(-radiusSpawn, radiusSpawn);

        Vector2 pos = new Vector2(posX, posZ) + centralPosition;

        while (!isCanSpawn(pos,radiusCheck))
        {
            posX = Random.Range(-radiusSpawn, radiusSpawn);
            posZ = Random.Range(-radiusSpawn, radiusSpawn);
            pos = new Vector2(posX, posZ) + centralPosition;
        }

        return pos;
    }


    public bool isCanSpawn(Vector2 pos,float raduisCheck)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(pos, radiusCheck, checkLayers);
        return colliders.Length == 0;
    }
}
