using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance = null;

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
