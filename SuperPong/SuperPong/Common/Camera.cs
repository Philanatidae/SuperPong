using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SuperPong.Common
{
	public class Camera
	{
		public float Zoom = 1;
		public Vector2 Position;
		public float Rotation;

		Rectangle _bounds;

		public Matrix TransformMatrix
		{
			get
			{
				return Matrix.CreateTranslation(new Vector3(Position.X,
				                                            Position.Y * -1,
				                                            0))
							 * Matrix.CreateRotationZ(Rotation)
							 * Matrix.CreateScale(Zoom)
							 * Matrix.CreateTranslation(new Vector3(_bounds.Width * 0.5f,
																	_bounds.Height * 0.5f,
																   0));
			}
		}

		public Camera(Viewport viewport)
		{
			_bounds = viewport.Bounds;
		}

		public Vector2 ScreenToWorldCoords(Vector2 position)
		{
			return Vector2.Transform(position, Matrix.Invert(TransformMatrix));
		}

		public Vector2 WorldToScreenCoords(Vector2 position)
		{
			return Vector2.Transform(position, TransformMatrix);
		}

	}
}
