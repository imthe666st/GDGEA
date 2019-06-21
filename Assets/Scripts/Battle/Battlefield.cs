using UnityEngine;

namespace Battle {
    using System.Collections.Generic;
    using System.Linq;

    using Camera;

    using Enemy;

    public class Battlefield : MonoBehaviour
    {
        public FieldTile FieldTilePrefab;
        public StaticCameraMarker CameraMarkerPrefab;
        public BattlePlayer BattlePlayerPrefab;

        public int fieldWidth = 8;
        public int fieldHeight = 8;
        public int enemySpawnWidth = 2;
        public int playerSpawnWidth = 2;
        
        protected FieldTile[] Tiles;

        protected List<FieldTile> EnemySpawnTiles;

        [HideInInspector]
        public List<Enemy> Enemies;
        
        private void Awake()
        {
            GameManager.Instance.Battlefield = this;

            this.Enemies = new List<Enemy>();
            this.Tiles = new FieldTile[this.fieldWidth * this.fieldHeight];
            
            this.GenerateField();

            this.EnemySpawnTiles = this.Tiles.Skip((this.fieldWidth - this.enemySpawnWidth) * this.fieldHeight).ToList();
            
            this.SpawnPlayer();
            this.SpawnEnemies();
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
        }

        private void SpawnPlayer()
        {
            var value = Random.Range(0, this.fieldHeight * this.playerSpawnWidth);
            var pos = this.Tiles[value].transform.position - new Vector3(0, 0, 0.1f);

            Instantiate(this.BattlePlayerPrefab, pos, Quaternion.identity, this.transform);
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
    }
}
