using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using WandererWorld.Components;
using WandererWorld.Manager;

namespace WandererWorld.Systems
{
    public class RobotSystem
    {
        public void CreateRobots()
        {
            var robots = EntityComponentManager.GetManager().GetComponentByType(typeof(RobotComponent));

            foreach (var robot in robots)
            {
                var hm = (RobotComponent)EntityComponentManager.GetManager().GetComponent(robot.Key, typeof(RobotComponent));

                hm.Effect.Projection = hm.RobotProjection;
                hm.Effect.View = hm.RobotView;

                hm.Effect.VertexColorEnabled = false;
                hm.Effect.TextureEnabled = true;
                hm.Effect.EnableDefaultLighting();
                hm.Effect.LightingEnabled = false;

                foreach (ModelMesh mesh in hm.Model.Meshes)
                {
                    foreach (ModelMeshPart mp in mesh.MeshParts)
                        mp.Effect = hm.Effect;
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            float rotation = 0;
            float speed = 0;

            var robots = EntityComponentManager.GetManager().GetComponentByType(typeof(RobotComponent));

            foreach (var r in robots)
            {
                var robot = (RobotComponent)EntityComponentManager.GetManager().GetComponent(r.Key, typeof(RobotComponent));

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
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Right))
                {
                    rotation = -0.01f * MathHelper.PiOver4 * (float)gameTime.ElapsedGameTime.Milliseconds;
                }

                robot.Rotation *= Quaternion.CreateFromYawPitchRoll(rotation, 0, 0);

            }
        }

        /// <summary>
        /// Transform a model part
        /// </summary>
        /// <param name="origin">Original transformation matrix</param>
        /// <param name="rotation">Rotation (radians)</param>
        /// <returns></returns>
        private Matrix TransformPart(Matrix origin, float rotation)
        {
            Vector3 originPosition = origin.Translation;

            origin.Translation = Vector3.Zero;
            origin *= Matrix.CreateRotationX(rotation);
            origin.Translation = originPosition;

            return origin;
        }

        public void Draw()
        {
            var robots = EntityComponentManager.GetManager().GetComponentByType(typeof(RobotComponent));

            foreach (var r in robots)
            {
                var robot = (RobotComponent)EntityComponentManager.GetManager().GetComponent(r.Key, typeof(RobotComponent));

                robot.Effect.TextureEnabled = true;
                robot.Effect.Texture = robot.Texture;

                robot.Model.CopyAbsoluteBoneTransformsTo(robot.TransformMatrices);

                robot.PlaneObjectWorld = Matrix.Identity * robot.Scale * Matrix.CreateFromQuaternion(robot.Rotation) * Matrix.CreateTranslation(robot.Position);

                foreach (ModelMesh mesh in robot.Model.Meshes)
                {
                    robot.Effect.World = robot.TransformMatrices[mesh.ParentBone.Index] * robot.PlaneObjectWorld * Matrix.Identity;

                    foreach (Effect e in mesh.Effects)
                    {
                        e.CurrentTechnique.Passes[0].Apply();
                    }
                    mesh.Draw();
                }
            }            
        }
    }
}
