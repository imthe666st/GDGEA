namespace Equipment
{
	using System.Collections.Generic;

	using UnityEngine;

	[CreateAssetMenu(fileName = "Inventory")]
	public class Inventory : ScriptableObject
	{
		public Weapon CurrentWeapon;
		public Modifier CurrentModifier1;
		public Modifier CurrentModifier2;

		public List<Weapon> CollectedWeapons;
		public List<Modifier> CollectedModifier;
	}
}
