namespace Player
{
	using System;

	using DefaultNamespace;

	using UnityEngine;

	[CreateAssetMenu(fileName = "Stats")]
	public class Stats : ScriptableObject
	{
		public int BaseMovement = 2;
		public int BaseEasyHealth   = 15;
		public int BaseMediumHealth = 10;
		public int BaseHardHealth   = 5;
		
		public int BaseHealth
		{
			get
			{
				switch (GameManager.Instance.difficulty)
				{
					case Difficulty.Easy:
						return this.BaseEasyHealth;
					case Difficulty.Normal:
						return this.BaseMediumHealth;
					case Difficulty.Hard:
						return this.BaseHardHealth;
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
			}
		}

		public int Damage
		{
			get
			{
				var inventory = GameManager.Instance.playerInventory;

				var damage                                     = 0;
				if (inventory.CurrentWeapon != null) damage    += inventory.CurrentWeapon.Damage;

				foreach (var modifier in inventory.CollectedModifier) damage += modifier.Damage;

				return damage;
			}
		}
		
		public int Movement
		{
			get
			{
				var inventory = GameManager.Instance.playerInventory;

				var movement                                     = this.BaseMovement;
				if (inventory.CurrentWeapon != null) movement    += inventory.CurrentWeapon.Movement;

				foreach (var modifier in inventory.CollectedModifier) movement += modifier.Movement;


				return movement;
			}
		}
		
		public int MinDistance
		{
			get
			{
				var inventory = GameManager.Instance.playerInventory;

				var minDistance = 0;

				if (inventory.CurrentWeapon != null) minDistance    += inventory.CurrentWeapon.MinDistance;

				foreach (var modifier in inventory.CollectedModifier) minDistance += modifier.MinDistance;

				return minDistance;
			}
		}
		
		public int MaxDistance
		{
			get
			{
				var inventory = GameManager.Instance.playerInventory;

				var maxDistance = 0;
                
				if (inventory.CurrentWeapon != null) maxDistance    += inventory.CurrentWeapon.MaxDistance;

				foreach (var modifier in inventory.CollectedModifier) maxDistance += modifier.MaxDistance;

				return maxDistance;
			}
		}
		
		public float CritDamage
		{
			get
			{
				var inventory = GameManager.Instance.playerInventory;

				var critDamage = 2f;

				foreach (var modifier in inventory.CollectedModifier) critDamage += modifier.CritDamage;

				return critDamage;
			}
		}
		
		public float CritChance
		{
			get
			{
				var inventory = GameManager.Instance.playerInventory;

				var critChance = 0f;

				foreach (var modifier in inventory.CollectedModifier) critChance += modifier.CritChance;

				return critChance;
			}
		}
	}
}
