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
	private HouseSpriteCollection[] housesSpriteCollection;

	private List<House> houses;

	void Awake()
	{
		instance = this;

		houses = new List<House>();
	}

	public int GetRandomHouseSpriteIndex()
	{
		return Random.Range(0, housesSpriteCollection.Length);
	}

	public HouseSpriteCollection GetRandomHouseSprite()
	{
		return housesSpriteCollection[Random.Range(0, housesSpriteCollection.Length)];
	}

	public HouseSpriteCollection GetHouseSpriteByIndex(int index)
	{
		return housesSpriteCollection[index];
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
