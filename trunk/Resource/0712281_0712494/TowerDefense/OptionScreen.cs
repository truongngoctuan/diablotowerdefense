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

namespace TowerDefense
{
    public class OptionScreen
    {
        Texture2D backgroundImage;
        Texture2D m_ttOptionFrom;
        Texture2D m_ttbtOK;
        Texture2D m_ttbtOK_Hovered;

        SpriteFont spFontFokard;

        Vector2 m_vt2OptionFromPosition;
        Vector2 m_vt2btOkPosition;

        public static RadioButton radioFullScreen;
        public static RadioButton radioMuteSound;
        public static VolumeButton volumebt;

        
        enum OptionScreenState { Opening, Opened, Closing, Closed };
        OptionScreenState m_OptionScreenState;

        public OptionScreen()
        {
            radioFullScreen = new RadioButton();
            radioMuteSound = new RadioButton();
            volumebt = new VolumeButton();
        }

        public void LoadResource()
        {
            backgroundImage = GlobalVar.glContentManager.Load<Texture2D>("Options\\OptionScreen");

            m_ttOptionFrom = GlobalVar.glContentManager.Load<Texture2D>("Options\\OptionFrom");

            m_ttbtOK = GlobalVar.glContentManager.Load<Texture2D>("Options\\btOK");
            m_ttbtOK_Hovered = GlobalVar.glContentManager.Load<Texture2D>("Options\\btOK_Hovered");

            spFontFokard = GlobalVar.glContentManager.Load<SpriteFont>("Options\\Folkard");

            //đọc file...
            ReadFromFile();
        }
        public void LoadContent()
        {
            m_vt2OptionFromPosition = new Vector2(GlobalVar.glViewport.X / 2 - m_ttOptionFrom.Width / 2,
                GlobalVar.glViewport.Y / 2 - m_ttOptionFrom.Height / 2);

            m_vt2btOkPosition = new Vector2(m_vt2OptionFromPosition.X + m_ttOptionFrom.Width / 2 - m_ttbtOK.Width / 2,
                m_vt2OptionFromPosition.Y + m_ttOptionFrom.Height * 0.9f - m_ttbtOK.Height);

            radioFullScreen.m_vt2radioPosition = m_vt2OptionFromPosition + new Vector2(50, 105);
            radioFullScreen.str = "Is FullScreen";
            radioFullScreen.iWidth = 150;
            radioFullScreen.iHeight = 25;

            radioMuteSound.m_vt2radioPosition = m_vt2OptionFromPosition + new Vector2(m_ttOptionFrom.Width / 2 + 25, 105);
            radioMuteSound.str = "Is Mute";
            radioMuteSound.iWidth = 100;
            radioMuteSound.iHeight = 25;

            volumebt.m_vt2VolumePosition = radioMuteSound.m_vt2radioPosition + new Vector2(0, 30);
        }

        public void Draw(SpriteBatch spriteBatch)
        {

            spriteBatch.Draw(backgroundImage, new Rectangle(0, 0, (int)GlobalVar.glViewport.X, (int)GlobalVar.glViewport.Y), Color.White);

            spriteBatch.Draw(m_ttOptionFrom, m_vt2OptionFromPosition, Color.White);

            switch (btOKState)
            {
                case OptionButtonState.Normal:
                    {
                        spriteBatch.Draw(m_ttbtOK, m_vt2btOkPosition, Color.White);
                        break;
                    }
                case OptionButtonState.Hovered:
                    {
                        spriteBatch.Draw(m_ttbtOK_Hovered, m_vt2btOkPosition, Color.White);
                        break;
                    }
            }

            spriteBatch.DrawString(spFontFokard, "Options", new Vector2(GlobalVar.glViewport.X / 2 - 25, 150), Color.LightGreen);

            //---------------------------------------
            //vẽ radio button
            radioFullScreen.Draw(spriteBatch);
            radioMuteSound.Draw(spriteBatch);
            volumebt.Draw(spriteBatch);
        }

        enum OptionButtonState { Normal, Hovered};
        OptionButtonState btOKState;

