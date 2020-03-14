using UnityEngine;
using System;

[Serializable]
public struct HouseParams
{
	public EHouseType HouseType;
	public int RewardCoins;
	public int RequiredWorkers;
	public Sprite DestroyedSprite;
	public Sprite BuildedSprite;
}
