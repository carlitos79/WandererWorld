using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using WandererWorld.Components;
using WandererWorld.Manager;
using WandererWorld.System;

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

        private HeightmapSystem heightMapSystem;
        private HeightMapRenderSystem heightMapRenderSystem;
        private HeightMapTranformSystem heightMapTranformSystem;

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

            heightMapSystem = new HeightmapSystem();
            heightMapRenderSystem = new HeightMapRenderSystem();
            heightMapTranformSystem = new HeightMapTranformSystem();

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
                ProjectionMatrix = Matrix.CreatePerspective(1.2f, 0.9f, 1.0f, 1500.0f),
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

            base.Draw(gameTime);
        }
    }
}
