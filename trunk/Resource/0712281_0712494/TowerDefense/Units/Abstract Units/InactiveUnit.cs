using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TowerDefense
{
    public abstract class InactiveUnit : SpriteUnit
    {
        protected InactiveUnit()
        {
        }

        //protected InactiveUnit(Vector2 vt2Position, float fDepth, int iFirstSprite, int iSprite, int nSprite, int nIntervalTime)
        //    : base(vt2Position, fDepth, iFirstSprite, iSprite, nSprite, nIntervalTime)
        //{
        //}
    }
}
