using System;
using System.Collections.Generic;
using System.Linq;

using Battle;

using Camera;

using DefaultNamespace;

using DG.Tweening;

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

	public bool CountingTime = false;

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
	private bool BeginingStage1;
	private bool BeginigStage2;

	#endregion

	public float timePlayed = 0;
	public int FightsEncountered = 0;
	public int EnemiesKilled = 0;

	[HideInInspector] 
	public bool GateOpen = false;
	[HideInInspector]
	public bool TextRead1 = false;
	[HideInInspector]
	public bool TextRead2 = false;

	public AudioSource BackgroundSource;
	public AudioSource BattleSource;
	public float FadeTime = 1;

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
			this.ShaderState = ShaderState.None;
			
			this.BackgroundSource.Play();
			this.BattleSource.volume = 0;
			this.BattleSource.Play();
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

		this.FightsEncountered++;
		
		this.FadeIntoBattle();
		
		SceneManager.LoadScene("BattleScene", LoadSceneMode.Additive);
		this.CameraController.cameraMode = CameraMode.Static;
	}

	public void EndBattle()
	{
		Destroy(this.Battlefield.gameObject);
		this.Battlefield = null;

		this.CameraController.cameraMode = CameraMode.FollowPlayer;

		this.FadeOutOfBattle();
		
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
		this.BeginingStage1 = this.TextRead1;
		this.BeginigStage2 = this.TextRead2;
	}

	private void Reload(Scene s, LoadSceneMode m)
	{
		this.playerInventory.CollectedModifier = this.savedModifier;
		this.OverWorldPlayer.transform.position = this.savedPosition;
		this.GateOpen = this.gateOpen;
		this.TextRead1 = this.BeginingStage1;
		this.TextRead2 = this.BeginigStage2;
		
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

	public bool SaveExists()
	{
		return !this.firstSave;
	}

	public void StartGame()
	{
		this.timePlayed = 0;
		this.FightsEncountered = 0;
		this.EnemiesKilled = 0;
		this.CountingTime = true;
	}

	private void FadeIntoBattle()
	{
		this.BackgroundSource.DOFade(0.3f, this.FadeTime);
		this.BattleSource.DOFade(0.9f, this.FadeTime);
	}

	public void FadeOutOfBattle()
	{
		this.BackgroundSource.DOFade(0.9f, this.FadeTime);
		this.BattleSource.DOFade(0, this.FadeTime);
	}

	public void Update()
	{
		if (this.CountingTime)
		{
			this.timePlayed += Time.deltaTime;
		}
	}

	public void ToggleWeapons()
	{
		var current = this.playerInventory.CurrentWeapon;

		var other = this.playerInventory.CollectedWeapons.First(w => w != current);

		this.playerInventory.CurrentWeapon = other;
		
		this.WeaponMarker.Set(other.Representation);
	}
}
