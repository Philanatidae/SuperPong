using ECS;

namespace SuperPong.Components
{
    public class AIComponent : IComponent
    {
        public AIPlayer AIPlayer = null;

        public AIComponent(AIPlayer player)
        {
            AIPlayer = player;
        }
    }
}
