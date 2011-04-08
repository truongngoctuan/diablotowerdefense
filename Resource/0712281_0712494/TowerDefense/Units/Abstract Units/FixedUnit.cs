using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics; 

namespace TowerDefense
{
    public abstract class FixedUnit : Unit
    {
        int _iTexture;

        protected FixedUnit()
        {
        }

        //protected FixedUnit(Vector2 vt2Position, float fDepth, int iTexture)
        //    : base(vt2Position, fDepth)
        //{
        //    _iTexture = iTexture;
        //}
    }
}
