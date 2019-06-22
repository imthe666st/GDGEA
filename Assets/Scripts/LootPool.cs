using System;
using System.Linq;

using Equipment;

using UnityEngine;

using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "LootPool")]
public class LootPool : ScriptableObject
{
	public LootData[] Loot;
	public float GlobalLootChance = 0.4f;

	public bool CanGetLoot()
	{
		return Random.Range(0, 1f) < this.GlobalLootChance;
	}

	public Modifier GetRandomLoot()
	{
		Debug.Assert(this.Loot != null);
		Debug.Assert(this.Loot.Length > 0);
		
		var sum = this.Loot.Sum(data => data.Chance);

		var value = Random.Range(0, sum);

		var sm = 0;

		foreach (var data in this.Loot)
		{
			sm += data.Chance;

			if (value <= sm)
			{
				return data.Loot;
			}
		}

		return null;
	}
}

[Serializable]
public class LootData
{
	[SerializeField]
	public Modifier Loot;
	public int Chance;
}
