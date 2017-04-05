using System;
using System.Collections.Generic;
using Events.Exceptions;

namespace Events
{
	public class EventManager
	{
		static EventManager instance = new EventManager();
		public static EventManager Instance
		{
			get
			{
				return instance;
			}
		}

		Dictionary<Type, List<IEventListener>> _listeners = new Dictionary<Type, List<IEventListener>>();

		public void RegisterListener<T>(IEventListener listener) where T : IEvent
		{
			RegisterListener(typeof(T), listener);
		}

		public void RegisterListener(Type type, IEventListener listener)
		{
			if (!type.IsEvent())
			{
				throw new TypeNotEventException();
			}

			EnsureInitiatedListener(type);

			if (_listeners[type].Contains(listener))
			{
				throw new ListenerAlreadyExistsException();
			}

			_listeners[type].Add(listener);
		}

		public void UnregisterListener(IEventListener listener)
		{
			foreach (Type key in _listeners.Keys)
			{
				UnregisterListener(key, listener);
			}
		}

		public bool UnregisterListener<T>(IEventListener listener) where T : IEvent
		{
			return UnregisterListener(typeof(T), listener);
		}

		public bool UnregisterListener(Type type, IEventListener listener)
		{
			if (!type.IsEvent())
			{
				throw new TypeNotEventException();
			}

			EnsureInitiatedListener(type);

			return _listeners[type].Remove(listener);
		}

		public bool TriggerEvent(IEvent evt)
		{
			EnsureInitiatedListener(evt.GetType());

			foreach (IEventListener listener in _listeners[evt.GetType()])
			{
				if (listener.Handle(evt))
				{
					return true;
				}
			}

			return false;
		}

		void EnsureInitiatedListener(Type type)
		{
			if (!_listeners.ContainsKey(type))
			{
				_listeners.Add(type, new List<IEventListener>());
			}
		}

	}
}
