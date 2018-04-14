using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WandererWorld.Components
{
    public class RobotCameraComponent : GenericComponent
    {
        public Model Model { get; set; }
        public float MaxRotation { get; set; }
        public float ModelRotation { get; set; }
        public bool Direction { get; set; }
        public float RotationSpeed { get; set; }
        public Matrix LeftArmMatrix { get; set; }
        public Matrix LeftLegMatrix { get; set; }
        public Matrix RightArmMatrix { get; set; }
        public Matrix RightLegMatrix { get; set; }
        public Quaternion Rotation { get; set; }
    }
}
