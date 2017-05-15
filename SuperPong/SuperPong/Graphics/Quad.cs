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