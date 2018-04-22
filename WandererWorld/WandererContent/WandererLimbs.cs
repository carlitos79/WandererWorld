using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace WandererWorld.WandererContent
{
    public abstract class WandererLimbs : IWanderer
    {
        private Game game;

        protected Matrix World;
        protected Vector3 scale;
        protected Texture2D texture, robotTexture;
        private VertexPositionNormalTexture[] vertices;
        private VertexBuffer vertexBuffer;
        private short[] indices;
        private IndexBuffer indexBuffer;
        protected BasicEffect Effect;
        private Matrix view;
        private Matrix projection;

        // Vertex positions
        private static readonly Vector3 FRONT_TOP_LEFT = new Vector3(-0.5f, 0.5f, 0.5f);
        private static readonly Vector3 FRONT_TOP_RIGHT = new Vector3(0.5f, 0.5f, 0.5f);
        private static readonly Vector3 FRONT_BOTTOM_LEFT = new Vector3(-0.5f, -0.5f, 0.5f);
        private static readonly Vector3 FRONT_BOTTOM_RIGHT = new Vector3(0.5f, -0.5f, 0.5f);
        private static readonly Vector3 BACK_TOP_LEFT = new Vector3(-0.5f, 0.5f, -0.5f);
        private static readonly Vector3 BACK_TOP_RIGHT = new Vector3(0.5f, 0.5f, -0.5f);
        private static readonly Vector3 BACK_BOTTOM_LEFT = new Vector3(-0.5f, -0.5f, -0.5f);
        private static readonly Vector3 BACK_BOTTOM_RIGHT = new Vector3(0.5f, -0.5f, -0.5f);

        // Normals
        private static readonly Vector3 RIGHT = new Vector3(1, 0, 0); // +X
        private static readonly Vector3 LEFT = new Vector3(-1, 0, 0); // -X
        private static readonly Vector3 UP = new Vector3(0, 1, 0); // +Y
        private static readonly Vector3 DOWN = new Vector3(0, -1, 0); // -Y
        private static readonly Vector3 FORWARD = new Vector3(0, 0, 1); // +Z
        private static readonly Vector3 BACKWARD = new Vector3(0, 0, -1); // -Z

        public WandererLimbs(Game game, string limbName)
        {
            this.game = game;

            DetermineLimb(limbName);
        }

        public void DetermineLimb(string limb)
        {
            switch (limb)
            {
                case "head":
                    scale = new Vector3(2, 2, 2);
                    break;
                case "torso":
                    scale = new Vector3(3, 5, 3);
                    break;
                case "rightArm":
                    scale = new Vector3(1, 4, 1);
                    break;
                case "leftArm":
                    scale = new Vector3(1, 4, 1);
                    break;
                case "rightLeg":
                    scale = new Vector3(1.5f, 5, 1.5f);
                    break;
                case "leftLeg":
                    scale = new Vector3(1.5f, 5, 1.5f);
                    break;
                case "rightHand":
                    scale = new Vector3(1, 1, 1);
                    break;
                case "leftHand":
                    scale = new Vector3(1, 1, 1);
                    break;
            }
        }

        public void LoadContent()
        {
            World = Matrix.Identity;

            robotTexture = game.Content.Load<Texture2D>("robot_texture");
            texture = robotTexture;

            SetupVertices();
            SetupVertexBuffer();

            SetupIndices();
            SetupIndexBuffer();

            SetVertexBufferAndIndices();
            SetEffects();
            SetupCamera();
        }

        public void SetEffects()
        {
            SetupCamera();

            Effect = new BasicEffect(game.GraphicsDevice);
            Effect.View = view;
            Effect.Projection = projection;
            Effect.VertexColorEnabled = false;
            Effect.TextureEnabled = true;
            Effect.EnableDefaultLighting();
            Effect.LightingEnabled = false;
        }

        private void SetupCamera()
        {
            view = Matrix.CreateLookAt(new Vector3(0, 0, 20), new Vector3(0, 0, 0), Vector3.Up);
            projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver2, game.GraphicsDevice.Viewport.AspectRatio, 0.1f, 1000f);
        }

        private void SetupVertices()
        {
            List<VertexPositionNormalTexture> vertexList = new List<VertexPositionNormalTexture>(36);

            // Front face
            vertexList.Add(new VertexPositionNormalTexture(FRONT_TOP_LEFT, FORWARD, new Vector2(0, 1)));
            vertexList.Add(new VertexPositionNormalTexture(FRONT_BOTTOM_RIGHT, FORWARD, new Vector2(1, 0)));
            vertexList.Add(new VertexPositionNormalTexture(FRONT_BOTTOM_LEFT, FORWARD, new Vector2(0, 0)));
            vertexList.Add(new VertexPositionNormalTexture(FRONT_TOP_LEFT, FORWARD, new Vector2(0, 1)));
            vertexList.Add(new VertexPositionNormalTexture(FRONT_TOP_RIGHT, FORWARD, new Vector2(1, 1)));
            vertexList.Add(new VertexPositionNormalTexture(FRONT_BOTTOM_RIGHT, FORWARD, new Vector2(1, 0)));

            // Top face
            vertexList.Add(new VertexPositionNormalTexture(BACK_TOP_LEFT, UP, new Vector2(0, 1)));
            vertexList.Add(new VertexPositionNormalTexture(FRONT_TOP_RIGHT, UP, new Vector2(1, 0)));
            vertexList.Add(new VertexPositionNormalTexture(FRONT_TOP_LEFT, UP, new Vector2(0, 0)));
            vertexList.Add(new VertexPositionNormalTexture(BACK_TOP_LEFT, UP, new Vector2(0, 1)));
            vertexList.Add(new VertexPositionNormalTexture(BACK_TOP_RIGHT, UP, new Vector2(1, 1)));
            vertexList.Add(new VertexPositionNormalTexture(FRONT_TOP_RIGHT, UP, new Vector2(1, 0)));

            // Right face
            vertexList.Add(new VertexPositionNormalTexture(FRONT_TOP_RIGHT, RIGHT, new Vector2(0, 1)));
            vertexList.Add(new VertexPositionNormalTexture(BACK_BOTTOM_RIGHT, RIGHT, new Vector2(1, 0)));
            vertexList.Add(new VertexPositionNormalTexture(FRONT_BOTTOM_RIGHT, RIGHT, new Vector2(0, 0)));
            vertexList.Add(new VertexPositionNormalTexture(FRONT_TOP_RIGHT, RIGHT, new Vector2(0, 1)));
            vertexList.Add(new VertexPositionNormalTexture(BACK_TOP_RIGHT, RIGHT, new Vector2(1, 1)));
            vertexList.Add(new VertexPositionNormalTexture(BACK_BOTTOM_RIGHT, RIGHT, new Vector2(1, 0)));

            // Bottom face
            vertexList.Add(new VertexPositionNormalTexture(FRONT_BOTTOM_LEFT, DOWN, new Vector2(0, 1)));
            vertexList.Add(new VertexPositionNormalTexture(BACK_BOTTOM_RIGHT, DOWN, new Vector2(1, 0)));
            vertexList.Add(new VertexPositionNormalTexture(BACK_BOTTOM_LEFT, DOWN, new Vector2(0, 0)));
            vertexList.Add(new VertexPositionNormalTexture(FRONT_BOTTOM_LEFT, DOWN, new Vector2(0, 1)));
            vertexList.Add(new VertexPositionNormalTexture(FRONT_BOTTOM_RIGHT, DOWN, new Vector2(1, 1)));
            vertexList.Add(new VertexPositionNormalTexture(BACK_BOTTOM_RIGHT, DOWN, new Vector2(1, 0)));

            // Left face
            vertexList.Add(new VertexPositionNormalTexture(BACK_TOP_LEFT, LEFT, new Vector2(0, 1)));
            vertexList.Add(new VertexPositionNormalTexture(FRONT_BOTTOM_LEFT, LEFT, new Vector2(1, 0)));
            vertexList.Add(new VertexPositionNormalTexture(BACK_BOTTOM_LEFT, LEFT, new Vector2(0, 0)));
            vertexList.Add(new VertexPositionNormalTexture(BACK_TOP_LEFT, LEFT, new Vector2(0, 1)));
            vertexList.Add(new VertexPositionNormalTexture(FRONT_TOP_LEFT, LEFT, new Vector2(1, 1)));
            vertexList.Add(new VertexPositionNormalTexture(FRONT_BOTTOM_LEFT, LEFT, new Vector2(1, 0)));

            // Back face
            vertexList.Add(new VertexPositionNormalTexture(BACK_TOP_RIGHT, BACKWARD, new Vector2(0, 1)));
            vertexList.Add(new VertexPositionNormalTexture(BACK_BOTTOM_LEFT, BACKWARD, new Vector2(1, 0)));
            vertexList.Add(new VertexPositionNormalTexture(BACK_BOTTOM_RIGHT, BACKWARD, new Vector2(0, 0)));
            vertexList.Add(new VertexPositionNormalTexture(BACK_TOP_RIGHT, BACKWARD, new Vector2(0, 1)));
            vertexList.Add(new VertexPositionNormalTexture(BACK_TOP_LEFT, BACKWARD, new Vector2(1, 1)));
            vertexList.Add(new VertexPositionNormalTexture(BACK_BOTTOM_LEFT, BACKWARD, new Vector2(1, 0)));

            vertices = vertexList.ToArray();
        }

        private void SetupVertexBuffer()
        {
            if (vertexBuffer == null)
            {
                vertexBuffer = new VertexBuffer(game.GraphicsDevice, typeof(VertexPositionNormalTexture), vertices.Length, BufferUsage.None);
                vertexBuffer.SetData(vertices);
            }
        }

        private void SetupIndices()
        {
            List<short> indexList = new List<short>(36);

            for (short i = 0; i < 36; ++i)
                indexList.Add(i);

            indices = indexList.ToArray();
        }

        private void SetupIndexBuffer()
        {
            indexBuffer = new IndexBuffer(game.GraphicsDevice, typeof(short), indices.Length, BufferUsage.None);
            indexBuffer.SetData(indices);
        }

        public void SetVertexBufferAndIndices()
        {
            game.GraphicsDevice.SetVertexBuffer(vertexBuffer);
            game.GraphicsDevice.Indices = indexBuffer;
        }

        public virtual void UpdateLimbMovement(GameTime gameTime)
        {

        }        

        public virtual void DrawLimb(GameTime gameTime)
        {

        }
    }
}
