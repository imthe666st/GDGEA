using System;

using DefaultNamespace;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

using Cursor = UnityEngine.Cursor;

public class SceneChanger : MonoBehaviour
{
	private void Awake()
	{
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	public GameObject toSelect;
	
	private void Update()
	{
		if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
		{
			EventSystem.current.SetSelectedGameObject(this.toSelect);
		}
	}

	public void ChangeScene(string scene)
	{
		SceneManager.LoadScene(scene, LoadSceneMode.Single);
	}

	public void Exit()
	{
		Application.Quit();
	}
}
