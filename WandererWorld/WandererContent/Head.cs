using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace WandererWorld.WandererContent
{
    public class Head : WandererLimbs
    {
        private Game game;
        private Matrix objectWorld;

        private Matrix renderRotation;
        private Vector3 renderPosition;
        private Vector3 movementRotation = Vector3.Zero;
        private Vector3 movementPosition = Vector3.Zero;
        private Vector3 jointPosition = new Vector3(0, 0.5f, 0);

        public Head(Game game, Vector3 position) : base(game, "head")
        {
            LoadContent();
            this.game = game;

            renderPosition = position;
            objectWorld = Matrix.Identity;
            renderRotation = Matrix.Identity;
        }

        public override void DrawLimb(GameTime gameTime)
        {
            objectWorld = Matrix.CreateScale(scale) * renderRotation * Matrix.CreateTranslation(renderPosition);
            Effect.World = objectWorld * World;
            Effect.Texture = texture;

            foreach (EffectPass effect in Effect.CurrentTechnique.Passes)
            {
                effect.Apply();
                game.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 12);
            }
        }

        public override void UpdateLimbMovement(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
                movementRotation = new Vector3(movementRotation.X - 0.05f, movementRotation.Y, movementRotation.Z);

            if (Keyboard.GetState().IsKeyDown(Keys.Left))
                movementRotation = new Vector3(movementRotation.X + 0.05f, movementRotation.Y, movementRotation.Z);

            World = Matrix.Identity *
                Matrix.CreateTranslation(movementPosition) *
                Matrix.CreateFromQuaternion(Quaternion.CreateFromYawPitchRoll(movementRotation.X, movementRotation.Y, movementRotation.Z)) *
                Matrix.CreateTranslation(jointPosition);
        }
    }
}
