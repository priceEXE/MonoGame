using MonoGameLibrary;
using Microsoft.Xna.Framework;
using System;
public class SimpleAI : MonoGameLibrary.GameComponent
{
    public GameObject target;
    public UnitMove unitMove;

    public override void Update()
    {
        Vector2 direct = target.position - gameObject.position;
        float length = MathF.Sqrt(direct.X * direct.X + direct.Y * direct.Y);
        if (length < 100f) direct = Vector2.Zero;
        if(direct!=Vector2.Zero) direct.Normalize();
        unitMove.direction = direct;
    }

    public override void Start()
    {
        target = GameObject.FindGameObject("Player");
        unitMove = gameObject.GetComponent<UnitMove>();
    }

    public override object Clone()
    {
        return new SimpleAI();
    }

}