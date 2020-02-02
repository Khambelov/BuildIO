using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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
	public IndieMarc.TopDown.Character currentBuilder;

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
		// buildProgress = 0f;
		ChangeSprite();
		smoke.gameObject.SetActive(false);
	}

	public int GetReward()
	{
		return Coins;
	}

	private void ChangeSprite(bool grow = false)
	{
		if (HouseState == EHouseState.Destroyed)
		{
			render.sprite = BuildContainer.Instance.GetHouseByIndex(houseParamIndex).DestroyedSprite;
		}

		if (HouseState == EHouseState.Builded)
		{
			render.sprite = BuildContainer.Instance.GetHouseByIndex(houseParamIndex).BuildedSprite;
			if(grow)
			{
				Vector3 beginScale = transform.localScale;
				transform.localScale = Vector3.zero;

				transform.DOScale(beginScale,0.2f);
			}
		}

	}

	private void Update() 
	{
		if(HouseState == EHouseState.Builded)
		{
			return;
		}

		if(buildProgress>=100 && HouseState== EHouseState.Destroyed)
		{
			HouseState =EHouseState.Builded;
			ChangeSprite(true);
			smoke.gameObject.SetActive(false);
			return;
		}

		if (currentBuilder && HouseState == EHouseState.Destroyed && currentBuilder.getEmployeesCount() >= RequiredWorkers)
		{
			
			if(buildProgress<100 && Vector2.Distance(transform.position,currentBuilder.transform.position)<3)
			{
				smoke.gameObject.SetActive(true);
				buildProgress += currentBuilder.getEmployeesCount()*Time.deltaTime;
				Debug.Log(buildProgress);
			}else
			{
				smoke.gameObject.SetActive(false);
				currentBuilder=null;
			}

			// owner = team;

			// HouseState = EHouseState.Building;
			// ChangeSprite();

			// StartCoroutine(BuildingHouse(buildSpeed));
		}
	}

	private IEnumerator BuildingHouse(float buildSpeed)
	{
		smoke.gameObject.SetActive(true);

		float seconds = 0;
		while (buildProgress < 100f)
		{
			buildProgress += buildSpeed;

			////////////////////
			Debug.Log("build speed "+buildSpeed  + "/"+buildProgress + " seconds "+seconds);
			seconds++;

			if(Vector2.Distance(transform.position,currentBuilder.transform.position)>3)
			{
				smoke.gameObject.SetActive(false);
				HouseState = EHouseState.Destroyed;
				Debug.Log("finish him");
				yield break;
			}

			yield return new WaitForSeconds(1f);
		}

		if (buildProgress >= 100f)
		{
			buildProgress = 100f;

			HouseState = EHouseState.Builded;
			BuildContainer.Instance.DropBuildedHouse(this);
			ChangeSprite(true);

			// Destroy(smoke);
			buildCollider.gameObject.SetActive(false);
			smoke.gameObject.SetActive(false);

			//Сделать что-то с персонажем, например, флажок покрасить
		}

		yield return null;
	}
}