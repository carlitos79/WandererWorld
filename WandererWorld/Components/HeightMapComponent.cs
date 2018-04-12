using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WandererWorld.Components
{
    public class HeightMapComponent : GenericComponent
    {
        public GraphicsDevice GraphicsDevice { get; set; }
        public Texture2D HeightMap { get; set; }
        public Texture2D HeightMapTexture { get; set; }
        public VertexPositionTexture[] Vertices { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public Vector2 TexturePosition { get; set; }
        public BasicEffect BasicEffect { get; set; }
        public int[] Indices { get; set; }
        public float[,] HeightMapData { get; set; }
    }
}
