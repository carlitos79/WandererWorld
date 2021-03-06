﻿using Microsoft.Xna.Framework.Graphics;
using WandererWorld.Components;
using WandererWorld.Manager;

namespace WandererWorld.Systems
{
    public class RobotSystem
    {
        public void CreateRobots()
        {
            var robots = EntityComponentManager.GetManager().GetComponentByType(typeof(RobotComponent));
            var robotCams = EntityComponentManager.GetManager().GetComponentByType(typeof(RobotCameraComponent));

            foreach (var c in robotCams)
            {
                foreach (var r in robots)
                {
                    var robot = (RobotComponent)EntityComponentManager.GetManager().GetComponent(r.Key, typeof(RobotComponent));
                    var robotCam = (RobotCameraComponent)EntityComponentManager.GetManager().GetComponent(r.Key, typeof(RobotCameraComponent));

                    robot.Effect.Projection = robot.RobotProjection;
                    robot.Effect.View = robot.RobotView;

                    robot.Effect.VertexColorEnabled = false;
                    robot.Effect.TextureEnabled = true;
                    robot.Effect.EnableDefaultLighting();
                    robot.Effect.LightingEnabled = false;

                    foreach (ModelMesh mesh in robotCam.Model.Meshes)
                    {
                        foreach (ModelMeshPart mp in mesh.MeshParts)
                            mp.Effect = robot.Effect;
                    }
                }
            }            
        }
    }
}
