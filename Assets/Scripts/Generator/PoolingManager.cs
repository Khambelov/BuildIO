using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingManager : MonoBehaviour
{
    private static PoolingManager instance;
    public static PoolingManager Instance {get{return instance;}}

    [Header("Prefabs")]
    public Human prefabHuman;
    public IndieMarc.TopDown.CharacterEmployee prefabEmpl;

    [Header("Pool parent")]
    public Transform deactivatedPool;

    List<Human> humanPool =new List<Human>();
    List<Human> humanPoolArchive =new List<Human>();

    List<IndieMarc.TopDown.CharacterEmployee> emplPool = new List<IndieMarc.TopDown.CharacterEmployee>();
    List<IndieMarc.TopDown.CharacterEmployee> emplPoolArchive = new List<IndieMarc.TopDown.CharacterEmployee>();

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

    public IndieMarc.TopDown.CharacterEmployee getEmploy()
    {
        if(emplPoolArchive.Count>0)
        {
            IndieMarc.TopDown.CharacterEmployee value = emplPoolArchive[0];
            emplPoolArchive.RemoveAt(0);
            emplPool.Add(value);
            
            value.transform.SetParent(null);
            value.gameObject.SetActive(true);
            return value;
        }
        else
        {
            IndieMarc.TopDown.CharacterEmployee value = Instantiate(prefabEmpl);
            emplPool.Add(value);
            return value;
        }
    }

    public void release(Human value)
    {
        value.busy = false;
        value.used = false;
        humanPool.Remove(value);
        humanPoolArchive.Add(value);
        value.transform.SetParent(deactivatedPool);
        value.gameObject.SetActive(false);
        ContainerEmploy.instance.removeHuman(value);
    }

    public void release(IndieMarc.TopDown.CharacterEmployee value)
    {
        emplPool.Remove(value);
        emplPoolArchive.Add(value);
        value.transform.SetParent(deactivatedPool);
        value.gameObject.SetActive(false);
    }
}
    