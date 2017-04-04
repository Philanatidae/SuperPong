using ECS;
using SuperPong.Input;

namespace SuperPong.Components
{
	public class InputComponent : IComponent
	{
		public InputComponent(InputMethod inputMethod)
		{
			InputMethod = inputMethod;
		}

		public readonly InputMethod InputMethod;

		public InputSnapshot Input
		{
			get
			{
				return InputMethod.GetSnapshot();
			}
		}
	}
}
