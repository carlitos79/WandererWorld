using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace WandererWorld.WandererContent
{
    public class Head : WandererLimbs
    {
        private Game game;
        public Matrix limbWorld;

        private Vector3 position;
        private Vector3 rotation = Vector3.Zero;

        public Head(Game game, Vector3 position) : base(game, "head")
        {
            LoadContent();
            this.game = game;

            this.position = position;
        }

        public override void DrawLimb(GameTime gameTime, Matrix world)
        {
            limbWorld = Matrix.CreateScale(scale) * World * Matrix.CreateTranslation(position);
            Effect.World = limbWorld * world;
            Effect.Texture = texture;

            game.GraphicsDevice.SetVertexBuffer(vertexBuffer);
            game.GraphicsDevice.Indices = indexBuffer;

            foreach (EffectPass effect in Effect.CurrentTechnique.Passes)
            {
                effect.Apply();
                game.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 12);
            }
        }

        public override void UpdateLimbMovement(GameTime gameTime)
        {
            // No code is necessary here since this limb gets its rotation and position from its parent
            // in the DrawLimb method through the parameter Matrix world and it WON'T swing.
        }
    }
}
