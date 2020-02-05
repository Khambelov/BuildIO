using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerEmploy : MonoBehaviour
{
    public static ContainerEmploy instance;
    public List<Human> emptyHumans= new List<Human>();

    private void Awake() {
        instance = this;
    }

    public IndieMarc.TopDown.CharacterEmployee getEmploy()
    {
        return PoolingManager.Instance.getEmploy();
    }

    public void addNewHuman(Human human)
    {
        emptyHumans.Add(human);
    }

    public void removeHuman(Human human)
    {
        emptyHumans.Remove(human);
    }
}
