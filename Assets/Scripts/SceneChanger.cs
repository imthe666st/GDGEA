using System;

using DefaultNamespace;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class SceneChanger : MonoBehaviour
{
	public void ChangeScene(string scene)
	{
		SceneManager.LoadScene(scene, LoadSceneMode.Single);
	}

	public void Exit()
	{
		Application.Quit();
	}
}
