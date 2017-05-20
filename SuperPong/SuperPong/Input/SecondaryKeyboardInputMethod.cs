using Microsoft.Xna.Framework.Input;

namespace SuperPong.Input
{
    public class SecondaryKeyboardInputMethod : InputMethod
    {
        public override void Update(float dt)
        {
            KeyboardState currentState = Keyboard.GetState();

            _snapshot._axis = 0;
            if (currentState.IsKeyDown(Keys.Up))
            {
                _snapshot._axis += 1;
            }
            if (currentState.IsKeyDown(Keys.Down))
            {
                _snapshot._axis -= 1;
            }
        }
    }
}
