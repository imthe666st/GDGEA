using UnityEngine;

namespace Battle {
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Camera;

    using DefaultNamespace;

    using Enemy;

    using Player;

    using Random = Random;

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
        public List<FieldTile> PlayerAttackable;
        public Queue<Enemy> EnemiesToMove = new Queue<Enemy>();
        public bool EnemyCanMove = false;
        public int EnemyMovedCounter = 0;
        
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

            var player = Instantiate(this.BattlePlayerPrefab, pos, Quaternion.identity, this.transform);
            player.PositionTile = this.Tiles[value];
            
            //this.UpdateTilesPlayerWalk();
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

        public void UpdateTilesPlayerAttack()
        {
            foreach (var tile in this.Tiles)
            {
                tile.ResetPlayerWalk();
            }

            var attackable = new List<FieldTile>();
            
            var checkqueue = new Queue<FieldTile>();
            
            var playerTile = GameManager.Instance.BattlePlayer.PositionTile;
            checkqueue.Enqueue(playerTile);

            var checkedTiles = new List<FieldTile>();
            
            while (checkqueue.Count > 0)
            {
                var tile = checkqueue.Dequeue();

                if (checkedTiles.Contains(tile))
                    continue;
                
                var distance = Mathf.Abs(playerTile.transform.position.x - tile.transform.position.x) 
                               + Mathf.Abs(playerTile.transform.position.y - tile.transform.position.y);

                if (distance > GameManager.Instance.BattlePlayer.MinDistance && distance <= GameManager.Instance.BattlePlayer.MaxDistance)
                {
                    tile.TileStatus |= TileStatus.PlayerAttackable;
                    attackable.Add(tile);
                }

                if (distance < GameManager.Instance.BattlePlayer.MaxDistance)
                {
                    if (tile.UpNeighbor != null) checkqueue.Enqueue(tile.UpNeighbor);
                    if (tile.DownNeighbor != null) checkqueue.Enqueue(tile.DownNeighbor);
                    if (tile.LeftNeighbor != null) checkqueue.Enqueue(tile.LeftNeighbor);
                    if (tile.RightNeighbor != null) checkqueue.Enqueue(tile.RightNeighbor);

                }
            }
            
            this.PlayerAttackable = attackable;
            GameManager.Instance.Cursor.PlayerAttackable = attackable;
        }

        private void Update()
        {
            this.UpdateBattle();
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
                        this.UpdateTilesPlayerWalk();
                        
                        GameManager.Instance.Cursor.Moveable      = true;
                        GameManager.Instance.Cursor.PlayerMovable = this.PlayerWalkable;
                    }
                    break;
                case BattleState.PlayerMoving:
                    break;
                case BattleState.PlayerToAttack:
                    break;
                case BattleState.PlayerAttacking:
                    break;
                case BattleState.EnemiesMoving:
                    this.MoveEnemies();
                    break;
                case BattleState.EnemiesAttacking:
                    GameManager.Instance.BattleState = BattleState.PlayerToMove;
                    break;
                case BattleState.EndAnimation:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        public void MoveEnemies()
        {
            if (!this.EnemyCanMove)
                return;

            if (this.EnemyMovedCounter >= this.Enemies.Count)
            {
                this.EnemyMovedCounter = 0;
                this.EnemiesToMove.Clear();
                this.EnemyCanMove = false;
                GameManager.Instance.BattleState = BattleState.EnemiesAttacking;
            }
            
            if (this.EnemiesToMove.Count == 0)
                foreach (var enemy in this.Enemies)
                    this.EnemiesToMove.Enqueue(enemy);

            var moving = this.EnemiesToMove.Dequeue();
            this.EnemyCanMove = !moving.Move();
            this.EnemyMovedCounter++;
        }
    }
}
