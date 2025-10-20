using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace MonoGameLibrary.GameCore
{
    /// <summary>
    /// 碰撞管理器，负责检测和处理游戏中的碰撞
    /// </summary>
    public class CollisionManager
    {
        private List<Collider> _colliders;

        public CollisionManager()
        {
            _colliders = new List<Collider>();
        }

        /// <summary>
        /// 添加碰撞器
        /// </summary>
        public void AddCollider(Collider collider)
        {
            _colliders.Add(collider);
        }

        /// <summary>
        /// 移除碰撞器
        /// </summary>
        public void RemoveCollider(Collider collider)
        {
            _colliders.Remove(collider);
        }

        /// <summary>
        /// 更新碰撞检测
        /// </summary>
        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < _colliders.Count; i++)
            {
                for (int j = i + 1; j < _colliders.Count; j++)
                {
                    if (_colliders[i].IsActive && _colliders[j].IsActive)
                    {
                        if (CheckCollision(_colliders[i], _colliders[j]))
                        {
                            _colliders[i].OnCollision(_colliders[j]);
                            _colliders[j].OnCollision(_colliders[i]);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 检测两个碰撞器是否碰撞
        /// </summary>
        private bool CheckCollision(Collider a, Collider b)
        {
            if (a.CollisionType == CollisionType.Circle && b.CollisionType == CollisionType.Circle)
            {
                return CheckCircleCollision(a, b);
            }
            else if (a.CollisionType == CollisionType.Rectangle && b.CollisionType == CollisionType.Rectangle)
            {
                return CheckRectangleCollision(a, b);
            }
            else
            {
                // 混合碰撞检测
                return CheckMixedCollision(a, b);
            }
        }

        private bool CheckCircleCollision(Collider a, Collider b)
        {
            float distance = Vector2.Distance(a.Position, b.Position);
            return distance < (a.Radius + b.Radius);
        }

        private bool CheckRectangleCollision(Collider a, Collider b)
        {
            return a.Bounds.Intersects(b.Bounds);
        }

        private bool CheckMixedCollision(Collider a, Collider b)
        {
            // 简化处理：将圆形碰撞器转换为矩形进行检测
            Rectangle circleRect = new Rectangle(
                (int)(a.Position.X - a.Radius),
                (int)(a.Position.Y - a.Radius),
                (int)(a.Radius * 2),
                (int)(a.Radius * 2)
            );
            return circleRect.Intersects(b.Bounds);
        }
    }

    /// <summary>
    /// 碰撞类型
    /// </summary>
    public enum CollisionType
    {
        Rectangle,
        Circle
    }

    /// <summary>
    /// 碰撞器基类
    /// </summary>
    public abstract class Collider
    {
        public Vector2 Position { get; set; }
        public bool IsActive { get; set; } = true;
        public CollisionType CollisionType { get; protected set; }
        public float Radius { get; protected set; }
        public Rectangle Bounds { get; protected set; }

        public abstract void OnCollision(Collider other);
    }
}
