using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary.Graphics;
namespace MonoGameLibrary;
public class Scene : IDisposable
{
    /// <summary>
    /// 内容加载器
    /// </summary>
    protected ContentManager content { get; }
    /// <summary>
    /// 场景是否已经卸载
    /// </summary>
    public bool isDisposed { get; private set; }
    public DrawRequest drawRequest;
    public List<GameObject> gameObjects;
    public GameTime gameTime;
    public Scene()
    {
        content = new ContentManager(Core.Content.ServiceProvider);
        content.RootDirectory = Core.Content.RootDirectory;
        drawRequest = new DrawRequest();
        gameObjects = new List<GameObject>();
    }

    ~Scene() => Dispose(false);

    public virtual void Initialize()
    {
        ///首先加载所有资产
        LoadContent();
        ///根据资产加载结果对组件做初始化
        foreach (var item in gameObjects)
        {
            item.AwakeGameObject();
        }
    }

    public virtual void LoadContent() { }

    public virtual void UnloadContent()
    {
        content.Unload();
    }

    public virtual void Update(GameTime gameTime)
    {
        this.gameTime = gameTime;
        foreach (var item in gameObjects)
        {
            item.UpdateGammeObject();
        }
    }
    public virtual void Draw(GameTime gameTime)
    {
        drawRequest.Draw(Core.SpriteBatch);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (isDisposed)
        {
            return;
        }
        if (disposing)
        {
            UnloadContent();
            content.Dispose();
        }
    }

    public GameObject GetGameObject(string name)
    {
        GameObject res = null;
        foreach (var item in gameObjects)
        {
            if (item.name == name) res = item;
        }
        return res;
    }

    public GameObject GetGameObjectWithTag(string tag)
    {
        GameObject res = null;
        foreach (var item in gameObjects)
        {
            if (item.tag == tag) res = item;
        }
        return res;
    }

    public void AddGameObject(GameObject gameObject)
    {
        gameObjects.Add(gameObject);
        gameObject.AwakeGameObject();
    }

    public void DeleteGameObject(GameObject gameObject)
    {
        if(gameObjects.Contains(gameObject))
        {
            gameObjects.Remove(gameObject);
        }
    }

    public void ActiveGameObject(GameObject gameObject)
    {
        if (gameObjects.Contains(gameObject))
        {
            gameObject.isActive = true;
            gameObject.StartGameObject();
        }
    }

    public void NegetiveGameObject(GameObject gameObject)
    {
        if (gameObjects.Contains(gameObject))
        {
            gameObject.isActive = false;
        }
    }
}

public class DrawRequest
{
    public List<Sprite> sprites;
    public List<Vector2> positions;

    public DrawRequest()
    {
        sprites = new List<Sprite>();
        positions = new List<Vector2>();
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        for (int i = 0; i < sprites.Count; i++)
        {
            sprites[i].Draw(spriteBatch, positions[i]);
        }
        sprites.Clear();
        positions.Clear();
    }

    public void RequestDraw(Sprite sprite, Vector2 position)
    {
        sprites.Add(sprite);
        positions.Add(position);
    }
    
    
}