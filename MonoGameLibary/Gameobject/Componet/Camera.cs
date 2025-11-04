using System;
using Microsoft.Xna.Framework;
using MonoGameLibrary;
public class Camera : MonoGameLibrary.GameComponent
{

    public static Camera main;
    public float width;
    public float height;
    public bool isMain;
    /// <summary>
    /// 每帧中将摄像机位置设置为与本体绑定的位置
    /// </summary>
    public override void Update()
    {
        gameObject.sence.drawRequest.cameraPos = gameObject.position;
    }

    public override object Clone()
    {
        Camera camera = new Camera();
        if (this.isMain)
        {
            ///更改主摄像机设置
            Camera.main = camera;
            Camera.main.isMain = true;
        }
        return camera;
    }
    /// <summary>
    /// 根据世界坐标转换到绘制坐标
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public Vector2 GetDrawPosition(Vector2 pos)
    {
        Vector2 origin = new Vector2(gameObject.position.X, -gameObject.position.Y);
        Vector2 viewPos = new Vector2(pos.X, -pos.Y);
        viewPos -= origin;
        Vector2 viewPort = Core.GetViewPort();
        viewPos += new Vector2( viewPort.X * .5f, 0);
        viewPos += new Vector2(0, viewPort.Y * .5f);
        return viewPos;
    }

}