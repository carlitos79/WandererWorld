using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using WandererWorld.Components;
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

        private HeightmapSystem heightMapSystem;
        private HeightMapRenderSystem heightMapRenderSystem;
        private HeightMapTranformSystem heightMapTranformSystem;
        private RobotSystem robotSystem;

        private CollisionSystem collisionSystem;

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
            heightMapRenderSystem = new HeightMapRenderSystem();
            heightMapTranformSystem = new HeightMapTranformSystem();
            robotSystem = new RobotSystem();

            collisionSystem = new CollisionSystem();

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
            Texture2D heightMapGrassTexture = Content.Load<Texture2D>("grass");
            Texture2D heightMapFireTexture = Content.Load<Texture2D>("fire");
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

            int heightMapId = EntityComponentManager.GetManager().CreateNewEntityId();
            EntityComponentManager.GetManager().AddComponentToEntity(heightMapId, heightMapComponent);
            EntityComponentManager.GetManager().AddComponentToEntity(heightMapId, heightMapCameraComponent);
            heightMapSystem.CreateHeightMaps();

            robotComponent = new RobotComponent
            {
                MaxRotation = MathHelper.PiOver4,
                Speed = 0,
                RotationSpeed = 0.003f,
                ModelRotation = 0,
                Model = robotModel,
                Texture = robotTexture,
                Direction = true,
                LeftArmMatrix = robotModel.Bones["LeftArm"].Transform,
                RightArmMatrix = robotModel.Bones["RightArm"].Transform,
                LeftLegMatrix = robotModel.Bones["LeftLeg"].Transform,
                RightLegMatrix = robotModel.Bones["RightLeg"].Transform,
                PlaneObjectWorld = Matrix.Identity,
                TransformMatrices = new Matrix[robotModel.Bones.Count],
                Effect = new BasicEffect(graphics.GraphicsDevice),
                Scale = Matrix.CreateScale(0.5f),
                Rotation = Quaternion.CreateFromAxisAngle(Vector3.Right, MathHelper.PiOver2) * Quaternion.CreateFromAxisAngle(Vector3.Up, MathHelper.Pi),
                Position = Vector3.Zero,
                RobotProjection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver2, 1280 / 720, 0.1f, 500f),
                RobotView = Matrix.CreateLookAt(new Vector3(70, 50, 30), new Vector3(0, 0, 20), Vector3.Backward)
            };

            int robotId = EntityComponentManager.GetManager().CreateNewEntityId();
            EntityComponentManager.GetManager().AddComponentToEntity(robotId, robotComponent);
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            heightMapTranformSystem.UpdateHeightMapCamera(gameTime);
            robotSystem.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            heightMapRenderSystem.RenderHeightMapCamera();
            robotSystem.Draw();

            base.Draw(gameTime);
        }
    }
}
