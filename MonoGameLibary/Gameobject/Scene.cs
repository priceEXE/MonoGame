using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary.Graphics;
using MonoGameLibrary.Physics2D;
namespace MonoGameLibrary;
public class Scene : IDisposable
{
    /// <summary>
    /// 内容加载器
    /// </summary>
    protected ContentManager content { get; }
    /// <summary>
    /// 场景卸载标识
    /// </summary>
    public bool isDisposed { get; private set; }
    /// <summary>
    /// 精灵渲染请求
    /// </summary>
    public DrawRequest drawRequest;
    /// <summary>
    /// 游戏物体列表
    /// </summary>
    public List<GameObject> gameObjects;
    /// <summary>
    /// 当前帧时间
    /// </summary>
    public GameTime gameTime;
    /// <summary>
    /// 当前游戏实例
    /// </summary>
    public Game game;
    private bool isFirst;
    private List<GameObject> toBeAdded;
    private List<GameObject> toBeDelete;
    private List<Collider2D> toCheck;
    /// <summary>
    /// Scene构造函数，Game类依赖注入
    /// </summary>
    /// <param name="game"></param>
    public Scene(Game game)
    {
        content = new ContentManager(Core.Content.ServiceProvider);
        content.RootDirectory = Core.Content.RootDirectory;
        drawRequest = new DrawRequest();
        gameObjects = new List<GameObject>();
        toBeAdded = new List<GameObject>();
        toBeDelete = new List<GameObject>();
        toCheck = new List<Collider2D>();
        this.game = game;
        isFirst = true;
    }
    /// <summary>
    /// 析构函数，强制释放内存
    /// </summary>
    ~Scene() => Dispose(false);
    /// <summary>
    /// Scene类的MonoGame生命周期函数，在Game类的生命周期函数中调用
    /// </summary>
    public virtual void Initialize()
    {
        ///以下是一个默认行为，重载的子类函数尾需调用父类的此函数
        ///首先加载所有资产
        LoadContent();
        ///将LoadContent期间实例化的物体添加至列表篇
        foreach (var item in toBeAdded)
        {
            gameObjects.Add(item);
        }
        toBeAdded.Clear();
        ///根据资产加载结果对组件做初始化
        foreach (var item in gameObjects)
        {
            ///初次唤醒调用所有组件的Awake函数
            item.AwakeGameObject();
        }
    }
    /// <summary>
    /// 内容加载的虚函数，加载行为在子类中定义
    /// </summary>
    public virtual void LoadContent() { }
    /// <summary>
    /// 释放加载内容的虚函数
    /// </summary>
    public virtual void UnloadContent()
    {
        content.Unload();
    }
    /// <summary>
    /// Scene的MonoGame生命周期函数Update，每帧更新一次
    /// </summary>
    /// <param name="gameTime">帧更新时的当前帧时间</param>
    public void Update(GameTime gameTime)
    {           
        this.gameTime = gameTime;
        foreach (var item in gameObjects)
        {
            if (isFirst) item.StartGameObject();
            item.UpdateGammeObject();
        }
        CollisionCheck();
        foreach (var item in toBeAdded)
        {
            gameObjects.Add(item);
        }
        foreach (var item in toBeDelete)
        {
            gameObjects.Remove(item);
        }
        toBeAdded.Clear();
        toBeDelete.Clear();
    }
    /// <summary>
    /// Scene的MonoGame生命周期函数Draw，每帧更新一次
    /// </summary>
    /// <param name="gameTime">Draw绘制时当前帧时间</param>
    public void Draw(GameTime gameTime)
    {
        drawRequest.Draw(Core.SpriteBatch);
    }
    /// <summary>
    /// GC函数
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    /// <summary>
    /// GC辅助函数
    /// </summary>
    /// <param name="disposing"></param>
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
    /// <summary>
    /// 获得场景中的一个实体
    /// </summary>
    /// <param name="name">物体名称</param>
    /// <returns></returns>
    public GameObject GetGameObject(string name)
    {
        GameObject res = null;
        foreach (var item in gameObjects)
        {
            if (item.name == name) res = item;
        }
        return res;
    }
    /// <summary>
    /// 获得场景中的一个物体
    /// </summary>
    /// <param name="tag">物体标签</param>
    /// <returns></returns>
    public GameObject GetGameObjectWithTag(string tag)
    {
        GameObject res = null;
        foreach (var item in gameObjects)
        {
            if (item.tag == tag) res = item;
        }
        return res;
    }
    /// <summary>
    /// 添加一个游戏物体
    /// </summary>
    /// <param name="gameObject">游戏物体</param>
    public void AddGameObject(GameObject gameObject)
    {
        toBeAdded.Add(gameObject);
        gameObject.AwakeGameObject();
    }
    /// <summary>
    /// 删除一个游戏物体
    /// </summary>
    /// <param name="gameObject"></param>
    public void DeleteGameObject(GameObject gameObject)
    {
        if (gameObjects.Contains(gameObject))
        {
            toBeDelete.Add(gameObject);
        }
    }
    /// <summary>
    /// 激活一个游戏物体
    /// </summary>
    /// <param name="gameObject">游戏物体类</param>
    public void ActiveGameObject(GameObject gameObject)
    {
        if (gameObjects.Contains(gameObject))
        {
            gameObject.isActive = true;
            gameObject.StartGameObject();
        }
    }
    /// <summary>
    /// 禁用一个游戏物体
    /// </summary>
    /// <param name="gameObject">游戏物体类</param>
    public void NegetiveGameObject(GameObject gameObject)
    {
        if (gameObjects.Contains(gameObject))
        {
            gameObject.isActive = false;
        }
    }
    /// <summary>
    /// 碰撞组件注册碰撞器
    /// </summary>
    /// <param name="collider2D"></param>
    public void RegisterCollisionCheck(Collider2D collider2D)
    {
        toCheck.Add(collider2D);
    }

