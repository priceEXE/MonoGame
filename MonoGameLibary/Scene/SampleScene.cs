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
        // 从XML文件创建图集.
        TextureAtlas atlas = TextureAtlas.FromFile(content, "images/atlas-definition.xml");
        //生成示例物体
        slimeObject = Config.Player();
        GameObject EnemyObject = Config.Enemy();
        //获得其上的动画控制器脚本
        Animator slimeA = slimeObject.GetComponent<Animator>();
        //设置动画注册表
        slimeA.RegisterAnimation("slime-animation", atlas.CreateAnimatedSprite("slime-animation"));
        EnemyObject.GetComponent<Animator>().RegisterAnimation("bat-animation", atlas.CreateAnimatedSprite("bat-animation"));
        //设置首个动画
        slimeA.ChangeAnimation("slime-animation");//转换动画
        EnemyObject.GetComponent<Animator>().ChangeAnimation("bat-animation");
        //在场景中实例化这个游戏对象
        GameObject.Institate(slimeObject);
        GameObject.Institate(EnemyObject);
        //创建摄像机实体
        GameObject camera = new GameObject("Camera");
        camera.AddComponent<Camera>().isMain = true;
        GameObject.Institate(camera).position = new Vector2(0,0);
    }
}