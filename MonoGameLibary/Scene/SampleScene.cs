using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using MonoGameLibrary.Input;
using ConfigSpace;
public class SampleScene : Scene
{
    private float speed = 5;
    private GameObject slimeObject;
    private Vector2 position = new Vector2(0, 0);

    public SampleScene(Game game) : base(game) {}
    public override void LoadContent()
    {
        base.LoadContent();
        // Create the texture atlas from the XML configuration file.
        TextureAtlas atlas = TextureAtlas.FromFile(content, "images/atlas-definition.xml");
        slimeObject = Config.SampleObject();
        slimeObject.AddComponent<UnitMove>();
        Animator slimeA = slimeObject.GetComponent<Animator>();
        slimeA.RegisterAnimation("slime-animation", atlas.CreateAnimatedSprite("slime-animation"));//设置注册表
        slimeA.RegisterAnimation("bat-animation", atlas.CreateAnimatedSprite("bat-animation"));//设置注册表
        slimeA.ChangeAnimation("slime-animation");//转换动画
        GameObject.Institate(slimeObject);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }

    public override void Draw(GameTime gameTime)
    {
        base.Draw(gameTime);
    }
}