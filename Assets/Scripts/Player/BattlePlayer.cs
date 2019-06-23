using UnityEngine;

namespace Player {
    using Battle;

    using DG.Tweening;

    using Enemy;

    using Marker;

    using Random = Random;

    public class BattlePlayer : MonoBehaviour
    {
        public FieldTile PositionTile;

        public float MoveTime = 0.2f;
        public float AttackTime = 0.1f;

        private int health;

        public HealthBarMarker HealthBarMarker;
        private AudioSource _audioSource;

        private void Awake()
        {
            this._audioSource = this.GetComponent<AudioSource>();
            GameManager.Instance.BattlePlayer = this;
            this.health = GameManager.Instance.stats.BaseHealth;
        }

        public void MoveToTile(FieldTile target)
        {
            var tempPlayerpos = this.PositionTile;

            var mv = DOTween.Sequence();

            while (tempPlayerpos != target)
            {
                var toMove = target.transform.position.ClearZ() - tempPlayerpos.transform.position.ClearZ();

                if (Mathf.Abs(toMove.x) > Mathf.Abs(toMove.y))
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

            //mv.OnComplete(() => { GameManager.Instance.Battlefield.EnemyCanMove = true; });
            
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

            var target = GameManager.Instance.Battlefield.Enemies.Find(e => e.PositionTile == tile);

            mv.Append(this.transform.DOMove(pos + 0.5f * dir, this.AttackTime))
              .Append(this.transform.DOMove(pos, this.AttackTime))
              .OnComplete(() =>
                          {
                              GameManager.Instance.BattleState =
                                  BattleState.EnemiesMoving;
                              GameManager.Instance.Battlefield
                                         .Enemies.Shuffle();
                              GameManager.Instance.Battlefield
                                         .PlayerAttackable.Clear();

                              var sq = DOTween.Sequence().AppendInterval(this.MoveTime * 2)
                                              .AppendCallback(() =>
                                                              {
                                                                  GameManager.Instance.Battlefield.EnemyCanMove = true;
                                                              });
                          });

            var moveEnemy = DOTween.Sequence();

            if (target == null)
                return;

            var tarPos = target.transform.position;

            moveEnemy.AppendInterval(this.AttackTime)
                     .AppendCallback(() => this.DealDamage(target))
                     .Append(target.transform.DOMove(tarPos + 0.25f * dir, this.AttackTime))
                     .Append(target.transform.DOMove(tarPos, this.AttackTime));

        }

        public void DealDamage(Enemy target)
        {
            if (target == null)
                return;

            var stats = GameManager.Instance.stats;
            
            var damage = stats.Damage;

            if (Random.Range(0, 1f) < stats.CritChance)
            {
                damage = (int)(stats.Damage * stats.CritDamage);
            }
            
            target.TakeDamage(damage);
            this._audioSource.Play();
        }

        public void TakeDamage(int damage)
        {
            this.health -= damage;

            var di = Instantiate(GameManager.Instance.Battlefield.DamageIndicatorPrefab,
                                 this.transform.position - Vector3.forward, Quaternion.identity,
                                 GameManager.Instance.Battlefield.transform);
            di.SetValue(damage.ToString());
            
            this.HealthBarMarker.Change((float) this.health / GameManager.Instance.stats.BaseHealth);

            this._audioSource.Play();

            if (this.health <= 0)
            {
                //TODO: DIE
            }
        }
    }
}
