namespace Equipment
{
	using System;
	using System.Collections.Generic;

	using UnityEngine;

	[CreateAssetMenu(fileName = "Inventory")]
	public class Inventory : ScriptableObject
	{
		public Weapon CurrentWeapon;

		public List<Weapon> CollectedWeapons = new List<Weapon>();
		
		[NonSerialized]
		public List<Modifier> CollectedModifier = new List<Modifier>();
	}
}
