using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using MonoGameLibrary.Input;
public class SampleScene : Scene
{
    // Defines the slime animated sprite.
    private AnimatedSprite _slime;

    // Defines the bat animated sprite.
    private AnimatedSprite _bat;

    private SpriteRender slime;

    private SpriteRender bat;

    private Animator slimeA;

    private float speed = 5;
    private GameObject slimeObject;
    private Vector2 position = new Vector2(0, 0);

    public SampleScene(Game game) : base(game) {}
    public override void LoadContent()
    {
        // Create the texture atlas from the XML configuration file.
        TextureAtlas atlas = TextureAtlas.FromFile(content, "images/atlas-definition.xml");

        // Create the slime animated sprite from the atlas.
        _slime = atlas.CreateAnimatedSprite("slime-animation");
        _slime.Scale = new Vector2(4.0f, 4.0f);
        slimeObject = new GameObject("slime");//创建实体
        
        slimeA = slimeObject.AddComponent<Animator>();
        //slime = slimeObject.AddComponent<SpriteRender>();
        slimeA.RegisterAnimation("slime-animation", atlas.CreateAnimatedSprite("slime-animation"));//设置注册表
        slimeA.RegisterAnimation("bat-animation", atlas.CreateAnimatedSprite("bat-animation"));//设置注册表
        slimeA.ChangeAnimation("slime-animation");//转换动画
        
        slimeObject.sence = this;
        slimeObject.position = new Vector2(0, 0);
        // Create the bat animated sprite from the atlas.
        if (slime == null)
        {
            _bat = atlas.CreateAnimatedSprite("bat-animation");
            
        }
        else
        {
            _bat = atlas.CreateAnimatedSprite("slime-animation");
        }
        _bat.Scale = new Vector2(4.0f, 4.0f);
        base.LoadContent();
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        if (InputManager.GetKeyDown(Keys.S))
        {
            position.Y += speed;
        }
        if(InputManager.GetMouseButtonDown(MouseButton.Left))
        {
            slimeObject.scale = new Vector2(4f, 4f);
            slimeA.ChangeAnimation("bat-animation");//转换动画
        }
        // Update the slime animated sprite.
        //_slime.Update(gameTime);
        //slimeObject.UpdateGammeObject();
        // Update the bat animated sprite.
        _bat.Update(gameTime);
        drawRequest.RequestDraw(_bat, new Vector2(0, 0));
        slimeObject.position = new Vector2(200, 0);
        slimeObject.UpdateGammeObject();
    }

    public override void Draw(GameTime gameTime)
    {
        base.Draw(gameTime);
    }
}