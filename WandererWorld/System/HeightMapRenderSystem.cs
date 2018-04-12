using Microsoft.Xna.Framework.Graphics;
using WandererWorld.Components;
using WandererWorld.Manager;

namespace WandererWorld.System
{
    public class HeightMapRenderSystem
    {
        public void RenderHeightMapCamera()
        {
            var heightMapCameras = EntityComponentManager.GetManager().GetComponentByType(typeof(HeightMapCameraComponent));
            var heightMaps = EntityComponentManager.GetManager().GetComponentByType(typeof(HeightMapCameraComponent));

            foreach (var heightMapCamera in heightMapCameras)
            {
                foreach (var heightMap in heightMaps)
                {
                    var camera = (HeightMapCameraComponent)EntityComponentManager.GetManager().GetComponent(heightMapCamera.Key, typeof(HeightMapCameraComponent));
                    var map = (HeightMapComponent)EntityComponentManager.GetManager().GetComponent(heightMapCamera.Key, typeof(HeightMapComponent));

                    foreach (EffectPass pass in map.BasicEffect.CurrentTechnique.Passes)
                    {
                        SetEffects(map, camera);

                        pass.Apply();
                        map.GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionTexture>(PrimitiveType.TriangleList, map.Vertices, 0, map.Vertices.Length, map.Indices, 0, map.Indices.Length / 3);
                        
                    }
                }
            }
        }

        public void SetEffects(HeightMapComponent heightMap, HeightMapCameraComponent heightMapCamera)
        {
            heightMap.BasicEffect.View = heightMapCamera.ViewMatrix;
            heightMap.BasicEffect.Projection = heightMapCamera.ProjectionMatrix;
            heightMap.BasicEffect.World = heightMapCamera.TerrainMatrix;
        }
    }
}
