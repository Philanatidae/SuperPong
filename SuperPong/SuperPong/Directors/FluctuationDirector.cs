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
using Microsoft.Xna.Framework;
using SuperPong.Common;
using SuperPong.Events;
using SuperPong.Fluctuations;
using SuperPong.Processes;

namespace SuperPong.Directors
{
    public class FluctuationDirector : BaseDirector, IEventListener
    {
        readonly MTRandom _random;

        readonly Timer _fluctuationTimer = new Timer(0);
        int _fluctuationUnlockedLevel = 1;
        Type _lastFluctuation = null;

        public FluctuationDirector(IPongDirectorOwner owner) : base(owner)
        {
            _random = new MTRandom();
        }

        public override void RegisterEvents()
        {
            EventManager.Instance.RegisterListener<BallServeEvent>(this);
            EventManager.Instance.RegisterListener<FluctuationEndEvent>(this);
        }

        public override void UnregisterEvents()
        {
            EventManager.Instance.UnregisterListener(this);
        }

        public bool Handle(IEvent evt)
        {
            if (evt is BallServeEvent)
            {
                HandleBallServe(evt as BallServeEvent);
            }

            if (evt is FluctuationEndEvent)
            {
                HandleFluctuationEnd(evt as FluctuationEndEvent);
            }

            return false;
        }

        void AttachFluctuationSequence()
        {
            int availableFluctuationsCount = 0;
            for (int i = 0; i < _fluctuationUnlockedLevel; i++)
            {
                availableFluctuationsCount += Constants.Fluctuations.FLUCTUATIONS[i].Length;
            }

            Type[] fluctuations = new Type[availableFluctuationsCount];
            int k = 0;
            for (int i = 0; i < _fluctuationUnlockedLevel; i++)
            {
                for (int j = 0; j < Constants.Fluctuations.FLUCTUATIONS[i].Length; j++)
                {
                    fluctuations[k++] = Constants.Fluctuations.FLUCTUATIONS[i][j];
                }
            }

            Type fluctuationType = null;
            do
            {
                int index = _random.Next(0, fluctuations.Length - 1);
                fluctuationType = fluctuations[index];
            } while (fluctuationType == _lastFluctuation);

            WaitProcessKillOnEvent timerProcess = new WaitProcessKillOnEvent(_random.NextSingle() * (Constants.Fluctuations.TIMER_MAX - Constants.Fluctuations.TIMER_MIN) + Constants.Fluctuations.TIMER_MIN,
                                                                            typeof(GoalEvent));

            timerProcess.SetNext((Fluctuation)Activator.CreateInstance(fluctuationType, _owner));
            _processManager.Attach(timerProcess);
            _lastFluctuation = fluctuationType;

            _fluctuationUnlockedLevel = MathHelper.Min(++_fluctuationUnlockedLevel, Constants.Fluctuations.FLUCTUATIONS.Length);
        }

        // HANDLERS
        void HandleBallServe(BallServeEvent ballServeEvent)
        {
            _fluctuationUnlockedLevel = 1;
            AttachFluctuationSequence();
        }

        void HandleFluctuationEnd(FluctuationEndEvent fluctuationEndEvent)
        {
            if (fluctuationEndEvent.KillReason == Fluctuation.KillReason.NORMAL)
            {
                AttachFluctuationSequence();
            }
        }
    }
}
