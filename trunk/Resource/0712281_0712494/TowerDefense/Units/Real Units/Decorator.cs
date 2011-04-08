using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace TowerDefense
{
    public class Decorator : InactiveUnit
    {
        static int iFirstSprite = 1;
        static int nSprite = 4;
        static int nIntervalTime = 1000;
        static float fDepth = 1.0f;

        public Decorator(Vector2 vt2Position, int iSprite)
        {
            _iFirstSprite = iFirstSprite;
            _nSprite = nSprite;
            _iBaseIntervalTime = nIntervalTime;
            _fDepth = fDepth;

            _vt2Position = vt2Position;
            _iSprite = iSprite;         
        }
        //public Decorator(Vector2 vt2Position, int iSprite, int nSprite)
        //    : base(vt2Position, fDepth, iFirstSprite, iSprite, nSprite, nIntervalTime)
        //{
        //}

        public override void Draw(SpriteBatch spriteBatch)
        {
        }

        public override void Update(GameTime gameTime, KeyboardState keyboardState, MouseState mouseState)
        {
        }

        public override Unit Clone(Vector2 vt2Position)
        {
            return new Decorator(Position, _iSprite);
        }

        public override void LoadResource()
        {
            //throw new NotImplementedException();
        }
    }
}
