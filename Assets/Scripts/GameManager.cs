using System;

using Battle;

using Camera;

using DefaultNamespace;

using Equipment;

using Marker;

using Player;

using UnityEngine;
using UnityEngine.SceneManagement;

using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance = null;

	public bool isPaused = false;
	public Inventory playerInventory;
	public Difficulty difficulty = Difficulty.Normal;
	public Material MonoChrome;
	public Stats stats;
	
	[HideInInspector]
	public OverworldPlayer OverWorldPlayer;
	[HideInInspector] 
	public BattlePlayer BattlePlayer;
	[HideInInspector]
	public CameraController CameraController;
	[HideInInspector]
	public Battlefield Battlefield;
	[HideInInspector]
	public LevelData LevelData;
	[HideInInspector]
	public BattleState BattleState = BattleState.PlayerToMove;
	[HideInInspector]
	public CursorController Cursor = null;

	[HideInInspector]
	public HealthMarker HealthMarker = null;
	[HideInInspector]
	public WeaponMarker WeaponMarker = null;
	
	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
			this.ChangeShaderValue(ShaderState.None.toFloat());
		}
		else if (Instance != this)
		{
			Destroy(this.gameObject);
			return;
		}

		DontDestroyOnLoad(this.gameObject);
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
			this.StartBattle();
		}
	}

	public void StartBattle()
	{
		this.isPaused = true;
		this.BattleState = BattleState.PlayerToMove;
		
		SceneManager.LoadScene("BattleScene", LoadSceneMode.Additive);
		this.CameraController.cameraMode = CameraMode.Static;
	}

	public void EndBattle()
	{
		Destroy(this.Battlefield.gameObject);
		this.Battlefield = null;

		this.CameraController.cameraMode = CameraMode.FollowPlayer;

		SceneManager.UnloadSceneAsync("BattleScene");

		if (this.LevelData.LootPool.CanGetLoot())
		{
			var loot = this.LevelData.LootPool.GetRandomLoot();
			
			this.playerInventory.CollectedModifier.Add(Instantiate(loot));
		}

		this.isPaused = false;
	}

	public void ChangeShaderValue(float value)
	{
		this.MonoChrome.SetFloat("_Status", value);
	}
}
