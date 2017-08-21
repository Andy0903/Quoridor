using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using GeonBit.UI;
using Lidgren.Network;

namespace Quoridor
{
    public enum GameState
    {
        MainMenu,
        Playing,
    }

    public enum NumberOfPlayers
    {
        TwoPlayers,
        FourPlayers,
    }

    public class Game1 : Game
    {
        public GraphicsDeviceManager myGraphics;
        SpriteBatch mySpriteBatch;
        MainMenu myMainMenu;
        //public Player LocalPlayer { get; set; }
        public GameState State { get; set; }
        public NumberOfPlayers PlayerNumbers { get; set; }
        GameBoard myGameBoard;

        public Game1()
        {
            myGraphics = new GraphicsDeviceManager(this);
            myGraphics.PreferredBackBufferHeight = 740;
            myGraphics.PreferredBackBufferWidth = 1280;
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

            NetworkManager.myConfig = new NetPeerConfiguration("QuoridorConfig");   //Must be same appIdentifier as the server uses.
            NetworkManager.myClient = new NetClient(NetworkManager.myConfig);

            base.Initialize();
        }

        public void ConstructBoard()
        {
            myGameBoard = new GameBoard();
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
            NetworkManager.Update();
            switch (State)
            {
                case GameState.MainMenu:
                    UserInterface.Active.Update(gameTime);
                    break;
                case GameState.Playing:
                    PlayerManager.Update();
                    myGameBoard.Update();
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
