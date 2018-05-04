using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace WandererWorld.Components
{
    // Holds data for a part of a larger heightmap
    public struct HeightMapChunk
    {
        public ushort[] Indices { get; set; }
        public VertexPositionNormalTexture[] Vertices { get; set; }
    }

    public class HeightMapComponent : GenericComponent
    {
        public GraphicsDevice GraphicsDevice { get; set; }
        public Texture2D HeightMap { get; set; }
        public Texture2D HeightMapTexture { get; set; }
        public VertexPositionNormalTexture[] Vertices { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public Vector2 TexturePosition { get; set; }
        public BasicEffect BasicEffect { get; set; }
        public int[] Indices { get; set; }
        public float[,] HeightMapData { get; set; }
        public List<HeightMapChunk> Chunks { get; set; }
        public bool Loaded { get; set; }
    }
}
