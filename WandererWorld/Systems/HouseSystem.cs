using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WandererWorld.Components;
using WandererWorld.Manager;

namespace WandererWorld.Systems
{
    public class HouseSystem
    {
        private Game game;

        public HouseSystem(Game g)
        {
            game = g;
        }

        public void Update()
        {
            var cam = (HeightMapCameraComponent)EntityComponentManager.GetManager().GetComponentByType(typeof(HeightMapCameraComponent)).Values.First();
            var d = EntityComponentManager.GetManager().GetComponentByType(typeof(HouseComponent));
            foreach(var i in d)
            {
                var bbox = (HouseBoundingBox)EntityComponentManager.GetManager().GetComponent(i.Key, typeof(HouseBoundingBox));
                var house = (HouseComponent)i.Value;
                if (cam.Frustum.Intersects(bbox.BoundingBox))
                {
                    // Render the walls with one texture...
                    var vertexBuffer = new VertexBuffer(
                        game.GraphicsDevice,
                        typeof(VertexPositionNormalTexture),
                        house.WallVertices.Length,
                        BufferUsage.None);
                    vertexBuffer.SetData(house.WallVertices);
                    var indexBuffer = new IndexBuffer(game.GraphicsDevice, typeof(short), house.WallIndices.Length, BufferUsage.None);
                    indexBuffer.SetData(house.WallIndices);
                    RenderHouses(house, vertexBuffer, indexBuffer, house.WallTexture);

                    // ...Render the roof with another texture
                    vertexBuffer = new VertexBuffer(
                        game.GraphicsDevice,
                        typeof(VertexPositionNormalTexture),
                        house.RoofVertices.Length,
                        BufferUsage.None);
                    vertexBuffer.SetData(house.RoofVertices);
                    indexBuffer = new IndexBuffer(game.GraphicsDevice, typeof(short), house.RoofIndices.Length, BufferUsage.None);
                    indexBuffer.SetData(house.WallIndices);
                    RenderHouses(house, vertexBuffer, indexBuffer, house.RoofTexture);
                }
            }
        }

        private void RenderHouses(HouseComponent h, VertexBuffer vb, IndexBuffer ib, Texture2D texture)
        {
            game.GraphicsDevice.SetVertexBuffer(vb);
            game.GraphicsDevice.Indices = ib;

            var effect = new BasicEffect(game.GraphicsDevice);
            effect.View = ((HeightMapCameraComponent)EntityComponentManager.GetManager().GetComponentByType(typeof(HeightMapCameraComponent)).First().Value).ViewMatrix;
            effect.Projection = ((HeightMapCameraComponent)EntityComponentManager.GetManager().GetComponentByType(typeof(HeightMapCameraComponent)).First().Value).ProjectionMatrix;
            effect.VertexColorEnabled = false;
            effect.TextureEnabled = true;
            effect.EnableDefaultLighting();
            effect.LightingEnabled = false;
            var objectWorld = Matrix.CreateScale(h.Scale) * h.Rotation * Matrix.CreateTranslation(h.Position);
            effect.World = objectWorld * ((HeightMapCameraComponent)EntityComponentManager.GetManager().GetComponentByType(typeof(HeightMapCameraComponent)).First().Value).TerrainMatrix;
            effect.Texture = texture;

            foreach (EffectPass ep in effect.CurrentTechnique.Passes)
            {
                ep.Apply();
                game.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 18);
            }
        }
    }
}
