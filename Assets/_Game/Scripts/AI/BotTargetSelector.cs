using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BotTargetSelector : MonoBehaviour
{
    public static BotTargetSelector Instance;
    public float maxDist = 10;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
       
    }

    IndieMarc.TopDown.Character player;

    public Transform findTarget(IndieMarc.TopDown.Character botChar,Transform lastTarget,out bool humanMode)
    {
        if(player==null)
            player= PlayerMover.Instance.GetComponent<IndieMarc.TopDown.Character>();

        humanMode = false;
        List<Transform> finals = new List<Transform>();
         Vector2 playerPos = PlayerMover.Instance.transform.position;

        if(botChar.getEmployeesCount()>15){
            if(botChar.getEmployeesCount()>player.getEmployeesCount())
            {
                finals.Add(PlayerMover.Instance.transform);
            }

            List<House> founded = BuildContainer.Instance.houses.OrderBy(h => Vector2.Distance(playerPos, h.transform.position)).ToList();
            int len = founded.Count;
            // len = Mathf.Clamp(len,0,40);
            for(int i=0;i<len;i++)
            {
                if(!founded[i].isBuilded())
                    finals.Add(founded[i].transform);
            }
            
            
                // finals.Clear();
           
        }

        List<Human> founded2 = ContainerEmploy.instance.emptyHumans.OrderBy(h => Vector2.Distance(playerPos, h.transform.position)).ToList();
        List<Human> founded3 = new List<Human>();
        foreach(Human human in founded2)
        {
            if(!human.used && !human.busy && human.gameObject.activeSelf)
            {
                founded3.Add(human);
            }
        }
        int len2 = founded3.Count;
        len2 = Mathf.Clamp(len2,0,40);
        for(int i=0;i<len2;i++)
        {
            finals.Add(founded3[i].transform);
        }

        finals.Remove(lastTarget);

        if(finals.Count>0){
            Transform target = finals[Random.Range(0,finals.Count)];
            Human testHuman = target.GetComponent<Human>();
            if(testHuman)
            {
                testHuman.busy = true;
                 humanMode = true;
            }

            return target;
        }

        return null;
    }

    public Transform findHuman(IndieMarc.TopDown.Character botChar)
    {
         List<Human> founded = ContainerEmploy.instance.emptyHumans.OrderBy(h => Vector2.Distance(botChar.transform.position, h.transform.position)).ToList();
         return founded.First().transform;
    }

    public bool canFindHumans()
    {
        return ContainerEmploy.instance.emptyHumans.Count>0;
    }
}
