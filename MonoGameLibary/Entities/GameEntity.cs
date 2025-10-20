using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary.Graphics;

namespace MonoGameLibrary.Entities
{
    /// <summary>
    /// 游戏实体基类，所有游戏对象的基类
    /// </summary>
    public abstract class GameEntity
    {
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public float Rotation { get; set; }
        public Vector2 Scale { get; set; } = Vector2.One;
        public bool IsActive { get; set; } = true;
        public bool IsVisible { get; set; } = true;
        public string Tag { get; set; } = "";

        // 精灵相关
        protected Sprite _sprite;
        protected AnimatedSprite _animatedSprite;

        public GameEntity()
        {
        }

        public GameEntity(Vector2 position)
        {
            Position = position;
        }

        /// <summary>
        /// 实体加载时调用
        /// </summary>
        public virtual void Load()
        {
        }

        /// <summary>
        /// 实体卸载时调用
        /// </summary>
        public virtual void Unload()
        {
        }

        /// <summary>
        /// 更新实体
        /// </summary>
        public virtual void Update(GameTime gameTime)
        {
            // 更新位置
            Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

            // 更新动画
            _animatedSprite?.Update(gameTime);
        }

        /// <summary>
        /// 绘制实体
        /// </summary>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (_sprite != null)
            {
                _sprite.Draw(spriteBatch, Position);
            }
            else if (_animatedSprite != null)
            {
                _animatedSprite.Draw(spriteBatch, Position);
            }
        }

        /// <summary>
        /// 设置精灵
        /// </summary>
        public void SetSprite(Sprite sprite)
        {
            _sprite = sprite;
            _animatedSprite = null;
        }

        /// <summary>
        /// 设置动画精灵
        /// </summary>
        public void SetAnimatedSprite(AnimatedSprite animatedSprite)
        {
            _animatedSprite = animatedSprite;
            _sprite = null;
        }

        /// <summary>
        /// 获取实体的边界框
        /// </summary>
        public virtual Rectangle GetBounds()
        {
            if (_sprite != null)
            {
                return new Rectangle(
                    (int)Position.X,
                    (int)Position.Y,
                    (int)(_sprite.Width * Scale.X),
                    (int)(_sprite.Height * Scale.Y)
                );
            }
            else if (_animatedSprite != null)
            {
                return new Rectangle(
                    (int)Position.X,
                    (int)Position.Y,
                    (int)(_animatedSprite.Width * Scale.X),
                    (int)(_animatedSprite.Height * Scale.Y)
                );
            }
            return Rectangle.Empty;
        }
    }
}
