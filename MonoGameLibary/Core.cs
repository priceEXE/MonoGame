using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary.Input;

namespace MonoGameLibrary;

public class Core : Game
{
    internal static Core s_instance;
    /// <summary>
    /// 单例实例
    /// </summary>
    public static Core Instance => s_instance;

    /// <summary>
    /// 获得图像设备管理器
    /// </summary>
    public static GraphicsDeviceManager Graphics { get; private set; }

    /// <summary>
    /// 图像设备
    /// </summary>
    public static new GraphicsDevice GraphicsDevice { get; private set; }

    /// <summary>
    /// 渲染管线
    /// </summary>
    public static SpriteBatch SpriteBatch { get; private set; }

    /// <summary>
    /// 内容管理器
    /// </summary>
    public static new ContentManager Content { get; private set; }

    /// <summary>
    /// 输入管理类
    /// </summary>
    public static InputManager Input { get; private set; }

    /// <summary>
    /// 游戏是否退出
    /// </summary>
    public static bool ExitOnEscape { get; set; }
    /// <summary>
    /// 当前场景
    /// </summary>
    public static Scene curScene;
    /// <summary>
    /// 下一个场景
    /// </summary>
    public static Scene nextScene;

    /// <summary>
    /// Creates a new Core instance.
    /// </summary>
    /// <param name="title">窗口标题</param>
    /// <param name="width">窗口宽</param>
    /// <param name="height">窗口高</param>
    /// <param name="fullScreen">全屏模式</param>
    public Core(string title, int width, int height, bool fullScreen)
    {
        // 保证游戏进程单线程
        if (s_instance != null)
        {
            throw new InvalidOperationException($"Only a single Core instance can be created");
        }

        // 设置单例实例
        s_instance = this;

        // 创建一个新的图像设备管理类
        Graphics = new GraphicsDeviceManager(this);

        // 设置参数
        Graphics.PreferredBackBufferWidth = width;
        Graphics.PreferredBackBufferHeight = height;
        Graphics.IsFullScreen = fullScreen;

        // 应用参数设置
        Graphics.ApplyChanges();

        // 设置窗口标题
        Window.Title = title;

        // 设置内容管理器为基类的实例
        // content manager.
        Content = base.Content;

        // 设置资源路径
        Content.RootDirectory = "Content";

        // 鼠标指针是否可视
        IsMouseVisible = true;

        // 是否ESC退出游戏
        ExitOnEscape = true;
    }
    /// <summary>
    /// 基本的初始化生命周期函数
    /// </summary>
    protected override void Initialize()
    {
        ///基类初始化
        base.Initialize();

        // 设置图像设备为基类的引用
        GraphicsDevice = base.GraphicsDevice;

        // 创建渲染管线
        SpriteBatch = new SpriteBatch(GraphicsDevice);

        // 创建输入管理类
        Input = new InputManager();
    }
    /// <summary>
    /// 基本的Update生命周期函数
    /// </summary>
    /// <param name="gameTime">帧时间</param>
    protected override void Update(GameTime gameTime)
    {
        // 更新输入管理类
        Input.Update(gameTime);
        //如果按ESC退出
        if (ExitOnEscape && InputManager.Keyboard.GetKeyDown(Keys.Escape))
        {
            Exit();
        }
        //基类更新
        base.Update(gameTime);
    }
    /// <summary>
    /// 更换当前场景
    /// </summary>
    protected void ChangeScene()
    {
        if(nextScene != null)
        {
            curScene.Dispose();
            curScene = nextScene;
        }
    }
}
