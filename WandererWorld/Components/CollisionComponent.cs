using Microsoft.Xna.Framework;

namespace WandererWorld.Components
{
    public class CollisionComponent : GenericComponent
    {
        public BoundingBox BBox { get; set; }
        public BoundingSphere BSphere { get; set; }
    }
}
