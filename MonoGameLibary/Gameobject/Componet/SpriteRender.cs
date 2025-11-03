using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;

public class SpriteRender : MonoGameLibrary.GameComponent
{
    public Sprite sprite;
    public SpriteRender()
    {
        sprite = null;
    }
    public override void Update()
    {
        if(sprite != null)
        {
            sprite.Scale = gameObject.scale;
            gameObject.sence.drawRequest.RequestDraw(sprite, gameObject.position);
            base.Update();
        }
    }

    
}