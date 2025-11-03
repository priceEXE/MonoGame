using System;
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

    private float speed = 5;

    private Vector2 position = new Vector2(0, 0);

    public SampleScene(Game game) : base(game) {}
    public override void LoadContent()
    {
        // Create the texture atlas from the XML configuration file.
        TextureAtlas atlas = TextureAtlas.FromFile(content, "images/atlas-definition.xml");

        // Create the slime animated sprite from the atlas.
        _slime = atlas.CreateAnimatedSprite("slime-animation");
        _slime.Scale = new Vector2(4.0f, 4.0f);
        GameObject slimeObject = new GameObject("slime");
        slimeObject.sence = this;
        slimeObject.position = new Vector2(0, 0);
        slime = new SpriteRender(_slime,slimeObject);
        // Create the bat animated sprite from the atlas.
        _bat = atlas.CreateAnimatedSprite("bat-animation");
        _bat.Scale = new Vector2(4.0f, 4.0f);
        GameObject batObject = new GameObject("bat");
        batObject.position = new Vector2(200, 0);
        batObject.sence = this;
        bat = new SpriteRender(_bat,batObject);
        base.LoadContent();
    }

    public override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            game.Exit();
        if (InputManager.GetKeyDown(Keys.S))
        {
            position.Y += speed;
        }
        // Update the slime animated sprite.
        _slime.Update(gameTime);
        slime.Update();
        // Update the bat animated sprite.
        _bat.Update(gameTime);
        bat.Update();
        base.Update(gameTime);
        base.Update(gameTime);
    }

    public override void Draw(GameTime gameTime)
    {
        base.Draw(gameTime);
    }
}