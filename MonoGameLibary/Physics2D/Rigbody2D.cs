using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoGameLibrary;
using MonoGameLibrary.Physics2D;
///技术力有限，此类暂时弃用
/*
public class Rigbody2D : MonoGameLibrary.GameComponent
{
    private Collider2D collider2D;

    public override void Start()
    {
        collider2D = gameObject.GetComponent<Collider2D>();
    }

    /*
    public override void Update()
    {
        if(collider2D!=null)
        {
            List<Collider2D> toCheck = new List<Collider2D>();
            foreach (var item in gameObject.sence.gameObjects)
            {
                //获得当前场景中所有存在碰撞体的物体
                Collider2D temp = item.GetComponent<Collider2D>();
                Vector2 dire = temp.gameObject.position - gameObject.position;
                float length = MathF.Sqrt(dire.X * dire.X + dire.Y * dire.Y);
                ///当当前物体的碰撞体不为空且有可能发生碰撞时，将其添加到检查列表中
                if (temp == null && length < collider2D.radius + temp.radius) toCheck.Add(temp);
            }
            //检查所有碰撞
            foreach (var item in toCheck)
            {
                
            }
        }
    }
}*/