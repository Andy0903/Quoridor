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
        public Player LocalPlayer { get; set; }
        public GameState State { get; set; }
        public NumberOfPlayers PlayerNumbers { get; set; } //TODO ask server for amount of players.

        public Game1()
        {
            myGraphics = new GraphicsDeviceManager(this);
            myGraphics.PreferredBackBufferHeight = 720;
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
                    PlayerManager.Update();
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
                    break;
                default:
                    break;
            }

            base.Draw(gameTime);
        }
    }
}
