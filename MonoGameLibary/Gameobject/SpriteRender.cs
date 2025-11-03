using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;

public class SpriteRender : MonoGameLibrary.GameComponent
{
    public Sprite curSprite { get; private set; }
    public SpriteRender(Sprite sprite, GameObject gameObject) : base(gameObject)
    {
        curSprite = sprite;
    }
    public override void Update()
    {
        gameObject.sence.drawRequest.RequestDraw(curSprite, gameObject.position);
        base.Update();
    }
    
}