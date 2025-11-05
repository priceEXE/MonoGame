using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary.Entities;
using MonoGameLibrary;
using MonoGameLibrary.GameCore;

namespace MonoGameLibrary.GameCore
{
    /// <summary>
    /// UI系统，管理游戏界面
    /// </summary>
    public class UISystem
    {
        private Dictionary<string, UIElement> _uiElements;
        private SpriteFont _defaultFont;
        private Player _player;

        public UISystem()
        {
            _uiElements = new Dictionary<string, UIElement>();
        }

        /// <summary>
        /// 设置玩家引用
        /// </summary>
        public void SetPlayer(Player player)
        {
            _player = player;
        }

        /// <summary>
        /// 设置默认字体
        /// </summary>
        public void SetDefaultFont(SpriteFont font)
        {
            _defaultFont = font;
        }

        /// <summary>
        /// 添加UI元素
        /// </summary>
        public void AddUIElement(string name, UIElement element)
        {
            _uiElements[name] = element;
        }

        /// <summary>
        /// 更新UI系统
        /// </summary>
        public void Update(GameTime gameTime)
        {
            foreach (var element in _uiElements.Values)
            {
                element.Update(gameTime);
            }
        }

        /// <summary>
        /// 绘制UI
        /// </summary>
        public void Draw(SpriteBatch spriteBatch)
        {
            // 根据游戏状态绘制不同的UI
            switch (GameEngine.CurrentState)
            {
                case GameEngine.GameState.Title:
                    DrawTitleUI(spriteBatch);
                    break;
                case GameEngine.GameState.Playing:
                    DrawGameUI(spriteBatch);
                    break;
                case GameEngine.GameState.Paused:
                    DrawPauseUI(spriteBatch);
                    break;
                case GameEngine.GameState.GameOver:
                    DrawGameOverUI(spriteBatch);
                    break;
                case GameEngine.GameState.Victory:
                    DrawVictoryUI(spriteBatch);
                    break;
            }
        }

        /// <summary>
        /// 绘制标题界面 - 要要求：背景，标题（游戏名），开始游戏按键，选项按键，离开游戏按键
        /// </summary>
        private void DrawTitleUI(SpriteBatch spriteBatch)
        {
            if (_defaultFont == null) return;

            // 游戏标题
            string title = "Dungeon Slime";
            Vector2 titleSize = _defaultFont.MeasureString(title);
            Vector2 titlePosition = new Vector2(640 - titleSize.X / 2, 200);
            spriteBatch.DrawString(_defaultFont, title, titlePosition, Color.White);

            // 开始游戏按钮
            string startText = "开始游戏";
            Vector2 startSize = _defaultFont.MeasureString(startText);
            Vector2 startPosition = new Vector2(640 - startSize.X / 2, 300);
            spriteBatch.DrawString(_defaultFont, startText, startPosition, Color.Yellow);

            // 选项按键
            string optionsText = "选项";
            Vector2 optionsSize = _defaultFont.MeasureString(optionsText);
            Vector2 optionsPosition = new Vector2(640 - optionsSize.X / 2, 350);
            spriteBatch.DrawString(_defaultFont, optionsText, optionsPosition, Color.White);

            // 离开游戏按钮
            string exitText = "离开游戏";
            Vector2 exitSize = _defaultFont.MeasureString(exitText);
            Vector2 exitPosition = new Vector2(640 - exitSize.X / 2, 400);
            spriteBatch.DrawString(_defaultFont, exitText, exitPosition, Color.White);
        }

        /// <summary>
        /// 绘制游戏界面 - 要要求：左上角有角色血量，当前武器，弹夹状态，经验条，等级数，左下角或右下角可以挂当前持有buff图标，正上方可以放当前积分和倒计时
        /// </summary>
        private void DrawGameUI(SpriteBatch spriteBatch)
        {
            if (_defaultFont == null || _player == null) return;

            // 左上角：玩家血量条
            DrawHealthBar(spriteBatch, new Vector2(50, 50));

            // 左上角：经验条
            DrawExperienceBar(spriteBatch, new Vector2(50, 80));

            // 左上角：等级显示
            string levelText = $"等级: {_player.Level}";
            spriteBatch.DrawString(_defaultFont, levelText, new Vector2(50, 110), Color.White);

            // 左上角：当前武器信息
            if (_player.CurrentWeapon != null)
            {
                string weaponText = $"武器: {_player.CurrentWeapon.Name}";
                spriteBatch.DrawString(_defaultFont, weaponText, new Vector2(50, 140), Color.White);

                if (_player.CurrentWeapon is RangedWeapon rangedWeapon)
                {
                    string ammoText = $"弹药: {rangedWeapon.CurrentAmmo}/{rangedWeapon.MaxAmmo}";
                    spriteBatch.DrawString(_defaultFont, ammoText, new Vector2(50, 170), Color.White);
                }
            }

            // 左下角：增益效果显示
            DrawBuffs(spriteBatch, new Vector2(50, 600));

            // 正上方：当前积分和倒计时
            string scoreText = "分数: 0";
            Vector2 scoreSize = _defaultFont.MeasureString(scoreText);
            spriteBatch.DrawString(_defaultFont, scoreText, new Vector2(640 - scoreSize.X / 2, 50), Color.White);

            string timeText = "时间: 00:00";
            Vector2 timeSize = _defaultFont.MeasureString(timeText);
            spriteBatch.DrawString(_defaultFont, timeText, new Vector2(640 - timeSize.X / 2, 80), Color.White);
        }

