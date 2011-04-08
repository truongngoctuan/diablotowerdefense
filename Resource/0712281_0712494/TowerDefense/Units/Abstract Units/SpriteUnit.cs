using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TowerDefense
{
    public abstract class SpriteUnit : Unit
    {
        //protected Texture2D[] _imgSprites;
        protected int _iFirstSprite;
        protected int _iSprite;
        protected int _nSprite;
        protected int _iBaseIntervalTime;
        protected int _iTimeTillNextUpdate;

        protected SpriteUnit()
        {
        }

        //protected SpriteUnit(Vector2 vt2Position, float fDepth, int iFirstSprite, int iSprite, int nSprite, int nIntervalTime)
        //    : base(vt2Position, fDepth)
        //{
        //    _iFirstSprite = iFirstSprite;
        //    _iSprite = iSprite;
        //    _nSprite = nSprite;
        //    _nIntervalTime = nIntervalTime;
        //}
    }
}
