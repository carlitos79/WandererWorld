using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using WandererWorld.Components;
using WandererWorld.Interfaces;
using WandererWorld.Manager;

namespace WandererWorld.Systems
{
    public class HeightMapTranformSystem : IUpdateSystem
    {
        public void UpdateSystem(GameTime gameTime)
        {
            var heightMapCameras = EntityComponentManager.GetManager().GetComponentByType(typeof(HeightMapCameraComponent));
            var robotCameras = EntityComponentManager.GetManager().GetComponentByType(typeof(RobotCameraComponent));

            foreach (var rc in robotCameras)
            {
                foreach (var hmc in heightMapCameras)
                {
                    var heightMapCamera = (HeightMapCameraComponent)EntityComponentManager.GetManager().GetComponent(hmc.Key, typeof(HeightMapCameraComponent));
                    var robotCamera = (RobotCameraComponent)EntityComponentManager.GetManager().GetComponent(rc.Key, typeof(RobotCameraComponent));

                    Vector3 tempMovement = Vector3.Zero;
                    Vector3 tempRotation = Vector3.Zero;

                    KeyboardState key = Keyboard.GetState();

                    //move backward
                    if (robotCamera.Rotation.Z > robotCamera.RotationInDegrees && key.IsKeyDown(Keys.Up))
                    {
                        tempMovement.Z = -heightMapCamera.Movement.Z;
                    }
                    //move forward
                    if (robotCamera.Rotation.Z < robotCamera.RotationInDegrees && key.IsKeyDown(Keys.Up))
                    {
                        tempMovement.Z = +heightMapCamera.Movement.Z;
                    }
                    //left rotation
                    if (robotCamera.Rotation.X > robotCamera.RotationInDegrees && key.IsKeyDown(Keys.Up))
                    {
                        tempRotation.Y = -heightMapCamera.Rotation.Y * 0.1f;
                    }
                    //right rotation
                    if (robotCamera.Rotation.X < robotCamera.RotationInDegrees && key.IsKeyDown(Keys.Up))
                    {
                        tempRotation.Y = +heightMapCamera.Rotation.Y * 0.1f;
                    }

                    //move camera to new position
                    heightMapCamera.ViewMatrix = heightMapCamera.ViewMatrix * Matrix.CreateRotationX(tempRotation.X) * Matrix.CreateRotationY(tempRotation.Y) * Matrix.CreateTranslation(tempMovement);

                    //update position
                    heightMapCamera.Position += tempMovement;
                    heightMapCamera.Direction += tempRotation;
                }
            }                
        }
    }
}
