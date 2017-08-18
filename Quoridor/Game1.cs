using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using GeonBit.UI;
using Lidgren.Network;

namespace Quoridor
{
    public class Game1 : Game
    {
        GraphicsDeviceManager myGraphics;
        SpriteBatch mySpriteBatch;
        MainMenu myMainMenu;

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
            NetworkManager.Update();
            PlayerManager.Update();
            UserInterface.Active.Update(gameTime);

            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            UserInterface.Active.Draw(mySpriteBatch);
            base.Draw(gameTime);
        }
    }
}
