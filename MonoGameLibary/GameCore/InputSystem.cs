using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary.Input;

namespace MonoGameLibrary.GameCore
{
    /// <summary>
    /// 输入系统，扩展原有的输入管理
    /// </summary>
    public class InputSystem
    {
        private KeyboardState _previousKeyboardState;
        private MouseState _previousMouseState;

        public InputSystem()
        {
            _previousKeyboardState = Keyboard.GetState();
            _previousMouseState = Mouse.GetState();
        }

        /// <summary>
        /// 更新输入系统
        /// </summary>
        public void Update(GameTime gameTime)
        {
            _previousKeyboardState = Keyboard.GetState();
            _previousMouseState = Mouse.GetState();
        }

        /// <summary>
        /// 获取移动输入
        /// </summary>
        public Vector2 GetMovementInput()
        {
            Vector2 movement = Vector2.Zero;

            if (InputManager.GetKey(Keys.W))
                movement.Y -= 1;
            if (InputManager.GetKey(Keys.S))
                movement.Y += 1;
            if (InputManager.GetKey(Keys.A))
                movement.X -= 1;
            if (InputManager.GetKey(Keys.D))
                movement.X += 1;

            if (movement != Vector2.Zero)
            {
                movement.Normalize();
            }

            return movement;
        }

        /// <summary>
        /// 获取鼠标世界位置
        /// </summary>
        public Vector2 GetMouseWorldPosition()
        {
            var mouseState = Mouse.GetState();
            return new Vector2(mouseState.X, mouseState.Y);
        }

        /// <summary>
        /// 获取鼠标方向
        /// </summary>
        public Vector2 GetMouseDirection(Vector2 fromPosition)
        {
            Vector2 mousePosition = GetMouseWorldPosition();
            Vector2 direction = mousePosition - fromPosition;
            if (direction != Vector2.Zero)
            {
                direction.Normalize();
            }
            return direction;
        }

        /// <summary>
        /// 检查是否按下攻击键
        /// </summary>
        public bool IsAttackPressed()
        {
            return InputManager.GetMouseButtonDown(MouseButton.Left);
        }

        /// <summary>
        /// 检查是否按下暂停键
        /// </summary>
        public bool IsPausePressed()
        {
            return InputManager.GetKeyDown(Keys.Escape);
        }

        /// <summary>
        /// 检查武器切换输入
        /// </summary>
        public int GetWeaponSwitchInput()
        {
            if (InputManager.GetKeyDown(Keys.D1))
                return 1;
            if (InputManager.GetKeyDown(Keys.D2))
                return 2;
            if (InputManager.GetKeyDown(Keys.D3))
                return 3;
            return 0;
        }

        /// <summary>
        /// 检查鼠标滚轮输入
        /// </summary>
        public int GetMouseWheelDelta()
        {
            var currentMouseState = Mouse.GetState();
            return currentMouseState.ScrollWheelValue - _previousMouseState.ScrollWheelValue;
        }
    }
}
