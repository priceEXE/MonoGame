using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoGameLibrary;
namespace ConfigSpace;
public class Config
{
    public static Func<GameObject> Player = () =>
    {

        GameObject gameObject = new GameObject("Player");
        gameObject.AddComponent<SpriteRender>();
        gameObject.AddComponent<Animator>();
        gameObject.scale = new Vector2(4f, 4f);
        gameObject.position = Vector2.Zero;
        gameObject.AddComponent<UnitMove>();
        gameObject.AddComponent<PlayerController>();
        return gameObject;
    };

    public static Func<GameObject> Enemy = () =>
    {
        GameObject gameObject = new GameObject("Enemy");
        gameObject.AddComponent<SpriteRender>();
        gameObject.AddComponent<Animator>();
        gameObject.AddComponent<SimpleAI>();
        gameObject.scale = new Vector2(4f, 4f);
        gameObject.position = Vector2.Zero;
        gameObject.AddComponent<UnitMove>();
        return gameObject;
    };

    public static Func<GameObject> GameManager = () =>
    {
        GameObject gameObject = new GameObject("GameManager");
        gameObject.AddComponent<GameManager>();
        return gameObject;
    };
    public static Func<GameObject> TilemapObject = () =>
    {
        GameObject gameObject = new GameObject("Tilemap");
        gameObject.AddComponent<TilemapRenderer>();
        gameObject.scale = new Vector2(4f, 4f);
        gameObject.position = new Vector2(-640, +360);
        return gameObject;
    };
}