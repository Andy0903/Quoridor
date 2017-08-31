using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GeonBit.UI;
using QuoridorNetwork;

namespace Quoridor
{
    public enum GameState
    {
        MainMenu,
        Playing,
    }

    public class Game1 : Game
    {
        public GraphicsDeviceManager myGraphics;
        SpriteBatch mySpriteBatch;
        MainMenu myMainMenu;
        public GameState State { get; set; }
        GameBoard myGameBoard;

        private void NetworkManager_OnGameReadyToStart(object aSender, GameReadyToStartMessage e)
        {
            myGameBoard = new GameBoard(e.PlayerNames);
        }

        public Game1()
        {
            myGraphics = new GraphicsDeviceManager(this);
            myGraphics.PreferredBackBufferHeight = 740;
            myGraphics.PreferredBackBufferWidth = 1000;
            Content.RootDirectory = "Content";

            myGraphics.SynchronizeWithVerticalRetrace = false;
            IsFixedTimeStep = true;
            myGraphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            UserInterface.Initialize(Content, BuiltinThemes.hd);
            myMainMenu = new MainMenu();
            State = GameState.MainMenu;

            NetworkManager.OnGameReadyToStart += NetworkManager_OnGameReadyToStart;
            NetworkManager.OnDisconnect += NetworkManager_OnDisconnect;
            NetworkManager.OnConnect += NetworkManager_OnConnect;

            base.Initialize();
        }

        private void NetworkManager_OnConnect(object sender, System.EventArgs e)
        {
            State = GameState.Playing;
        }

        private void NetworkManager_OnDisconnect(object sender, System.EventArgs e)
        {
            throw new System.NotImplementedException();
        }

        protected override void LoadContent()
        {
            mySpriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            switch (State)
            {
                case GameState.MainMenu:
                    UserInterface.Active.Update(gameTime);
                    break;
                case GameState.Playing:
                    NetworkManager.Update();
                    break;
                default:
                    break;
            }

            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            switch (State)
            {
                case GameState.MainMenu:
                    UserInterface.Active.Draw(mySpriteBatch);
                    break;
                case GameState.Playing:
                    mySpriteBatch.Begin();
                    myGameBoard.Draw(mySpriteBatch);
                    mySpriteBatch.End();
                    break;
            }

            base.Draw(gameTime);
        }
    }
}
