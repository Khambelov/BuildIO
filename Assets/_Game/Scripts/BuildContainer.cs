using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BuildContainer : MonoBehaviour
{
	public static BuildContainer Instance { get { return instance; } }
	private static BuildContainer instance;

	public GameObject BuildingSmoke;

	[SerializeField]
	private HouseParams[] housesCollection;

	public List<House> houses;

	void Awake()
	{
		instance = this;

		houses = new List<House>();
	}

	public int GetRandomHouseIndex()
	{
		return Random.Range(0, housesCollection.Length);
	}

	public HouseParams GetRandomHouse()
	{
		return housesCollection[Random.Range(0, housesCollection.Length)];
	}

	public HouseParams GetHouseByIndex(int index)
	{
		return housesCollection[index];
	}

	public HouseParams GetHouseByType(EHouseType type)
	{
		return housesCollection.FirstOrDefault(h => h.HouseType == type);
	}

	public void AddNewHouse(House house)
	{
		houses.Add(house);
	}

	public House FindNearestHouse(Vector3 position)
	{
		return houses.OrderBy(h => Vector3.Distance(position, h.transform.position)).FirstOrDefault();
	}

	public void DropBuildedHouse(House house)
	{
		houses.Remove(house);
	}
}
