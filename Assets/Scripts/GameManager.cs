using System;
using System.Collections.Generic;

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

	public bool CanEncounter = true;
	
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

	[HideInInspector]
	public bool HasPredifinedEnemy = false;
	[HideInInspector]
	public Enemy.Enemy PredefinedEnemy = null;
	[HideInInspector]
	public Action OnPredifinedEnemyDone;
	[HideInInspector]
	public int PredefinedEnemyCount = 1;
	[HideInInspector]
	public bool PredefinedEnemyNoLoot = false;

	private ShaderState _shaderState;
	public ShaderState ShaderState
	{
		get => this._shaderState;
		set
		{
			this._shaderState = value;
			this.ChangeShaderValue(value.toFloat());
		}
	}

	#region savefile
	
	private List<Modifier> savedModifier;
	private Vector3        savedPosition;
	private bool gateOpen;
	private string savedScene;
	private bool firstSave = true;

	#endregion

	[HideInInspector] 
	public bool GateOpen = false;

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
			this.ShaderState = ShaderState.None;
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

		if (!this.CanEncounter)
			return;

		if (Random.Range(0f, 1f) <= this.LevelData.encounterChance)
		{
			Debug.Log("RANDOM ENCOUNTER!!!!");
			this.StartBattle();
		}
	}

	public void StartPredefinedBattle(Enemy.Enemy enemy, Action onDone = null, int count = 1, bool noLoot = false)
	{
		this.HasPredifinedEnemy = true;
		this.PredefinedEnemy = enemy;
		this.OnPredifinedEnemyDone = onDone;
		this.PredefinedEnemyCount = count;
		this.PredefinedEnemyNoLoot = noLoot;
		
		this.StartBattle();
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

		if (!this.HasPredifinedEnemy || !this.PredefinedEnemyNoLoot)
		{
			if (this.LevelData.LootPool.CanGetLoot())
			{
				var loot = this.LevelData.LootPool.GetRandomLoot();
			
				this.playerInventory.CollectedModifier.Add(Instantiate(loot));

				this.CameraController.SpawnTextBox("You've found something: " + loot.Description);
			}

		}
		this.isPaused = false;

		if (this.HasPredifinedEnemy)
		{
			this.HasPredifinedEnemy = false;
			this.PredefinedEnemy = null;
			this.PredefinedEnemyCount = 1;
			this.PredefinedEnemyNoLoot = false;
			this.OnPredifinedEnemyDone?.Invoke();
		}
	}

	public void ChangeShaderValue(float value)
	{
		this.MonoChrome.SetFloat("_Status", value);
	}

	public void Save()
	{
		this.savedModifier = new List<Modifier>(this.playerInventory.CollectedModifier);
		this.savedPosition = this.OverWorldPlayer.transform.position;
		this.gateOpen = this.GateOpen;
		this.savedScene = SceneManager.GetActiveScene().name;
		this.firstSave = false;
	}

	private void Reload(Scene s, LoadSceneMode m)
	{
		this.playerInventory.CollectedModifier = this.savedModifier;
		this.OverWorldPlayer.transform.position = this.savedPosition;
		this.GateOpen = this.gateOpen;
		SceneManager.sceneLoaded -= this.Reload;
	}
	
	public void Load()
	{
		if (this.firstSave)
		{
			return;
		}
		SceneManager.sceneLoaded += this.Reload;
		SceneManager.LoadScene(this.savedScene, LoadSceneMode.Single);
	}
}
