using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

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

	public IndieMarc.TopDown.Character currentBuilder;

	public static Color white = Color.white;
	public static Color invis = new Color(1, 1, 1, 0.5f);

	public Collider2D buildCollider;
	public GameObject smoke;
	public Image progressBuild;
	public Image iconState;
	public Sprite finishIcon;

	void OnEnable()
	{
		render = GetComponent<SpriteRenderer>();
		HouseState = EHouseState.Destroyed;
		BuildContainer.Instance.AddNewHouse(this);
		smoke.gameObject.SetActive(false);

		progressBuild.gameObject.SetActive(false);
		iconState.gameObject.SetActive(false);

		SetStartParams();
	}

	private void SetStartParams()
	{
		var parameters = BuildContainer.Instance.GetHouseByType(HouseType);

		HouseType = parameters.HouseType;
		Coins = parameters.RewardCoins;
		RequiredWorkers = parameters.RequiredWorkers;

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
			HouseState = EHouseState.Building;
			ChangeSprite();

			AudioManager.Instance.PlayLoopSound("Building");

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
			render.sprite = BuildContainer.Instance.GetHouseByType(HouseType).DestroyedSprite;
		}

		if (HouseState == EHouseState.Builded)
		{
			render.sprite = BuildContainer.Instance.GetHouseByType(HouseType).BuildedSprite;
			if (grow)
			{
				Vector3 beginScale = transform.localScale;
				transform.localScale = Vector3.zero;

				transform.DOScale(beginScale, 0.2f);
			}
		}

	}

	private void Update()
	{
		if (HouseState == EHouseState.Builded)
		{
			return;
		}

		if (buildProgress >= 100 && HouseState == EHouseState.Destroyed)
		{
			AudioManager.Instance.StopLoopSound("Building");
			AudioManager.Instance.PlaySound("BuildDone");
			progressBuild.gameObject.SetActive(false);
			progressBuild.color = Color.white;
			progressBuild.fillAmount = 0f;

			iconState.gameObject.SetActive(true);
			iconState.sprite = finishIcon;

			HouseState = EHouseState.Builded;
			ChangeSprite(true);
			smoke.gameObject.SetActive(false);

			if (HouseType == EHouseType.Orange)
			{
				GetComponent<Animator>().enabled = true;
			}

			currentBuilder.buildCount++;
			currentBuilder.score += Coins;

			return;
		}

		if (currentBuilder && HouseState == EHouseState.Destroyed && currentBuilder.getEmployeesCount() >= RequiredWorkers)
		{
			if (buildProgress < 100 && Vector2.Distance(transform.position, currentBuilder.transform.position) < 3)
			{
				AudioManager.Instance.PlayLoopSound("Building");
				progressBuild.gameObject.SetActive(true);
				progressBuild.color = currentBuilder.teamColor;
				iconState.gameObject.SetActive(true);

				smoke.gameObject.SetActive(true);
				buildProgress += currentBuilder.getEmployeesCount() * Time.deltaTime;
				progressBuild.fillAmount = buildProgress / 100f;

			}
			else
			{
				AudioManager.Instance.StopLoopSound("Building");
				progressBuild.gameObject.SetActive(false);
				progressBuild.color = Color.white;
				progressBuild.fillAmount = 0f;
				iconState.gameObject.SetActive(false);

				smoke.gameObject.SetActive(false);
				currentBuilder = null;
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
			Debug.Log("build speed " + buildSpeed + "/" + buildProgress + " seconds " + seconds);
			seconds++;

			if (Vector2.Distance(transform.position, currentBuilder.transform.position) > 3)
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