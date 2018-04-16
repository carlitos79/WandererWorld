﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using WandererWorld.Components;
using WandererWorld.Interfaces;
using WandererWorld.Manager;
using WandererWorld.Systems;

namespace WandererWorld
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private HeightMapComponent heightMapComponent;
        private HeightMapCameraComponent heightMapCameraComponent;

        private RobotComponent robotComponent;
        private RobotCameraComponent robotCameraComponent;

        private HeightmapSystem heightMapSystem;        
        private IUpdateSystem heightMapTranformSystem;
        private IRenderSystem heightMapRenderSystem;

        private RobotSystem robotSystem;
        private IUpdateSystem robotTranformSystem;
        private IRenderSystem robotRenderSystem;

        private CollisionSystem collisionSystem;
        private HouseSystem houseSystem;
        
        private Updater systemsUpdater;
        private Renderer systemRenderer;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;

            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            heightMapComponent = new HeightMapComponent();
            heightMapCameraComponent = new HeightMapCameraComponent();

            robotComponent = new RobotComponent();

            heightMapSystem = new HeightmapSystem();
            heightMapTranformSystem = new HeightMapTranformSystem();
            heightMapRenderSystem = new HeightMapRenderSystem();                   

            robotSystem = new RobotSystem();
            robotTranformSystem = new RobotTranformSystem();
            robotRenderSystem = new RobotRenderSystem();

            collisionSystem = new CollisionSystem();
            houseSystem = new HouseSystem(this);

            systemRenderer = new Renderer();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {           
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Texture2D heightMapTexture2D = Content.Load<Texture2D>("US_Canyon");
            Texture2D heightMapGrassTexture = Content.Load<Texture2D>("grass2");
            Texture2D heightMapFireTexture = Content.Load<Texture2D>("fire");
            Texture2D BrickTexture = Content.Load<Texture2D>("brick");
            Model robotModel = Content.Load<Model>("Lab2Model");
            Texture2D robotTexture = Content.Load<Texture2D>("robot_texture");

            heightMapComponent = new HeightMapComponent
            {
                HeightMap = heightMapTexture2D,
                HeightMapTexture = heightMapGrassTexture,
                GraphicsDevice = graphics.GraphicsDevice,
                Width = heightMapTexture2D.Width,
                Height = heightMapTexture2D.Height,
                HeightMapData = new float[heightMapTexture2D.Width, heightMapTexture2D.Height]
            };

            heightMapCameraComponent = new HeightMapCameraComponent()
            {
                ViewMatrix = Matrix.CreateLookAt(new Vector3(-100, 0, 0), Vector3.Zero, Vector3.Up),
                ProjectionMatrix = Matrix.CreatePerspective(1.2f, 0.9f, 1.0f, 1000.0f),
                TerrainMatrix = Matrix.CreateTranslation(new Vector3(0, -100, 256)),
                Position = new Vector3(-100, 0, 0),
                Direction = Vector3.Zero,
                Movement = new Vector3(1, 1, 1),
                Rotation = new Vector3(2, 2, 2) * 0.01f,
                TerrainPosition = new Vector3(0, -100, 256),
            };

            var h1 = new HouseComponent(new Vector3(40, 100, 40), new Vector3(50, 30, -50), Matrix.Identity, BrickTexture);
            var h2 = new HouseComponent(new Vector3(40, 40, 40), new Vector3(1000, 55, -50), Matrix.Identity, BrickTexture);

            int heightMapId = EntityComponentManager.GetManager().CreateNewEntityId();
            EntityComponentManager.GetManager().AddComponentToEntity(heightMapId, heightMapComponent);
            EntityComponentManager.GetManager().AddComponentToEntity(heightMapId, heightMapCameraComponent);

            int h1Id = EntityComponentManager.GetManager().CreateNewEntityId();
            EntityComponentManager.GetManager().AddComponentToEntity(h1Id, h1);

            int h2Id = EntityComponentManager.GetManager().CreateNewEntityId();
            EntityComponentManager.GetManager().AddComponentToEntity(h2Id, h2);
            heightMapSystem.CreateHeightMaps();

            robotComponent = new RobotComponent
            {                
                Speed = 0,                
                Texture = robotTexture,                
                PlaneObjectWorld = Matrix.Identity,
                TransformMatrices = new Matrix[robotModel.Bones.Count],
                Effect = new BasicEffect(graphics.GraphicsDevice),
                Scale = Matrix.CreateScale(0.5f),
                Position = Vector3.Zero,
                RobotProjection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver2, 1280 / 720, 0.1f, 500f),
                RobotView = Matrix.CreateLookAt(new Vector3(70, 50, 30), new Vector3(0, 0, 20), Vector3.Backward)
            };

            robotCameraComponent = new RobotCameraComponent
            {
                MaxRotation = MathHelper.PiOver4,
                RotationSpeed = 0.003f,
                ModelRotation = 0,
                Model = robotModel,
                Direction = true,
                LeftArmMatrix = robotModel.Bones["LeftArm"].Transform,
                RightArmMatrix = robotModel.Bones["RightArm"].Transform,
                LeftLegMatrix = robotModel.Bones["LeftLeg"].Transform,
                RightLegMatrix = robotModel.Bones["RightLeg"].Transform,
                Rotation = Quaternion.CreateFromAxisAngle(Vector3.Right, MathHelper.PiOver2) * Quaternion.CreateFromAxisAngle(Vector3.Up, MathHelper.Pi),
                RotationInDegrees = 0
            };

            int robotId = EntityComponentManager.GetManager().CreateNewEntityId();
            EntityComponentManager.GetManager().AddComponentToEntity(robotId, robotComponent);
            EntityComponentManager.GetManager().AddComponentToEntity(robotId, robotCameraComponent);
            robotSystem.CreateRobots();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            systemsUpdater = new Updater(gameTime);

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            systemsUpdater.Update(heightMapTranformSystem, robotTranformSystem);

            //Debug.WriteLine("Width: " + heightMapComponent.Width);
            //Debug.WriteLine("Height: " + heightMapComponent.Height);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {            
            GraphicsDevice.Clear(Color.CornflowerBlue);

            systemRenderer.Render(heightMapRenderSystem, robotRenderSystem);
            houseSystem.Update();

            base.Draw(gameTime);
        }
    }
}
