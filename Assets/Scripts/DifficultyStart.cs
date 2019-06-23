using System;
using System.Collections;
using System.Collections.Generic;

using DefaultNamespace;

using UnityEngine;
using UnityEngine.SceneManagement;

public class DifficultyStart : MonoBehaviour
{
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.F9))
		{
			GameManager.Instance.difficulty = Difficulty.Cheat;
			this.StartGame();
		}
	}

	public void StartEasy()
	{
		GameManager.Instance.difficulty = Difficulty.Easy;
		this.StartGame();
	}
	
	public void StartMedium()
	{
		GameManager.Instance.difficulty = Difficulty.Normal;
		this.StartGame();
	}
	
	public void StartHard()
	{
		GameManager.Instance.difficulty = Difficulty.Hard;
		this.StartGame();
	}

	private void StartGame()
	{
		GameManager.Instance.StartGame();
		SceneManager.LoadScene("Level1", LoadSceneMode.Single);
	}
}
