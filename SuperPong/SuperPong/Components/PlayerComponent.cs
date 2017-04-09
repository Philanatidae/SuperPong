using System;
using ECS;
using SuperPong.Input;

namespace SuperPong.Components
{
	public class PlayerComponent : IComponent
	{
		public Player Player;

		public PlayerComponent(Player player)
		{
			Player = player;
		}

		public InputSnapshot Input
		{
			get
			{
				return Player.InputMethod.GetSnapshot();
			}
		}
	}
}
