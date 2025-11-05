# 架构设计方案
by 沈诗杰
## 一、ECS框架类
GameEntity类
数据段：
```csharp
Vector2 position //记录实体世界坐标位置
float Rotation //记录实体世界旋转角度
List<GameComponent> gameComponent //记录实体挂载组件
Scene secne //记录实体处于的场景类
string name
string[] tags
bool isActive
```
函数段：
```csharp
public void AwakeGameEntity()
{
    foreach( var item in gameComponent )
    {
        item.Awake();
    }
}
public void Start() {...}
public void Update() {...}

public void AddComponent<T>() {...}
public void DeleteComponent<T>() {...}
public void GetComponent<T>()() {...}

public static GameEntity FindGameEntity(string name) {...}
public static GameEntity FindGameEntityWithTag(string tag) {...}
public static GameEntity Institate(GameEntity gameEntity) {...} //此类需要查找Scene中托管的GameEntity列表，并将gameEntity添加入其中，所以Game类的Scene静态变量需要公开
public static void Destory(GameEntity gameEntity) {...}
```
作用：管理游戏实体的基类，通过事件触发生命周期函数调用，根据泛型添加组件,提供静态工具方法用于管理游戏实体

GameComponent类
函数段:
```csharp
public virtual void Awake() {}
public virtual void Start() {}
public virtual void Update() {}
```
作用：定义GameCommponent的生命周期虚函数，所有的脚本代码继承此类，通过重载实现生命周期虚函数

Scene类
相对教程中的Scene示例，增加以下字段
public List<GameEntity> gameEntitys  //用于储存场景中已有的游戏物体
对于通过代码在场景中添加游戏实体的需求，仅允许通过GameEntity的Insitate方法操作，不可直接操作gameEntitys列表（涉及到生命周期函数的触发行为）

## 二、碰撞检测类
使用已有的圆碰撞类，并进行Component化改造

## 三、输入检测类
定义一个类InputSystem的静态方法用来获取键盘输入和,无需进行Component化改造
在gameplay时额外新增一个PlayerController类做输入检测到移动命令发送的转换

## 四、精灵渲染类、音效管理类、TileMap、GumUI
在教程的代码基础上进行component化改造
可以简单的将教程代码的功能模块封装进继承自GameComponent的类来进行改造，建议将功能模块执行时需要的数据分离到单独的类或结构中，而不是硬编码在代码中

## 五：GamePlay类
一些与gamePlay机制相关的不可复用代码，包括玩家的移动和旋转（实际上应该分为UnitMove和UnitRotate两个独立的类），敌人AI（向敌人Entity挂载的脚本发送移动命令），buff系统（我的Unity项目中有成型代码可以移用），一些数据缓存类，一个全局的GameManager类（管理一些游戏全局数据）

## 六：序列化与反序列化
如果不想将游戏数据硬编码在代码中的话，在MonoGame框架下最好是将数据序列化为xml文件（具体可参考MonoGame官方教程精灵图集存储划分数据和加载数据的方式），但是前期的开发阶段我建议编写一个专门的Config类来“序列化”这些数据，如果时间充裕可以专门开发序列化和反序列化的工具类，但是我希望开发的重点在弄出一个能玩的游戏出来

## 七：精灵素材与音效素材
我在群里发了一套《20分钟黎明》的解包素材，可以在此素材包的基础上开始制作（只需其中的Sprite，Texture，Audio文件夹，其余属于Unity的资产）

## 八：反馈
在进行各个功能模块的组件化改造时，如果涉及到想要在ECS架构类中添加字段，实现功能的需求，请先QQ@我，我会根据实际情况判断是否需要修改框架类
如果框架类有重大调整或更改，我会在我的分支的README中记载版本号和更新，并在QQ群中戳各位
以上

