using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace TowerDefense.Option
{
    public class ImageButton: CustomControl
    {
        public enum OptionButtonState { Normal, Hovered };
        OptionButtonState btOKState;

        Texture2D m_ttbtOK;
        Texture2D m_ttbtOK_Hovered;


        public override void LoadResource(ContentManager content)
        {
            m_ttbtOK = content.Load<Texture2D>("Options\\btOK");
            m_ttbtOK_Hovered = content.Load<Texture2D>("Options\\btOK_Hovered");

            Width = m_ttbtOK.Width;
            Height = m_ttbtOK.Height;
        }

        //public void LoadContent()
        //{
        //    //m_vt2btOkPosition = new Vector2(m_vt2OptionFromPosition.X + m_ttOptionFrom.Width / 2 - m_ttbtOK.Width / 2,
        //    //    m_vt2OptionFromPosition.Y + m_ttOptionFrom.Height * 0.9f - m_ttbtOK.Height);
        //}

        public override void Update(MouseState OldMouseState,
            KeyboardState oldKeyboardState)
        {
            MouseState ms = Mouse.GetState();

            btOKState = OptionButtonState.Normal;

            if (Position.X + m_ttbtOK.Width * 0.1f < ms.X && ms.X < Position.X + m_ttbtOK.Width * 0.9f)
            {
                if (Position.Y + m_ttbtOK.Height * 0.1f < ms.Y && ms.Y < Position.Y + m_ttbtOK.Height * 0.9f)
                {
                    btOKState = OptionButtonState.Hovered;

                    //kiểm tra có nhấn hay ko, nếu nhấn thì thoát
                    if (OldMouseState.LeftButton == ButtonState.Pressed &&
                        ms.LeftButton == ButtonState.Released)
                    {
                        //cập nhật các giá trị - ghi lại vào file, fullscreen -sound...
                        //if (m_bIsRadioChangeState == true)
                        {
                            GlobalVar.optionVariables.ToggleFullScreen();
                            //LoadContent();

                            GlobalVar.optionVariables.MuteSound();
                            GlobalVar.optionVariables.ChangeVolume();
                            //LockUnlockVolume();
                        }
                        GlobalVar.optionVariables.WriteToFile();
                    }
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            switch (btOKState)
            {
                case OptionButtonState.Normal:
                    {
                        spriteBatch.Draw(m_ttbtOK, Position, Color.White);
                        break;
                    }
                case OptionButtonState.Hovered:
                    {
                        spriteBatch.Draw(m_ttbtOK_Hovered, Position, Color.White);
                        break;
                    }
            }
        }
    }
}
