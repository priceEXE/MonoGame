using System;
using Microsoft.Xna.Framework;

namespace MonoGameLibrary.Entities
{
    /// <summary>
    /// 增益效果类型
    /// </summary>
    public enum BuffType
    {
        SpeedBoost,     // 加速
        InfiniteAmmo,   // 无限弹药
        HealthRegen,    // 回血
        DamageBoost     // 加伤
    }

    /// <summary>
    /// 增益效果类
    /// </summary>
    public class Buff
    {
        public BuffType Type { get; set; }
        public float Value { get; set; }
        public float Duration { get; set; }
        public float RemainingTime { get; set; }
        public bool IsExpired => RemainingTime <= 0f;

        public Buff(BuffType type, float value, float duration)
        {
            Type = type;
            Value = value;
            Duration = duration;
            RemainingTime = duration;
        }

        /// <summary>
        /// 更新增益效果
        /// </summary>
        public void Update(GameTime gameTime)
        {
            RemainingTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        /// <summary>
        /// 获取增益效果的描述
        /// </summary>
        public string GetDescription()
        {
            switch (Type)
            {
                case BuffType.SpeedBoost:
                    return $"移动速度 +{(Value - 1) * 100:F0}%";
                case BuffType.InfiniteAmmo:
                    return "无限弹药";
                case BuffType.HealthRegen:
                    return $"每秒回复 {Value} 血量";
                case BuffType.DamageBoost:
                    return $"伤害 +{(Value - 1) * 100:F0}%";
                default:
                    return "未知增益";
            }
        }
    }
}