    public void CollisionCheck()
    {
        //检查所有碰撞
        //此处只进行简单的两两碰撞检查
        int length = toCheck.Count;
        for (int i = 0; i < length; i++)
        {
            for (int j = i + 1; j < length; j++)
            {
                //如果碰撞成立，调用脚本中所有的OnCollisionStay函数
                if (toCheck[i].CheckCollision(toCheck[j]))
                {
                    GameObject gameObject = toCheck[i].gameObject;
                    foreach (GameComponent item in gameObject.gameComponents)
                    {
                        ///反射调用其中的OnCollisionStay方法
                        Type type = item.GetType();
                        MethodInfo methodInfo = type.GetMethod("OnCollisionStay", BindingFlags.Public | BindingFlags.Instance);
                        methodInfo?.Invoke(item, null);
                    }
                }
            }
        }
        //清除注册的碰撞
        toCheck.Clear();
    }
}
/// <summary>
/// 渲染请求类，MonoGame生命周期函数与ECS框架的Update组件间的胶水代码
/// </summary>
public class DrawRequest
{
    /// <summary>
    /// 进行渲染操作的摄像机位置
    /// </summary>
    public Vector2 cameraPos;
    /// <summary>
    /// 待渲染的精灵
    /// </summary>
    public List<Sprite> sprites;
    /// <summary>
    /// 待渲染的精灵渲染位置
    /// </summary>
    public List<Vector2> positions;
    /// <summary>
    /// 无参构造函数
    /// </summary>
    public DrawRequest()
    {
        sprites = new List<Sprite>();
        positions = new List<Vector2>();
    }
    /// <summary>
    /// 由MonoGame.Draw调用，在渲染管线上批量渲染
    /// </summary>
    /// <param name="spriteBatch">渲染管线引用</param>
    public void Draw(SpriteBatch spriteBatch)
    {
        if(Camera.main!=null)
        {
            for (int i = 0; i < sprites.Count; i++)
            {
                sprites[i].Draw(spriteBatch, Camera.main.GetDrawPosition(positions[i]));
            }
            sprites.Clear();
            positions.Clear();
        }
    }
    /// <summary>
    /// 由ECS.Update调用，提交渲染请求
    /// </summary>
    /// <param name="sprite">精灵</param>
    /// <param name="position">精灵位置</param>
    public void RequestDraw(Sprite sprite, Vector2 position)
    {
        sprites.Add(sprite);
        positions.Add(position);
    }
}