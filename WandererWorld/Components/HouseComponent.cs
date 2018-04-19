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
        public Texture2D WallTexture { get; set; }
        public Texture2D RoofTexture { get; set; }
        public VertexPositionNormalTexture[] WallVertices { get; set; }
        public VertexPositionNormalTexture[] RoofVertices { get; set; }
        public short[] WallIndices { get; private set; }
        public short[] RoofIndices { get; private set; }

        public VertexBuffer WallVertexBuffer { get; private set; }
        public IndexBuffer WallIndexBuffer { get; private set; }
        public VertexBuffer RoofVertexBuffer { get; private set; }
        public IndexBuffer RoofIndexBuffer { get; private set; }

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
        private static readonly Vector3 ROOF_LEFT = new Vector3(-0.5f, 0.85f, 0f);
        private static readonly Vector3 ROOF_RIGHT = new Vector3(0.5f, 0.85f, 0f);

        public HouseComponent(Vector3 scale, Vector3 pos, Matrix rot, Texture2D wall, Texture2D roof)
        {
            Scale = scale;
            Position = pos;
            Rotation = rot;
            WallTexture = wall;
            RoofTexture = roof;
            SetUpVertices();
            SetUpIndices();
        }

        private void SetUpVertices()
        {
            List<VertexPositionNormalTexture> wallVertexList = new List<VertexPositionNormalTexture>(36);
            List<VertexPositionNormalTexture> roofVertexList = new List<VertexPositionNormalTexture>(18);

            // Front face
            wallVertexList.Add(new VertexPositionNormalTexture(FRONT_TOP_LEFT, Vector3.Forward, new Vector2(0, 0)));
            wallVertexList.Add(new VertexPositionNormalTexture(FRONT_BOTTOM_RIGHT, Vector3.Forward, new Vector2(1, 1)));
            wallVertexList.Add(new VertexPositionNormalTexture(FRONT_BOTTOM_LEFT, Vector3.Forward, new Vector2(0, 1)));
            wallVertexList.Add(new VertexPositionNormalTexture(FRONT_TOP_LEFT, Vector3.Forward, new Vector2(0, 0)));
            wallVertexList.Add(new VertexPositionNormalTexture(FRONT_TOP_RIGHT, Vector3.Forward, new Vector2(1, 0)));
            wallVertexList.Add(new VertexPositionNormalTexture(FRONT_BOTTOM_RIGHT, Vector3.Forward, new Vector2(1, 1)));

            // Top face
            wallVertexList.Add(new VertexPositionNormalTexture(BACK_TOP_LEFT, Vector3.Up, new Vector2(0, 0)));
            wallVertexList.Add(new VertexPositionNormalTexture(FRONT_TOP_RIGHT, Vector3.Up, new Vector2(1, 1)));
            wallVertexList.Add(new VertexPositionNormalTexture(FRONT_TOP_LEFT, Vector3.Up, new Vector2(0, 1)));
            wallVertexList.Add(new VertexPositionNormalTexture(BACK_TOP_LEFT, Vector3.Up, new Vector2(0, 0)));
            wallVertexList.Add(new VertexPositionNormalTexture(BACK_TOP_RIGHT, Vector3.Up, new Vector2(1, 0)));
            wallVertexList.Add(new VertexPositionNormalTexture(FRONT_TOP_RIGHT, Vector3.Up, new Vector2(1, 0)));

            // Right face
            wallVertexList.Add(new VertexPositionNormalTexture(FRONT_TOP_RIGHT, Vector3.Right, new Vector2(0, 0)));
            wallVertexList.Add(new VertexPositionNormalTexture(BACK_BOTTOM_RIGHT, Vector3.Right, new Vector2(1, 1)));
            wallVertexList.Add(new VertexPositionNormalTexture(FRONT_BOTTOM_RIGHT, Vector3.Right, new Vector2(0, 1)));
            wallVertexList.Add(new VertexPositionNormalTexture(FRONT_TOP_RIGHT, Vector3.Right, new Vector2(0, 0)));
            wallVertexList.Add(new VertexPositionNormalTexture(BACK_TOP_RIGHT, Vector3.Right, new Vector2(1, 0)));
            wallVertexList.Add(new VertexPositionNormalTexture(BACK_BOTTOM_RIGHT, Vector3.Right, new Vector2(1, 1)));

            // Bottom face
            wallVertexList.Add(new VertexPositionNormalTexture(FRONT_BOTTOM_LEFT, Vector3.Down, new Vector2(0, 0)));
            wallVertexList.Add(new VertexPositionNormalTexture(BACK_BOTTOM_RIGHT, Vector3.Down, new Vector2(1, 1)));
            wallVertexList.Add(new VertexPositionNormalTexture(BACK_BOTTOM_LEFT, Vector3.Down, new Vector2(0, 1)));
            wallVertexList.Add(new VertexPositionNormalTexture(FRONT_BOTTOM_LEFT, Vector3.Down, new Vector2(0, 1)));
            wallVertexList.Add(new VertexPositionNormalTexture(FRONT_BOTTOM_RIGHT, Vector3.Down, new Vector2(1, 1)));
            wallVertexList.Add(new VertexPositionNormalTexture(BACK_BOTTOM_RIGHT, Vector3.Down, new Vector2(1, 1)));

            // Left face
            wallVertexList.Add(new VertexPositionNormalTexture(BACK_TOP_LEFT, Vector3.Left, new Vector2(0, 0)));
            wallVertexList.Add(new VertexPositionNormalTexture(FRONT_BOTTOM_LEFT, Vector3.Left, new Vector2(1, 1)));
            wallVertexList.Add(new VertexPositionNormalTexture(BACK_BOTTOM_LEFT, Vector3.Left, new Vector2(0, 1)));
            wallVertexList.Add(new VertexPositionNormalTexture(BACK_TOP_LEFT, Vector3.Left, new Vector2(0, 0)));
            wallVertexList.Add(new VertexPositionNormalTexture(FRONT_TOP_LEFT, Vector3.Left, new Vector2(1, 0)));
            wallVertexList.Add(new VertexPositionNormalTexture(FRONT_BOTTOM_LEFT, Vector3.Left, new Vector2(1, 1)));

            // Back face
            wallVertexList.Add(new VertexPositionNormalTexture(BACK_TOP_RIGHT, Vector3.Backward, new Vector2(0, 0)));
            wallVertexList.Add(new VertexPositionNormalTexture(BACK_BOTTOM_LEFT, Vector3.Backward, new Vector2(1, 1)));
            wallVertexList.Add(new VertexPositionNormalTexture(BACK_BOTTOM_RIGHT, Vector3.Backward, new Vector2(0, 1)));
            wallVertexList.Add(new VertexPositionNormalTexture(BACK_TOP_RIGHT, Vector3.Backward, new Vector2(0, 0)));
            wallVertexList.Add(new VertexPositionNormalTexture(BACK_TOP_LEFT, Vector3.Backward, new Vector2(1, 0)));
            wallVertexList.Add(new VertexPositionNormalTexture(BACK_BOTTOM_LEFT, Vector3.Backward, new Vector2(1, 1)));

            // Roof front face
            roofVertexList.Add(new VertexPositionNormalTexture(ROOF_LEFT, Vector3.Forward + Vector3.Up, new Vector2(0, 1)));
            roofVertexList.Add(new VertexPositionNormalTexture(FRONT_TOP_RIGHT, Vector3.Forward, new Vector2(1, 0)));
            roofVertexList.Add(new VertexPositionNormalTexture(FRONT_TOP_LEFT, Vector3.Forward, new Vector2(0, 0)));
            roofVertexList.Add(new VertexPositionNormalTexture(ROOF_LEFT, Vector3.Forward + Vector3.Up, new Vector2(0, 1)));
            roofVertexList.Add(new VertexPositionNormalTexture(ROOF_RIGHT, Vector3.Forward + Vector3.Up, new Vector2(1, 1)));
            roofVertexList.Add(new VertexPositionNormalTexture(FRONT_TOP_RIGHT, Vector3.Forward, new Vector2(1, 0)));

            // Roof back face
            roofVertexList.Add(new VertexPositionNormalTexture(ROOF_RIGHT, Vector3.Forward + Vector3.Up, new Vector2(0, 1)));
            roofVertexList.Add(new VertexPositionNormalTexture(BACK_TOP_LEFT, Vector3.Forward, new Vector2(1, 0)));
            roofVertexList.Add(new VertexPositionNormalTexture(BACK_TOP_RIGHT, Vector3.Forward, new Vector2(0, 0)));
            roofVertexList.Add(new VertexPositionNormalTexture(ROOF_RIGHT, Vector3.Forward + Vector3.Up, new Vector2(0, 1)));
            roofVertexList.Add(new VertexPositionNormalTexture(ROOF_LEFT, Vector3.Forward + Vector3.Up, new Vector2(1, 1)));
            roofVertexList.Add(new VertexPositionNormalTexture(BACK_TOP_LEFT, Vector3.Forward, new Vector2(1, 0)));

            // Roof left face
            roofVertexList.Add(new VertexPositionNormalTexture(ROOF_LEFT, Vector3.Left, new Vector2(1, 1)));
            roofVertexList.Add(new VertexPositionNormalTexture(FRONT_TOP_LEFT, Vector3.Left, new Vector2(2, 0)));
            roofVertexList.Add(new VertexPositionNormalTexture(BACK_TOP_LEFT, Vector3.Left, new Vector2(0, 0)));

            // Roof right face
            roofVertexList.Add(new VertexPositionNormalTexture(ROOF_RIGHT, Vector3.Right, new Vector2(1, 1)));
            roofVertexList.Add(new VertexPositionNormalTexture(BACK_TOP_RIGHT, Vector3.Right, new Vector2(2, 0)));
            roofVertexList.Add(new VertexPositionNormalTexture(FRONT_TOP_RIGHT, Vector3.Right, new Vector2(0, 0)));

            WallVertices = wallVertexList.ToArray();
            RoofVertices = roofVertexList.ToArray();
        }

        private void SetUpIndices()
        {
            List<short> wallIndices = new List<short>(36);
            for (short i = 0; i < 54; ++i)
                wallIndices.Add(i);
            WallIndices = wallIndices.ToArray();

            List<short> roofIndices = new List<short>(36);
            for (short i = 0; i < 18; ++i)
                roofIndices.Add(i);
            RoofIndices = roofIndices.ToArray();
        }
    }
}
