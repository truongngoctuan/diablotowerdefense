using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace TowerDefense.GameState
{
    /// <summary>
    /// chua chuyen tu single nguoc tro ve mainmenu dc
    /// </summary>
    public class MainMenuGameState: GameState
    {
        KeyboardState oldKeyboardState;
        public MyAnimatedMenu glAnimatedMenu;

        public override void NextState(ref Game1 context)
        {
            //intro to mainmenu
        }

        public void NextState(ref Game1 context, GameStage gs)
        {
            switch (gs)
            {
                case GameStage.SinglePlayer:
                    {
                        break;
                    }
                case GameStage.Options:
                    {
                        context.CurrentGameState.Clean();
                        //intro to mainmenu
                        context.CurrentGameState = new OptionGameState();
                        context.CurrentGameState.Initialize();
                        context.CurrentGameState.LoadContent(context.Content);
                        break;
                    }
                case GameStage.HighScore:
                    {
                        break;
                    }
            }

        }

        public override void PreviousState(ref Game1 context)
        {
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Escape) && oldKeyboardState.IsKeyUp(Keys.Escape))
            {
                if (glAnimatedMenu.IsRootMain())
                {
                    GlobalVar.glGame.Exit();
                }
                else
                {
                    glAnimatedMenu = glAnimatedMenu.UpToParent();
                }
            }
            glAnimatedMenu.Update(gameTime);

            oldKeyboardState = keyboardState;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            glAnimatedMenu.Draw(gameTime, spriteBatch);  
        }

        public override void Initialize()
        {
            glAnimatedMenu = new MyAnimatedMenu("xmlData.txt");
            glAnimatedMenu.Initialize();
        }

        public override void LoadContent(ContentManager content)
        {


            glAnimatedMenu.LoadContent(content);
            //MenuItem.LoadResource();
        }

        public override void Clean()
        {
            //throw new NotImplementedException();
        }
    }
}
