using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary.Entities;

namespace MonoGameLibrary.GameCore
{
    /// <summary>
    /// 场景管理器，负责管理游戏中的不同场景
    /// </summary>
    public class SceneManager
    {
        private Dictionary<string, Scene> _scenes;
        private Scene _currentScene;

        public SceneManager()
        {
            _scenes = new Dictionary<string, Scene>();
        }

        /// <summary>
        /// 添加场景
        /// </summary>
        public void AddScene(string name, Scene scene)
        {
            _scenes[name] = scene;
        }

        /// <summary>
        /// 切换场景
        /// </summary>
        public void ChangeScene(string name)
        {
            if (_scenes.ContainsKey(name))
            {
                _currentScene?.Unload();
                _currentScene = _scenes[name];
                _currentScene?.Load();
            }
        }

        /// <summary>
        /// 获取当前场景
        /// </summary>
        public Scene GetCurrentScene()
        {
            return _currentScene;
        }

        /// <summary>
        /// 更新当前场景
        /// </summary>
        public void Update(GameTime gameTime)
        {
            _currentScene?.Update(gameTime);
        }

        /// <summary>
        /// 绘制当前场景
        /// </summary>
        public void Draw(SpriteBatch spriteBatch)
        {
            _currentScene?.Draw(spriteBatch);
        }
    }

    /// <summary>
    /// 场景基类
    /// </summary>
    public abstract class Scene
    {
        protected List<GameEntity> _entities;

        public Scene()
        {
            _entities = new List<GameEntity>();
        }

        /// <summary>
        /// 场景加载时调用
        /// </summary>
        public virtual void Load()
        {
            foreach (var entity in _entities)
            {
                entity.Load();
            }
        }

        /// <summary>
        /// 场景卸载时调用
        /// </summary>
        public virtual void Unload()
        {
            foreach (var entity in _entities)
            {
                entity.Unload();
            }
            _entities.Clear();
        }

        /// <summary>
        /// 添加实体到场景
        /// </summary>
        public void AddEntity(GameEntity entity)
        {
            _entities.Add(entity);
        }

        /// <summary>
        /// 从场景移除实体
        /// </summary>
        public void RemoveEntity(GameEntity entity)
        {
            _entities.Remove(entity);
        }

        /// <summary>
        /// 更新场景
        /// </summary>
        public virtual void Update(GameTime gameTime)
        {
            for (int i = _entities.Count - 1; i >= 0; i--)
            {
                if (_entities[i].IsActive)
                {
                    _entities[i].Update(gameTime);
                }
                else
                {
                    _entities.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// 绘制场景
        /// </summary>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            foreach (var entity in _entities)
            {
                if (entity.IsVisible)
                {
                    entity.Draw(spriteBatch);
                }
            }
        }
    }
}
