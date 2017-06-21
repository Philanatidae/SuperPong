using Microsoft.Xna.Framework.Input;

namespace SuperPong.Input
{
    public class SecondaryKeyboardInputMethod : InputMethod
    {
        public override void Update(float dt)
        {
            KeyboardState currentState = Keyboard.GetState();

            _snapshot._axis = 0;
            if (currentState.IsKeyDown(Settings.Instance.Data.SecondaryKey1))
            {
                _snapshot._axis += 1;
            }
            if (currentState.IsKeyDown(Settings.Instance.Data.SecondaryKey2))
            {
                _snapshot._axis -= 1;
            }
        }
    }
}
