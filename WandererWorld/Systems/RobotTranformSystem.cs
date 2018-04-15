using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using WandererWorld.Components;
using WandererWorld.Interfaces;
using WandererWorld.Manager;

namespace WandererWorld.Systems
{
    public class RobotTranformSystem : IUpdateSystem
    {
        public void UpdateSystem(GameTime gameTime)
        {
            float rotation = 0;
            float speed = 0;

            var robots = EntityComponentManager.GetManager().GetComponentByType(typeof(RobotCameraComponent));

            foreach (var r in robots)
            {
                var robot = (RobotCameraComponent)EntityComponentManager.GetManager().GetComponent(r.Key, typeof(RobotCameraComponent));

                if (Keyboard.GetState().IsKeyDown(Keys.Up))
                {
                    speed = 0.1f * (float)gameTime.ElapsedGameTime.Milliseconds;

                    if (robot.ModelRotation < robot.MaxRotation)
                    {
                        if (robot.Direction)
                            robot.ModelRotation += speed * robot.RotationSpeed * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                        else
                            robot.ModelRotation -= speed * robot.RotationSpeed * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                    }
                    else
                    {
                        if (!robot.Direction)
                            robot.ModelRotation -= speed * robot.RotationSpeed * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                        else
                            robot.ModelRotation += speed * robot.RotationSpeed * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                    }

                    if (robot.ModelRotation > robot.MaxRotation || robot.ModelRotation < -robot.MaxRotation)
                        robot.Direction = !robot.Direction;

                    robot.Model.Bones["RightArm"].Transform = TransformPart(robot.RightArmMatrix, robot.ModelRotation);
                    robot.Model.Bones["LeftArm"].Transform = TransformPart(robot.LeftArmMatrix, -robot.ModelRotation);
                    robot.Model.Bones["RightLeg"].Transform = TransformPart(robot.RightLegMatrix, -robot.ModelRotation);
                    robot.Model.Bones["LeftLeg"].Transform = TransformPart(robot.LeftLegMatrix, robot.ModelRotation);
                }                

                if (Keyboard.GetState().IsKeyDown(Keys.Left))
                {
                    rotation = 0.01f * MathHelper.PiOver4 * (float)gameTime.ElapsedGameTime.Milliseconds;
                    robot.RotationInDegrees = (int)(0.01f * MathHelper.PiOver4 * (float)gameTime.ElapsedGameTime.Milliseconds);
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Right))
                {
                    rotation = -0.01f * MathHelper.PiOver4 * (float)gameTime.ElapsedGameTime.Milliseconds;
                    robot.RotationInDegrees = (int)(-0.01f * MathHelper.PiOver4 * (float)gameTime.ElapsedGameTime.Milliseconds);
                }

                rotation = rotation % 360;
                robot.RotationInDegrees = robot.RotationInDegrees % 360;

                robot.Rotation *= Quaternion.CreateFromYawPitchRoll(rotation, 0, 0);
                //Debug.WriteLine(robot.RotationInDegrees);

            }
        }

        private Matrix TransformPart(Matrix origin, float rotation)
        {
            Vector3 originPosition = origin.Translation;

            origin.Translation = Vector3.Zero;
            origin *= Matrix.CreateRotationX(rotation);
            origin.Translation = originPosition;

            return origin;
        }
    }
}
