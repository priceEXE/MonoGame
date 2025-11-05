using MonoGameLibrary;
using Microsoft.Xna.Framework;
using System;
using System.Diagnostics;
public class SimpleAI : MonoGameLibrary.GameComponent
{
    private GameManager gameManager;
    private GameObject target;
    private UnitMove unitMove;
    private SpriteRender spriteRender;
    public override void Update()
    {
        ///寻找目标
        if (target == null) target = GameObject.FindGameObject("Player");
        ///处理移动
        if (target != null)
        {
            Vector2 direct = target.position - gameObject.position;
            float length = MathF.Sqrt(direct.X * direct.X + direct.Y * direct.Y);
            if (length < 200f)
            {
                if (spriteRender.sprite != null) spriteRender.sprite.Color = Color.Red;
                if (length < 5f) direct = Vector2.Zero;
                if (direct != Vector2.Zero) direct.Normalize();
                unitMove.direction = direct;
            }
            else
            {
                if(spriteRender.sprite!=null)
                {
                    spriteRender.sprite.Color = Color.White;
                }
            }
        }
        else
        {  
            unitMove.direction = Vector2.Zero;
        }
    }

    public override void Start()
    {
        unitMove = gameObject.GetComponent<UnitMove>();
        gameManager = GameObject.FindGameObject("GameManager").GetComponent<GameManager>();
        spriteRender = gameObject.GetComponent<SpriteRender>();
        //Debug.WriteLine(isActive);
        //Debug.WriteLine(wasActive);

    }
    public void OnCollisionStay()
    {
        gameManager.OnEnemyDestoy(this.gameObject);
        GameObject.Destory(this.gameObject);
    }

    public override object Clone()
    {
        return new SimpleAI();
    }

}