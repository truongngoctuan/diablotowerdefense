using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using System.IO;
using KkunFGame;

namespace TowerDefense 
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        //ReferenceGraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        KeyboardState oldKeyboardState;
        MouseState oldMouseState;
        Vector2 lastPressedPosition;

        Texture2D textureLogo;
        Video introMovie;
        float fIntroMovieScale;
        Texture2D cursorTexture;
        Vector2 vt2CursorPosition;
        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            //graphics = new ReferenceGraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            //Directory.SetCurrentDirectory("Content");
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            base.Initialize();

            GlobalVar.glGraphics = graphics;


            this.IsMouseVisible = false;
            vt2CursorPosition = new Vector2();
            
            MyAnimatedMenu.LoadResource();
            MenuItem.LoadResource();

            GlobalVar.glGame = this;
            GlobalVar.glContentManager = Content;



            GlobalVar.glOptionScreen = new OptionScreen();

            OptionScreen.ReadFromFile();
            OptionScreen.ToggleFullScreen();
            OptionScreen.MuteSound();
            OptionScreen.ChangeVolume();
            OptionScreen.LockUnlockVolume();


            GlobalVar.glIntroPlayer = new VideoPlayer();
            GlobalVar.glViewport = new Vector2(graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height);
            
            //GlobalVar.glMapSize = new Vector2(3000, 3000);
            //GlobalVar.glWorldSpace = new WorldSpace((int)(GlobalVar.glMapSize.Y / GlobalVar.glvtCellSize.Y), (int)(GlobalVar.glMapSize.X / GlobalVar.glvtCellSize.X));

            GlobalVar.glUnitManager = new UnitManager();
            GlobalVar.glStageManager = new StageManager();
            GlobalVar.glRootCoordinate = new Vector2(0, 0);

            GlobalVar.glIconSize = new Vector2(50, 50);
            GlobalVar.glIconPosition = new Vector2(GlobalVar.glViewport.X - GlobalVar.glIconSize.X / 2 - 10, GlobalVar.glIconSize.Y / 2 + 10);

            AudioPlayer.LoadContent();            

            if (introMovie.Width >= introMovie.Height)
                fIntroMovieScale = GlobalVar.glViewport.X / introMovie.Width;
            else
                fIntroMovieScale = GlobalVar.glViewport.Y / introMovie.Height;
            GlobalVar.SetGameStage(GameStage.MainMenu);
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            
            // TODO: use this.Content to load your game content here
            cursorTexture = Content.Load<Texture2D>("cursor");
            introMovie = Content.Load<Video>("introMovie");
            textureLogo = Content.Load<Texture2D>("HighScore_background");

            ResourceManager._rsTexture2Ds = new Texture2D[4];
            ResourceManager._rsTexture2Ds[0] = Content.Load<Texture2D>(@"Menu\Background");
            ResourceManager._rsTexture2Ds[1] = Content.Load<Texture2D>(@"Menu\CenterItem");
            ResourceManager._rsTexture2Ds[2] = Content.Load<Texture2D>(@"Menu\MenuItem");
            ResourceManager._rsTexture2Ds[3] = Content.Load<Texture2D>(@"Menu\MenuItem_Hovered");
            
            ResourceManager._rsFonts = new SpriteFont[1];
            ResourceManager._rsFonts[0] = Content.Load<SpriteFont>(@"Options\Folkard");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// 

        bool m_bPlayIntro = false;
        bool m_bIsLeftButtonDown = false;
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            KeyboardState keyboardState = Keyboard.GetState();
            MouseState mouseState = Mouse.GetState();
            vt2CursorPosition.X = mouseState.X;
            vt2CursorPosition.Y = mouseState.Y;

            switch (GlobalVar.glGameStage)
            {
                case GameStage.Intro:
                    {
                        if (GlobalVar.iTimeTillIntroMovie <= 0)
                        {
                            if (GlobalVar.glIntroPlayer.State == MediaState.Stopped)
                            {
                                if (m_bPlayIntro == true)
                                {
                                    GlobalVar.glIntroPlayer.Stop();
                                    GlobalVar.SetGameStage(GameStage.MainMenu);
                                }
                                GlobalVar.glIntroPlayer.IsLooped = false;
                                GlobalVar.glIntroPlayer.Play(introMovie);
                                m_bPlayIntro = true;
                            }
                            if (keyboardState.IsKeyDown(Keys.Space) == true && oldKeyboardState.IsKeyDown(Keys.Space) == false)
                            {
                                GlobalVar.glIntroPlayer.Stop();
                                GlobalVar.SetGameStage(GameStage.MainMenu);
                            }
                        }
                        else
                        {
                            GlobalVar.iTimeTillIntroMovie -= gameTime.ElapsedGameTime.Milliseconds;
                        }

                        break;
                    }
                case GameStage.MainMenu:
                    {
                        if (keyboardState.IsKeyDown(Keys.Escape) && oldKeyboardState.IsKeyUp(Keys.Escape))
                        {
                            if (GlobalVar.glAnimatedMenu.IsRootMain())
                            {
                                this.Exit();
                            }
                            else
                            {
                                GlobalVar.glAnimatedMenu = GlobalVar.glAnimatedMenu.UpToParent();
                            }
                        }
                        GlobalVar.glAnimatedMenu.Update(Mouse.GetState());

                        //GlobalVar.glGameStage = GameStage.SinglePlayer;
                        break;
                    }
                case GameStage.HighScore:
                    {
                        if (keyboardState.IsKeyDown(Keys.Escape) == true && oldKeyboardState.IsKeyDown(Keys.Escape) == false)
                        {
                            GlobalVar.SetGameStage(GameStage.MainMenu);
                        }    
                        break;
                    }
                case GameStage.Options:
                    {
                        if (keyboardState.IsKeyDown(Keys.Escape) == true && oldKeyboardState.IsKeyDown(Keys.Escape) == false)
                        {
                            GlobalVar.SetGameStage(GameStage.MainMenu);
                        }
                        GlobalVar.glOptionScreen.Update(oldMouseState, oldKeyboardState);
                        break;
                    }
                case GameStage.Loading:
                    {
                        GlobalVar.glLoadScreen.Update();
                        break;
                    }
                case GameStage.SinglePlayer:
                    {
                        if (!m_bIsLeftButtonDown)
                        {
                            if (mouseState.LeftButton == ButtonState.Pressed)
                            {//chưa nhấn chuột thì cập nhật vị trí chuột cũ
                                m_bIsLeftButtonDown = true;
                                lastPressedPosition = new Vector2(mouseState.X, mouseState.Y);
                            }
                        }
                        else
                        {
                            if (mouseState.LeftButton == ButtonState.Released)
                            {//thả chuột
                                m_bIsLeftButtonDown = false;
                                return;
                            }

                            //--------------------------
                            //trường hợp kéo map là đây
                            Vector2 curCursor = new Vector2();
                            curCursor.X = mouseState.X;
                            curCursor.Y = mouseState.Y;

                            //cập nhật vị trí tương đối của map
                            GlobalVar.glRootCoordinate -= (curCursor - lastPressedPosition);

                            //giới hạn map không cho chạy vượt quá quy định cho phép
                            //chặn top left
                            if (GlobalVar.glRootCoordinate.X < 0)
                            {
                                GlobalVar.glRootCoordinate.X = 0;
                            }

                            if (GlobalVar.glRootCoordinate.Y < -200 * GlobalVar.glMapScale)
                            {//ưu tiên cái này để nó vẫn còn thấy được hết cả sprites
                                GlobalVar.glRootCoordinate.Y = -200 * GlobalVar.glMapScale;
                            }

                            if (GlobalVar.glRootCoordinate.X + GlobalVar.glViewport.X > (GlobalVar.ConvertTileToPixelX(0, GlobalVar.glMapSize.X - 1)
                                                                                    + GlobalVar.glvtCellSize.X) * GlobalVar.glMapScale)
                            {
                                GlobalVar.glRootCoordinate.X = (GlobalVar.ConvertTileToPixelX(0, GlobalVar.glMapSize.X - 1)
                                                            + GlobalVar.glvtCellSize.X) * GlobalVar.glMapScale - GlobalVar.glViewport.X;
                            }

                            if (GlobalVar.glRootCoordinate.Y + GlobalVar.glViewport.Y > (GlobalVar.ConvertTileToPixelY(GlobalVar.glMapSize.Y - 1, GlobalVar.glMapSize.X)
                                                                                    + GlobalVar.glvtCellSize.Y / 2) * GlobalVar.glMapScale)
                            {
                                GlobalVar.glRootCoordinate.Y = (GlobalVar.ConvertTileToPixelY(GlobalVar.glMapSize.Y - 1, GlobalVar.glMapSize.X)
                                                            + GlobalVar.glvtCellSize.Y / 2) * GlobalVar.glMapScale - GlobalVar.glViewport.Y;
                            }

                            lastPressedPosition = curCursor;
                        }

                        GlobalVar.glUnitManager.Update(gameTime, keyboardState, mouseState);
                        GlobalVar.glStageManager.Update(gameTime, keyboardState, mouseState);

                        //-----------------------------------------------------
                        //thoát game ra mainmenu
                        if (oldKeyboardState.IsKeyDown(Keys.Escape) &&
                            keyboardState.IsKeyUp(Keys.Escape))
                        {
                            GlobalVar.SetGameStage(GameStage.MainMenu);
                            GlobalVar.glCurrentMap = null;
                            GlobalVar.glUnitManager = new UnitManager();
                            GlobalVar.glStageManager = new StageManager();
                            ResourceManager.nCreepSprites = 0;
                            ResourceManager.nTowerSprites = 0;
                        }
                        break;
                    }
            }
            
            oldKeyboardState = keyboardState;
            oldMouseState = mouseState;
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// 
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            switch (GlobalVar.glGameStage)
            {
                case GameStage.Intro:
                    {
                        if (GlobalVar.iTimeTillIntroMovie <= 0)
                        {
                            if (GlobalVar.glIntroPlayer.State != MediaState.Stopped)
                            {
                                spriteBatch.Draw(GlobalVar.glIntroPlayer.GetTexture(), new Vector2(GlobalVar.glViewport.X / 2, GlobalVar.glViewport.Y / 2), null, Color.White, 0.0f, new Vector2(introMovie.Width / 2, introMovie.Height / 2), fIntroMovieScale, SpriteEffects.None, 1.0f);
                            }
                        }
                        else
                        {
                            spriteBatch.Draw(textureLogo, new Rectangle(0, 0, (int)GlobalVar.glViewport.X, (int)GlobalVar.glViewport.Y), Color.White);
                        }
                        break;
                    }
                case GameStage.MainMenu:
                    {
                        spriteBatch.Draw(ResourceManager._rsTexture2Ds[0],
                                new Rectangle(0, 0, (int)GlobalVar.glViewport.X, (int)GlobalVar.glViewport.Y),
                                Color.White);
                        GlobalVar.glAnimatedMenu.Draw(spriteBatch);                        
                        break;
                    }
                case GameStage.HighScore:
                    {
                        GlobalVar.glHighScoreScreen.Draw(spriteBatch);
                        break;
                    }
                case GameStage.Options:
                    {
                        GlobalVar.glOptionScreen.Draw(spriteBatch);
                        //spriteBatch.DrawString(ResourceManager._rsFonts[0], "Options", new Vector2(250, 400), Color.Black);
                        break;
                    }
                case GameStage.SinglePlayer:
                    {
                        GlobalVar.glStageManager.Draw(spriteBatch);
                        GlobalVar.glUnitManager.Draw(spriteBatch);
                        break;
                    }
                case GameStage.Multiplayers:
                    {
                        spriteBatch.DrawString(ResourceManager._rsFonts[0], "Multiplayers", new Vector2(250, 400), Color.Black);
                        break;
                    }
                case GameStage.Loading:
                    {
                        
                        GlobalVar.glLoadScreen.Draw(spriteBatch);
                        break;
                    }
                case GameStage.InGame:
                    {
                        spriteBatch.DrawString(ResourceManager._rsFonts[0], "InGame", new Vector2(250, 400), Color.Black);
                        break;
                    }
            }
            spriteBatch.Draw(cursorTexture, vt2CursorPosition, null, Color.White, 0.0f, new Vector2(cursorTexture.Width/2, cursorTexture.Height/2), 1.0f, SpriteEffects.None, 1.0f);
            spriteBatch.End();
            ParticleSystemManager.Draw(spriteBatch);
            base.Draw(gameTime);
        }
    }
}
