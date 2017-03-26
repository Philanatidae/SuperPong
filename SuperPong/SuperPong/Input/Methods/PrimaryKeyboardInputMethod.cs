using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SuperPong.Input.Methods
{
	public class PrimaryKeyboardInputMethod : InputMethod
	{
		public PrimaryKeyboardInputMethod()
		{
		}

		public override void Update(GameTime gameTime)
		{
			KeyboardState currentState = Keyboard.GetState();

			_buttons = 0;
			if (currentState.IsKeyDown(Keys.W))
			{
				_buttons |= (byte)Buttons.Up;
			}
			if (currentState.IsKeyDown(Keys.S))
			{
				_buttons |= (byte)Buttons.Down;
			}
		}
	}
}
