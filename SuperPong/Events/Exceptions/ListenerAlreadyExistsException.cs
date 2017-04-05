using System;
namespace Events.Exceptions
{
	public class ListenerAlreadyExistsException : Exception
	{
		public ListenerAlreadyExistsException()
			:base("The type passed does not exend IComponent")
		{
		}
	}
}
