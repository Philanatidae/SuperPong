using Microsoft.Xna.Framework.Input;

namespace SuperPong.Input
{
    public class PrimaryKeyboardInputMethod : InputMethod
    {
        public override void Update(float dt)
        {
            KeyboardState currentState = Keyboard.GetState();

            _snapshot._axis = 0;
            if (currentState.IsKeyDown(Settings.Instance.Data.PrimaryKey1))
            {
                _snapshot._axis += 1;
            }
            if (currentState.IsKeyDown(Settings.Instance.Data.PrimaryKey2))
            {
                _snapshot._axis -= 1;
            }

            // Update join/leave/start
            JoinKeyPressed = currentState.IsKeyDown(Settings.Instance.Data.PrimaryKey1)
                                         || currentState.IsKeyDown(Settings.Instance.Data.PrimaryKey2);
            StartKeyPressed = currentState.IsKeyDown(Keys.Enter);
            PauseKeyPressed = currentState.IsKeyDown(Keys.Enter);
        }
    }
}
