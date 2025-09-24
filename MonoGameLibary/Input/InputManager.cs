using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
namespace MonoGameLibrary.Input;

public class InputManager
{
    public static KeyboardInfo Keyboard;
    public static MouseInfo mouse;

    public InputManager()
    {
        //初始化键盘和鼠标
        Keyboard = new KeyboardInfo();
        mouse = new MouseInfo();
    }
    public void Update(GameTime gameTime = default)
    {
        Keyboard.Update();
        mouse.Update();
    }
    public static bool GetKeyDown(Keys keys)
    {
        return Keyboard.GetKeyDown(keys);
    }
    public static bool GetKey(Keys keys)
    {
        return Keyboard.GetKey(keys);
    }
    public static bool GetKeyUp(Keys keys)
    {
        return Keyboard.GetKeyUp(keys);
    }
    public static bool GetMouseButtonDown(MouseButton mouseButton)
    {
        return mouse.GetMouseButtonDown(mouseButton);
    }
    public static bool GetMouseButton(MouseButton mouseButton)
    {
        return mouse.GetMouseButton(mouseButton);
    }
    public static bool GetMouseButtonUp(MouseButton mouseButton)
    {
        return mouse.GetMouseButtonUp(mouseButton);
    }
}