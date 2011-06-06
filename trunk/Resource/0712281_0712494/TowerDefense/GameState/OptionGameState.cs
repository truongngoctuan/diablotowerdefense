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

        public override void NextState(ref Game1 context)
        {
        }

        public override void PreviousState(ref Game1 context)
        {
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            MouseState mouseState = Mouse.GetState();

            if (keyboardState.IsKeyDown(Keys.Escape) == true && oldKeyboardState.IsKeyDown(Keys.Escape) == false)
            {
                GlobalVar.SetGameStage(GameStage.MainMenu);
            }
            glOptionScreen.Update(oldMouseState, oldKeyboardState);

            oldKeyboardState = keyboardState;
            oldMouseState = mouseState;
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime, Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            glOptionScreen.Draw(spriteBatch);
        }

        public override void Initialize()
        {
            glOptionScreen = new OptionScreen();
        }

        public override void LoadContent(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            glOptionScreen.LoadResource(content);
            glOptionScreen.LoadContent();
        }

        public override void Clean()
        {
        }
    }
}
