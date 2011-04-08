using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using TowerDefense.Units.Real_Units;

namespace TowerDefense
{
    public abstract class Tower : MainUnit
    {
        protected int _nCost;
        protected int _iBaseDamage;
        protected int _iDamage;

        protected int _iBaseRange;
        protected int _iRange;

        protected int _iBaseAttackSpeed;
        protected int _iAttackSpeed;

        protected int _iTimeBetweenBulletUpdates;
        protected int _iTimeTillNextBulletUpdate;

        protected enum DamageType { Normal, Pierce, AOE, Elemental };
        protected DamageType _damageType;

        protected List<Bullet> bullets = new List<Bullet>();
        protected int _iTimeTillNextShot;

        protected static DamageType Str2DamageType(string strDamageType)
        {
            switch (strDamageType)
            {
                case "Normal":
                    {
                        return DamageType.Normal;
                    }
                case "Pierce":
                    {
                        return DamageType.Pierce;
                    }
                case "AOE":
                    {
                        return DamageType.AOE;
                    }
                case "Elemental":
                    {
                        return DamageType.Elemental;
                    }
                default:
                    {
                        return DamageType.Normal;
                    }
            }
        }

        public abstract void TargetCreep(Creep creep);
        public abstract void Hit(Creep creep);

        public virtual void DrawIcon(SpriteBatch spriteBatch)
        {
            Texture2D[] imgSprites = ResourceManager._rsTowerSprites;
            int iSprite = this._iFirstSprite;
            ResourceManager.DrawIcon(spriteBatch, imgSprites[iSprite], _vt2Position, _fDepth);
        }
        public virtual void DrawModel(SpriteBatch spriteBatch)
        {
            Texture2D[] imgSprites = ResourceManager._rsTowerSprites;
            int iSprite = this._iFirstSprite;
            Vector2 vt2Position = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);

            ResourceManager.DrawModel(spriteBatch, imgSprites[iSprite], new Vector2(imgSprites[iSprite].Width / 2, imgSprites[iSprite].Height / 2), vt2Position, _fScale, _fDepth);
        }


        MouseState oldMouseState;
        bool _bPressed = false;
        protected bool InBoundCheck(Vector2 vt2Position)
        {
            bool bInBound = false;
            if (_vt2Position.X - _iWidth / 2 < vt2Position.X && vt2Position.X < _vt2Position.X + _iWidth / 2)
            {
                if (_vt2Position.Y - _iHeight / 2 < vt2Position.Y && vt2Position.Y < _vt2Position.Y + _iHeight / 2)
                {
                    bInBound = true;
                }
            }
            return bInBound;
        }

        public void CheckSelected(MouseState mouseState)
        {            
            if (_bPressed == true)
            {
                if (oldMouseState.LeftButton == ButtonState.Pressed && mouseState.LeftButton == ButtonState.Released)
                {
                    if (InBoundCheck(new Vector2(mouseState.X, mouseState.Y)))
                    {
                        if (_bSelected == true)
                            _bSelected = false;
                        else
                            _bSelected = true;
                    }
                    else
                        _bPressed = false;
                }
            }

            if (oldMouseState.LeftButton == ButtonState.Released && mouseState.LeftButton == ButtonState.Pressed)
            {
                if (InBoundCheck(new Vector2(mouseState.X, mouseState.Y)))
                    _bPressed = true;
                else
                {
                    if(_bSelected == true)
                        GlobalVar.glUnitManager.AddSelectedTower(mouseState);
                    _bSelected = false;
                }
            }
            oldMouseState = mouseState;
        }

        protected float GetDistance(Vector2 targetPosition)
        {
            Vector2 delta = targetPosition - _vt2Position;
            return (float)Math.Sqrt(delta.X * delta.X + 4 * delta.Y * delta.Y);
        }        
    }
}
