using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoGameLibrary.Entities;

namespace MonoGameLibrary.GameCore
{
    /// <summary>
    /// AI系统，管理所有怪物的AI行为
    /// </summary>
    public class AISystem
    {
        private List<Monster> _monsters;
        private Player _player;

        public AISystem()
        {
            _monsters = new List<Monster>();
        }

        /// <summary>
        /// 设置玩家引用
        /// </summary>
        public void SetPlayer(Player player)
        {
            _player = player;
        }

        /// <summary>
        /// 添加怪物
        /// </summary>
        public void AddMonster(Monster monster)
        {
            _monsters.Add(monster);
        }

        /// <summary>
        /// 移除怪物
        /// </summary>
        public void RemoveMonster(Monster monster)
        {
            _monsters.Remove(monster);
        }

        /// <summary>
        /// 更新AI系统
        /// </summary>
        public void Update(GameTime gameTime)
        {
            if (_player == null) return;

            // 更新所有怪物的AI
            for (int i = _monsters.Count - 1; i >= 0; i--)
            {
                if (_monsters[i].IsActive)
                {
                    UpdateMonsterAI(_monsters[i], gameTime);
                }
                else
                {
                    _monsters.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// 更新单个怪物的AI
        /// </summary>
        private void UpdateMonsterAI(Monster monster, GameTime gameTime)
        {
            // 计算到玩家的距离
            float distanceToPlayer = Vector2.Distance(monster.Position, _player.Position);

            // 根据怪物类型执行不同的AI行为
            switch (monster)
            {
                case TankMonster tankMonster:
                    UpdateTankMonsterAI(tankMonster, distanceToPlayer);
                    break;
                case ExplosiveMonster explosiveMonster:
                    UpdateExplosiveMonsterAI(explosiveMonster, distanceToPlayer);
                    break;
                case RangedMonster rangedMonster:
                    UpdateRangedMonsterAI(rangedMonster, distanceToPlayer);
                    break;
            }
        }

        /// <summary>
        /// 坦克怪物AI
        /// </summary>
        private void UpdateTankMonsterAI(TankMonster monster, float distanceToPlayer)
        {
            if (distanceToPlayer <= monster.DetectionRange)
            {
                if (distanceToPlayer <= monster.AttackRange)
                {
                    // 在攻击范围内，停止移动并攻击
                    monster.Velocity = Vector2.Zero;
                    monster.CurrentState = Monster.MonsterState.Attacking;
                }
                else
                {
                    // 追击玩家
                    Vector2 direction = _player.Position - monster.Position;
                    if (direction != Vector2.Zero)
                    {
                        direction.Normalize();
                        monster.Velocity = direction * monster.MoveSpeed;
                        monster.CurrentState = Monster.MonsterState.Chasing;
                    }
                }
            }
            else
            {
                // 超出检测范围，停止移动
                monster.Velocity = Vector2.Zero;
                monster.CurrentState = Monster.MonsterState.Idle;
            }
        }

        /// <summary>
        /// 自爆怪物AI
        /// </summary>
        private void UpdateExplosiveMonsterAI(ExplosiveMonster monster, float distanceToPlayer)
        {
            if (distanceToPlayer <= monster.DetectionRange)
            {
                if (distanceToPlayer <= monster.AttackRange)
                {
                    // 在攻击范围内，立即自爆
                    monster.Velocity = Vector2.Zero;
                    monster.CurrentState = Monster.MonsterState.Attacking;
                }
                else
                {
                    // 快速追击玩家
                    Vector2 direction = _player.Position - monster.Position;
                    if (direction != Vector2.Zero)
                    {
                        direction.Normalize();
                        monster.Velocity = direction * monster.MoveSpeed;
                        monster.CurrentState = Monster.MonsterState.Chasing;
                    }
                }
            }
            else
            {
                monster.Velocity = Vector2.Zero;
                monster.CurrentState = Monster.MonsterState.Idle;
            }
        }

        /// <summary>
        /// 远程怪物AI
        /// </summary>
        private void UpdateRangedMonsterAI(RangedMonster monster, float distanceToPlayer)
        {
            if (distanceToPlayer <= monster.DetectionRange)
            {
                if (distanceToPlayer <= monster.AttackRange)
                {
                    // 在攻击范围内，停止移动并攻击
                    monster.Velocity = Vector2.Zero;
                    monster.CurrentState = Monster.MonsterState.Attacking;
                }
                else
                {
                    // 保持距离追击
                    Vector2 direction = _player.Position - monster.Position;
                    if (direction != Vector2.Zero)
                    {
                        direction.Normalize();
                        // 保持一定距离
                        Vector2 targetPosition = _player.Position - direction * (monster.AttackRange * 0.8f);
                        Vector2 moveDirection = targetPosition - monster.Position;
                        if (moveDirection != Vector2.Zero)
                        {
                            moveDirection.Normalize();
                            monster.Velocity = moveDirection * monster.MoveSpeed;
                        }
                        monster.CurrentState = Monster.MonsterState.Chasing;
                    }
                }
            }
            else
            {
                monster.Velocity = Vector2.Zero;
                monster.CurrentState = Monster.MonsterState.Idle;
            }
        }
    }
}
