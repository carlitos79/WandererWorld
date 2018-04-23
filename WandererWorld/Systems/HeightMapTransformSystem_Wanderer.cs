using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using WandererWorld.Components;
using WandererWorld.Manager;

namespace WandererWorld.Systems
{
    class HeightMapTransformSystem_Wanderer
    {
        public void UpdateHeightMap_Wanderer(GameTime gameTime)
        {
            var heightMapCameras = EntityComponentManager.GetManager().GetComponentByType(typeof(HeightMapCameraComponent));

            foreach (var hmc in heightMapCameras)
            {
                var heightMapCamera = (HeightMapCameraComponent)EntityComponentManager.GetManager().GetComponent(hmc.Key, typeof(HeightMapCameraComponent));

                Vector3 tempMovement = Vector3.Zero;
                Vector3 tempRotation = Vector3.Zero;

                KeyboardState key = Keyboard.GetState();

                //move backward
                if (key.IsKeyDown(Keys.Down))
                {
                    tempMovement.Z = -heightMapCamera.Movement.Z;
                }
                //move forward
                if (key.IsKeyDown(Keys.Up))
                {
                    tempMovement.Z = +heightMapCamera.Movement.Z;
                }
                //left rotation
                if (key.IsKeyDown(Keys.Left))
                {
                    tempRotation.Y = -heightMapCamera.Rotation.Y * 0.3f;
                }
                //right rotation
                if (key.IsKeyDown(Keys.Right))
                {
                    tempRotation.Y = +heightMapCamera.Rotation.Y * 0.3f;
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
