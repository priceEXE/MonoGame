using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoGameLibrary.GameCore;

namespace MonoGameLibrary.Entities
{
    /// <summary>
    /// 武器基类
    /// </summary>
    public abstract class Weapon
    {
        public string Name { get; set; }
        public float Damage { get; set; }
        public float AttackRange { get; set; }
        public float AttackCooldown { get; set; }
        public float CurrentCooldown { get; set; }
        public bool CanAttack => CurrentCooldown <= 0f;

        public Weapon(string name, float damage, float attackRange, float attackCooldown)
        {
            Name = name;
            Damage = damage;
            AttackRange = attackRange;
            AttackCooldown = attackCooldown;
        }

        /// <summary>
        /// 更新武器
        /// </summary>
        public virtual void Update(GameTime gameTime)
        {
            if (CurrentCooldown > 0)
            {
                CurrentCooldown -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
        }

        /// <summary>
        /// 攻击
        /// </summary>
        public abstract void Attack(Vector2 position, Vector2 direction);
    }

    /// <summary>
    /// 近战武器（刀）- 攻击距离短，范围攻击，需要攻击动画
    /// </summary>
    public class MeleeWeapon : Weapon
    {
        public float AttackRadius { get; set; }
        public float AttackAngle { get; set; }

        public MeleeWeapon(string name, float damage, float attackRange, float attackCooldown, float attackRadius, float attackAngle)
            : base(name, damage, attackRange, attackCooldown)
        {
            AttackRadius = attackRadius;
            AttackAngle = attackAngle;
        }

        public override void Attack(Vector2 position, Vector2 direction)
        {
            if (!CanAttack) return;

            CurrentCooldown = AttackCooldown;

            // 创建近战攻击区域
            var attackArea = new MeleeAttackArea(position, direction, AttackRadius, AttackAngle, Damage);
            // 这里应该将攻击区域添加到碰撞检测系统
        }
    }

    /// <summary>
    /// 远程武器基类
    /// </summary>
    public abstract class RangedWeapon : Weapon
    {
        public int MaxAmmo { get; set; }
        public int CurrentAmmo { get; set; }
        public float ReloadTime { get; set; }
        public float CurrentReloadTime { get; set; }
        public bool IsReloading => CurrentReloadTime > 0f;

        public RangedWeapon(string name, float damage, float attackRange, float attackCooldown, int maxAmmo, float reloadTime)
            : base(name, damage, attackRange, attackCooldown)
        {
            MaxAmmo = maxAmmo;
            CurrentAmmo = maxAmmo;
            ReloadTime = reloadTime;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (IsReloading)
            {
                CurrentReloadTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (CurrentReloadTime <= 0)
                {
                    CurrentAmmo = MaxAmmo;
                }
            }
        }

        public override void Attack(Vector2 position, Vector2 direction)
        {
            if (!CanAttack || IsReloading || CurrentAmmo <= 0) return;

            CurrentCooldown = AttackCooldown;
            CurrentAmmo--;

            // 创建投射物
            CreateProjectile(position, direction);
        }

        /// <summary>
        /// 创建投射物
        /// </summary>
        protected abstract void CreateProjectile(Vector2 position, Vector2 direction);

        /// <summary>
        /// 重新装弹
        /// </summary>
        public void Reload()
        {
            if (!IsReloading && CurrentAmmo < MaxAmmo)
            {
                CurrentReloadTime = ReloadTime;
            }
        }
    }

    /// <summary>
    /// 手枪 - 攻击距离较长，单体攻击，弹容10，可快速连发
    /// </summary>
    public class Pistol : RangedWeapon
    {
        public Pistol() : base("Pistol", 25f, 300f, 0.3f, 10, 2.0f)
        {
        }

        protected override void CreateProjectile(Vector2 position, Vector2 direction)
        {
            var projectile = new Projectile(position, direction, 500f, Damage, AttackRange);
            // 添加到游戏世界
        }
    }

    /// <summary>
    /// 步枪（冲锋枪）- 伤害高，弹夹30/30，更快的射击，换弹夹也更长，穿透更强
    /// </summary>
    public class Rifle : RangedWeapon
    {
        public Rifle() : base("Rifle", 35f, 400f, 0.1f, 30, 3.0f)
        {
        }

        protected override void CreateProjectile(Vector2 position, Vector2 direction)
        {
            var projectile = new Projectile(position, direction, 600f, Damage, AttackRange);
            // 添加到游戏世界
        }
    }

    /// <summary>
    /// 狙击枪 - 超射程，超高伤，高穿透，弹夹5/5，射速慢，换弹中规中矩
    /// </summary>
    public class SniperRifle : RangedWeapon
    {
        public SniperRifle() : base("Sniper Rifle", 100f, 800f, 1.5f, 5, 2.5f)
        {
        }

        protected override void CreateProjectile(Vector2 position, Vector2 direction)
        {
            var projectile = new Projectile(position, direction, 800f, Damage, AttackRange);
            // 添加到游戏世界
        }
    }

    /// <summary>
    /// 近战攻击区域
    /// </summary>
    public class MeleeAttackArea : GameEntity
    {
        public float Damage { get; set; }
        public float LifeTime { get; set; } = 0.2f;

        public MeleeAttackArea(Vector2 position, Vector2 direction, float radius, float angle, float damage)
            : base(position)
        {
            Damage = damage;
            Tag = "MeleeAttack";
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
    /// 投射物
    /// </summary>
    public class Projectile : GameEntity
    {
        public float Speed { get; set; }
        public float Damage { get; set; }
        public float MaxDistance { get; set; }
        public float TraveledDistance { get; set; }
        public Vector2 Direction { get; set; }

        public Projectile(Vector2 position, Vector2 direction, float speed, float damage, float maxDistance)
            : base(position)
        {
            Direction = direction;
            Speed = speed;
            Damage = damage;
            MaxDistance = maxDistance;
            Velocity = direction * speed;
            Tag = "Projectile";
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            TraveledDistance += Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            if (TraveledDistance >= MaxDistance)
            {
                IsActive = false;
            }
        }
    }
}
