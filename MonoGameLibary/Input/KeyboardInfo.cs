using Microsoft.Xna.Framework.Input;
namespace MonoGameLibrary.Input;

public class KeyboardInfo
{
    /// <summary>
    /// 上一帧中的键盘输入信息
    /// </summary>
    public KeyboardState PreviousState { get; private set; }
    /// <summary>
    /// 此帧中的键盘输入信息
    /// </summary>
    public KeyboardState CurrentState { get; private set; }
    /// <summary>
    /// 是否有键盘的键被按下
    /// </summary>
    public bool isPressed { get; private set; }
    /// <summary>
    /// 按下的键位数组
    /// </summary>
    public Keys[] CurKey { get; private set; }
    /// <summary>
    /// 构造函数
    /// </summary>
    public KeyboardInfo()
    {
        PreviousState = new KeyboardState();
        CurrentState = Keyboard.GetState();
    }
    /// <summary>
    /// 在一帧中更新键盘信息
    /// </summary>
    public void Update()
    {
        PreviousState = CurrentState;
        CurrentState = Keyboard.GetState();
        CurKey = CurrentState.GetPressedKeys();
        isPressed = CurrentState.GetPressedKeyCount() > 0 ? true : false;
    }

    /// <summary>
    /// 等同于Unity的GetKeyDown方法
    /// </summary>
    /// <param name="key">The key to check.</param>
    /// <returns>true if the specified key was just pressed on the current frame; otherwise, false.</returns>
    public bool GetKeyDown(Keys key)
    {
        return CurrentState.IsKeyDown(key) && PreviousState.IsKeyUp(key);
    }

    /// <summary>
    /// 等同于Unity的GetKeyUp
    /// </summary>
    /// <param name="key">The key to check.</param>
    /// <returns>true if the specified key was just released on the current frame; otherwise, false.</returns>
    public bool GetKeyUp(Keys key)
    {
        return CurrentState.IsKeyUp(key) && PreviousState.IsKeyDown(key);
    }

    /// <summary>
    /// 自定义方法，是否持续按键 
    /// </summary>
    /// <param name="key">键位枚举</param>
    /// <returns></returns>
    public bool GetKey(Keys key)
    {
        return CurrentState.IsKeyDown(key);
    }
    /// <summary>
    /// 获得按下的按键，仅接受唯一的按键作为输出
    /// </summary>
    /// <returns>按下按键的枚举</returns>
    public Keys GetPressedKey()
    {
        if (isPressed && CurKey.Length == 1)
        {
            return CurKey[0];
        }
        return default;
    }
}
