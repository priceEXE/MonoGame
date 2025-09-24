using System;
using System.Collections.Generic;
namespace MonoGameLibrary.Graphics;

public class Animation
{
    /// <summary>
    /// 动画精灵纹理区域的列表
    /// </summary>
    public List<TextureRegion> Framses { get; set; }
    /// <summary>
    /// 动画帧的延迟时间
    /// </summary>
    public TimeSpan Delay { get; set; }
    /// <summary>
    /// 无参构造函数，默认100毫秒延迟
    /// </summary>
    public Animation()
    {
        Framses = new List<TextureRegion>();
        Delay = TimeSpan.FromMilliseconds(100);
    }
    /// <summary>
    /// 有参构造函数
    /// </summary>
    /// <param name="framses">动画纹理区域的列表</param>
    /// <param name="delay">动画的延迟</param>
    public Animation(List<TextureRegion> framses, TimeSpan delay)
    {
        Framses = framses;
        Delay = delay;
    }
}