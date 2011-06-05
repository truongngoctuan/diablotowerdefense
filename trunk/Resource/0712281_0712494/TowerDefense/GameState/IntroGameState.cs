using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace TowerDefense.GameState
{
    /// <summary>
    /// con lien ket voi Gloval 2 thanh phan: glgraphics va SetGameStage
    /// </summary>
    public class IntroGameState: GameState
    {
        bool m_bPlayIntro;
        Video introMovie;
        Texture2D textureLogo;
        float fIntroMovieScale;
        Vector2 glViewport;

        VideoPlayer glIntroPlayer;

        int iTimeTillIntroMovie;

        KeyboardState oldKeyboardState;
        public override void NextState(ref Game1 context)
        {
            context.CurrentGameState.Clean();
            //intro to mainmenu
            context.CurrentGameState = new MainMenuGameState();
            context.CurrentGameState.Initialize();
            context.CurrentGameState.LoadContent(context.Content);
        }

        public override void PreviousState(ref Game1 context)
        {
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();

            if (iTimeTillIntroMovie <= 0)
            {
                if (glIntroPlayer.State == MediaState.Stopped)
                {
                    if (m_bPlayIntro == true)
                    {
                        GlobalVar.SetGameStage(GameStage.MainMenu);
                    }
                }
                else
                {
                    glIntroPlayer.Play(introMovie);
                }

                if (keyboardState.IsKeyDown(Keys.Space) == true && oldKeyboardState.IsKeyDown(Keys.Space) == false)
                {
                    glIntroPlayer.Stop();
                    GlobalVar.SetGameStage(GameStage.MainMenu);
                }
            }
            else
            {
                iTimeTillIntroMovie -= gameTime.ElapsedGameTime.Milliseconds;
            }

            oldKeyboardState = keyboardState;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

            if (iTimeTillIntroMovie <= 0)
            {
                if (glIntroPlayer.State != MediaState.Stopped)
                {
                    spriteBatch.Draw(glIntroPlayer.GetTexture(), new Vector2(glViewport.X / 2, glViewport.Y / 2), null, Color.White, 0.0f, new Vector2(introMovie.Width / 2, introMovie.Height / 2), fIntroMovieScale, SpriteEffects.None, 1.0f);
                }
            }
            else
            {
                spriteBatch.Draw(textureLogo, new Rectangle(0, 0, (int)glViewport.X, (int)glViewport.Y), Color.White);
            }
        }

        public override void Initialize()
        {
            m_bPlayIntro = false;
            glIntroPlayer = new VideoPlayer();
            glIntroPlayer.IsLooped = false;
            iTimeTillIntroMovie = 1000;

            glViewport = new Vector2(GlobalVar.glGraphics.GraphicsDevice.Viewport.Width, GlobalVar.glGraphics.GraphicsDevice.Viewport.Height);
        }

        public override void LoadContent(ContentManager content)
        {
            introMovie = content.Load<Video>("introMovie");
            textureLogo = content.Load<Texture2D>("HighScore_background");

            if (introMovie.Width >= introMovie.Height)
                fIntroMovieScale = glViewport.X / introMovie.Width;
            else
                fIntroMovieScale = glViewport.Y / introMovie.Height;

            glIntroPlayer.Play(introMovie);
            m_bPlayIntro = true;
        }

        public override void Clean()
        {
            introMovie = null;
            textureLogo = null;
            glIntroPlayer = null;
        }
    }
}
