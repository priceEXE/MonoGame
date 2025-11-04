using MonoGameLibrary;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;

public class GameManager : MonoGameLibrary.GameComponent
{
    public GameObject enemyPrefab;
    public List<GameObject> enemys = new List<GameObject>();

    public override void Update()
    {
        ///生成敌人
        if (enemys.Count != 5)
        {
            GameObject e = GameObject.Institate(enemyPrefab);
            e.position = new Vector2(MathTool.RandomRange(-500, 500), MathTool.RandomRange(-500, 500));
            enemys.Add(e);
        }
    }

    public void OnEnemyDestoy(GameObject enemy)
    {
        enemys.Remove(enemy);
    }

    public override object Clone()
    {
        GameManager gameManager = new GameManager();
        gameManager.enemyPrefab = enemyPrefab;
        return gameManager;
    }
}