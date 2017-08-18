using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using GeonBit.UI;

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
        }

        protected override void Initialize()
        {
            UserInterface.Initialize(Content, BuiltinThemes.hd);
            myMainMenu = new MainMenu();
            
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
