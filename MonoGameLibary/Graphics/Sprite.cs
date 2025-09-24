using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameLibrary.Graphics;

public class Sprite
{
    /// <summary>
    /// 精灵的纹理区域
    /// </summary>
    public TextureRegion Region { get; set; }
    /// <summary>
    /// 精灵的颜色蒙版，默认为白色（无蒙版）
    /// </summary>
    public Color Color { get; set; } = Color.White;
    /// <summary>
    /// 精灵的旋转弧度，默认为0
    /// </summary>
    public float Rotation { get; set; } = 0f;
    /// <summary>
    /// 精灵的缩放矢量，默认为(1,1)
    /// </summary>
    public Vector2 Scale { get; set; } = Vector2.One;
    /// <summary>
    /// 精灵的原点，默认为(0,0)
    /// </summary>
    public Vector2 Origin { get; set; } = Vector2.Zero;
    /// <summary>
    /// 精灵的翻转效果
    /// </summary>
    public SpriteEffects Effects { get; set; } = SpriteEffects.None;
    /// <summary>
    /// 精灵的排序深度
    /// </summary>
    public float LayerDepth { get; set; } = 0f;
    /// <summary>
    /// 渲染精灵的宽度
    /// </summary>
    public float Width => Region.Width * Scale.X;
    /// <summary>
    /// 渲染精灵的高度
    /// </summary>
    public float Height => Region.Height * Scale.Y;
    /// <summary>
    /// 默认无参构造函数
    /// </summary>
    public Sprite() { }
    /// <summary>
    /// 根据纹理区域初始化精灵
    /// </summary>
    /// <param name="region"></param>
    public Sprite(TextureRegion region)
    {
        Region = region;
    }
    /// <summary>
    /// 将原点设置到纹理区域的中心
    /// </summary>
    public void CenterOrigion()
    {
        Origin = new Vector2(Region.Width, Region.Height) * 0.5f;
    }
    /// <summary>
    /// 绘制精灵
    /// </summary>
    /// <param name="spriteBatch"></param>
    /// <param name="position"></param>
    public void Draw(SpriteBatch spriteBatch, Vector2 position)
    {
        Region.Draw(spriteBatch,position,Color,Rotation,Origin,Scale,Effects,LayerDepth);
    }

}
