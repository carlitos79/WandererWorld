using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using WandererWorld.Components;
using WandererWorld.Interfaces;
using WandererWorld.Manager;

namespace WandererWorld.Systems
{
    public class HeightMapRenderSystem : IRenderSystem
    {
        public void RenderSystem()
        {
            var heightMapCameras = EntityComponentManager.GetManager().GetComponentByType(typeof(HeightMapCameraComponent));
            var heightMaps = EntityComponentManager.GetManager().GetComponentByType(typeof(HeightMapCameraComponent));

            foreach (var heightMapCamera in heightMapCameras)
            {
                var cam = (HeightMapCameraComponent)heightMapCamera.Value;
                cam.Frustum = new BoundingFrustum(cam.ViewMatrix * cam.ProjectionMatrix);
                foreach (var heightMap in heightMaps)
                {
                    var camera = (HeightMapCameraComponent)EntityComponentManager.GetManager().GetComponent(heightMapCamera.Key, typeof(HeightMapCameraComponent));
                    var map = (HeightMapComponent)EntityComponentManager.GetManager().GetComponent(heightMapCamera.Key, typeof(HeightMapComponent));

                    foreach (EffectPass pass in map.BasicEffect.CurrentTechnique.Passes)
                    {
                        SetEffects(map, camera);
                        //StoreInGraphicsCard(heightmap);

                        pass.Apply();
                        map.GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionNormalTexture>(PrimitiveType.TriangleList, map.Vertices, 0, map.Vertices.Length, map.Indices, 0, map.Indices.Length / 3);
                        //heightmap.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, heightmap.Indices.Length / 3);
                    }
                }
            }
        }

        public void StoreInGraphicsCard(HeightMapComponent heightmap)
        {
            var vertices = from v in heightmap.Vertices select v;
            int verticesCount = vertices.Count();

            var indices = from i in heightmap.Indices select i;
            int indicesCount = indices.Count();

            var vertexBuffer = new VertexBuffer(heightmap.GraphicsDevice, typeof(VertexPositionNormalTexture), verticesCount, BufferUsage.None);
            var indexBuffer = new IndexBuffer(heightmap.GraphicsDevice, IndexElementSize.SixteenBits, indicesCount, BufferUsage.None);

            heightmap.GraphicsDevice.SetVertexBuffer(vertexBuffer);
            heightmap.GraphicsDevice.Indices = indexBuffer;
        }

        public void SetEffects(HeightMapComponent heightMap, HeightMapCameraComponent heightMapCamera)
        {
            heightMap.BasicEffect.View = heightMapCamera.ViewMatrix;
            heightMap.BasicEffect.Projection = heightMapCamera.ProjectionMatrix;
            heightMap.BasicEffect.World = heightMapCamera.TerrainMatrix;
        }
    }
}
