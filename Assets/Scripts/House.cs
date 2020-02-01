using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour
{
	public EHouseState HouseState;
	public bool BuildIsCancelled { get; }

	private SpriteRenderer render;

	private int coins;
	private float buildProgress;

	private GameObject owner;

	void Start()
	{
		HouseState = EHouseState.Destroyed;
		BuildContainer.Instance.AddNewHouse(this);
		ChangeSprite();
		owner = null;
		buildProgress = 0f;
		coins = 1000;
	}

	public void StartBuilding(float buildSpeed, GameObject team)
	{
		if (HouseState == EHouseState.Destroyed)
		{
			owner = team;

			HouseState = EHouseState.Building;
			ChangeSprite();

			StartCoroutine(BuildingHouse(buildSpeed));
		}
	}

	public void CancelBuildings()
	{
		StopAllCoroutines();

		HouseState = EHouseState.Destroyed;
		buildProgress = 0f;
		ChangeSprite();
	}

	public int GetReward()
	{
		return coins;
	}

	private void ChangeSprite()
	{
		if (HouseState == EHouseState.Destroyed)
			render.sprite = BuildContainer.Instance.DestroyHouse;
		if (HouseState == EHouseState.Builded)
			render.sprite = BuildContainer.Instance.BuildedHouse;

	}

	private IEnumerator BuildingHouse(float buildSpeed)
	{
		var smoke = Instantiate(BuildContainer.Instance.BuildingSmoke, Vector3.zero, Quaternion.identity, transform);

		while (buildProgress < 100f)
		{
			buildProgress += buildSpeed;

			yield return new WaitForSeconds(1f);
		}

		if (buildProgress >= 100f)
		{
			buildProgress = 100f;

			HouseState = EHouseState.Builded;
			BuildContainer.Instance.DropBuildedHouse(this);
			ChangeSprite();

			Destroy(smoke);

			//Сделать что-то с персонажем, например, флажок покрасить
		}

		yield return null;
	}
}