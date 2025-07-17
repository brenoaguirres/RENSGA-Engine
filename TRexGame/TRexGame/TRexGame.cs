using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TRexGame.Engine.Entities;
using TRexGame.Engine.Resources;
using TRexGame.Engine.Tools;
using TRexGame.GameEntities.TRex;
using TRexGame.Plugins;

namespace TRexGame
{
    public class TRexGame : Game
    {
        #region CONSTANTS
        public const int WINDOW_W = 600;
        public const int WINDOW_H = 150;
        #endregion

        #region FIELDS
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private GameResources _gameResources;

        private TRex _tRex;
        private Box2DTest _physicsTest = new();
        #endregion

        #region CONSTRUCTOR
        public TRexGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _gameResources = new GameResources();
        }
        #endregion

        #region GAME LOOP
        protected override void Initialize()
        {
            base.Initialize();

            _graphics.PreferredBackBufferWidth = WINDOW_W;
            _graphics.PreferredBackBufferHeight = WINDOW_H;
            _graphics.ApplyChanges();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _gameResources.LoadResourcePack(Content);

            _tRex = new(_gameResources, WINDOW_W, WINDOW_H);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            EntityManager.Instance.InstantiationStage();
            EntityManager.Instance.AwakeStage();
            EntityManager.Instance.StartStage();
            // missing animation stage
            EntityManager.Instance.FixedUpdateStage();
            EntityManager.Instance.UpdateStage(gameTime);
            EntityManager.Instance.CleanupStage();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            _spriteBatch.Begin();

            EntityManager.Instance.RenderEntityStage(_spriteBatch, gameTime);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
        #endregion
    }
}
