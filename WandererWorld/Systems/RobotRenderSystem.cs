using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WandererWorld.Components;
using WandererWorld.Manager;

namespace WandererWorld.Systems
{
    public class RobotRenderSystem
    {
        public void RenderRobotCamera()
        {
            var robots = EntityComponentManager.GetManager().GetComponentByType(typeof(RobotComponent));
            var robotCams = EntityComponentManager.GetManager().GetComponentByType(typeof(RobotCameraComponent));

            foreach (var c in robotCams)
            {
                foreach (var r in robots)
                {
                    var robot = (RobotComponent)EntityComponentManager.GetManager().GetComponent(r.Key, typeof(RobotComponent));
                    var robotCam = (RobotCameraComponent)EntityComponentManager.GetManager().GetComponent(r.Key, typeof(RobotCameraComponent));

                    robot.Effect.TextureEnabled = true;
                    robot.Effect.Texture = robot.Texture;

                    robotCam.Model.CopyAbsoluteBoneTransformsTo(robot.TransformMatrices);

                    robot.PlaneObjectWorld = Matrix.Identity * robot.Scale * Matrix.CreateFromQuaternion(robotCam.Rotation) * Matrix.CreateTranslation(robot.Position);

                    foreach (ModelMesh mesh in robotCam.Model.Meshes)
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
}
