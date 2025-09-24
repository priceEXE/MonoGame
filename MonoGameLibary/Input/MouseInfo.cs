using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MonoGameLibrary.Input;

public class MouseInfo
{
    public MouseState CurrentState { get; private set; }
    public MouseState PreviousState { get; private set; }

    public Point Position
    {
        get => CurrentState.Position;
        set => SetPosition(value.X, value.Y);
    }

    public int x
    {
        get => CurrentState.X;
        set => SetPosition(value, CurrentState.X);
    }
    public int y
    {
        get => CurrentState.Y;
        set => SetPosition(CurrentState.Y, value);
    }
    public Point PositionDelta => CurrentState.Position - PreviousState.Position;

    public int XDelta => CurrentState.X - PreviousState.X;
    public int YDelta => CurrentState.Y - PreviousState.Y;

    public bool WasMoved => PositionDelta != Point.Zero;

    public int ScrollWhell => CurrentState.ScrollWheelValue;

    public int ScrollWhellDelta => CurrentState.ScrollWheelValue - PreviousState.ScrollWheelValue;


    public MouseInfo()
    {
        PreviousState = new MouseState();
        CurrentState = Mouse.GetState();
    }

    public void Update()
    {
        PreviousState = CurrentState;
        CurrentState = Mouse.GetState();
    }
    /// <summary>
    /// 检测某个键是否按下
    /// </summary>
    /// <param name="mouseButton"></param>
    /// <returns></returns>
    public bool GetMouseButtonDown(MouseButton mouseButton)
    {
        switch (mouseButton)
        {
            case MouseButton.Left:
                return CurrentState.LeftButton == ButtonState.Pressed && PreviousState.LeftButton == ButtonState.Released;
            case MouseButton.Middle:
                return CurrentState.MiddleButton == ButtonState.Pressed && PreviousState.MiddleButton == ButtonState.Released;
            case MouseButton.Right:
                return CurrentState.RightButton == ButtonState.Pressed && PreviousState.RightButton == ButtonState.Released;
            case MouseButton.XButton1:
                return CurrentState.XButton1 == ButtonState.Pressed && PreviousState.XButton1 == ButtonState.Released;
            case MouseButton.Xbutton2:
                return CurrentState.XButton2 == ButtonState.Pressed && PreviousState.XButton2 == ButtonState.Released;
            default:
                return false;
        }
    }
    /// <summary>
    /// 检测某个键是否弹起
    /// </summary>
    /// <param name="mouseButton"></param>
    /// <returns></returns>
    public bool GetMouseButtonUp(MouseButton mouseButton)
    {
        switch (mouseButton)
        {
            case MouseButton.Left:
                return CurrentState.LeftButton == ButtonState.Released && PreviousState.LeftButton == ButtonState.Pressed;
            case MouseButton.Middle:
                return CurrentState.MiddleButton == ButtonState.Released && PreviousState.MiddleButton == ButtonState.Pressed;
            case MouseButton.Right:
                return CurrentState.RightButton == ButtonState.Released && PreviousState.RightButton == ButtonState.Pressed;
            case MouseButton.XButton1:
                return CurrentState.XButton1 == ButtonState.Released && PreviousState.XButton1 == ButtonState.Pressed;
            case MouseButton.Xbutton2:
                return CurrentState.XButton2 == ButtonState.Released && PreviousState.XButton2 == ButtonState.Pressed;
            default:
                return false;
        }
    }
    /// <summary>
    /// 检查某个键是否按住
    /// </summary>
    /// <param name="mouseButton"></param>
    /// <returns></returns>
    public bool GetMouseButton(MouseButton mouseButton)
    {
        switch (mouseButton)
        {
            case MouseButton.Left:
                return CurrentState.LeftButton == ButtonState.Pressed;
            case MouseButton.Middle:
                return CurrentState.MiddleButton == ButtonState.Pressed;
            case MouseButton.Right:
                return CurrentState.RightButton == ButtonState.Pressed;
            case MouseButton.XButton1:
                return CurrentState.XButton1 == ButtonState.Pressed;
            case MouseButton.Xbutton2:
                return CurrentState.XButton2 == ButtonState.Pressed;
            default:
                return false;
        }
    }

    public void SetPosition(int x, int y)
    {
        Mouse.SetPosition(x, y);
        CurrentState = new MouseState(
            x,
            y,
            CurrentState.ScrollWheelValue,
            CurrentState.LeftButton,
            CurrentState.MiddleButton,
            CurrentState.RightButton,
            CurrentState.XButton1,
            CurrentState.XButton2
        );
    }
    

}