using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoGameLibrary;
namespace ConfigSpace;
public class Config
{
    public static Func<GameObject> SampleObject = () =>
    {
        
        GameObject gameObject = new GameObject("sampleObject");
        gameObject.AddComponent<SpriteRender>();
        gameObject.AddComponent<Animator>();
        gameObject.scale = new Vector2(4f, 4f);
        gameObject.position = Vector2.Zero;
        return gameObject;
    };
}