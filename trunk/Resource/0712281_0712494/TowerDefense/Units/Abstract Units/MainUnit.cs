using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TowerDefense
{
    public abstract class MainUnit : ActiveUnit
    {
        protected int _iMaxLife;
        protected int _iMaxEnergy;
        protected bool _bSelected;
        public bool BSelected
        {
            get { return _bSelected; }
            set { _bSelected = value; }
        }

        protected int _iLife;
        public int ILife
        {
            get { return _iLife; }
            set { _iLife = value; }
        }
        protected int _iEnergy;
        public int IEnergy
        {
            get { return _iEnergy; }
            set { _iEnergy = value; }
        }

        //protected MainUnit(Vector2 vt2Position, float fDepth, int iFirstSprite, int iSprite, int nSprite, int nIntervalTime, Effect effect, int nBatchSize, int nDelay, int nBatchDelay)
        //    : base(vt2Position, fDepth, iFirstSprite, iSprite, nSprite, nIntervalTime, effect)
        //{
        //}
        
    }
}
