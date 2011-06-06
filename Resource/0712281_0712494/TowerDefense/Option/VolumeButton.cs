using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TowerDefense.Option
{
    public class VolumeButton: CustomControl
    {
        static Texture2D m_ttVolume;
        static Texture2D m_ttVolumeButton;

        Vector2 m_vt2VolumeButtonPosition;

        public float fVolume = 1;

        public bool bIsEnable = true;

        public TowerDefense.Option.RadioButton.OptionRadioState radioState;

        public VolumeButton()
        {
            //LoadResource();
            Width = 109;
            Height = 30;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (bIsEnable)
            {
                spriteBatch.Draw(m_ttVolume, Position, Color.White);
                spriteBatch.Draw(m_ttVolumeButton, m_vt2VolumeButtonPosition - new Vector2(m_ttVolumeButton.Width / 2, m_ttVolumeButton.Height / 2 * 0.85f), Color.White);
            }
            else
            {
                spriteBatch.Draw(m_ttVolume, Position, Color.Goldenrod);
                spriteBatch.Draw(m_ttVolumeButton, m_vt2VolumeButtonPosition - new Vector2(m_ttVolumeButton.Width / 2, m_ttVolumeButton.Height / 2 * 0.85f), Color.Goldenrod);
            }

        }

        public override void Update(MouseState OldMouseState,
    KeyboardState oldKeyboardState)
        {
            MouseState ms = Mouse.GetState();

            if (bIsEnable)
            {
                if (Position.X < ms.X && ms.X < Position.X + Width)
                {
                    if (Position.Y < ms.Y && ms.Y < Position.Y + Height)
                    {
                        //kiểm tra có nhấn hay ko
                        if (OldMouseState.LeftButton == ButtonState.Pressed &&
                            ms.LeftButton == ButtonState.Released)
                        {
                            int iDeltaX = (int)((float)(ms.X - Position.X) / 100f * 10);

                            fVolume = (float)iDeltaX / 10f;

                        }
                    }
                }
            }

            m_vt2VolumeButtonPosition = Position + new Vector2(10, 0) * (int)(fVolume * 10);
        }

        public override void LoadResource(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            m_ttVolume = GlobalVar.glContentManager.Load<Texture2D>("Options\\volume");
            m_ttVolumeButton = GlobalVar.glContentManager.Load<Texture2D>("Options\\volumebutton");
        }
    }
}
