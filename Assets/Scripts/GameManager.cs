using Player;

using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance = null;

	public bool isPaused = false;
	public OverworldPlayer OverWorldPlayer;

	protected LevelData LevelData;

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
		
		SceneManager.sceneLoaded += this.SceneManagerOnSceneLoaded;
	}

	private void SceneManagerOnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
	{
		if (loadSceneMode == LoadSceneMode.Single && scene.name.Contains("Level"))
		{
			this.LevelData = FindObjectOfType<LevelData>();
		}
	}


	public void OverworldPlayerMoved()
	{
		if (this.LevelData == null)
		{
			this.LevelData = FindObjectOfType<LevelData>();
		}
		
		if (Random.Range(0f, 1f) <= this.LevelData.encounterChance)
		{
			Debug.Log("RANDOM ENCOUNTER!!!!");
		}
	}
}
