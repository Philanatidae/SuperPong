using System;

namespace ECS.Exceptions
{
	public class TypeNotComponentException : Exception
	{
		public TypeNotComponentException()
			:base("The type passed does not exend IEvent")
		{
		}
	}
}
