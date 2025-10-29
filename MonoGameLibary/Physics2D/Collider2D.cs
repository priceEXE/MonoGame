using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace MonoGameLibrary.Physics2D;
public readonly struct Collision2D
{
    public readonly Collider2D collider;
    
}


public abstract class Collider2D
{
    public bool isTrigger;
    public virtual void OnCollisonEnter(Collision2D collision2D) { }
    public virtual void OnCollisonStay(Collision2D collision2D) { }
    public virtual void OnCollisonExit(Collision2D collision2D) { }
    public virtual void OnTriggerEnter(Collision2D collision2D) { }
    public virtual void OnTriggerStay(Collision2D collision2D) { }
    public virtual void OnTriggerExit(Collision2D collision2D) {}
}
