using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace TowerDefense.GameState
{
    class OptionGameState:GameState
    {
        KeyboardState oldKeyboardState;
        MouseState oldMouseState;
        public OptionScreen glOptionScreen;

        public void NextState(ref Game1 context)
        {
            context.CurrentGameState.Clean();
            //intro to mainmenu
            context.CurrentGameState = new MainMenuGameState();
            context.CurrentGameState.Initialize();
            context.CurrentGameState.LoadContent(context.Content);
        }

        public void PreviousState(ref Game1 context)
        {
        }

        public void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            MouseState mouseState = Mouse.GetState();

            //if (keyboardState.IsKeyDown(Keys.Escape) == true && oldKeyboardState.IsKeyDown(Keys.Escape) == false)
            //{
            //    GlobalVar.SetGameStage(GameStage.MainMenu);
            //}
            glOptionScreen.Update(oldMouseState, oldKeyboardState);

            oldKeyboardState = keyboardState;
            oldMouseState = mouseState;
        }

        public void Draw(Microsoft.Xna.Framework.GameTime gameTime, Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            glOptionScreen.Draw(spriteBatch);
        }

        public void Initialize()
        {
            glOptionScreen = new OptionScreen();
        }

        public void LoadContent(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            glOptionScreen.LoadResource(content);
            glOptionScreen.LoadContent();
        }

        public void Clean()
        {
        }
    }
}
