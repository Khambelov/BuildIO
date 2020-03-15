using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseSpawnerV2 : MonoBehaviour
{
    public HouseV2[] housePrefabs;
    public Transform mainField;

    [Header("params")]
    public float buildedPercentOnStart = 10;
    public Vector3 scale = Vector3.one;


    List<HouseV2> spawned = new List<HouseV2>();

    private void Start() 
    {
        spawn(generateStates());
    }

    List<int> generateStates()
    {
        int spawnNum = mainField.childCount;
        int buildedNum =(int)(spawnNum*(buildedPercentOnStart/100));

        List<int> stateNums = new List<int>();
        for(int i=0;i<spawnNum;i++)
        {
            stateNums.Add(i);
        }

        for(int i=0;i<buildedNum;i++)
        {
            stateNums.RemoveAt(Random.Range(0,stateNums.Count));
        }

        return stateNums;
    }

    void spawn(List<int> destroyedNums)
    {
        int spawnNum = mainField.childCount;
        for(int i=0;i<spawnNum;i++)
        {
            HouseV2 house = Instantiate(
                housePrefabs[Random.Range(0,housePrefabs.Length)],
                mainField.GetChild(i).transform.position,
                mainField.GetChild(i).transform.rotation
                );

            house.resetPos();
            house.currentState = destroyedNums.Contains(i) ? EHouseState.Destroyed : EHouseState.Builded;
            house.draw();
            house.transform.localScale = scale;

            mainField.GetChild(i).gameObject.SetActive(false);
            spawned.Add(house);
        }

        foreach(HouseV2 house in spawned)
        {
            house.transform.SetParent(mainField);
        }
    }
}
