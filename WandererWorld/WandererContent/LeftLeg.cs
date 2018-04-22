using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace WandererWorld.WandererContent
{
    public class LeftLeg : WandererLimbs
    {
        private List<IWanderer> children = new List<IWanderer>();

        private Game game;
        private Matrix objectWorld;

        private Matrix renderRotation;
        private Vector3 renderPosition;
        private Vector3 movementRotation = Vector3.Zero;
        private Vector3 movementPosition = new Vector3(0, 1.5f, 0);
        private Vector3 jointPosition = new Vector3(0, 0, 0);

        public LeftLeg(Game game, Vector3 position) : base(game, "leftLeg")
        {
            LoadContent();
            this.game = game;

            renderPosition = position;
            objectWorld = Matrix.Identity;
            renderRotation = Matrix.Identity;
            movementRotation.Y = 4;

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
            float speed = 0;
            float maxRotation = 2.36f;
            float minRotation = 3.9f;

            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                speed += 0.003f * (float)gameTime.ElapsedGameTime.Milliseconds;

                if (movementRotation.Y >= maxRotation)
                {
                    movementRotation.Y -= speed;
                }
                else
                {
                    movementRotation.Y = minRotation;
                }
            }

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
