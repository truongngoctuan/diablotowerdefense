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
        public static MyAnimatedMenu glAnimatedMenu;
        public static Texture2D[] _rsTexture2Ds;

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
            glAnimatedMenu.Update(Mouse.GetState());

            oldKeyboardState = keyboardState;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_rsTexture2Ds[0],
                                new Rectangle(0, 0, (int)GlobalVar.glViewport.X, (int)GlobalVar.glViewport.Y),
                                Color.White);
            glAnimatedMenu.Draw(spriteBatch);  
        }

        public override void Initialize()
        {
            _rsTexture2Ds = new Texture2D[4];
        }

        public override void LoadContent(ContentManager content)
        {
            glAnimatedMenu = new MyAnimatedMenu("xmlData.txt");

            _rsTexture2Ds[0] = content.Load<Texture2D>(@"Menu\Background");
            _rsTexture2Ds[1] = content.Load<Texture2D>(@"Menu\CenterItem");
            _rsTexture2Ds[2] = content.Load<Texture2D>(@"Menu\MenuItem");
            _rsTexture2Ds[3] = content.Load<Texture2D>(@"Menu\MenuItem_Hovered");

            MyAnimatedMenu.LoadResource();
            MenuItem.LoadResource();
        }

        public override void Clean()
        {
            //throw new NotImplementedException();
        }
    }
}
