using System;

namespace ECS.Exceptions
{
	public class ComponentAlreadyExistsException : Exception
	{
		public ComponentAlreadyExistsException()
			: base("Component does not exist for this entity.")
		{
		}
	}
}
