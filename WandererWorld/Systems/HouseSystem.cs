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
                var vertexBuffer = new VertexBuffer(
                    game.GraphicsDevice, 
                    typeof(VertexPositionNormalTexture), 
                    ((HouseComponent)i).Vertices.Length, 
                    BufferUsage.None);
                vertexBuffer.SetData(((HouseComponent)i).Vertices);
                var indexBuffer = new IndexBuffer(game.GraphicsDevice, typeof(short), ((HouseComponent)i).Indices.Length, BufferUsage.None);
                indexBuffer.SetData(((HouseComponent)i).Indices);
                RenderHouses((HouseComponent)i, vertexBuffer, indexBuffer);
            }
        }

        private void RenderHouses(HouseComponent h, VertexBuffer vb, IndexBuffer ib)
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
            effect.Texture = h.Texture;

            foreach (EffectPass ep in effect.CurrentTechnique.Passes)
            {
                ep.Apply();
                game.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 18);
            }
        }
    }
}
