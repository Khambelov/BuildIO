using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BuildContainer : MonoBehaviour
{
	public static BuildContainer Instance { get { return instance; } }
	private static BuildContainer instance;

	public Sprite DestroyHouse;
	public Sprite BuildedHouse;
	public GameObject BuildingSmoke;

	private List<House> houses;

	void Awake()
	{
		instance = this;

		houses = new List<House>();
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
