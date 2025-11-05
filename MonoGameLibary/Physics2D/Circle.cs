using System;
using Microsoft.Xna.Framework;

namespace MonoGameLibrary.Physics2D;

public struct Circle : IEquatable<Circle>
{
    //默认的空圆
    private static readonly Circle emptyCircle = new Circle();
    /// <summary>
    /// 圆心横坐标
    /// </summary>
    public float x;
    /// <summary>
    /// 圆心纵坐标
    /// </summary>
    public float y;
    /// <summary>
    /// 圆半径
    /// </summary>
    public float radius;
    /// <summary>
    /// 圆的最顶部坐标
    /// </summary>
    public readonly float top => y + radius;
    /// <summary>
    /// 圆的最底部坐标
    /// </summary>
    public readonly float bottom => y - radius;
    /// <summary>
    /// 圆的最左部坐标
    /// </summary>
    public readonly float left => x - radius;
    /// <summary>
    /// 圆的最右部坐标
    /// </summary>
    public readonly float right => x + radius;
    /// <summary>
    /// 圆坐标
    /// </summary>
    public readonly Vector2 position => new Vector2(x, y);
    /// <summary>
    /// 空圆引用
    /// </summary>
    public static Circle Empty => emptyCircle;
    /// <summary>
    /// 是否为空圆的判断
    /// </summary>
    public readonly bool isEmpty => x == 0 && y == 0 && radius == 0;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="x">x坐标</param>
    /// <param name="y">y坐标</param>
    /// <param name="radius">圆半径</param>
    public Circle(float x, float y, float radius)
    {
        this.x = x;
        this.y = y;
        this.radius = radius;
    }
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="point">圆心坐标</param>
    /// <param name="radius">圆半径</param>
    public Circle(Vector2 point, float radius)
    {
        this.x = point.X;
        this.y = point.Y;
        this.radius = radius;
    }
    /// <summary>
    /// 检查两个圆之间是否重叠
    /// </summary>
    /// <param name="other">另外一个需要检查的圆</param>
    /// <returns>重叠时返回true，否则为fasle</returns>
    public bool Intersects(Circle other)
    {
        float radiiSquared = (this.radius + other.radius) * (this.radius + other.radius);
        float distanceSquared = Vector2.DistanceSquared(this.position, other.position);
        return distanceSquared < radiiSquared;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override readonly bool Equals(object obj) => obj is Circle other && Equals(other);

    public readonly bool Equals(Circle circle) => this.x == circle.x &&
                                                    this.y == circle.y &&
                                                    this.radius == circle.radius;
    public override readonly int GetHashCode() => HashCode.Combine(x, y, radius);

    /// <summary>
    /// 重载==运算符
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator ==(Circle lhs, Circle rhs) => lhs.Equals(rhs);
    /// <summary>
    /// 重载!=运算符
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator !=(Circle lhs, Circle rhs) => !lhs.Equals(rhs);

    public object Clone()
    {
        return new Circle(this.x, this.y, this.radius);
    }
}
