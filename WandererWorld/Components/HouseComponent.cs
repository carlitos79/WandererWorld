using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WandererWorld.Components
{
    public class HouseComponent : GenericComponent
    {
        public Vector3 Scale { get; set; }
        public Vector3 Position { get; set; }
        public Matrix Rotation { get; set; }
        public Texture2D Texture { get; set; }
        public VertexPositionNormalTexture[] Vertices { get; set; }
        public short[] Indices { get; private set; }

        public VertexBuffer VertexBuffer { get; private set; }
        
        public IndexBuffer IndexBuffer { get; private set; }

        // Vertex positions for box
        private static readonly Vector3 FRONT_TOP_LEFT = new Vector3(-0.5f, 0.5f, 0.5f);
        private static readonly Vector3 FRONT_TOP_RIGHT = new Vector3(0.5f, 0.5f, 0.5f);
        private static readonly Vector3 FRONT_BOTTOM_LEFT = new Vector3(-0.5f, -0.5f, 0.5f);
        private static readonly Vector3 FRONT_BOTTOM_RIGHT = new Vector3(0.5f, -0.5f, 0.5f);
        private static readonly Vector3 BACK_TOP_LEFT = new Vector3(-0.5f, 0.5f, -0.5f);
        private static readonly Vector3 BACK_TOP_RIGHT = new Vector3(0.5f, 0.5f, -0.5f);
        private static readonly Vector3 BACK_BOTTOM_LEFT = new Vector3(-0.5f, -0.5f, -0.5f);
        private static readonly Vector3 BACK_BOTTOM_RIGHT = new Vector3(0.5f, -0.5f, -0.5f);

        // Vertex positions for roof
        private static readonly Vector3 ROOF_LEFT = new Vector3(-0.5f, 1f, 0f);
        private static readonly Vector3 ROOF_RIGHT = new Vector3(0.5f, 1f, 0f);

        public HouseComponent(Vector3 scale, Vector3 pos, Matrix rot, Texture2D tex)
        {
            Scale = scale;
            Position = pos;
            Rotation = rot;
            Texture = tex;
            SetUpVertices();
            SetUpIndices();
        }

        private void SetUpVertices()
        {
            List<VertexPositionNormalTexture> vertexList = new List<VertexPositionNormalTexture>(36);

            // Front face
            vertexList.Add(new VertexPositionNormalTexture(FRONT_TOP_LEFT, Vector3.Forward, new Vector2(0, 1)));
            vertexList.Add(new VertexPositionNormalTexture(FRONT_BOTTOM_RIGHT, Vector3.Forward, new Vector2(1, 0)));
            vertexList.Add(new VertexPositionNormalTexture(FRONT_BOTTOM_LEFT, Vector3.Forward, new Vector2(0, 0)));
            vertexList.Add(new VertexPositionNormalTexture(FRONT_TOP_LEFT, Vector3.Forward, new Vector2(0, 1)));
            vertexList.Add(new VertexPositionNormalTexture(FRONT_TOP_RIGHT, Vector3.Forward, new Vector2(1, 1)));
            vertexList.Add(new VertexPositionNormalTexture(FRONT_BOTTOM_RIGHT, Vector3.Forward, new Vector2(1, 0)));

            // Top face
            vertexList.Add(new VertexPositionNormalTexture(BACK_TOP_LEFT, Vector3.Up, new Vector2(0, 1)));
            vertexList.Add(new VertexPositionNormalTexture(FRONT_TOP_RIGHT, Vector3.Up, new Vector2(1, 0)));
            vertexList.Add(new VertexPositionNormalTexture(FRONT_TOP_LEFT, Vector3.Up, new Vector2(0, 0)));
            vertexList.Add(new VertexPositionNormalTexture(BACK_TOP_LEFT, Vector3.Up, new Vector2(0, 1)));
            vertexList.Add(new VertexPositionNormalTexture(BACK_TOP_RIGHT, Vector3.Up, new Vector2(1, 1)));
            vertexList.Add(new VertexPositionNormalTexture(FRONT_TOP_RIGHT, Vector3.Up, new Vector2(1, 0)));

            // Right face
            vertexList.Add(new VertexPositionNormalTexture(FRONT_TOP_RIGHT, Vector3.Right, new Vector2(0, 1)));
            vertexList.Add(new VertexPositionNormalTexture(BACK_BOTTOM_RIGHT, Vector3.Right, new Vector2(1, 0)));
            vertexList.Add(new VertexPositionNormalTexture(FRONT_BOTTOM_RIGHT, Vector3.Right, new Vector2(0, 0)));
            vertexList.Add(new VertexPositionNormalTexture(FRONT_TOP_RIGHT, Vector3.Right, new Vector2(0, 1)));
            vertexList.Add(new VertexPositionNormalTexture(BACK_TOP_RIGHT, Vector3.Right, new Vector2(1, 1)));
            vertexList.Add(new VertexPositionNormalTexture(BACK_BOTTOM_RIGHT, Vector3.Right, new Vector2(1, 0)));

            // Bottom face
            vertexList.Add(new VertexPositionNormalTexture(FRONT_BOTTOM_LEFT, Vector3.Down, new Vector2(0, 1)));
            vertexList.Add(new VertexPositionNormalTexture(BACK_BOTTOM_RIGHT, Vector3.Down, new Vector2(1, 0)));
            vertexList.Add(new VertexPositionNormalTexture(BACK_BOTTOM_LEFT, Vector3.Down, new Vector2(0, 0)));
            vertexList.Add(new VertexPositionNormalTexture(FRONT_BOTTOM_LEFT, Vector3.Down, new Vector2(0, 1)));
            vertexList.Add(new VertexPositionNormalTexture(FRONT_BOTTOM_RIGHT, Vector3.Down, new Vector2(1, 1)));
            vertexList.Add(new VertexPositionNormalTexture(BACK_BOTTOM_RIGHT, Vector3.Down, new Vector2(1, 0)));

            // Left face
            vertexList.Add(new VertexPositionNormalTexture(BACK_TOP_LEFT, Vector3.Left, new Vector2(0, 1)));
            vertexList.Add(new VertexPositionNormalTexture(FRONT_BOTTOM_LEFT, Vector3.Left, new Vector2(1, 0)));
            vertexList.Add(new VertexPositionNormalTexture(BACK_BOTTOM_LEFT, Vector3.Left, new Vector2(0, 0)));
            vertexList.Add(new VertexPositionNormalTexture(BACK_TOP_LEFT, Vector3.Left, new Vector2(0, 1)));
            vertexList.Add(new VertexPositionNormalTexture(FRONT_TOP_LEFT, Vector3.Left, new Vector2(1, 1)));
            vertexList.Add(new VertexPositionNormalTexture(FRONT_BOTTOM_LEFT, Vector3.Left, new Vector2(1, 0)));

            // Back face
            vertexList.Add(new VertexPositionNormalTexture(BACK_TOP_RIGHT, Vector3.Backward, new Vector2(0, 1)));
            vertexList.Add(new VertexPositionNormalTexture(BACK_BOTTOM_LEFT, Vector3.Backward, new Vector2(1, 0)));
            vertexList.Add(new VertexPositionNormalTexture(BACK_BOTTOM_RIGHT, Vector3.Backward, new Vector2(0, 0)));
            vertexList.Add(new VertexPositionNormalTexture(BACK_TOP_RIGHT, Vector3.Backward, new Vector2(0, 1)));
            vertexList.Add(new VertexPositionNormalTexture(BACK_TOP_LEFT, Vector3.Backward, new Vector2(1, 1)));
            vertexList.Add(new VertexPositionNormalTexture(BACK_BOTTOM_LEFT, Vector3.Backward, new Vector2(1, 0)));

            // Roof front face
            vertexList.Add(new VertexPositionNormalTexture(ROOF_LEFT, Vector3.Forward + Vector3.Up, new Vector2(0, 1)));
            vertexList.Add(new VertexPositionNormalTexture(FRONT_TOP_RIGHT, Vector3.Forward, new Vector2(1, 0)));
            vertexList.Add(new VertexPositionNormalTexture(FRONT_TOP_LEFT, Vector3.Forward, new Vector2(0, 0)));
            vertexList.Add(new VertexPositionNormalTexture(ROOF_LEFT, Vector3.Forward + Vector3.Up, new Vector2(0, 1)));
            vertexList.Add(new VertexPositionNormalTexture(ROOF_RIGHT, Vector3.Forward + Vector3.Up, new Vector2(1, 1)));
            vertexList.Add(new VertexPositionNormalTexture(FRONT_TOP_RIGHT, Vector3.Forward, new Vector2(1, 0)));

            // Roof back face
            vertexList.Add(new VertexPositionNormalTexture(ROOF_RIGHT, Vector3.Forward + Vector3.Up, new Vector2(0, 1)));
            vertexList.Add(new VertexPositionNormalTexture(BACK_TOP_LEFT, Vector3.Forward, new Vector2(1, 0)));
            vertexList.Add(new VertexPositionNormalTexture(BACK_TOP_RIGHT, Vector3.Forward, new Vector2(0, 0)));
            vertexList.Add(new VertexPositionNormalTexture(ROOF_RIGHT, Vector3.Forward + Vector3.Up, new Vector2(0, 1)));
            vertexList.Add(new VertexPositionNormalTexture(ROOF_LEFT, Vector3.Forward + Vector3.Up, new Vector2(1, 1)));
            vertexList.Add(new VertexPositionNormalTexture(BACK_TOP_LEFT, Vector3.Forward, new Vector2(1, 0)));

            // Roof left face
            vertexList.Add(new VertexPositionNormalTexture(ROOF_LEFT, Vector3.Left, new Vector2(0, 1)));
            vertexList.Add(new VertexPositionNormalTexture(FRONT_TOP_LEFT, Vector3.Left, new Vector2(1, 0)));
            vertexList.Add(new VertexPositionNormalTexture(BACK_TOP_LEFT, Vector3.Left, new Vector2(0, 0)));

            // Roof right face
            vertexList.Add(new VertexPositionNormalTexture(ROOF_RIGHT, Vector3.Right, new Vector2(0, 1)));
            vertexList.Add(new VertexPositionNormalTexture(FRONT_TOP_RIGHT, Vector3.Right, new Vector2(1, 0)));
            vertexList.Add(new VertexPositionNormalTexture(BACK_TOP_RIGHT, Vector3.Right, new Vector2(0, 0)));


            Vertices = vertexList.ToArray();
        }

        private void SetUpIndices()
        {
            List<short> indexList = new List<short>(54);

            for (short i = 0; i < 54; ++i)
                indexList.Add(i);

            Indices = indexList.ToArray();
        }
    }
}
