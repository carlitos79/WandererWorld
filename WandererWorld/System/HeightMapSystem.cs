using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using WandererWorld.Components;
using WandererWorld.Manager;

namespace WandererWorld.System
{
    public class HeightmapSystem
    {
        public void CreateHeightMaps()
        {
            var heightMapsList = EntityComponentManager.GetManager().GetComponentByType(typeof(HeightMapComponent));

            foreach (var heightMap in heightMapsList)
            {
                var hm = (HeightMapComponent)EntityComponentManager.GetManager().GetComponent(heightMap.Key, typeof(HeightMapComponent));

                SetHeights(hm);
                SetVertices(hm);
                SetIndices(hm);
                SetEffects(hm);
            }
        }

        public void SetHeights(HeightMapComponent heightmap)
        {
            Color[] greyValues = new Color[heightmap.Width * heightmap.Height];
            heightmap.HeightMap.GetData(greyValues);
            heightmap.HeightMapData = new float[heightmap.Width, heightmap.Height];

            for (int x = 0; x < heightmap.Width; x++)
            {
                for (int y = 0; y < heightmap.Height; y++)
                {
                    heightmap.HeightMapData[x, y] = greyValues[x + y * heightmap.Width].G / 3.1f;
                }
            }
        }

        public void SetIndices(HeightMapComponent heightmap)
        {
            // amount of triangles
            heightmap.Indices = new int[6 * (heightmap.Width - 1) * (heightmap.Height - 1)];
            int number = 0;

            // collect data for corners
            for (int y = 0; y < heightmap.Height - 1; y++)
                for (int x = 0; x < heightmap.Width - 1; x++)
                {
                    // create double triangles
                    heightmap.Indices[number] = x + (y + 1) * heightmap.Width;          // up left
                    heightmap.Indices[number + 1] = x + y * heightmap.Width + 1;        // down right
                    heightmap.Indices[number + 2] = x + y * heightmap.Width;            // down left
                    heightmap.Indices[number + 3] = x + (y + 1) * heightmap.Width;      // up left
                    heightmap.Indices[number + 4] = x + (y + 1) * heightmap.Width + 1;  // up right
                    heightmap.Indices[number + 5] = x + y * heightmap.Width + 1;        // down right
                    number += 6;
                }
        }

        public void SetVertices(HeightMapComponent heightmap)
        {
            heightmap.Vertices = new VertexPositionTexture[heightmap.Width * heightmap.Height];

            for (int x = 0; x < heightmap.Width; x++)
            {
                for (int y = 0; y < heightmap.Height; y++)
                {
                    heightmap.TexturePosition = new Vector2((float)x / 150.5f, (float)y / 150.5f);
                    heightmap.Vertices[x + y * heightmap.Width] = new VertexPositionTexture(new Vector3(x, heightmap.HeightMapData[x, y], -y), heightmap.TexturePosition);                   
                }
            }
        }

        public void SetEffects(HeightMapComponent heightmap)
        {
            heightmap.BasicEffect = new BasicEffect(heightmap.GraphicsDevice);            
            heightmap.BasicEffect.TextureEnabled = true;
            heightmap.BasicEffect.Texture = heightmap.HeightMapTexture;            
        }
    }
}
