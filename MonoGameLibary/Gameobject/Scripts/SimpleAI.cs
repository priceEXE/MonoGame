using MonoGameLibrary;
using Microsoft.Xna.Framework;
using System;
public class SimpleAI : MonoGameLibrary.GameComponent
{
    private GameManager gameManager;
    public GameObject target;
    public UnitMove unitMove;

    public override void Update()
    {
        ///处理移动
        Vector2 direct = target.position - gameObject.position;
        float length = MathF.Sqrt(direct.X * direct.X + direct.Y * direct.Y);
        if (length < 100f) direct = Vector2.Zero;
        if(direct!=Vector2.Zero) direct.Normalize();
        unitMove.direction = direct;
        //处理死亡
        if(length < 100f)
        {
            gameManager.OnEnemyDestoy(this.gameObject);
            GameObject.Destory(gameObject);
        } 
    }

    public override void Start()
    {
        target = GameObject.FindGameObject("Player");
        unitMove = gameObject.GetComponent<UnitMove>();
        gameManager = GameObject.FindGameObject("GameManager").GetComponent<GameManager>();
    }

    public override object Clone()
    {
        return new SimpleAI();
    }

}