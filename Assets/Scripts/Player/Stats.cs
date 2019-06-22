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
				if (inventory.CurrentModifier1 != null) damage += inventory.CurrentModifier1.Damage;
				if (inventory.CurrentModifier2 != null) damage += inventory.CurrentModifier2.Damage;

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
				if (inventory.CurrentModifier1 != null) movement += inventory.CurrentModifier1.Movement;
				if (inventory.CurrentModifier2 != null) movement += inventory.CurrentModifier2.Movement;

                
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
				if (inventory.CurrentModifier1 != null) minDistance += inventory.CurrentModifier1.MinDistance;
				if (inventory.CurrentModifier2 != null) minDistance += inventory.CurrentModifier2.MinDistance;

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
				if (inventory.CurrentModifier1 != null) maxDistance += inventory.CurrentModifier1.MaxDistance;
				if (inventory.CurrentModifier2 != null) maxDistance += inventory.CurrentModifier2.MaxDistance;

				return maxDistance;
			}
		}
		
		public float CritDamage
		{
			get
			{
				var inventory = GameManager.Instance.playerInventory;

				var critDamage = 2f;

				if (inventory.CurrentModifier1 != null) critDamage += inventory.CurrentModifier1.CritDamage;
				if (inventory.CurrentModifier2 != null) critDamage += inventory.CurrentModifier2.CritDamage;

				return critDamage;
			}
		}
		
		public float CritChance
		{
			get
			{
				var inventory = GameManager.Instance.playerInventory;

				var critChance = 0f;
                
				if (inventory.CurrentModifier1 != null) critChance += inventory.CurrentModifier1.CritChance;
				if (inventory.CurrentModifier2 != null) critChance += inventory.CurrentModifier2.CritChance;

				return critChance;
			}
		}
	}
}
