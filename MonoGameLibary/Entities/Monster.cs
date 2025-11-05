using System;
using Microsoft.Xna.Framework;
using MonoGameLibrary.Entities;

namespace MonoGameLibrary.Entities
{
    /// <summary>
    /// 怪物基类
    /// </summary>
    public abstract class Monster : GameEntity
    {
        // 怪物属性
        public float MaxHealth { get; set; }
        public float CurrentHealth { get; set; }
        public float Damage { get; set; }
        public float MoveSpeed { get; set; }
        public float AttackRange { get; set; }
        public float AttackCooldown { get; set; }
        public float CurrentAttackCooldown { get; set; }
        public float ExperienceReward { get; set; }

        // AI相关
        public Vector2 TargetPosition { get; set; }
        public float DetectionRange { get; set; } = 200f;
        public float AttackRangeSquared { get; set; }

        // 动画状态
        public enum MonsterState
        {
            Idle,
            Chasing,
            Attacking,
            Dead
        }

        public MonsterState CurrentState { get; set; } = MonsterState.Idle;

        public Monster(Vector2 position) : base(position)
        {
            Tag = "Monster";
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (CurrentState != MonsterState.Dead)
            {
                UpdateAI(gameTime);
                UpdateAttack(gameTime);
            }
        }

        /// <summary>
        /// 更新AI行为
        /// </summary>
        protected virtual void UpdateAI(GameTime gameTime)
        {
            // 寻找玩家
            var player = FindNearestPlayer();
            if (player != null)
            {
                TargetPosition = player.Position;
                float distanceToPlayer = Vector2.Distance(Position, player.Position);

                if (distanceToPlayer <= DetectionRange)
                {
                    if (distanceToPlayer <= AttackRange)
                    {
                        CurrentState = MonsterState.Attacking;
                    }
                    else
                    {
                        CurrentState = MonsterState.Chasing;
                        // 移动到玩家位置
                        Vector2 direction = player.Position - Position;
                        if (direction != Vector2.Zero)
                        {
                            direction.Normalize();
                            Velocity = direction * MoveSpeed;
                        }
                    }
                }
                else
                {
                    CurrentState = MonsterState.Idle;
                    Velocity = Vector2.Zero;
                }
            }
        }

        /// <summary>
        /// 更新攻击
        /// </summary>
        protected virtual void UpdateAttack(GameTime gameTime)
        {
            if (CurrentAttackCooldown > 0)
            {
                CurrentAttackCooldown -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (CurrentState == MonsterState.Attacking && CurrentAttackCooldown <= 0)
            {
                Attack();
                CurrentAttackCooldown = AttackCooldown;
            }
        }

        /// <summary>
        /// 攻击
        /// </summary>
        protected virtual void Attack()
        {
            var player = FindNearestPlayer();
            if (player != null)
            {
                player.TakeDamage(Damage);
            }
        }

        /// <summary>
        /// 受到伤害
        /// </summary>
        public virtual void TakeDamage(float damage)
        {
            CurrentHealth -= damage;
            if (CurrentHealth <= 0)
            {
                Die();
            }
        }

        /// <summary>
        /// 死亡
        /// </summary>
        protected virtual void Die()
        {
            CurrentState = MonsterState.Dead;
            IsActive = false;
            // 掉落经验
            DropExperience();
        }

        /// <summary>
        /// 掉落经验
        /// </summary>
        protected virtual void DropExperience()
        {
            // 创建经验球
            var experienceOrb = new ExperienceOrb(Position, ExperienceReward);
            // 添加到游戏世界
        }

        /// <summary>
        /// 寻找最近的玩家
        /// </summary>
        protected Player FindNearestPlayer()
        {
            // 这里应该从场景管理器中获取玩家
            // 简化实现
            return null;
        }
    }

    /// <summary>
    /// 坦克怪物 - 要求：高血低速高伤近战，经验掉落多
    /// </summary>
    public class TankMonster : Monster
    {
        public TankMonster(Vector2 position) : base(position)
        {
            MaxHealth = 150f;
            CurrentHealth = MaxHealth;
            Damage = 40f;
            MoveSpeed = 80f;
            AttackRange = 50f;
            AttackCooldown = 2.0f;
            ExperienceReward = 50f; // 经验掉落多
        }
    }

