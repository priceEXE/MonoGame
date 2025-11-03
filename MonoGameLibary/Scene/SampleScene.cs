using Microsoft.Xna.Framework;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using ConfigSpace;
using System.Collections.Generic;
public class SampleScene : Scene
{
    private GameObject slimeObject;
    /// <summary>
    /// 所有此场景下需要生成的prefabs物体
    /// todo：从配置文件反序列化这些物体
    /// </summary>
    private List<GameObject> prefabs;
    /// <summary>
    /// 构造函数，包括当前游戏进程的依赖注入
    /// </summary>
    /// <param name="game"></param>
    public SampleScene(Game game) : base(game) { }
    /// <summary>
    /// 场景子类只需定义如何加载和配置资源，以及生成实体，无需定义生命周期函数的调用
    /// </summary>
    public override void LoadContent()
    {
        // Create the texture atlas from the XML configuration file.
        TextureAtlas atlas = TextureAtlas.FromFile(content, "images/atlas-definition.xml");
        slimeObject = Config.SampleObject();
        slimeObject.AddComponent<UnitMove>();
        slimeObject.AddComponent<PlayerController>();
        Animator slimeA = slimeObject.GetComponent<Animator>();
        slimeA.RegisterAnimation("slime-animation", atlas.CreateAnimatedSprite("slime-animation"));//设置注册表
        slimeA.RegisterAnimation("bat-animation", atlas.CreateAnimatedSprite("bat-animation"));//设置注册表
        slimeA.ChangeAnimation("slime-animation");//转换动画
        GameObject.Institate(slimeObject);
    }
}