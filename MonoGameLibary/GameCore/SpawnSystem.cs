using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoGameLibrary.Entities;

namespace MonoGameLibrary.GameCore
{
    /// <summary>
    /// 生成系统，管理怪物的生成
    /// </summary>
    public class SpawnSystem
    {
        private List<SpawnPoint> _spawnPoints;
        private Player _player;
        private Random _random;
        private float _spawnTimer;
        private float _spawnInterval;
        private int _maxMonsters;
        private int _currentMonsterCount;

        public SpawnSystem()
        {
            _spawnPoints = new List<SpawnPoint>();
            _random = new Random();
            _spawnInterval = 2.0f; // 每2秒生成一次
            _maxMonsters = 20;
        }

        /// <summary>
        /// 设置玩家引用
        /// </summary>
        public void SetPlayer(Player player)
        {
            _player = player;
        }

        /// <summary>
        /// 添加生成点
        /// </summary>
        public void AddSpawnPoint(Vector2 position, float radius)
        {
            _spawnPoints.Add(new SpawnPoint(position, radius));
        }

        /// <summary>
        /// 更新生成系统
        /// </summary>
        public void Update(GameTime gameTime)
        {
            if (_player == null) return;

            _spawnTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            // 检查是否需要生成怪物
            if (_spawnTimer >= _spawnInterval && _currentMonsterCount < _maxMonsters)
            {
                SpawnMonster();
                _spawnTimer = 0f;
            }

            // 更新生成点
            UpdateSpawnPoints();
        }

        /// <summary>
        /// 生成怪物
        /// </summary>
        private void SpawnMonster()
        {
            // 选择生成点
            var spawnPoint = SelectSpawnPoint();
            if (spawnPoint == null) return;

            // 选择怪物类型
            var monsterType = SelectMonsterType();
            Monster monster = null;

            // 创建怪物
            switch (monsterType)
            {
                case MonsterType.Tank:
                    monster = new TankMonster(spawnPoint.Position);
                    break;
                case MonsterType.Explosive:
                    monster = new ExplosiveMonster(spawnPoint.Position);
                    break;
                case MonsterType.Ranged:
                    monster = new RangedMonster(spawnPoint.Position);
                    break;
            }

            if (monster != null)
            {
                // 添加到游戏世界
                // 这里应该通过场景管理器添加怪物
                _currentMonsterCount++;
            }
        }

        /// <summary>
        /// 选择生成点
        /// </summary>
        private SpawnPoint SelectSpawnPoint()
        {
            if (_spawnPoints.Count == 0) return null;

            // 优先选择距离玩家较近的生成点
            var validSpawnPoints = new List<SpawnPoint>();
            foreach (var spawnPoint in _spawnPoints)
            {
                float distanceToPlayer = Vector2.Distance(spawnPoint.Position, _player.Position);
                if (distanceToPlayer >= 100f && distanceToPlayer <= 300f) // 距离玩家100-300像素
                {
                    validSpawnPoints.Add(spawnPoint);
                }
            }

            if (validSpawnPoints.Count == 0)
            {
                // 如果没有合适的生成点，随机选择一个
                return _spawnPoints[_random.Next(_spawnPoints.Count)];
            }

            return validSpawnPoints[_random.Next(validSpawnPoints.Count)];
        }

        /// <summary>
        /// 选择怪物类型
        /// </summary>
        private MonsterType SelectMonsterType()
        {
            // 根据玩家等级调整怪物类型概率
            float tankProbability = 0.3f;
            float explosiveProbability = 0.3f;

            float randomValue = (float)_random.NextDouble();

            if (randomValue < tankProbability)
                return MonsterType.Tank;
            else if (randomValue < tankProbability + explosiveProbability)
                return MonsterType.Explosive;
            else
                return MonsterType.Ranged;
        }

        /// <summary>
        /// 更新生成点
        /// </summary>
        private void UpdateSpawnPoints()
        {
            // 根据玩家位置动态调整生成点
            // 这里可以实现更复杂的生成逻辑
        }

        /// <summary>
        /// 怪物死亡时调用
        /// </summary>
        public void OnMonsterDeath()
        {
            _currentMonsterCount--;
        }

        /// <summary>
        /// 设置生成参数
        /// </summary>
        public void SetSpawnParameters(float interval, int maxMonsters)
        {
            _spawnInterval = interval;
            _maxMonsters = maxMonsters;
        }
    }

    /// <summary>
    /// 生成点
    /// </summary>
    public class SpawnPoint
    {
        public Vector2 Position { get; set; }
        public float Radius { get; set; }
        public float LastSpawnTime { get; set; }

        public SpawnPoint(Vector2 position, float radius)
        {
            Position = position;
            Radius = radius;
        }
    }

    /// <summary>
    /// 怪物类型
    /// </summary>
    public enum MonsterType
    {
        Tank,
        Explosive,
        Ranged
    }
}
