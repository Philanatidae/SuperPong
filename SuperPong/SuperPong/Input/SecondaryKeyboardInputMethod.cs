using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SuperPong.Input
{
	public class SecondaryKeyboardInputMethod : InputMethod
	{
		public override void Update(GameTime gameTime)
		{
			KeyboardState currentState = Keyboard.GetState();

			_buttons = 0;
			if (currentState.IsKeyDown(Keys.Up))
			{
				_buttons |= (byte)Buttons.Up;
			}
			if (currentState.IsKeyDown(Keys.Down))
			{
				_buttons |= (byte)Buttons.Down;
			}
		}
	}
}
