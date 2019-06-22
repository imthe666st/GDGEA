using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncounterFreeZone : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D other)
	{
		Debug.Log("TEST");
		GameManager.Instance.CanEncounter = false;
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		GameManager.Instance.CanEncounter = true;
	}
}
