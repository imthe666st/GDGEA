using UnityEngine;

namespace Battle {
    using System;

    using Camera;

    public class Battlefield : MonoBehaviour
    {
        public FieldTile FieldTilePrefab;
        public StaticCameraMarker CameraMarkerPrefab;

        public const int FieldWidth = 8;
        public const int FieldHeight = 8;

        protected FieldTile[] Tiles;

        private void Awake()
        {
            GameManager.Instance.Battlefield = this;
            this.Tiles = new FieldTile[FieldWidth * FieldHeight];
            
            this.GenerateField();
        }

        private void GenerateField()
        {
            //Tiles
            var counter = 0;
            for (var i = 0; i < FieldWidth; ++i)
            {
                for (var j = 0; j < FieldHeight; ++j, ++counter)
                {
                    this.Tiles[counter] = Instantiate(this.FieldTilePrefab, new Vector3(i, j), Quaternion.identity,
                                                      this.transform);
                }
            }
            
            //Connect them up!

            //right-left
            for (var i = 0; i < FieldWidth * (FieldHeight - 1); ++i)
            {
                this.Tiles[i].SetNeighbor(Direction.Right, this.Tiles[i + FieldWidth]);
            }

            for (var i = 0; i < FieldWidth * FieldHeight; ++i)
            {
                if ((i + 1) % FieldHeight == 0)
                    continue;

                this.Tiles[i].SetNeighbor(Direction.Up, this.Tiles[i + 1]);
            }

            //Camera Marker
            Instantiate(this.CameraMarkerPrefab, new Vector3((FieldWidth - 1) / 2f, (FieldHeight - 1) / 2f, -10), Quaternion.identity,
                        this.transform);
        }
    }
}
