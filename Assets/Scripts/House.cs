using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour
{
	public EHouseState HouseState;
	public EHouseType HouseType;
	public bool BuildIsCancelled { get; }

	private SpriteRenderer render;
	private int houseParamIndex;

	private int coins;
	private int requiredWorkers;
	private float buildProgress;

	private GameObject owner;

	void Start()
	{
		HouseState = EHouseState.Destroyed;
		BuildContainer.Instance.AddNewHouse(this);
		houseParamIndex = BuildContainer.Instance.GetRandomHouseIndex();

		SetStartParams();
	}

	private void SetStartParams()
	{
		var parameters = BuildContainer.Instance.GetHouseByIndex(houseParamIndex);

		HouseType = parameters.HouseType;
		coins = parameters.RewardCoins;
		requiredWorkers = parameters.RequiredWorkers;

		owner = null;
		buildProgress = 0f;

		ChangeSprite();
	}

	public void StartBuilding(float buildSpeed, int countWorkers, GameObject team)
	{
		if (HouseState == EHouseState.Destroyed && countWorkers >= requiredWorkers)
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
			render.sprite = BuildContainer.Instance.GetHouseByIndex(houseParamIndex).DestroyedSprite;
		if (HouseState == EHouseState.Builded)
			render.sprite = BuildContainer.Instance.GetHouseByIndex(houseParamIndex).BuildedSprite;

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