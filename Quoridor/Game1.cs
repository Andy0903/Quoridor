using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GeonBit.UI;
using QuoridorNetwork;
using System;

namespace Quoridor
{
    public enum GameState
    {
        MainMenu,
        Playing,
    }

    public class Game1 : Game
    {
        public GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        MainMenu mainMenu;
        public GameState state { get; set; }
        GameBoard gameBoard;

        private void NetworkManager_OnGameReadyToStart(object aSender, GameReadyToStartMessage e)
        {
            gameBoard = new GameBoard(e.PlayerNames);
            state = GameState.Playing;
        }

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 740;
            graphics.PreferredBackBufferWidth = 1000;
            Content.RootDirectory = "Content";

            graphics.SynchronizeWithVerticalRetrace = false;
            IsFixedTimeStep = true;
            graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            UserInterface.Initialize(Content, BuiltinThemes.hd);
            mainMenu = new MainMenu();
            state = GameState.MainMenu;

            NetworkManager.OnGameReadyToStart += NetworkManager_OnGameReadyToStart;
            NetworkManager.OnDisconnect += NetworkManager_OnDisconnect;
            NetworkManager.OnConnect += NetworkManager_OnConnect;

            base.Initialize();
        }

        private void NetworkManager_OnConnect(object sender, EventArgs e)
        {
            Console.WriteLine("OnConnect");
        }

        private void NetworkManager_OnDisconnect(object sender, EventArgs e)
        {
            Exit();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            NetworkManager.Update();
            switch (state)
            {
                case GameState.MainMenu:
                    UserInterface.Active.Update(gameTime);
                    break;
                case GameState.Playing:
                    break;
                default:
                    break;
            }

            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            switch (state)
            {
                case GameState.MainMenu:
                    UserInterface.Active.Draw(spriteBatch);
                    break;
                case GameState.Playing:
                    spriteBatch.Begin();
                    gameBoard.Draw(spriteBatch);
                    spriteBatch.End();
                    break;
            }

            base.Draw(gameTime);
        }
    }
}
