using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using WandererWorld.Components;

namespace WandererWorld.Systems
{
    public class CollisionSystem
    {
        public BoundingBox SetBoundingBoxHeightMap(HeightMapComponent heightMap, Matrix world)
        {
            VertexPositionNormalTexture[] verticesToNormal;
            verticesToNormal = heightMap.Vertices.ToArray();

            for (int i = 0; i < heightMap.Vertices.Length; i++)
            {
                verticesToNormal[i].Position = Vector3.Transform(heightMap.Vertices[i].Position, world);
            }

            return BoundingBox.CreateFromPoints(verticesToNormal.Select(x => x.Position));
        }

        public BoundingBox SetBoundingBoxWanderer(RobotCameraComponent robot, Matrix world)
        {
            Vector3 min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            Vector3 max = new Vector3(float.MinValue, float.MinValue, float.MinValue);

            // For each mesh of the model
            foreach (ModelMesh mesh in robot.Model.Meshes)
            {
                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                {
                    // Vertex buffer parameters
                    int vertexStride = meshPart.VertexBuffer.VertexDeclaration.VertexStride;
                    int vertexBufferSize = meshPart.NumVertices * vertexStride;

                    // Get vertex data as float
                    float[] vertexData = new float[vertexBufferSize / sizeof(float)];
                    meshPart.VertexBuffer.GetData<float>(vertexData);

                    // Iterate through vertices (possibly) growing bounding box, all calculations are done in world space
                    for (int i = 0; i < vertexBufferSize / sizeof(float); i += vertexStride / sizeof(float))
                    {
                        Vector3 transformedPosition = Vector3.Transform(new Vector3(vertexData[i], vertexData[i + 1], vertexData[i + 2]), world);

                        min = Vector3.Min(min, transformedPosition);
                        max = Vector3.Max(max, transformedPosition);
                    }
                }
            }
            return new BoundingBox(min, max);
        }

        public bool CheckIfCollision(CollisionComponent a, CollisionComponent b)
        {
            if (a.BBox.Intersects(b.BBox))
            {
                return true;
            }
            return false;
        }
    }
}
