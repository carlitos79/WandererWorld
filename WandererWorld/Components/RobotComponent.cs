using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WandererWorld.Components
{
    public class RobotComponent : GenericComponent
    {
        public float Speed { get; set; }        
        public Matrix PlaneObjectWorld { get; set; }
        public Matrix[] TransformMatrices { get; set; }
        public BasicEffect Effect { get; set; }
        public GraphicsDevice Graphics { get; set; }
        public Texture2D Texture { get; set; }
        public Matrix RobotProjection { get; set; }
        public Matrix RobotView { get; set; }
        public Matrix Scale { get; set; }        
        public Vector3 Position { get; set; }
    }
}
