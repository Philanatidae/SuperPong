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
using ECS;
using Microsoft.Xna.Framework;
using SuperPong.Components;

namespace SuperPong.Systems
{
    public class AIThinkSystem : EntitySystem
    {
        Family _aiPaddles = Family.All(typeof(PaddleComponent), typeof(AIComponent), typeof(TransformComponent)).Get();
        Family _balls = Family.All(typeof(BallComponent), typeof(TransformComponent)).Get();

        ImmutableList<Entity> _aiPaddleEntities;
        ImmutableList<Entity> _ballEntities;

        public AIThinkSystem(Engine engine) : base(engine)
        {
            _aiPaddleEntities = GetEngine().GetEntitiesFor(_aiPaddles);
            _ballEntities = GetEngine().GetEntitiesFor(_balls);
        }

        public override void Update(float dt)
        {
            if (_ballEntities.Count > 0)
            {
                // TODO: Some sort of voting system if there are multiple balls
                Entity ball = _ballEntities[0];
                TransformComponent ballTransform = ball.GetComponent<TransformComponent>();
                BallComponent ballComp = ball.GetComponent<BallComponent>();

                for (int i = 0; i < _aiPaddleEntities.Count; i++)
                {
                    Entity ai = _aiPaddleEntities[i];
                    AIComponent aiComp = ai.GetComponent<AIComponent>();
                    PaddleComponent aiPaddleComp = ai.GetComponent<PaddleComponent>();
                    TransformComponent aiTransform = ai.GetComponent<TransformComponent>();

                    aiComp.AIPlayer.AIInputMethod.Think(aiTransform.Position,
                                                        aiPaddleComp.Normal,
                                                        ballTransform.Position,
                                                        ballTransform.Position - ballTransform.LastPosition,
                                                        ball);
                }
            }
        }
    }
}
