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
    public abstract class CustomControl:AbstractSubject
    {
        protected Vector2 _vt2Position;

        public Vector2 Position
        {
            get { return _vt2Position; }
            set { _vt2Position = value; }
        }

        private int _iWidth;

        public int Width
        {
            get { return _iWidth; }
            set { _iWidth = value; }
        }
        private int _iHeight;

        public int Height
        {
            get { return _iHeight; }
            set { _iHeight = value; }
        }

        public abstract void Update(MouseState OldMouseState, KeyboardState oldKeyboardState);
        public abstract void Draw(SpriteBatch spriteBatch);
        public abstract void LoadResource(ContentManager content);
    }
}