    /// <summary>
    /// 自爆怪物 - 要求：低血高速中伤近战自爆，被直接打死会掉落经验，自爆不掉落经验
    /// </summary>
    public class ExplosiveMonster : Monster
    {
        public float ExplosionRadius { get; set; } = 100f;
        public float ExplosionDamage { get; set; } = 60f;
        public bool IsExploding { get; set; } = false;

        public ExplosiveMonster(Vector2 position) : base(position)
        {
            MaxHealth = 30f;        // 低血
            CurrentHealth = MaxHealth;
            Damage = 20f;           // 中伤
            MoveSpeed = 150f;       // 高速
            AttackRange = 30f;      // 近战
            AttackCooldown = 1.0f;
            ExperienceReward = 0f;   // 自爆不掉落经验
        }

        protected override void Attack()
        {
            Explode();
        }

        protected override void Die()
        {
            if (!IsExploding)
            {
                Explode();
            }
            base.Die();
        }

        /// <summary>
        /// 爆炸
        /// </summary>
        private void Explode()
        {
            IsExploding = true;
            // 创建爆炸效果
            var explosion = new Explosion(Position, ExplosionRadius, ExplosionDamage);
            // 添加到游戏世界
            IsActive = false;
        }
    }

    /// <summary>
    /// 远程怪物 - 要求：普通的中血中速中伤远程
    /// </summary>
    public class RangedMonster : Monster
    {
        public RangedMonster(Vector2 position) : base(position)
        {
            MaxHealth = 80f;        // 中血
            CurrentHealth = MaxHealth;
            Damage = 25f;           // 中伤
            MoveSpeed = 100f;       // 中速
            AttackRange = 200f;     // 远程
            AttackCooldown = 1.5f;
            ExperienceReward = 30f;
        }

        protected override void Attack()
        {
            // 创建远程攻击投射物
            var player = FindNearestPlayer();
            if (player != null)
            {
                Vector2 direction = player.Position - Position;
                if (direction != Vector2.Zero)
                {
                    direction.Normalize();
                    var projectile = new MonsterProjectile(Position, direction, 300f, Damage, 300f);
                    // 添加到游戏世界
                }
            }
        }
    }

    /// <summary>
    /// 经验球
    /// </summary>
    public class ExperienceOrb : GameEntity
    {
        public float ExperienceValue { get; set; }
        public float AttractionRange { get; set; } = 50f;

        public ExperienceOrb(Vector2 position, float experienceValue) : base(position)
        {
            ExperienceValue = experienceValue;
            Tag = "ExperienceOrb";
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // 吸引玩家
            var player = FindNearestPlayer();
            if (player != null)
            {
                float distance = Vector2.Distance(Position, player.Position);
                if (distance <= AttractionRange)
                {
                    Vector2 direction = player.Position - Position;
                    if (direction != Vector2.Zero)
                    {
                        direction.Normalize();
                        Velocity = direction * 200f; // 吸引速度
                    }
                }
            }
        }

        private Player FindNearestPlayer()
        {
            // 简化实现
            return null;
        }
    }

    /// <summary>
    /// 爆炸效果
    /// </summary>
    public class Explosion : GameEntity
    {
        public float Radius { get; set; }
        public float Damage { get; set; }
        public float LifeTime { get; set; } = 0.5f;

        public Explosion(Vector2 position, float radius, float damage) : base(position)
        {
            Radius = radius;
            Damage = damage;
            Tag = "Explosion";
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            LifeTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (LifeTime <= 0)
            {
                IsActive = false;
            }
        }
    }

    /// <summary>
    /// 怪物投射物
    /// </summary>
    public class MonsterProjectile : Projectile
    {
        public MonsterProjectile(Vector2 position, Vector2 direction, float speed, float damage, float maxDistance)
            : base(position, direction, speed, damage, maxDistance)
        {
            Tag = "MonsterProjectile";
        }
    }
}