        /// <summary>
        /// 绘制血量条
        /// </summary>
        private void DrawHealthBar(SpriteBatch spriteBatch, Vector2 position)
        {
            float barWidth = 200f;
            float barHeight = 20f;
            float healthPercentage = _player.CurrentHealth / _player.MaxHealth;

            // 背景 - 使用简单的矩形绘制
            Rectangle backgroundRect = new Rectangle((int)position.X, (int)position.Y, (int)barWidth, (int)barHeight);
            // 这里应该使用实际的纹理，暂时用注释表示
            // spriteBatch.Draw(healthBarTexture, backgroundRect, Color.DarkRed);

            // 血量
            Rectangle healthRect = new Rectangle((int)position.X, (int)position.Y, (int)(barWidth * healthPercentage), (int)barHeight);
            // spriteBatch.Draw(healthBarTexture, healthRect, Color.Red);
        }

        /// <summary>
        /// 绘制经验条
        /// </summary>
        private void DrawExperienceBar(SpriteBatch spriteBatch, Vector2 position)
        {
            float barWidth = 200f;
            float barHeight = 15f;
            float expPercentage = _player.Experience / (_player.Level * 100f);

            // 背景 - 使用简单的矩形绘制
            Rectangle backgroundRect = new Rectangle((int)position.X, (int)position.Y, (int)barWidth, (int)barHeight);
            // spriteBatch.Draw(experienceBarTexture, backgroundRect, Color.DarkBlue);

            // 经验
            Rectangle expRect = new Rectangle((int)position.X, (int)position.Y, (int)(barWidth * expPercentage), (int)barHeight);
            // spriteBatch.Draw(experienceBarTexture, expRect, Color.Blue);
        }

        /// <summary>
        /// 绘制增益效果
        /// </summary>
        private void DrawBuffs(SpriteBatch spriteBatch, Vector2 position)
        {
            if (_player.ActiveBuffs.Count == 0) return;

            string buffsText = "增益效果:";
            spriteBatch.DrawString(_defaultFont, buffsText, position, Color.White);

            for (int i = 0; i < _player.ActiveBuffs.Count; i++)
            {
                var buff = _player.ActiveBuffs[i];
                string buffText = $"- {buff.GetDescription()} ({buff.RemainingTime:F1}s)";
                Vector2 buffPosition = position + new Vector2(0, 25 + i * 20);
                spriteBatch.DrawString(_defaultFont, buffText, buffPosition, Color.Yellow);
            }
        }

        /// <summary>
        /// 绘制暂停界面 - 要要求：显示返回游戏按钮，退出游戏选项，返回标题界面选项，还要显示当前玩家的属性，持有BUFF等
        /// </summary>
        private void DrawPauseUI(SpriteBatch spriteBatch)
        {
            if (_defaultFont == null) return;

            // 半透明背景 - 使用简单的矩形绘制
            Rectangle backgroundRect = new Rectangle(0, 0, 1280, 720);
            // spriteBatch.Draw(backgroundTexture, backgroundRect, Color.Black * 0.5f);

            // 暂停文本
            string pauseText = "游戏暂停";
            Vector2 pauseSize = _defaultFont.MeasureString(pauseText);
            Vector2 pausePosition = new Vector2(640 - pauseSize.X / 2, 200);
            spriteBatch.DrawString(_defaultFont, pauseText, pausePosition, Color.White);

            // 继续游戏按钮
            string resumeText = "继续游戏";
            Vector2 resumeSize = _defaultFont.MeasureString(resumeText);
            Vector2 resumePosition = new Vector2(640 - resumeSize.X / 2, 250);
            spriteBatch.DrawString(_defaultFont, resumeText, resumePosition, Color.Yellow);

            // 返回标题界面选项
            string menuText = "返回标题界面";
            Vector2 menuSize = _defaultFont.MeasureString(menuText);
            Vector2 menuPosition = new Vector2(640 - menuSize.X / 2, 300);
            spriteBatch.DrawString(_defaultFont, menuText, menuPosition, Color.White);

            // 退出游戏选项
            string exitText = "退出游戏";
            Vector2 exitSize = _defaultFont.MeasureString(exitText);
            Vector2 exitPosition = new Vector2(640 - exitSize.X / 2, 350);
            spriteBatch.DrawString(_defaultFont, exitText, exitPosition, Color.White);

            // 显示当前玩家的属性
            if (_player != null)
            {
                string healthText = $"血量: {_player.CurrentHealth:F0}/{_player.MaxHealth:F0}";
                spriteBatch.DrawString(_defaultFont, healthText, new Vector2(50, 400), Color.White);

                string levelText = $"等级: {_player.Level}";
                spriteBatch.DrawString(_defaultFont, levelText, new Vector2(50, 430), Color.White);

                string expText = $"经验: {_player.Experience:F0}";
                spriteBatch.DrawString(_defaultFont, expText, new Vector2(50, 460), Color.White);

                // 显示当前持有的BUFF
                if (_player.ActiveBuffs.Count > 0)
                {
                    string buffsText = "当前BUFF:";
                    spriteBatch.DrawString(_defaultFont, buffsText, new Vector2(50, 490), Color.Yellow);
                    
                    for (int i = 0; i < _player.ActiveBuffs.Count; i++)
                    {
                        var buff = _player.ActiveBuffs[i];
                        string buffText = $"- {buff.GetDescription()} ({buff.RemainingTime:F1}s)";
                        spriteBatch.DrawString(_defaultFont, buffText, new Vector2(70, 520 + i * 20), Color.White);
                    }
                }
            }
        }

