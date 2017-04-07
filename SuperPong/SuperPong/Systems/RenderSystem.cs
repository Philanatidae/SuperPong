﻿using ECS;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;
using SuperPong.Components;

namespace SuperPong.Systems
{
	public class RenderSystem
	{
		readonly Engine _engine;

		public static readonly Vector2 FlipY = new Vector2(1, -1);
		public static readonly Vector2 HalfHalf = new Vector2(0.5f, 0.5f);

		Family _spriteFamily = Family.All(typeof(SpriteComponent), typeof(TransformComponent)).Get();
		Family _fontFamily = Family.All(typeof(FontComponent), typeof(TransformComponent)).Get();
		ImmutableList<Entity> _spriteEntities;
		ImmutableList<Entity> _fontEntities;

		SpriteBatch _spriteBatch;
		public SpriteBatch SpriteBatch
		{
			get {
				return _spriteBatch;
			}
		}

		public RenderSystem(GraphicsDevice graphics, Engine engine)
		{
			_engine = engine;
			_spriteEntities = engine.GetEntitiesFor(_spriteFamily);
			_fontEntities = engine.GetEntitiesFor(_fontFamily);

			_spriteBatch = new SpriteBatch(graphics);
		}

		public void DrawEntities(GameTime gameTime)
		{
			DrawEntities(Constants.Render.GROUP_MASK_ALL, gameTime);
		}

		public void DrawEntities(byte groupMask, GameTime gameTime)
		{
			DrawEntities(Matrix.Identity, groupMask, gameTime);
		}

		public void DrawEntities(Matrix transformMatrix, byte groupMask, GameTime gameTime)
		{
			_spriteBatch.Begin(SpriteSortMode.Deferred,
							   null,
							   null,
							   null,
							   null,
							   null,
							   transformMatrix);

			foreach (Entity entity in _spriteEntities)
			{
				SpriteComponent spriteComp = entity.GetComponent<SpriteComponent>();
				if((spriteComp.RenderGroup & groupMask) == 0) {
					continue;
				}

				TransformComponent transformComp = entity.GetComponent<TransformComponent>();

				Vector2 scale = new Vector2(spriteComp.Bounds.X / spriteComp.Texture.Width,
				                            spriteComp.Bounds.Y / spriteComp.Texture.Height);
				Vector2 origin = new Vector2(spriteComp.Texture.Bounds.Width,
				                             spriteComp.Texture.Bounds.Height) * HalfHalf;
 
				_spriteBatch.Draw(spriteComp.Texture,
								  transformComp.position * FlipY,
								  null,
								  Color.White,
				                  transformComp.rotation,
				                  origin,
				                  scale,
								  SpriteEffects.None,
								  0);
			}

			foreach (Entity entity in _fontEntities)
			{
				FontComponent fontComp = entity.GetComponent<FontComponent>();
				if ((fontComp.RenderGroup & groupMask) == 0)
				{
					continue;
				}

				TransformComponent transformComp = entity.GetComponent<TransformComponent>();

				Vector2 scale = Vector2.One;
				Vector2 origin = fontComp.Font.MeasureString(fontComp.Content);

				_spriteBatch.DrawString(fontComp.Font,
										fontComp.Content,
										transformComp.position * FlipY,
										fontComp.Color,
										transformComp.rotation,
										origin,
										scale,
										SpriteEffects.None,
										0);
			}

			_spriteBatch.End();
		}

		public Engine getEngine()
		{
			return _engine;
		}
	}
}