using System;

namespace SuperPong.Input
{
	public enum Buttons
	{
		Up = 0x1,
		Down = 0x2
	};

	public class InputSnapshot
	{
		internal byte _buttons;

		public InputSnapshot():this(0)
		{
		}

		public InputSnapshot(byte buttons)
		{
			_buttons = buttons;
		}

		public bool IsButtonDown(Buttons button)
		{
			return (_buttons & (byte)button) != 0;
		}

		public bool IsButtonUp(Buttons button)
		{
			return !IsButtonDown(button);
		}
	}
}