        /// <summary>
        /// 绘制结算界面 - 要要求：如果倒计时结束前死亡，显示存活时间和得分，如果到倒计时结束，只显示当前分数并且恭喜通关
        /// </summary>
        private void DrawGameOverUI(SpriteBatch spriteBatch)
        {
            if (_defaultFont == null) return;

            // 半透明背景
            Rectangle backgroundRect = new Rectangle(0, 0, 1280, 720);
            // spriteBatch.Draw(backgroundTexture, backgroundRect, Color.Black * 0.7f);

            // 游戏结束文本
            string gameOverText = "游戏结束";
            Vector2 gameOverSize = _defaultFont.MeasureString(gameOverText);
            Vector2 gameOverPosition = new Vector2(640 - gameOverSize.X / 2, 250);
            spriteBatch.DrawString(_defaultFont, gameOverText, gameOverPosition, Color.Red);

            // 存活时间（死亡情况）
            string survivalText = "存活时间: 00:00";
            Vector2 survivalSize = _defaultFont.MeasureString(survivalText);
            Vector2 survivalPosition = new Vector2(640 - survivalSize.X / 2, 300);
            spriteBatch.DrawString(_defaultFont, survivalText, survivalPosition, Color.White);

            // 最终分数
            string scoreText = "最终分数: 0";
            Vector2 scoreSize = _defaultFont.MeasureString(scoreText);
            Vector2 scorePosition = new Vector2(640 - scoreSize.X / 2, 330);
            spriteBatch.DrawString(_defaultFont, scoreText, scorePosition, Color.White);

            // 重新开始按钮
            string restartText = "重新开始";
            Vector2 restartSize = _defaultFont.MeasureString(restartText);
            Vector2 restartPosition = new Vector2(640 - restartSize.X / 2, 400);
            spriteBatch.DrawString(_defaultFont, restartText, restartPosition, Color.Yellow);
        }

        /// <summary>
        /// 绘制胜利界面 - 要要求：如果到倒计时结束，只显示当前分数并且恭喜通关
        /// </summary>
        private void DrawVictoryUI(SpriteBatch spriteBatch)
        {
            if (_defaultFont == null) return;

            // 半透明背景
            Rectangle backgroundRect = new Rectangle(0, 0, 1280, 720);
            // spriteBatch.Draw(backgroundTexture, backgroundRect, Color.Black * 0.7f);

            // 恭喜通关文本
            string victoryText = "恭喜通关！";
            Vector2 victorySize = _defaultFont.MeasureString(victoryText);
            Vector2 victoryPosition = new Vector2(640 - victorySize.X / 2, 300);
            spriteBatch.DrawString(_defaultFont, victoryText, victoryPosition, Color.Gold);

            // 当前分数
            string scoreText = "当前分数: 0";
            Vector2 scoreSize = _defaultFont.MeasureString(scoreText);
            Vector2 scorePosition = new Vector2(640 - scoreSize.X / 2, 350);
            spriteBatch.DrawString(_defaultFont, scoreText, scorePosition, Color.White);
        }
    }

    /// <summary>
    /// UI元素基类
    /// </summary>
    public abstract class UIElement
    {
        public Vector2 Position { get; set; }
        public bool IsVisible { get; set; } = true;
        public bool IsActive { get; set; } = true;

        public virtual void Update(GameTime gameTime)
        {
        }

        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
