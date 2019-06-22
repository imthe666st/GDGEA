﻿using Enemy;

using UnityEngine;

public class LevelData : MonoBehaviour
{
	public float encounterChance = 0.1f;
	public EnemyPool EnemyPool;
	
	private void Awake()
	{
		GameManager.Instance.LevelData = this;
	}
}
