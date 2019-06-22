using UnityEngine;

namespace Battle {
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Camera;

    using DefaultNamespace;

    using Enemy;

    using Player;

    using Random = UnityEngine.Random;

    public class Battlefield : MonoBehaviour
    {
        public FieldTile FieldTilePrefab;
        public StaticCameraMarker CameraMarkerPrefab;
        public BattlePlayer BattlePlayerPrefab;
        public CursorController CursorPrefab;

        public int fieldWidth = 8;
        public int fieldHeight = 8;
        public int enemySpawnWidth = 2;
        public int playerSpawnWidth = 2;
        
        public FieldTile[] Tiles;

        protected List<FieldTile> EnemySpawnTiles;

        [HideInInspector]
        public List<Enemy> Enemies;

        public List<FieldTile> PlayerWalkable;
        
        private void Awake()
        {
            GameManager.Instance.Battlefield = this;

            this.Enemies = new List<Enemy>();
            this.Tiles = new FieldTile[this.fieldWidth * this.fieldHeight];
            
            this.GenerateField();
            foreach (var tile in this.Tiles)
            {
                tile.Reset();
            }
            
            this.EnemySpawnTiles = this.Tiles.Skip((this.fieldWidth - this.enemySpawnWidth) * this.fieldHeight).ToList();
            
            this.SpawnPlayer();
            this.SpawnEnemies();
            
            this.UpdateBattle();
        }

        private void GenerateField()
        {
            //Tiles
            var counter = 0;
            for (var i = 0; i < this.fieldWidth; ++i)
            {
                for (var j = 0; j < this.fieldHeight; ++j, ++counter)
                {
                    this.Tiles[counter] = Instantiate(this.FieldTilePrefab, this.transform);
                    this.Tiles[counter].transform.localPosition = new Vector3(i, j);
                }
            }
            
            //Connect them up!

            //right-left
            for (var i = 0; i < this.fieldWidth * (this.fieldHeight - 1); ++i)
            {
                this.Tiles[i].SetNeighbor(Direction.Right, this.Tiles[i + this.fieldWidth]);
            }

            for (var i = 0; i < this.fieldWidth * this.fieldHeight; ++i)
            {
                if ((i + 1) % this.fieldHeight == 0)
                    continue;

                this.Tiles[i].SetNeighbor(Direction.Up, this.Tiles[i + 1]);
            }

            //Camera Marker
            Instantiate(this.CameraMarkerPrefab, new Vector3((this.fieldWidth - 1) / 2f, (this.fieldHeight - 1) / 2f, -10), Quaternion.identity,
                        this.transform);
        }

        private void SpawnEnemies()
        {
            GameManager.Instance.LevelData.EnemyPool.GetRandom().Spawn(this);
            
            this.UpdateTilesEnemyAttack();
        }

        private void SpawnPlayer()
        {
            var value = Random.Range(0, this.fieldHeight * this.playerSpawnWidth);
            var pos = this.Tiles[value].transform.position - new Vector3(0, 0, 0.1f);

            Instantiate(this.BattlePlayerPrefab, pos, Quaternion.identity, this.transform);
            
            this.UpdateTilesPlayerWalk();
        }
        
        public bool CanSpawnEnemy() => this.EnemySpawnTiles.Count > 0;

        public Vector3 GetNewEnemyLocation()
        {
            var value = Random.Range(0, this.EnemySpawnTiles.Count);

            var tile = this.EnemySpawnTiles[value];
            this.EnemySpawnTiles.RemoveAt(value);
            
            return tile.transform.position - new Vector3(0, 0, 0.1f);
        }

        public void RegisterEnemy(Enemy enemy)
        {
            this.Enemies.Add(enemy);
        }

        public void UpdateTilesPlayerWalk()
        {
            foreach (var tile in this.Tiles) tile.Reset();
            
            var player = GameManager.Instance.BattlePlayer;
            var playerPos = player.transform.position;
            
            var walkableTiles = new List<FieldTile>();
            this.Tiles[this.fieldHeight * (int)playerPos.x + (int)playerPos.y].PlayerWalk(player.Movement + 1, 
            walkableTiles, new List<FieldTile>());

            this.PlayerWalkable = walkableTiles;
        }

        public void UpdateTilesEnemyAttack()
        {
            foreach (var enemy in this.Enemies)
            {
                var enemyPos = enemy.transform.position;

                var attackAbleTiles = new List<FieldTile>();
                this.Tiles[this.fieldHeight * (int) enemyPos.x + (int) enemyPos.y].EnemyAttack(enemy.MinDistance
                                                                                               + 2,
                                                                                               enemy.MaxDistance +
                                                                                               2, 
                                                                                               GameManager.Instance
                                                                                               .difficulty == Difficulty.Easy,
                                                                                               attackAbleTiles, new List<FieldTile>());
            }
        }

        public void UpdateBattle()
        {
            switch (GameManager.Instance.BattleState)
            {
                case BattleState.PlayerToMove:
                    if (GameManager.Instance.Cursor == null)
                    {
                        GameManager.Instance.Cursor = Instantiate(this.CursorPrefab, GameManager.Instance
                                                                                                .BattlePlayer.transform
                                                                                                .position -
                                                                                     new Vector3(0, 0, 0.1f),
                                                                  Quaternion.identity, this.transform);
                    }
                    GameManager.Instance.Cursor.Moveable = true;
                    GameManager.Instance.Cursor.PlayerMovable = this.PlayerWalkable;
                    
                    break;
                case BattleState.PlayerMoving:
                    break;
                case BattleState.PlayerToAttack:
                    break;
                case BattleState.PlayerAttacking:
                    break;
                case BattleState.EnemiesMoving:
                    break;
                case BattleState.EnemiesAttacking:
                    break;
                case BattleState.EndAnimation:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
