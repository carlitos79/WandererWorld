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
            var d = EntityComponentManager.GetManager().GetComponentByType(typeof(HouseComponent));
            foreach(var i in d.Values)
            {
                // Render the walls with one texture...
                var vertexBuffer = new VertexBuffer(
                    game.GraphicsDevice, 
                    typeof(VertexPositionNormalTexture), 
                    ((HouseComponent)i).WallVertices.Length, 
                    BufferUsage.None);
                vertexBuffer.SetData(((HouseComponent)i).WallVertices);
                var indexBuffer = new IndexBuffer(game.GraphicsDevice, typeof(short), ((HouseComponent)i).WallIndices.Length, BufferUsage.None);
                indexBuffer.SetData(((HouseComponent)i).WallIndices);
                RenderHouses((HouseComponent)i, vertexBuffer, indexBuffer, ((HouseComponent)i).WallTexture);

                // ...Render the roof with another texture
                vertexBuffer = new VertexBuffer(
                    game.GraphicsDevice,
                    typeof(VertexPositionNormalTexture),
                    ((HouseComponent)i).RoofVertices.Length,
                    BufferUsage.None);
                vertexBuffer.SetData(((HouseComponent)i).RoofVertices);
                indexBuffer = new IndexBuffer(game.GraphicsDevice, typeof(short), ((HouseComponent)i).RoofIndices.Length, BufferUsage.None);
                indexBuffer.SetData(((HouseComponent)i).WallIndices);
                RenderHouses((HouseComponent)i, vertexBuffer, indexBuffer, ((HouseComponent)i).RoofTexture);
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
