using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace WandererWorld.WandererContent
{
    public class RightArm : WandererLimbs
    {
        private Game game;
        private Matrix limbWorld;

        private Vector3 position;
        private Vector3 rotation = Vector3.Zero;
        private Vector3 jointPosition = new Vector3(0, 1.5f, 0);

        public RightArm(Game game, Vector3 position) : base(game, "rightArm")
        {
            LoadContent();
            this.game = game;

            this.position = position;
            rotation.Y = 4;
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
            float speed = 0;
            float maxRotation = 2.3f;
            float minRotation = 4;

            // Here is where the limb sets in motion (swings)
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                speed += 0.003f * (float)gameTime.ElapsedGameTime.Milliseconds;

                if (rotation.Y >= maxRotation)
                {
                    rotation.Y -= speed;
                }
                else
                {
                    rotation.Y = minRotation;
                }
            }

            World = Matrix.Identity *
                Matrix.CreateTranslation(jointPosition) *
                Matrix.CreateFromQuaternion(Quaternion.CreateFromYawPitchRoll(rotation.X, rotation.Y, rotation.Z)) *
                Matrix.CreateTranslation(jointPosition);
        }
    }
}
