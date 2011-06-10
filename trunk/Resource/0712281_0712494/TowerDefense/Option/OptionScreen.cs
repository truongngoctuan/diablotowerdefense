using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
using TowerDefense.Option;

namespace TowerDefense
{
    /// <summary>
    /// xem nhu day la from chinh trong state option
    /// </summary>
    public class OptionScreen
    {
        Texture2D backgroundImage;
        Texture2D m_ttOptionFrom;

        SpriteFont spFontFokard;

        Vector2 m_vt2OptionFromPosition;

        public static CheckBox cbFullScreen;
        public static CheckBox cbMuteSound;
        public static VolumeButton volumebt;

        static Vector2 glViewport;

        ImageButton OkButon;

        public OptionScreen()
        {
            cbFullScreen = new CheckBox(GlobalVar.optionVariables.IsFullScreen, 150, 25, "Is FullScreen");
            cbMuteSound = new CheckBox(GlobalVar.optionVariables.IsMuteSound, 100, 25, "Is Mute");
            volumebt = new VolumeButton();
            OkButon = new ImageButton();
        }

        public void LoadResource(ContentManager content)
        {
            backgroundImage = content.Load<Texture2D>("Options\\OptionScreen");
            m_ttOptionFrom = content.Load<Texture2D>("Options\\OptionFrom");
            spFontFokard = content.Load<SpriteFont>("Options\\Folkard");

            OkButon.LoadResource(content);
            cbFullScreen.LoadResource(content);
            cbMuteSound.LoadResource(content);
            volumebt.LoadResource(content);
        }

        public void LoadContent()
        {
            glViewport = new Vector2(GlobalVar.glGraphics.GraphicsDevice.Viewport.Width, GlobalVar.glGraphics.GraphicsDevice.Viewport.Height);

            m_vt2OptionFromPosition = new Vector2(glViewport.X / 2 - m_ttOptionFrom.Width / 2,
                glViewport.Y / 2 - m_ttOptionFrom.Height / 2);

            OkButon.Position = new Vector2(m_vt2OptionFromPosition.X + m_ttOptionFrom.Width / 2 - OkButon.Width / 2,
                m_vt2OptionFromPosition.Y + m_ttOptionFrom.Height * 0.9f - OkButon.Height);

            cbFullScreen.Position = m_vt2OptionFromPosition + new Vector2(50, 105);
            cbMuteSound.Position = m_vt2OptionFromPosition + new Vector2(m_ttOptionFrom.Width / 2 + 25, 105);
            volumebt.Position = cbMuteSound.Position + new Vector2(0, 30);


            cbFullScreen.Active = () =>
            {//sau nay co the sua thanh con tro den bien fullscreen trong constructor luon,
                //ko can cai dat lai ham active
                GlobalVar.optionVariables.IsFullScreen = cbFullScreen.Checked;
                GlobalVar.optionVariables.ToggleFullScreen();
                //cbFullScreen.Observer.Update("ToggleFullScreen");
            };


            cbMuteSound.Active = () =>
            {
                GlobalVar.optionVariables.IsMuteSound = cbMuteSound.Checked;
                GlobalVar.optionVariables.MuteSound();
                //cbFullScreen.Observer.Update("MuteSound");
            };

            volumebt.Active = () =>
            {
                GlobalVar.optionVariables.Volume = volumebt.Volume;
                GlobalVar.optionVariables.ChangeVolume();
                //cbFullScreen.Observer.Update("MuteSound");
            };

            OkButon.Active = () =>
            {
                GlobalVar.optionVariables.WriteToFile();
                GlobalVar.SetGameStage(GameStage.MainMenu);
            };
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(backgroundImage, new Rectangle(0, 0, (int)GlobalVar.glViewport.X, (int)GlobalVar.glViewport.Y), Color.White);

            spriteBatch.Draw(m_ttOptionFrom, m_vt2OptionFromPosition, Color.White);
            OkButon.Draw(spriteBatch);
            spriteBatch.DrawString(spFontFokard, "Options", new Vector2(glViewport.X / 2 - 25, 150), Color.LightGreen);

            //---------------------------------------
            //vẽ radio button
            cbFullScreen.Draw(spriteBatch);
            cbMuteSound.Draw(spriteBatch);
            volumebt.Draw(spriteBatch);
        }

        public void Update(MouseState OldMouseState,
            KeyboardState oldKeyboardState)
        {
            MouseState ms = Mouse.GetState();

            OkButon.Update(OldMouseState, oldKeyboardState);

            cbFullScreen.Update(OldMouseState, oldKeyboardState);

            //TowerDefense.Option.CheckBox.OptionCheckBoxState sta = cbMuteSound.radioState;
            cbMuteSound.Update(OldMouseState, oldKeyboardState);
            
            LockVolume(!cbMuteSound.Checked);

            volumebt.Update(OldMouseState, oldKeyboardState);
        }

        static public void LockVolume(bool bIsLock)
        {
            if (bIsLock)
            {
                volumebt.bIsEnable = false;
            }
            else
            {
                volumebt.bIsEnable = true;
            }
        }
    }
}
