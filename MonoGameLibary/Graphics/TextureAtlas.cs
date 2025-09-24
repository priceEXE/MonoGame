using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameLibrary.Graphics;
/// <summary>
/// 纹理图集，可以将单个纹理切分为多个纹理区域，并创建相应的精灵和动画
/// </summary>
public class TextureAtlas
{
    private Dictionary<string, TextureRegion> _regions;
    private Dictionary<string, Animation> _animations;

    /// <summary>
    /// 纹理图集缓存的纹理
    /// </summary>
    public Texture2D Texture { get; set; }

    /// <summary>
    /// 默认构造一个纹理为空的纹理图集
    /// </summary>
    public TextureAtlas()
    {
        _regions = new Dictionary<string, TextureRegion>();
        _animations = new Dictionary<string, Animation>();
    }

    /// <summary>
    /// 使用指定的纹理创建一个新的纹理图集
    /// </summary>
    /// <param name="texture">纹理</param>
    public TextureAtlas(Texture2D texture)
    {
        Texture = texture;
        _regions = new Dictionary<string, TextureRegion>();
        _animations = new Dictionary<string, Animation>();
    }

    /// <summary>
    /// 将指定的纹理区域添加到此纹理图集中
    /// </summary>
    /// <param name="name">纹理区域的标签名称</param>
    /// <param name="x">区域左上方点的x坐标</param>
    /// <param name="y">区域左上方点的y坐标</param>
    /// <param name="width">区域宽度</param>
    /// <param name="height">区域高度</param>
    public void AddRegion(string name, int x, int y, int width, int height)
    {
        TextureRegion region = new TextureRegion(Texture, x, y, width, height);
        _regions.Add(name, region);
    }

    /// <summary>
    /// 通过标签名称获得纹理区域
    /// </summary>
    /// <param name="name">纹理标签名称</param>
    /// <returns>名称绑定的标签名称</returns>
    public TextureRegion GetRegion(string name)
    {
        return _regions[name];
    }

    /// <summary>
    /// 通过标签名称移除纹理区域
    /// </summary>
    /// <param name="name">纹理标签名称</param>
    /// <returns></returns>
    public bool RemoveRegion(string name)
    {
        return _regions.Remove(name);
    }

    /// <summary>
    /// 清空所有纹理区域
    /// </summary>
    public void Clear()
    {
        _regions.Clear();
    }
    /// <summary>
    /// 通过标签名称创建一个精灵
    /// </summary>
    /// <param name="regionName"></param>
    /// <returns></returns>
    public Sprite CreateSprite(string regionName)
    {
        return new Sprite(GetRegion(regionName));
    }
    /// <summary>
    /// 通过动画名称创建一个动画精灵
    /// </summary>
    /// <param name="animationName"></param>
    /// <returns></returns>
    public AnimatedSprite CreateAnimatedSprite(string animationName)
    {
        return new AnimatedSprite(GetAnimation(animationName));
    }
    /// <summary>
    /// 添加动画
    /// </summary>
    /// <param name="name">动画名称</param>
    /// <param name="animation">动画类</param>
    public void AddAnimation(string name, Animation animation)
    {
        _animations.Add(name, animation);
    }
    /// <summary>
    /// 获取动画
    /// </summary>
    /// <param name="name">动画名称</param>
    /// <returns></returns>
    public Animation GetAnimation(string name)
    {
        return _animations[name];
    }
    /// <summary>
    /// 移除动画
    /// </summary>
    /// <param name="name">动画名称</param>
    /// <returns>是否成功移除</returns>
    public bool RemoveAnimation(string name)
    {
        return _animations.Remove(name);
    }
    /// <summary>
    /// 基于xml的配置文件创建纹理图集
    /// </summary>
    /// <param name="content">用于加载xml文件的ContentManager对象</param>
    /// <param name="fileName">xml文件名</param>
    /// <returns></returns>
    public static TextureAtlas FromFile(ContentManager content, string fileName)
{
    TextureAtlas atlas = new TextureAtlas();

    string filePath = Path.Combine(content.RootDirectory, fileName);

    using (Stream stream = TitleContainer.OpenStream(filePath))
    {
        using (XmlReader reader = XmlReader.Create(stream))
        {
            XDocument doc = XDocument.Load(reader);
            XElement root = doc.Root;

            // The <Texture> element contains the content path for the Texture2D to load.
            // So we will retrieve that value then use the content manager to load the texture.
            string texturePath = root.Element("Texture").Value;
            atlas.Texture = content.Load<Texture2D>(texturePath);

            // The <Regions> element contains individual <Region> elements, each one describing
            // a different texture region within the atlas.  
            //
            // Example:
            // <Regions>
            //      <Region name="spriteOne" x="0" y="0" width="32" height="32" />
            //      <Region name="spriteTwo" x="32" y="0" width="32" height="32" />
            // </Regions>
            //
            // So we retrieve all of the <Region> elements then loop through each one
            // and generate a new TextureRegion instance from it and add it to this atlas.
            var regions = root.Element("Regions")?.Elements("Region");

            if (regions != null)
            {
                foreach (var region in regions)
                {
                    string name = region.Attribute("name")?.Value;
                    int x = int.Parse(region.Attribute("x")?.Value ?? "0");
                    int y = int.Parse(region.Attribute("y")?.Value ?? "0");
                    int width = int.Parse(region.Attribute("width")?.Value ?? "0");
                    int height = int.Parse(region.Attribute("height")?.Value ?? "0");

                    if (!string.IsNullOrEmpty(name))
                    {
                        atlas.AddRegion(name, x, y, width, height);
                    }
                }
            }

            // The <Animations> element contains individual <Animation> elements, each one describing
            // a different animation within the atlas.
            //
            // Example:
            // <Animations>
            //      <Animation name="animation" delay="100">
            //          <Frame region="spriteOne" />
            //          <Frame region="spriteTwo" />
            //      </Animation>
            // </Animations>
            //
            // So we retrieve all of the <Animation> elements then loop through each one
            // and generate a new Animation instance from it and add it to this atlas.
            var animationElements = root.Element("Animations").Elements("Animation");

            if (animationElements != null)
            {
                foreach (var animationElement in animationElements)
                {
                    string name = animationElement.Attribute("name")?.Value;
                    float delayInMilliseconds = float.Parse(animationElement.Attribute("delay")?.Value ?? "0");
                    TimeSpan delay = TimeSpan.FromMilliseconds(delayInMilliseconds);

                    List<TextureRegion> frames = new List<TextureRegion>();

                    var frameElements = animationElement.Elements("Frame");

                    if (frameElements != null)
                    {
                        foreach (var frameElement in frameElements)
                        {
                            string regionName = frameElement.Attribute("region").Value;
                            TextureRegion region = atlas.GetRegion(regionName);
                            frames.Add(region);
                        }
                    }

                    Animation animation = new Animation(frames, delay);
                    atlas.AddAnimation(name, animation);
                }
            }

            return atlas;
        }
    }
}

}
