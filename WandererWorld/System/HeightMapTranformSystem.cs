using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using WandererWorld.Components;
using WandererWorld.Manager;

namespace WandererWorld.System
{
    public class HeightMapTranformSystem
    {
        public void UpdateHeightMapCamera(GameTime gameTime)
        {
            var heightMapCameras = EntityComponentManager.GetManager().GetComponentByType(typeof(HeightMapCameraComponent));

            foreach (var heightMapCamera in heightMapCameras)
            {
                var camera = (HeightMapCameraComponent)EntityComponentManager.GetManager().GetComponent(heightMapCamera.Key, typeof(HeightMapCameraComponent));

                Vector3 tempMovement = Vector3.Zero;
                Vector3 tempRotation = Vector3.Zero;

                KeyboardState key = Keyboard.GetState();

                //move backward
                if (key.IsKeyDown(Keys.L))
                {
                    tempMovement.Z = -camera.Movement.Z;
                }
                //move forward
                if (key.IsKeyDown(Keys.O))
                {
                    tempMovement.Z = +camera.Movement.Z;
                }
                //left rotation
                if (key.IsKeyDown(Keys.Q))
                {
                    tempRotation.Y = -camera.Rotation.Y;
                }
                //right rotation
                if (key.IsKeyDown(Keys.E))
                {
                    tempRotation.Y = +camera.Rotation.Y;
                }

                //move camera to new position
                camera.ViewMatrix = camera.ViewMatrix * Matrix.CreateRotationX(tempRotation.X) * Matrix.CreateRotationY(tempRotation.Y) * Matrix.CreateTranslation(tempMovement);

                //update position
                camera.Position += tempMovement;
                camera.Direction += tempRotation;
            }            
        }
    }
}
