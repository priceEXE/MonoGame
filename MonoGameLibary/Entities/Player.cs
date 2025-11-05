using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary.Graphics;
using MonoGameLibrary.Input;

namespace MonoGameLibrary.Entities
{
    /// <summary>
    /// 玩家类，继承自游戏实体
    /// </summary>
    public class Player : GameEntity
    {
        // 玩家属性
        public float MaxHealth { get; set; } = 100f;
        public float CurrentHealth { get; set; } = 100f;
        public float Experience { get; set; } = 0f;
        public int Level { get; set; } = 1;
        public float MoveSpeed { get; set; } = 200f;
        public float InvulnerabilityTime { get; set; } = 0f;
        public bool IsInvulnerable => InvulnerabilityTime > 0f;

        // 武器和增益
        public Weapon CurrentWeapon { get; set; }
        public List<Buff> ActiveBuffs { get; private set; }

    // 动画状态 - 要求：静止、移动、受击
        public enum AnimationState
        {
            Idle,       // 静止
            Moving,     // 移动
            Attacking,  // 攻击
            Hurt        // 受击（暂时无敌的虚化表现）
        }

        public AnimationState CurrentAnimationState { get; set; } = AnimationState.Idle;
        public Vector2 LastMoveDirection { get; set; } = Vector2.Zero;

        // 动画精灵
        private AnimatedSprite _idleSprite;
        private AnimatedSprite _movingSprite;
        private AnimatedSprite _attackingSprite;
        private AnimatedSprite _hurtSprite;

        public Player(Vector2 position) : base(position)
        {
            ActiveBuffs = new List<Buff>();
            Tag = "Player";
        }

        public override void Load()
        {
            base.Load();
            // 这里应该加载玩家的精灵和动画
            // 示例：从图集创建动画精灵
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // 更新无敌时间
            if (InvulnerabilityTime > 0)
            {
                InvulnerabilityTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            // 处理输入
            HandleInput(gameTime);

            // 更新武器
            CurrentWeapon?.Update(gameTime);

            // 更新增益效果
            UpdateBuffs(gameTime);

            // 更新动画状态
            UpdateAnimationState();
        }

        /// <summary>
        /// 处理玩家输入
        /// </summary>
        private void HandleInput(GameTime gameTime)
        {
            Vector2 moveDirection = Vector2.Zero;

            // WASD移动
            if (InputManager.GetKey(Keys.W))
                moveDirection.Y -= 1;
            if (InputManager.GetKey(Keys.S))
                moveDirection.Y += 1;
            if (InputManager.GetKey(Keys.A))
                moveDirection.X -= 1;
            if (InputManager.GetKey(Keys.D))
                moveDirection.X += 1;

            // 标准化移动方向
            if (moveDirection != Vector2.Zero)
            {
                moveDirection.Normalize();
                LastMoveDirection = moveDirection;
                CurrentAnimationState = AnimationState.Moving;
            }
            else
            {
                CurrentAnimationState = AnimationState.Idle;
            }

            // 应用移动速度
            float currentMoveSpeed = MoveSpeed;
            foreach (var buff in ActiveBuffs)
            {
                if (buff.Type == BuffType.SpeedBoost)
                {
                    currentMoveSpeed *= buff.Value;
                }
            }

            Velocity = moveDirection * currentMoveSpeed;

            // 武器切换 - 要求：123可以切换武器（或者鼠标滚轮）
            if (InputManager.GetKeyDown(Keys.D1))
            {
                // 切换到武器1（刀）
            }
            if (InputManager.GetKeyDown(Keys.D2))
            {
                // 切换到武器2（手枪）
            }
            if (InputManager.GetKeyDown(Keys.D3))
            {
                // 切换到武器3（步枪）
            }
            if (InputManager.GetKeyDown(Keys.D4))
            {
                // 切换到武器4（狙击枪）
            }

            // 鼠标攻击
            if (InputManager.GetMouseButtonDown(MouseButton.Left))
            {
                Attack();
            }
        }

        /// <summary>
        /// 攻击
        /// </summary>
        private void Attack()
        {
            if (CurrentWeapon != null)
            {
                CurrentAnimationState = AnimationState.Attacking;
                CurrentWeapon.Attack(Position, GetMouseDirection());
            }
        }

        /// <summary>
        /// 获取鼠标方向
        /// </summary>
        private Vector2 GetMouseDirection()
        {
            var mouseState = Mouse.GetState();
            var mousePosition = new Vector2(mouseState.X, mouseState.Y);
            var direction = mousePosition - Position;
            if (direction != Vector2.Zero)
            {
                direction.Normalize();
            }
            return direction;
        }

        /// <summary>
        /// 受到伤害
        /// </summary>
        public void TakeDamage(float damage)
        {
            if (!IsInvulnerable)
            {
                CurrentHealth -= damage;
                InvulnerabilityTime = 1.0f; // 1秒无敌时间
                CurrentAnimationState = AnimationState.Hurt;

                if (CurrentHealth <= 0)
                {
                    Die();
                }
            }
        }

        /// <summary>
        /// 获得经验
        /// </summary>
        public void GainExperience(float exp)
        {
            Experience += exp;
            CheckLevelUp();
        }

        /// <summary>
        /// 检查升级
        /// </summary>
        private void CheckLevelUp()
        {
            float requiredExp = Level * 100f; // 每级需要100经验
            if (Experience >= requiredExp)
            {
                LevelUp();
            }
        }

        /// <summary>
        /// 升级
        /// </summary>
        private void LevelUp()
        {
            Level++;
            Experience = 0f;
            // 这里应该显示升级选择界面
            // 可以选择三种强化之一
        }

        /// <summary>
        /// 死亡
        /// </summary>
        private void Die()
        {
            IsActive = false;
            // 游戏结束逻辑
        }

        /// <summary>
        /// 添加增益效果
        /// </summary>
        public void AddBuff(Buff buff)
        {
            ActiveBuffs.Add(buff);
        }

        /// <summary>
        /// 更新增益效果
        /// </summary>
        private void UpdateBuffs(GameTime gameTime)
        {
            for (int i = ActiveBuffs.Count - 1; i >= 0; i--)
            {
                ActiveBuffs[i].Update(gameTime);
                if (ActiveBuffs[i].IsExpired)
                {
                    ActiveBuffs.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// 更新动画状态
        /// </summary>
        private void UpdateAnimationState()
        {
            AnimatedSprite targetSprite = null;

            switch (CurrentAnimationState)
            {
                case AnimationState.Idle:
                    targetSprite = _idleSprite;
                    break;
                case AnimationState.Moving:
                    targetSprite = _movingSprite;
                    break;
                case AnimationState.Attacking:
                    targetSprite = _attackingSprite;
                    break;
                case AnimationState.Hurt:
                    targetSprite = _hurtSprite;
                    break;
            }

            if (targetSprite != null && _animatedSprite != targetSprite)
            {
                SetAnimatedSprite(targetSprite);
            }
        }
    }
}
