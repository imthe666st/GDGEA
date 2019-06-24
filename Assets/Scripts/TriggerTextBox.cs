using System;

using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerTextBox : MonoBehaviour
{
    public string[] Input;

    private int access = 0;

	private void Start()
	{
		if (SceneManager.GetActiveScene().name.Contains("1"))
		{
			if (GameManager.Instance.TextRead1)
			{
				Destroy(this.gameObject);
			}
		}
		
		if (SceneManager.GetActiveScene().name.Contains("2"))
		{
			if(GameManager.Instance.TextRead2)
				Destroy(this.gameObject);
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
    {
        this.NextBox();
	}

	private void NextBox()
	{
		if (this.access >= this.Input.Length)
		{
			Destroy(this.gameObject);
			
			if (SceneManager.GetActiveScene().name.Contains("1"))
			{
				GameManager.Instance.TextRead1 = true;
			}
		
			if (SceneManager.GetActiveScene().name.Contains("2"))
			{
				GameManager.Instance.TextRead2 = true;
			}
			
			return;
		}
		
		GameManager.Instance.CameraController.SpawnTextBox(this.Input[this.access++]).OnClosed(() =>
																							   {
                                                                                                 this.NextBox();
																							   });
	}
}
