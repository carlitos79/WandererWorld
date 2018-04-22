using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace WandererWorld.WandererContent
{
    public class WandererBody : WandererLimbs
    {
        private List<IWanderer> children = new List<IWanderer>();

        private Game game;
        private Matrix objectWorld;
        public float rotationInDegrees;

        private Matrix renderRotation;
        private Vector3 renderPosition;
        public Vector3 movementRotation = Vector3.Zero;
        private Vector3 movementPosition = Vector3.Zero;

        //Children's position
        private Vector3 headPosition = new Vector3(0, 4.5f, 0);
        private Vector3 rightArmPosition = new Vector3(2.2f, -0.5f, 0);
        private Vector3 leftArmPosition = new Vector3(-2.2f, -0.5f, 0);
        private Vector3 leftLegPosition = new Vector3(-0.8f, 2.8f, 0);
        private Vector3 rightLegPosition = new Vector3(0.8f, 1.8f, 0);

        public WandererBody(Game game) : base(game, "torso")
        {
            LoadContent();
            this.game = game;

            renderPosition = new Vector3(0, 1, 0);
            objectWorld = renderRotation = Matrix.Identity;

            children.Add(new Head(game, headPosition));
            children.Add(new RightArm(game, rightArmPosition));
            children.Add(new LeftArm(game, leftArmPosition));
            children.Add(new RightLeg(game, rightLegPosition));
            children.Add(new LeftLeg(game, leftLegPosition));
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
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                movementRotation = new Vector3(movementRotation.X - 0.05f, movementRotation.Y, movementRotation.Z);
            }                

            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                movementRotation = new Vector3(movementRotation.X + 0.05f, movementRotation.Y, movementRotation.Z);
            }

            World = Matrix.Identity *
                Matrix.CreateFromQuaternion(Quaternion.CreateFromYawPitchRoll(movementRotation.X, movementRotation.Y, movementRotation.Z)) *
                Matrix.CreateTranslation(movementPosition);

            foreach (IWanderer child in children)
            {
                child.UpdateLimbMovement(gameTime);
            }
        }
    }
}
