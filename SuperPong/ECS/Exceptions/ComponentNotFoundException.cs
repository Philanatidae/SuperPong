using System;

namespace ECS.Exceptions
{
	public class ComponentNotFoundException : Exception
	{
		public ComponentNotFoundException()
			:base("Component does not exist for this entity.") {
		}
	}
}
