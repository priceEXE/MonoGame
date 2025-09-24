using System;
using Microsoft.Xna.Framework;

namespace MonoGameLibrary.Graphics;

public class AnimatedSprite : Sprite
{
    /// <summary>
    /// 当前帧索引
    /// </summary>
    private int _currentFrameIndex;
    /// <summary>
    /// 帧持续时间
    /// </summary>
    private TimeSpan _elapsed;
    /// <summary>
    /// 动画对象
    /// </summary>
    private Animation _animation;
    /// <summary>
    /// 对外设置动画
    /// </summary>
    public Animation Animation
    {
        get => _animation;
        set
        {
            _animation = value;
            Region = _animation.Framses[0];
        }
    }
    /// <summary>
    /// 构造空的动画精灵
    /// </summary>
    public AnimatedSprite() { }
    /// <summary>
    /// 构造指定动画的动画精灵
    /// </summary>
    /// <param name="animation"></param>
    public AnimatedSprite(Animation animation) : base(animation.Framses[0])
    {
        Animation = animation;
    }
    /// <summary>
    /// 根据游戏时间更新动画
    /// </summary>
    /// <param name="gemeTime"></param>
    public void Update(GameTime gameTime)
    {
        //类似于Unity的Time.deltaTime，累加时间
        _elapsed += gameTime.ElapsedGameTime;
        //当动画持续时间大于设定的延迟时间
        if (_elapsed >= _animation.Delay)
        {
            //重置累计时间
            _elapsed -= Animation.Delay;
            _currentFrameIndex++;
            if (_currentFrameIndex >= _animation.Framses.Count)
            {
                _currentFrameIndex = 0;
            }
            Region = _animation.Framses[_currentFrameIndex];
        }
    }

}
