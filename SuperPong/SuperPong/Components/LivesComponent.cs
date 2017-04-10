using ECS;

namespace SuperPong.Components
{
	public class LivesComponent : IComponent
	{
		public LivesComponent(Player forPlayer, int lives)
		{
			For = forPlayer;
			Lives = lives;
		}

		public Player For;
		public int Lives;
	}
}
