using ECS;

namespace SuperPong.Components
{
    public class GoalComponent : IComponent
    {

        public GoalComponent(Player forPlayer)
        {
            For = forPlayer;
        }

        public Player For
        {
            get;
            set;
        }

    }
}
