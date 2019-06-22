using UnityEngine;

namespace Player {
    using System;

    using Battle;

    using DG.Tweening;

    public class BattlePlayer : MonoBehaviour
    {
        public int BaseMovement = 2;

        public FieldTile PositionTile;

        public float MoveTime = 0.2f;
        public float AttackTime = 0.1f;
        
        private void Awake()
        {
            GameManager.Instance.BattlePlayer = this;
        }

        public int Damage
        {
            get
            {
                var inventory = GameManager.Instance.playerInventory;

                var damage = 0;
                if (inventory.CurrentWeapon != null) damage    += inventory.CurrentWeapon.Damage;
                if (inventory.CurrentModifier1 != null) damage += inventory.CurrentModifier1.Damage;
                if (inventory.CurrentModifier2 != null) damage += inventory.CurrentModifier2.Damage;

                return damage;
            }
        }

        public int Movement
        {
            get
            {
                var inventory = GameManager.Instance.playerInventory;

                var movement = this.BaseMovement;
                if (inventory.CurrentWeapon != null) movement    += inventory.CurrentWeapon.Movement;
                if (inventory.CurrentModifier1 != null) movement += inventory.CurrentModifier1.Movement;
                if (inventory.CurrentModifier2 != null) movement += inventory.CurrentModifier2.Movement;

                
                return movement;
            }
        }

        public int MinDistance
        {
            get
            {
                var inventory = GameManager.Instance.playerInventory;

                var minDistance = 0;

                if (inventory.CurrentWeapon != null) minDistance += inventory.CurrentWeapon.MinDistance;
                if (inventory.CurrentModifier1 != null) minDistance += inventory.CurrentModifier1.MinDistance;
                if (inventory.CurrentModifier2 != null) minDistance += inventory.CurrentModifier2.MinDistance;

                return minDistance;
            }
        }

        public int MaxDistance
        {
            get
            {
                var inventory = GameManager.Instance.playerInventory;

                var maxDistance = 0;
                
                if (inventory.CurrentWeapon != null) maxDistance    += inventory.CurrentWeapon.MaxDistance;
                if (inventory.CurrentModifier1 != null) maxDistance += inventory.CurrentModifier1.MaxDistance;
                if (inventory.CurrentModifier2 != null) maxDistance += inventory.CurrentModifier2.MaxDistance;

                return maxDistance;
            }
        }
        
        public void MoveToTile(FieldTile target)
        {
            var tempPlayerpos = this.PositionTile;

            var mv = DOTween.Sequence();

            while (tempPlayerpos != target)
            {
                var toMove = target.transform.position.ClearZ() - tempPlayerpos.transform.position.ClearZ();

                if (toMove.x > toMove.y)
                {
                    if (Mathf.Abs(toMove.x) > float.Epsilon)
                        tempPlayerpos = this.CreateXMove(mv, toMove.x, tempPlayerpos);
                }
                else
                {
                    if (Mathf.Abs(toMove.y) > float.Epsilon)
                        tempPlayerpos = this.CreateYMove(mv, toMove.y, tempPlayerpos);
                }
            }

            this.PositionTile = target;
        }

        private FieldTile CreateYMove(Sequence sq, float yDiff, FieldTile playerPos)
        {
            var targetTile = yDiff < 0 ? playerPos.DownNeighbor : playerPos.UpNeighbor;
            sq.Append(this.transform.DOMove(targetTile.transform.position.ClearZ() + this.transform.position.OnlyZ(), this.MoveTime));
            return targetTile;
        }

        private FieldTile CreateXMove(Sequence sq, float xDiff, FieldTile playerPos)
        {
            var targetTile = xDiff < 0 ? playerPos.LeftNeighbor : playerPos.RightNeighbor;
            sq.Append(this.transform.DOMove(targetTile.transform.position.ClearZ() + this.transform.position.OnlyZ(), this.MoveTime));
            return targetTile;
        }

        public void Attack(FieldTile tile)
        {
            var mv = DOTween.Sequence();
            var pos = this.transform.position;

            var dir = tile.transform.position - pos;
            dir.Normalize();

            mv.Append(this.transform.DOMove(pos + 0.5f * dir, this.AttackTime))
              .Append(this.transform.DOMove(pos, this.AttackTime)).OnComplete(() =>
                                                                            {
                                                                                GameManager.Instance.BattleState =
                                                                                    BattleState.EnemiesMoving;
                                                                            });
        }
    }
}
