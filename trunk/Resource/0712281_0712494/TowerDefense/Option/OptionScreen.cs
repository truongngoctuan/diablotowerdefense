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
    public class OptionScreen
    {
        Texture2D backgroundImage;
        Texture2D m_ttOptionFrom;

        SpriteFont spFontFokard;

        Vector2 m_vt2OptionFromPosition;

        public static RadioButton radioFullScreen;
        public static RadioButton radioMuteSound;
        public static VolumeButton volumebt;

        static Vector2 glViewport;

        ImageButton OkButon;

        public OptionScreen()
        {
            radioFullScreen = new RadioButton();
            radioMuteSound = new RadioButton();
            volumebt = new VolumeButton();

            OkButon = new ImageButton();
        }

        public void LoadResource(ContentManager content)
        {
            backgroundImage = content.Load<Texture2D>("Options\\OptionScreen");
            m_ttOptionFrom = content.Load<Texture2D>("Options\\OptionFrom");
            spFontFokard = content.Load<SpriteFont>("Options\\Folkard");

            OkButon.LoadResource(content);
            radioFullScreen.LoadResource(content);
            radioMuteSound.LoadResource(content);
            volumebt.LoadResource(content);
        }

        public void LoadContent()
        {
            glViewport = new Vector2(GlobalVar.glGraphics.GraphicsDevice.Viewport.Width, GlobalVar.glGraphics.GraphicsDevice.Viewport.Height);

            m_vt2OptionFromPosition = new Vector2(glViewport.X / 2 - m_ttOptionFrom.Width / 2,
                glViewport.Y / 2 - m_ttOptionFrom.Height / 2);

            OkButon.Position = new Vector2(m_vt2OptionFromPosition.X + m_ttOptionFrom.Width / 2 - OkButon.Width / 2,
                m_vt2OptionFromPosition.Y + m_ttOptionFrom.Height * 0.9f - OkButon.Height);

            radioFullScreen.Position = m_vt2OptionFromPosition + new Vector2(50, 105);
            radioFullScreen.Text = "Is FullScreen";
            radioFullScreen.Width = 150;
            radioFullScreen.Height = 25;

            radioMuteSound.Position = m_vt2OptionFromPosition + new Vector2(m_ttOptionFrom.Width / 2 + 25, 105);
            radioMuteSound.Text = "Is Mute";
            radioMuteSound.Width = 100;
            radioMuteSound.Height = 25;

            volumebt.Position = radioMuteSound.Position + new Vector2(0, 30);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(backgroundImage, new Rectangle(0, 0, (int)GlobalVar.glViewport.X, (int)GlobalVar.glViewport.Y), Color.White);

            spriteBatch.Draw(m_ttOptionFrom, m_vt2OptionFromPosition, Color.White);
            OkButon.Draw(spriteBatch);
            spriteBatch.DrawString(spFontFokard, "Options", new Vector2(glViewport.X / 2 - 25, 150), Color.LightGreen);

            //---------------------------------------
            //vẽ radio button
            radioFullScreen.Draw(spriteBatch);
            radioMuteSound.Draw(spriteBatch);
            volumebt.Draw(spriteBatch);
        }

        public void Update(MouseState OldMouseState,
            KeyboardState oldKeyboardState)
        {
            MouseState ms = Mouse.GetState();

            OkButon.Update(OldMouseState, oldKeyboardState);

            radioFullScreen.Update(OldMouseState, oldKeyboardState);

            TowerDefense.Option.RadioButton.OptionRadioState sta = radioMuteSound.radioState;
            radioMuteSound.Update(OldMouseState, oldKeyboardState);
            if (sta != radioMuteSound.radioState)
            {
                LockUnlockVolume();
            }

            volumebt.Update(OldMouseState, oldKeyboardState);
        }

        static public void LockUnlockVolume()
        {
            if (radioMuteSound.radioState == TowerDefense.Option.RadioButton.OptionRadioState.Checked)
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
