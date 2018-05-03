using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using WandererWorld.Components;
using WandererWorld.Manager;

namespace WandererWorld.Systems
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
                CalculateNormals(hm);
                SetEffects(hm);
                SetChunks(hm);
            }
        }

        private void SetChunks(HeightMapComponent heightmap)
        {
            ushort iCapacity = ushort.MaxValue;
            ushort vCapacity = (ushort)(iCapacity / 6);
            ushort height = (ushort)(vCapacity / heightmap.Width);
            ushort width = (ushort)(heightmap.Width);
            vCapacity = (ushort)(height * width);

            if (heightmap.Chunks == null)
            {
                heightmap.Chunks = new List<HeightMapChunk>();
            }

            int nChunks = (int)(heightmap.Height / height);
            for (int i = 0; i < nChunks; ++i)
            {
                HeightMapChunk hmc = new HeightMapChunk();
                var vertices = new List<VertexPositionNormalTexture>();
                for (int y = i * (height - 1); y < i * (height - 1) + height; ++y)
                {
                    for (int x = 0; x < width; ++x)
                    {
                        vertices.Add(heightmap.Vertices[y * heightmap.Width + x]);
                    }
                }
                hmc.Vertices = vertices.ToArray();

                var indices = new ushort[6 * (width - 1) * (height - 1)];
                ushort n = 0;
                for (int y = 0; y < height - 1; y++)
                    for (int x = 0; x < width - 1; x++)
                    {
                        int botLeft = y * width + x;
                        int botRight = botLeft + 1;
                        int topLeft = botLeft + width;
                        int topRight = topLeft + 1;

                        indices[n++] = (ushort)topLeft;
                        indices[n++] = (ushort)botRight;
                        indices[n++] = (ushort)botLeft;

                        indices[n++] = (ushort)topLeft;
                        indices[n++] = (ushort)topRight;
                        indices[n++] = (ushort)botRight;
                    }
                hmc.Indices = indices;

                heightmap.Chunks.Add(hmc);
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

        private void CalculateNormals(HeightMapComponent heightmap)
        {
            for (int i = 0; i < heightmap.Vertices.Length; i++)
                heightmap.Vertices[i].Normal = new Vector3(0, 0, 0);

            for (int i = 0; i < heightmap.Indices.Length / 3; i++)
            {
                int index1 = heightmap.Indices[i * 3];
                int index2 = heightmap.Indices[i * 3 + 1];
                int index3 = heightmap.Indices[i * 3 + 2];

                Vector3 side1 = heightmap.Vertices[index1].Position - heightmap.Vertices[index3].Position;
                Vector3 side2 = heightmap.Vertices[index1].Position - heightmap.Vertices[index2].Position;
                Vector3 normal = Vector3.Cross(side1, side2);

                heightmap.Vertices[index1].Normal += normal;
                heightmap.Vertices[index2].Normal += normal;
                heightmap.Vertices[index3].Normal += normal;
            }

            for (int i = 0; i < heightmap.Vertices.Length; i++)
                heightmap.Vertices[i].Normal.Normalize();
        }

        public void SetVertices(HeightMapComponent heightmap)
        {
            heightmap.Vertices = new VertexPositionNormalTexture[heightmap.Width * heightmap.Height];

            for (int x = 0; x < heightmap.Width; x++)
            {
                for (int y = 0; y < heightmap.Height; y++)
                {
                    heightmap.TexturePosition = new Vector2((float)x / 150.5f, (float)y / 150.5f);
                    heightmap.Vertices[x + y * heightmap.Width] = new VertexPositionNormalTexture(new Vector3(x, heightmap.HeightMapData[x, y], -y), Vector3.Zero, heightmap.TexturePosition);
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
