using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary.Graphics;

namespace MonoGameLibrary;
/// <summary>
/// 游戏实体类
/// </summary>
public class GameObject
{
    /// <summary>
    /// 实体坐标
    /// </summary>
    public Vector2 position;
    /// <summary>
    /// 实体旋转角度
    /// </summary>
    public float Rotation;
    /// <summary>
    /// 实体尺寸
    /// </summary>
    public Vector2 scale;
    /// <summary>
    /// 实体挂载组件
    /// </summary>
    List<GameComponent> gameComponents;
    /// <summary>
    /// 游戏物体所处的场景类
    /// </summary>
    public Scene sence;
    /// <summary>
    /// 实体名称
    /// </summary>
    public string name;
    /// <summary>
    /// 实体标签
    /// </summary>
    public string tag;
    /// <summary>
    /// 实体激活标识
    /// </summary>
    public bool isActive;
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="name">实体名称</param>
    /// <param name="tag">实体标签，默认为Default</param>
    public GameObject(string name,string tag = "Default")
    {
        this.name = name;
        this.tag = tag;
        this.scale = new Vector2(1f, 1f);
        gameComponents = new List<GameComponent>();
    }
    /// <summary>
    /// 由Scene.Initialize调用，唤醒组件Awake函数
    /// </summary>
    public void AwakeGameObject()
    {
        foreach (var item in gameComponents)
        {
            item.Awake();
        }
    }
    /// <summary>
    /// 唤醒组件Start函数
    /// </summary>
    public void StartGameObject()
    {
        foreach (var item in gameComponents)
        {
            item.Start();
        }
    }
    /// <summary>
    /// 唤醒组件Update函数
    /// </summary>
    public void UpdateGammeObject()
    {
        foreach (var item in gameComponents)
        {
            item.Update();
        }
    }   
    /// <summary>
    /// 添加一个组件到实体
    /// </summary>
    /// <typeparam name="T">组件类名称</typeparam>
    /// <returns>挂载的组件引用</returns>
    public T AddComponent<T>() where T : GameComponent, new()
    {
        T t = new T();
        t.gameObject = this;
        gameComponents.Add(t);
        return t;
    }
    /// <summary>
    /// 获得组件上的一个实体
    /// </summary>
    /// <typeparam name="T">组件类名称</typeparam>
    /// <returns>返回第一个寻找到的组件的引用</returns>
    public T GetComponent<T>() where T : GameComponent
    {
        T t = null;
        foreach (var item in gameComponents)
        {
            if (item.GetType() == typeof(T))
            {
                t = (T)item;
                break;
            }
        }
        return t;
    }
    /// <summary>
    /// 删除实体上的组件
    /// </summary>
    /// <typeparam name="T">组件类名称</typeparam>
    /// <remarks>一次性删除所有同类型组件</remarks>
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
    /// <summary>
    /// 寻找运行场景中的实体
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static GameObject FindGameObject(string name)
    {    
        return Core.curScene.GetGameObject(name);;
    }
    /// <summary>
    /// 寻找场景中的物体
    /// </summary>
    /// <param name="name">实体名称</param>
    /// <returns></returns>
    public static GameObject FindGameObjectWithTag(string name)
    {
        return Core.curScene.GetGameObjectWithTag(name);
    }
    /// <summary>
    /// 在场景中生成一个物体
    /// </summary>
    /// <param name="gameObject"></param>
    /// <returns></returns>
    public static GameObject Institate(GameObject gameObject)
    {
        Core.curScene.AddGameObject(gameObject);
        return gameObject;
    }
    /// <summary>
    /// 销毁一个场景中物体
    /// </summary>
    /// <param name="gameObject">指向被销毁物体的引用</param>
    public static void Destory(GameObject gameObject)
    {
        Core.curScene.DeleteGameObject(gameObject);
        ///GC机制自动保证GameObject和其组件一同销毁
    }
    

}

public class GameComponent
{
    /// <summary>
    /// 此组件依赖的实体
    /// </summary>
    public GameObject gameObject;
    /// <summary>
    /// ECS.Awake的虚函数
    /// </summary>
    public virtual void Awake() { }
    /// <summary>
    /// ECS.Start的虚函数
    /// </summary>
    public virtual void Start() { }
    //对于渲染组件，在Update时向Scnen的渲染中间层递交一个DrawRequest
    /// <summary>
    /// ECS.Update的虚函数
    /// </summary>
    public virtual void Update() { }
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="gameObject"></param>
    public GameComponent() { }
}
