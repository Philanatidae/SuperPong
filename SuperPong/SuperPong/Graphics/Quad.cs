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

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SuperPong.Graphics
{
    public class Quad
    {
        public VertexPositionNormalTexture[] Vertices
        {
            get;
            private set;
        }

        int[] _indices = new int[12];

        VertexDeclaration _vertexDeclaration;

        public Quad(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4, Vector3 Normal)
        {
            Vertices = new VertexPositionNormalTexture[4] {
                new VertexPositionNormalTexture(v1, Normal, new Vector2(0, 0)),
                new VertexPositionNormalTexture(v2, Normal, new Vector2(1, 0)),
                new VertexPositionNormalTexture(v3, Normal, new Vector2(0, 1)),
                new VertexPositionNormalTexture(v4, Normal, new Vector2(1, 1))
            };
            GenerateIndices();
            CreateVertexDeclaration();
        }

        void GenerateIndices()
        {
            _indices[0] = 0;
            _indices[1] = 1;
            _indices[2] = 2;
            _indices[3] = 2;
            _indices[4] = 1;
            _indices[5] = 3;

            _indices[6] = 2;
            _indices[7] = 1;
            _indices[8] = 0;
            _indices[9] = 3;
            _indices[10] = 1;
            _indices[11] = 2;
        }

        void CreateVertexDeclaration()
        {
            _vertexDeclaration = new VertexDeclaration(new VertexElement[]
            {
                new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
                new VertexElement(12, VertexElementFormat.Vector3, VertexElementUsage.Normal, 0),
                new VertexElement(24, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0)
            });
        }

        public void Draw(GraphicsDevice graphicsDevice)
        {
            graphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.TriangleList,
                                                          Vertices, 0, 4,
                                                          _indices, 0, 4
                                                     );
        }

    }
}