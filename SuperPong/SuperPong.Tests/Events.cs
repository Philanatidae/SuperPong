/*
This file is part of Super Pong.

Super Pong is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

Super Pong is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with Super Pong.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;
using Events;
using Events.Exceptions;
using NUnit.Framework;

namespace SuperPong.Tests
{
    [TestFixture]
    public class Events
    {
        [Test]
        public void ValidateExceptions()
        {
            Assert.Catch(typeof(TypeNotEventException), () =>
            {
                EventManager.Instance.RegisterListener(typeof(Generic1), new Listener1());
            });
            Assert.Catch(typeof(TypeNotEventException), () =>
            {
                EventManager.Instance.UnregisterListener(typeof(Generic1), new Listener1());
            });

            Assert.DoesNotThrow(() =>
            {
                EventManager.Instance.TriggerEvent(new Event1());
            });
            Assert.DoesNotThrow(() =>
            {
                EventManager.Instance.QueueEvent(new Event1());
            });

            Listener1 listener = new Listener1();
            Assert.DoesNotThrow(() =>
            {
                EventManager.Instance.RegisterListener(typeof(Event1), listener);
            });
            Assert.DoesNotThrow(() =>
            {
                EventManager.Instance.UnregisterListener(typeof(Event1), listener);
            });

            EventManager.Instance.RegisterListener(typeof(Event1), listener);
            Assert.Throws(typeof(ListenerAlreadyExistsException), () =>
            {
                EventManager.Instance.RegisterListener(typeof(Event1), listener);
            });
        }

        [Test]
        public void TriggerEvent()
        {
            Listener1 listener1 = new Listener1();
            Listener2 listener2 = new Listener2();

            EventManager.Instance.RegisterListener<Event1>(listener1);

            Assert.False(EventManager.Instance.TriggerEvent(new Event1()));

            EventManager.Instance.RegisterListener<Event1>(listener2);
            Assert.True(EventManager.Instance.TriggerEvent(new Event1()));

            EventManager.Instance.UnregisterListener<Event1>(listener2);
            Assert.False(EventManager.Instance.TriggerEvent(new Event1()));

            EventManager.Instance.RegisterListener<Event1>(listener2);
            EventManager.Instance.UnregisterListener(listener2);
            Assert.False(EventManager.Instance.TriggerEvent(new Event1()));

            Assert.That(() =>
            {
                bool eventPropogated = false;

                PassListener listener = new PassListener((bool pass) =>
                {
                    eventPropogated = pass;
                });
                EventManager.Instance.RegisterListener<Event1>(listener);

                EventManager.Instance.TriggerEvent(new Event1());

                EventManager.Instance.UnregisterListener(listener);

                return eventPropogated;
            });
        }

        [Test]
        public void QueueEvent()
        {
            Assert.That(() =>
            {
                bool eventPropogated = false;

                PassListener listener = new PassListener((bool pass) =>
                {
                    eventPropogated = pass;
                });
                EventManager.Instance.RegisterListener<Event1>(listener);

                EventManager.Instance.QueueEvent(new Event1());

                EventManager.Instance.UnregisterListener(listener);

                return !eventPropogated;
            });

            Assert.That(() =>
            {
                bool eventPropogated = false;

                PassListener listener = new PassListener((bool pass) =>
                {
                    eventPropogated = pass;
                });
                EventManager.Instance.RegisterListener<Event1>(listener);

                EventManager.Instance.QueueEvent(new Event1());
                EventManager.Instance.Dispatch();

                EventManager.Instance.UnregisterListener(listener);

                return eventPropogated;
            });
        }
    }

    class Generic1
    {
    }

    class Event1 : IEvent
    {
    }

    class Listener1 : IEventListener
    {
        public bool Handle(IEvent evt)
        {
            return false;
        }
    }
    class Listener2 : IEventListener
    {
        public bool Handle(IEvent evt)
        {
            return true;
        }
    }

    class PassListener : IEventListener
    {
        Action<bool> _func;
        public PassListener(Action<bool> func)
        {
            _func = func;
        }

        public bool Handle(IEvent evt)
        {
            if (evt is Event1)
            {
                _func.Invoke(true);
                return true;
            }
            _func.Invoke(false);
            return false;
        }
    }
}
