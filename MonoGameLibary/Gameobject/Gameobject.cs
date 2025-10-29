using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary.Graphics;

namespace MonoGameLibrary;

public class GameObject
{
    public Vector2 position;
    public float Rotation;
    List<GameComponent> gameComponents;
    /// <summary>
    /// 游戏物体所处的场景类
    /// </summary>
    Scene sence;
    public string name;
    public string tag;
    public bool isActive;

    public GameObject(string name,string tag = "Default")
    {
        this.name = name;
        this.tag = tag;
        gameComponents = new List<GameComponent>();
    }

    public void AwakeGameObject()
    {
        foreach (var item in gameComponents)
        {
            item.Awake();
        }
    }

    public void StartGameObject()
    {
        foreach (var item in gameComponents)
        {
            item.Start();
        }
    }
    public void UpdateGammeObject()
    {
        foreach (var item in gameComponents)
        {
            item.Update();
        }
    }

    public GameComponent AddComponent<T>() where T : GameComponent, new()
    {
        GameComponent t = new T();
        gameComponents.Add(t);
        return t;
    }

    public GameComponent GetComponent<T>() where T : GameComponent
    {
        GameComponent t = null;
        foreach (var item in gameComponents)
        {
            if (item.GetType() == typeof(T))
            {
                t = item;
                break;
            }
        }
        return t;
    }

    public void DeleteComponent<T>() where T : GameComponent
    {
        List<GameComponent> toRemove = new List<GameComponent>();
        foreach (var item in gameComponents)
        {
            if (item.GetType() == typeof(T)) toRemove.Add(item);
        }
        foreach (var item in toRemove)
        {
            gameComponents.Remove(item);
        }
        toRemove.Clear();
    }

    public static GameObject FindGameObject(string name)
    {    
        return Core.curScene.GetGameObject(name);;
    }

    public static GameObject FindGameObjectWithTag(string name)
    {
        return Core.curScene.GetGameObjectWithTag(name);
    }

    public static GameObject Institate(GameObject gameObject)
    {
        Core.curScene.AddGameObject(gameObject);
        return gameObject;
    }

    public static void Destory(GameObject gameObject)
    {
        Core.curScene.DeleteGameObject(gameObject);
    }
    

}

public class GameComponent
{
    /// <summary>
    /// 挂载此组件的GameObject
    /// </summary>
    public GameObject gameObject;
    public virtual void Awake() { }
    public virtual void Start() { }
    //对于渲染组件，在Update时向Scnen的渲染中间层递交一个DrawRequest
    public virtual void Update() { }

    public GameComponent() {}
}