        public void Update(MouseState OldMouseState,
            KeyboardState oldKeyboardState)
        {
            MouseState ms = Mouse.GetState();

            btOKState = OptionButtonState.Normal;

            if (m_vt2btOkPosition.X + m_ttbtOK.Width * 0.1f < ms.X && ms.X < m_vt2btOkPosition.X + m_ttbtOK.Width * 0.9f)
            {
                if (m_vt2btOkPosition.Y + m_ttbtOK.Height * 0.1f < ms.Y && ms.Y < m_vt2btOkPosition.Y + m_ttbtOK.Height * 0.9f)
                {
                    btOKState = OptionButtonState.Hovered;

                    //kiểm tra có nhấn hay ko, nếu nhấn thì thoát
                    if (OldMouseState.LeftButton == ButtonState.Pressed &&
                        ms.LeftButton == ButtonState.Released)
                    {
                        //cập nhật các giá trị - ghi lại vào file, fullscreen -sound...
                        //if (m_bIsRadioChangeState == true)
                        {
                            ToggleFullScreen();
                            LoadContent();

                            MuteSound();
                            ChangeVolume();
                            LockUnlockVolume();
                            

                        }
                        WriteToFile();
                    }
                }
            }

            radioFullScreen.Update(OldMouseState, oldKeyboardState);

            OptionRadioState sta = radioMuteSound.radioState;
            radioMuteSound.Update(OldMouseState, oldKeyboardState);
            if (sta != radioMuteSound.radioState)
            {
                LockUnlockVolume();
            }

            volumebt.Update(OldMouseState, oldKeyboardState);
        }

        static public void MuteSound()
        {
            if (radioMuteSound.radioState == OptionRadioState.Checked)
            {
                //mute
                AudioPlayer.Mute(true);
            }
            else
            {
                //unmute
                AudioPlayer.Mute(false);
            }
        }

        static public void LockUnlockVolume()
        {
            if (radioMuteSound.radioState == OptionRadioState.Checked)
            {
                volumebt.bIsEnable = false;
            }
            else
            {
                volumebt.bIsEnable = true;
            }
        }

        static public void ChangeVolume()
        {

            AudioPlayer.ChangeVolume(volumebt.fVolume);
        }

        static public void ToggleFullScreen()
        {
            if (radioFullScreen.radioState == OptionRadioState.Normal && GlobalVar.glGraphics.IsFullScreen == true)
            {
                GlobalVar.glGraphics.PreferredBackBufferWidth = 800;
                GlobalVar.glGraphics.PreferredBackBufferHeight = 600;

                GlobalVar.glViewport.X = 800;
                GlobalVar.glViewport.Y = 600;
                GlobalVar.glGraphics.ToggleFullScreen();
            }

            if (radioFullScreen.radioState == OptionRadioState.Checked && GlobalVar.glGraphics.IsFullScreen == false)
            {
                GlobalVar.glGraphics.PreferredBackBufferWidth = 1024;
                GlobalVar.glGraphics.PreferredBackBufferHeight = 768;

                GlobalVar.glViewport.X = 1024;
                GlobalVar.glViewport.Y = 768;
                GlobalVar.glGraphics.ToggleFullScreen();
            }
        }

        static public void ReadFromFile()
        {
            //khởi tạo
            FileStream fStream;

            fStream = new FileStream(@"Content\Options\option.conf",
                FileMode.Open,
                FileAccess.Read);

            StreamReader sr = new StreamReader(fStream);

            //lấy khung
            string strBuffer;
            strBuffer = sr.ReadLine();

            radioFullScreen.radioState = (OptionRadioState)int.Parse(strBuffer);

            strBuffer = sr.ReadLine();

            radioMuteSound.radioState = (OptionRadioState)int.Parse(strBuffer);

            strBuffer = sr.ReadLine();

            volumebt.fVolume = (float)int.Parse(strBuffer) * 0.1f;

            sr.Close();
            fStream.Close();
        }

        void WriteToFile()
        {
            //khởi tạo
            FileStream fStream;

            fStream = new FileStream(@"Content\Options\option.conf",
                FileMode.Create,
                FileAccess.Write);

            StreamWriter sr = new StreamWriter(fStream);

            //lấy khung

            sr.WriteLine(((int )radioFullScreen.radioState).ToString());
            sr.WriteLine(((int)radioMuteSound.radioState).ToString());
            sr.WriteLine(((int)(volumebt.fVolume * 10f)).ToString());

            sr.Close();
            fStream.Close();
        }
    }

    public enum OptionRadioState { Normal, Checked };

    public class RadioButton
    {
        static Texture2D m_ttradio;
        static Texture2D m_ttradioChecked;
        static SpriteFont spFontFokard;

        public Vector2 m_vt2radioPosition;
        public string str;

        public int iWidth;
        public int iHeight = 25;


