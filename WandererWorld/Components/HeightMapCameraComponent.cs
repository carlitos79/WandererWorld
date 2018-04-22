using Microsoft.Xna.Framework;

namespace WandererWorld.Components
{
    public class HeightMapCameraComponent : GenericComponent
    {
        public Matrix ViewMatrix { get; set; }
        public Matrix ProjectionMatrix { get; set; }
        public Matrix TerrainMatrix { get; set; }
        public Vector3 TerrainPosition { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Direction { get; set; }
        public Vector3 Movement { get; set; }
        public Vector3 Rotation { get; set; }
        public BoundingFrustum Frustum{ get; set; }
    }
}
