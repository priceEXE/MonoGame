using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary.GameCore;
using MonoGameLibrary.Graphics;
using MonoGameLibrary.Input;

namespace MonoGameLibrary.GameCore
{
    /// <summary>
    /// 游戏引擎核心类，管理游戏的主要系统和生命周期
    /// </summary>
    public class GameEngine : Core
    {
        // 系统管理器
        public static SceneManager SceneManager { get; private set; }
        public static AudioManager AudioManager { get; private set; }
        public static CollisionManager CollisionManager { get; private set; }
        public static InputSystem InputSystem { get; private set; }
        public static AISystem AISystem { get; private set; }
        public static SpawnSystem SpawnSystem { get; private set; }
        public static UISystem UISystem { get; private set; }

        /// <summary>
        /// 游戏状态
        /// </summary>
        public enum GameState
        {
            Title,
            Playing,
            Paused,
            GameOver,
            Victory
        }

        public static GameState CurrentState { get; set; } = GameState.Title;

        public GameEngine(string title, int width, int height, bool fullScreen) 
            : base(title, width, height, fullScreen)
        {
        }

        protected override void Initialize()
        {
            base.Initialize();

            // 初始化系统管理器
            SceneManager = new SceneManager();
            AudioManager = new AudioManager();
            CollisionManager = new CollisionManager();
            InputSystem = new InputSystem();
            AISystem = new AISystem();
            SpawnSystem = new SpawnSystem();
            UISystem = new UISystem();
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // 根据游戏状态更新不同系统
            switch (CurrentState)
            {
                case GameState.Playing:
                    SceneManager.Update(gameTime);
                    AISystem.Update(gameTime);
                    SpawnSystem.Update(gameTime);
                    CollisionManager.Update(gameTime);
                    break;
                case GameState.Paused:
                    // 暂停状态下只更新UI
                    break;
            }

            UISystem.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            SpriteBatch.Begin(samplerState: SamplerState.PointClamp);

            // 绘制当前场景
            SceneManager.Draw(SpriteBatch);

            // 绘制UI
            UISystem.Draw(SpriteBatch);

            SpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
