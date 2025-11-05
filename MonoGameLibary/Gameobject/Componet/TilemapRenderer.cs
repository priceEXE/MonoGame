using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;

public class TilemapRenderer : MonoGameLibrary.GameComponent
{
    public Tilemap tilemap;
    public TilemapRenderer()
    {
        tilemap = null;
    }
    public override void Update()
    {
        if (tilemap != null)
        {
            tilemap.Scale = gameObject.scale;
            gameObject.sence.drawRequest.RequestDraw(tilemap, gameObject.position);
            base.Update();
        }
    }

    public override object Clone()
    {
        TilemapRenderer tilemapRenderer = new TilemapRenderer();
        tilemapRenderer.tilemap = tilemap;
        return tilemapRenderer;
    }
    
}