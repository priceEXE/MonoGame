using MonoGameLibrary;
using Microsoft.Xna.Framework;
public class UnitMove : MonoGameLibrary.GameComponent
{
    public float speed;
    public Vector2 direction;
    public UnitMove()
    {
        speed = 500f;
        direction = Vector2.Zero;
    }
    public override void Update()
    {
        ///确保每帧之间计算的移动距离准确
        gameObject.position += direction * speed * (float)gameObject.sence.gameTime.ElapsedGameTime.TotalSeconds;
    }

    public override object Clone()
    {
        return new UnitMove();
    }
}