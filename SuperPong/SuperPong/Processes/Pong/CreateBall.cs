using System;
using ECS;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SuperPong.Entities;

namespace SuperPong.Processes.Pong
{
	public class CreateBall : Command
	{
		readonly Engine _engine;
		readonly Texture2D _texture;
		readonly float _direction;

		public CreateBall(Engine engine, Texture2D texture, float direction)
		{
			_engine = engine;
			_texture = texture;
			_direction = direction;
		}

		protected override void OnTrigger()
		{
			BallEntity.Create(_engine, _texture, Vector2.Zero, _direction);
		}
	}
}
