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

		public CreateBall(Engine engine, Texture2D texture)
		{
			_engine = engine;
			_texture = texture;
		}

		protected override void OnTrigger()
		{
			BallEntity.Create(_engine, _texture, Vector2.Zero);
		}
	}
}
