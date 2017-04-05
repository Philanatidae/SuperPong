using System;
namespace Events.Exceptions
{
	public class TypeNotEventException : Exception
	{
		public TypeNotEventException()
			:base("The type passed does not exend IComponent")
		{
		}
	}
}
