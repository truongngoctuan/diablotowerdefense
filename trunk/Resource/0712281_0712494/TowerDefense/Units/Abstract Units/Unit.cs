using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TowerDefense
{
    public abstract class Unit
    {
        protected Vector2 _vt2Position;
        protected int _iHeight;
        public int IHeight
        {
            get { return _iHeight; }
            set { _iHeight = value; }
        }
        protected int _iWidth;
        public int IWidth
        {
            get { return _iWidth; }
            set { _iWidth = value; }
        }
        protected float _fScale = 1.0f;
        public float FScale
        {
            get { return _fScale; }
            set { _fScale = value; }
        }

        public Vector2 Position
        {
            get { return _vt2Position; }
        }
        protected float _fDepth;

        //protected Unit(Vector2 vt2Position, float fDepth)
        //{
        //    _vt2Position = vt2Position;
        //    _fDepth = fDepth;
        //}

        protected Unit()
        {
        }

        public abstract void Draw(SpriteBatch spriteBatch);
        public abstract void Update(GameTime gameTime, KeyboardState keyboardState, MouseState mouseState);
        public abstract Unit Clone(Vector2 vt2Position);
        public abstract void LoadResource();
    }
}
