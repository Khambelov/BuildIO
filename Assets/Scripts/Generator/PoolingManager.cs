using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingManager : MonoBehaviour
{
    private static PoolingManager instance;
    public static PoolingManager Instance {get{return instance;}}

    [Header("Prefabs")]
    public Human prefabHuman;
    public House prefabHouse;

    [Header("Pool parent")]
    public Transform deactivatedPool;

    List<Human> humanPool =new List<Human>();
    List<Human> humanPoolArchive =new List<Human>();

    List<House> housePool = new List<House>();
    List<House> housePoolArchive = new List<House>();

    // List<PoolObject> boosterPool;
    // List<PoolObject> boosterPoolArchive;

    private void Awake() 
    {
        instance = this;    
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    public Human getHuman()
    {
        if(humanPoolArchive.Count>0)
        {
            Human value = humanPoolArchive[0];
            humanPoolArchive.RemoveAt(0);
            humanPool.Add(value);
            
            value.transform.SetParent(null);
            value.gameObject.SetActive(true);
            return value;
        }
        else
        {
            Human value = Instantiate(prefabHuman);
            humanPool.Add(value);
            return value;
        }
    }

    public House getHouse()
    {
        if(housePoolArchive.Count>0)
        {
            House value = housePoolArchive[0];
            housePoolArchive.RemoveAt(0);
            housePool.Add(value);
            
            value.transform.SetParent(null);
            value.gameObject.SetActive(true);
            return value;
        }
        else
        {
            House value = Instantiate(prefabHouse);
            housePool.Add(value);
            return value;
        }
    }

    public void release(Human value)
    {
        humanPool.Remove(value);
        humanPoolArchive.Add(value);
        value.transform.SetParent(deactivatedPool);
        value.gameObject.SetActive(false);
    }

    public void release(House value)
    {
        housePool.Remove(value);
        housePoolArchive.Add(value);
        value.transform.SetParent(deactivatedPool);
        value.gameObject.SetActive(false);
    }
}
    