using Enemy;

using UnityEngine;

public class LevelData : MonoBehaviour
{
	public float encounterChance = 0.1f;
	public EnemyPool EnemyPool;
	public LootPool LootPool;
	
	private void Awake()
	{
		GameManager.Instance.LevelData = this;
	}
}
