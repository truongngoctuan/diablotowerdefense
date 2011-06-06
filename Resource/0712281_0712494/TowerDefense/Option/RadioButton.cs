using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TowerDefense.Option
{
    public class RadioButton: CustomControl
    {
        public enum OptionRadioState { Normal, Checked };

        public Texture2D m_ttradio;
        public Texture2D m_ttradioChecked;
        public SpriteFont spFontFokard;

        private string _strText;

        public string Text
        {
            get { return _strText; }
            set { _strText = value; }
        }

        public OptionRadioState radioState;
        bool m_bIsRadioChangeState = false;

        public RadioButton()
        {
            //LoadResource();
            Height = 25;
        }

        public override void Update(Microsoft.Xna.Framework.Input.MouseState OldMouseState, Microsoft.Xna.Framework.Input.KeyboardState oldKeyboardState)
        {
            MouseState ms = Mouse.GetState();

            //kiểm tra cái radio button
            if (Position.X < ms.X && ms.X < Position.X + Width)
            {
                if (Position.Y < ms.Y && ms.Y < Position.Y + Height)
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

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {

            spriteBatch.DrawString(spFontFokard, _strText, Position + new Vector2(25, 0), Color.YellowGreen);
            switch (radioState)
            {
                case OptionRadioState.Normal:
                    {
                        spriteBatch.Draw(m_ttradio, Position, Color.White);
                        break;
                    }
                case OptionRadioState.Checked:
                    {
                        spriteBatch.Draw(m_ttradioChecked, Position, Color.White);
                        break;
                    }
            }
        }

        public override void LoadResource(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            m_ttradio = GlobalVar.glContentManager.Load<Texture2D>("Options\\Radio");
            m_ttradioChecked = GlobalVar.glContentManager.Load<Texture2D>("Options\\radio_checked");

            spFontFokard = GlobalVar.glContentManager.Load<SpriteFont>("Options\\Folkard");
        }
    }
}
