using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TowerDefense
{
    public abstract class ActiveUnit : SpriteUnit
    {
        protected Effect _effect;         // hieu ung khi xuat hien, tan cong ...

        protected ActiveUnit()
        {
        }

        //protected ActiveUnit(Vector2 vt2Position, float fDepth, int iFirstSprite, int iSprite, int nSprite, int nIntervalTime, Effect effect)
        //    : base(vt2Position, fDepth, iFirstSprite, iSprite, nSprite, nIntervalTime)
        //{
        //    _effect = effect;
        //}
    }
}
