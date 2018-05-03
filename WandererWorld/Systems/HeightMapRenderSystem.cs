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
        private GraphicsDevice device;

        public HeightMapRenderSystem(GraphicsDevice d)
        {
            device = d;
        }

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
                    SetEffects(map, camera);

                    foreach (var c in map.Chunks)
                    {
                        foreach (EffectPass pass in map.BasicEffect.CurrentTechnique.Passes)
                        {
                            
                            StoreInGraphicsCard(c);

                            pass.Apply();
                            //map.GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionNormalTexture>(PrimitiveType.TriangleList, map.Vertices, 0, map.Vertices.Length, map.Indices, 0, map.Indices.Length / 3);
                            //heightmap.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, heightmap.Indices.Length / 3);
                            device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, c.Indices.Length / 3);

                        }
                    }
                }
            }
        }

        public void StoreInGraphicsCard(HeightMapChunk c)
        {
            var vertexBuffer = new VertexBuffer(
                device,
                typeof(VertexPositionNormalTexture),
                c.Vertices.Length,
                BufferUsage.None);
            vertexBuffer.SetData(c.Vertices);
            device.SetVertexBuffer(vertexBuffer);

                var indexBuffer = new IndexBuffer(
                                device,
                                typeof(ushort),
                                c.Indices.Length,
                                BufferUsage.None);
                indexBuffer.SetData(c.Indices);
                device.Indices = indexBuffer;
        }

        public void SetEffects(HeightMapComponent heightMap, HeightMapCameraComponent heightMapCamera)
        {
            heightMap.BasicEffect.View = heightMapCamera.ViewMatrix;
            heightMap.BasicEffect.Projection = heightMapCamera.ProjectionMatrix;
            heightMap.BasicEffect.World = heightMapCamera.TerrainMatrix;
        }
    }
}
