using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour
{
	public EHouseState HouseState;
	public EHouseType HouseType;
	public int Coins;
	public int RequiredWorkers;

	public bool BuildIsCancelled { get; }

	private SpriteRenderer render;
	public int houseParamIndex;

	private float buildProgress;

	private GameObject owner;

	public static Color white = Color.white;
	public static Color invis = new Color(1, 1, 1, 0.5f);

	public Collider2D buildCollider;
	public GameObject smoke;

	void Start()
	{
		render = GetComponent<SpriteRenderer>();
		HouseState = EHouseState.Destroyed;
		BuildContainer.Instance.AddNewHouse(this);
		smoke.gameObject.SetActive(false);

		SetStartParams();
	}

	private void SetStartParams()
	{
		var parameters = BuildContainer.Instance.GetHouseByIndex(houseParamIndex);

		HouseType = parameters.HouseType;
		Coins = parameters.RewardCoins;
		RequiredWorkers = parameters.RequiredWorkers;

		owner = null;
		buildProgress = 0f;

		ChangeSprite();
	}

	// private void OnTriggerEnter2D(Collider2D other)
	// {
	// 	Debug.Log(other.gameObject);
	// 	if (other.gameObject.GetComponent<IndieMarc.TopDown.Character>() != null)
	// 	{
	// 		render.color = invis;
	// 	}
	// }

	// private void OnTriggerExit2D(Collider2D other)
	// {
	// 	if (other.gameObject.GetComponent<IndieMarc.TopDown.Character>() != null)
	// 	{
	// 		render.color = white;
	// 	}
	// }

	public void StartBuilding(float buildSpeed, int countWorkers, GameObject team)
	{
		if (HouseState == EHouseState.Destroyed && countWorkers >= RequiredWorkers)
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
		return Coins;
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
		// var smoke = Instantiate(BuildContainer.Instance.BuildingSmoke, Vector3.zero, Quaternion.identity, transform);
		// smoke.transform.localPosition = Vector3.zero;
		smoke.gameObject.SetActive(true);
		while (buildProgress < 100f)
		{
			// buildProgress += buildSpeed;
			buildProgress += 20;

			yield return new WaitForSeconds(1f);
		}

		if (buildProgress >= 100f)
		{
			buildProgress = 100f;

			HouseState = EHouseState.Builded;
			BuildContainer.Instance.DropBuildedHouse(this);
			ChangeSprite();

			// Destroy(smoke);
			buildCollider.gameObject.SetActive(false);
			smoke.gameObject.SetActive(false);

			//Сделать что-то с персонажем, например, флажок покрасить
		}

		yield return null;
	}
}