using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;

namespace MonoGameLibrary.Physics2D;


public class Collider2D : MonoGameLibrary.GameComponent
{
    private Circle circle;
    public Circle Circle
    {
        get
        {
            return circle;
        }
    }
    public Collider2D() {}
    public Collider2D(Circle circle)
    {
        this.circle = circle;
    }
    public float radius => circle.radius;
    public override void Update()
    {
        ///更新圆的位置
        SpriteRender spriteRender = gameObject.GetComponent<SpriteRender>();
        if(spriteRender!=null && spriteRender.sprite!=null)
        {
        circle.x = gameObject.position.X;
        circle.y = gameObject.position.Y;
        circle.radius = spriteRender.sprite.Width  * .5f;
        gameObject.sence.RegisterCollisionCheck(this);
            
        }
    }
    public bool CheckCollision(Collider2D other)
    {
        return circle.Intersects(other.Circle);
    }

    public override object Clone()
    {
        return new Collider2D(this.Circle);
    }
}
