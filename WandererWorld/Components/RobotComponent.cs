using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WandererWorld.Components
{
    public class RobotComponent : GenericComponent
    {
        public float MaxRotation { get; set; }
        public float Speed { get; set; }
        public float RotationSpeed { get; set; }
        public float ModelRotation { get; set; }
        public Model Model { get; set; }
        public bool Direction { get; set; }
        public Matrix LeftArmMatrix { get; set; }
        public Matrix LeftLegMatrix { get; set; }
        public Matrix RightArmMatrix { get; set; }
        public Matrix RightLegMatrix { get; set; }
        public Matrix PlaneObjectWorld { get; set; }
        public Matrix[] TransformMatrices { get; set; }
        public BasicEffect Effect { get; set; }
        public GraphicsDevice Graphics { get; set; }
        public Texture2D Texture { get; set; }
        public Matrix RobotProjection { get; set; }
        public Matrix RobotView { get; set; }
        public Matrix Scale { get; set; }
        public Quaternion Rotation { get; set; }
        public Vector3 Position { get; set; }
    }
}
