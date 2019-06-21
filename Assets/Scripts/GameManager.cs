using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance = null;

	public bool isPaused = false;
	
	private void Awake()
	{
		if (Instance == null)
			Instance = this;
		else if (Instance != this)
		{
			Destroy(this.gameObject);
			return;
		}

		DontDestroyOnLoad(this.gameObject);
	}
}
