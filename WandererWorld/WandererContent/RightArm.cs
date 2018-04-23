using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace WandererWorld.WandererContent
{
    public class RightArm : WandererLimbs
    {
        private List<IWanderer> children = new List<IWanderer>();

        private Game game;
        private Matrix objectWorld;

        private Matrix renderRotation;
        private Vector3 renderPosition;
        private Vector3 movementRotation = Vector3.Zero;
        private Vector3 movementPosition = new Vector3(0, 1.5f, 0);
        private Vector3 jointPosition = new Vector3(0, 2.5f, 0);

        //Children's position
        private Vector3 rightHandPosition = new Vector3(2.2f, -3.2f, 0);

        public RightArm(Game game, Vector3 position) : base(game, "rightArm")
        {
            LoadContent();
            this.game = game;

            renderPosition = position;
            objectWorld = Matrix.Identity;
            renderRotation = Matrix.Identity;
            movementRotation.Y = 4;

            //children.Add(new RightHand(game, rightHandPosition));
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

            foreach (IWanderer child in children)
            {
                child.DrawLimb(gameTime);
            }
        }

        public override void UpdateLimbMovement(GameTime gameTime)
        {
            float speed = 0;
            float maxRotation = 2.3f;
            float minRotation = 4;

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
            {
                movementRotation = new Vector3(movementRotation.X - 0.05f, movementRotation.Y, movementRotation.Z);

                if (movementRotation.X <= -1.55f)
                {
                    movementRotation.X += 0.05f;
                }
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                movementRotation = new Vector3(movementRotation.X + 0.05f, movementRotation.Y, movementRotation.Z);

                if (movementRotation.X >= 1.55f)
                {
                    movementRotation.X -= 0.05f;
                }
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                movementRotation.X = 0;
            }

            World = Matrix.Identity *
                Matrix.CreateTranslation(movementPosition) *
                Matrix.CreateFromQuaternion(Quaternion.CreateFromYawPitchRoll(movementRotation.X, movementRotation.Y, movementRotation.Z)) *
                Matrix.CreateTranslation(jointPosition);

            foreach (IWanderer child in children)
            {
                child.UpdateLimbMovement(gameTime);
            }
        }
    }
}
