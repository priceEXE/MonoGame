using MonoGameLibrary;
using MonoGameLibrary.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
public class PlayerController : MonoGameLibrary.GameComponent
{
    public Keys upKey;
    public Keys downKey;
    public Keys leftKey;
    public Keys rightKey;

    /// <summary>
    /// 角色移动脚本（todo）
    /// </summary>
    public UnitMove unitMove;
    /// <summary>
    /// 角色旋转脚本（todo）
    /// </summary>
    public MonoGameLibrary.GameComponent UnitRotate;
    /// <summary>
    /// 默认的构造函数,提供默认的键位绑定
    /// </summary>
    public PlayerController()
    {
        upKey = Keys.W;
        downKey = Keys.S;
        leftKey = Keys.A;
        rightKey = Keys.D;
    }
    /// <summary>
    /// 根据绑定键位获得移动方向
    /// </summary>
    /// <returns></returns>
    public Vector2 GetDirection()
    {
        Vector2 res = Vector2.Zero;
        if (InputManager.GetKey(upKey)) res.Y -= 1;
        if (InputManager.GetKey(downKey)) res.Y += 1;
        if (InputManager.GetKey(leftKey)) res.X -= 1f;
        if (InputManager.GetKey(rightKey)) res.X += 1f;
        if(res != Vector2.Zero) res.Normalize();
        return res;
    }
    /// <summary>
    /// 设置移动指令
    /// </summary>
    public void SetMoveMent()
    {
        ///todo：操作UnitMove脚本
        if(unitMove!=null)  unitMove.direction = GetDirection();
        ///todo:操作UnitRotate脚本
    }
    /// <summary>
    /// 每次更新时更新移动指令
    /// </summary>
    public override void Update()
    {
        SetMoveMent();
    }
    /// <summary>
    /// 在物体激活时获得一次移动脚本
    /// </summary>
    public override void Start()
    {
        unitMove = gameObject.GetComponent<UnitMove>();
    }
}