        public OptionRadioState radioState;
        bool m_bIsRadioChangeState = false;

        public RadioButton()
        {
            LoadResource();
        }

        static void LoadResource()
        {
            m_ttradio = GlobalVar.glContentManager.Load<Texture2D>("Options\\Radio");
            m_ttradioChecked = GlobalVar.glContentManager.Load<Texture2D>("Options\\radio_checked");

            spFontFokard = GlobalVar.glContentManager.Load<SpriteFont>("Options\\Folkard");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(spFontFokard, str, m_vt2radioPosition + new Vector2(25, 0), Color.YellowGreen);
            switch (radioState)
            {
                case OptionRadioState.Normal:
                    {
                        spriteBatch.Draw(m_ttradio, m_vt2radioPosition, Color.White);
                        break;
                    }
                case OptionRadioState.Checked:
                    {
                        spriteBatch.Draw(m_ttradioChecked, m_vt2radioPosition, Color.White);
                        break;
                    }
            }
        }

        public void Update(MouseState OldMouseState,
    KeyboardState oldKeyboardState)
        {
            MouseState ms = Mouse.GetState();

            //kiểm tra cái radio button
            if (m_vt2radioPosition.X < ms.X && ms.X < m_vt2radioPosition.X + iWidth)
            {
                if (m_vt2radioPosition.Y < ms.Y && ms.Y < m_vt2radioPosition.Y + iHeight)
                {
                    //kiểm tra có nhấn hay ko
                    if (OldMouseState.LeftButton == ButtonState.Pressed &&
                        ms.LeftButton == ButtonState.Released)
                    {
                        m_bIsRadioChangeState = true;
                        if (radioState == OptionRadioState.Normal)
                        {
                            radioState = OptionRadioState.Checked;
                        }
                        else
                        {
                            radioState = OptionRadioState.Normal;
                        }
                    }
                }
            }

        }
    }

    public class VolumeButton
    {
        static Texture2D m_ttVolume;
        static Texture2D m_ttVolumeButton;
        

        public Vector2 m_vt2VolumePosition;
        Vector2 m_vt2VolumeButtonPosition;

        public float fVolume = 1;

        public bool bIsEnable = true;

        public int iWidth = 109;
        public int iHeight = 30;

        
        public OptionRadioState radioState;

        public VolumeButton()
        {
            LoadResource();
        }

        static void LoadResource()
        {
            m_ttVolume = GlobalVar.glContentManager.Load<Texture2D>("Options\\volume");
            m_ttVolumeButton = GlobalVar.glContentManager.Load<Texture2D>("Options\\volumebutton");


        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.DrawString(spFontFokard, str, m_vt2radioPosition + new Vector2(25, 0), Color.YellowGreen);
            if (bIsEnable)
            {
                spriteBatch.Draw(m_ttVolume, m_vt2VolumePosition, Color.White);
                spriteBatch.Draw(m_ttVolumeButton, m_vt2VolumeButtonPosition - new Vector2(m_ttVolumeButton.Width / 2, m_ttVolumeButton.Height / 2 * 0.85f), Color.White);
            }
            else
            {
                spriteBatch.Draw(m_ttVolume, m_vt2VolumePosition, Color.Goldenrod);
                spriteBatch.Draw(m_ttVolumeButton, m_vt2VolumeButtonPosition - new Vector2(m_ttVolumeButton.Width / 2, m_ttVolumeButton.Height / 2 * 0.85f), Color.Goldenrod);
            }

        }

        public void Update(MouseState OldMouseState,
    KeyboardState oldKeyboardState)
        {
            MouseState ms = Mouse.GetState();

            if (bIsEnable)
            {
                if (m_vt2VolumePosition.X < ms.X && ms.X < m_vt2VolumePosition.X + iWidth)
                {
                    if (m_vt2VolumePosition.Y < ms.Y && ms.Y < m_vt2VolumePosition.Y + iHeight)
                    {
                        //kiểm tra có nhấn hay ko
                        if (OldMouseState.LeftButton == ButtonState.Pressed &&
                            ms.LeftButton == ButtonState.Released)
                        {
                            int iDeltaX = (int)((float)(ms.X - m_vt2VolumePosition.X) / 100f * 10);

                            fVolume = (float)iDeltaX / 10f;

                        }
                    }
                }
            }

            m_vt2VolumeButtonPosition = m_vt2VolumePosition + new Vector2(10, 0) * (int)(fVolume * 10);
        }
    }
}
