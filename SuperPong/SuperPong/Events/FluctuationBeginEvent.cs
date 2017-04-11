using System;
using Events;
using SuperPong.Fluctuations;

namespace SuperPong.Events
{
	public class FluctuationBeginEvent : IEvent
	{
		public Fluctuation Fluctuation
		{
			get;
			private set;
		}

		public FluctuationBeginEvent(Fluctuation fluctuation)
		{
			Fluctuation = fluctuation;
		}
	}
}
