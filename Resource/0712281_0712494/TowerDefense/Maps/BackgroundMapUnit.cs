using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace TowerDefense
{
    public enum BackgroundMapUnitName
    {
        Desert,
        Grass,
        MoreDesertLessGrass,
        LessDesertMoreGrass,
        DesertEqualGrass,
        BrickLeftToRight,
        BrickRightToLeft,
        Object
    }
    public class BackgroundMapUnit
    {
        //số lượng sprite tương ứng trong cùng 1 loại map
        //d2ung cho việc ramcom khi clone, khi đó mỗi lần load map sẽ khác,
        //mặc dù cùng 1 map
        Vector2 _vt2Position;
        int _iSprite;
        float _fDepth;

        public BackgroundMapUnit(Vector2 vt2Position,
            int iSprite):this(vt2Position,
             iSprite,
            false)
        {
        }
        public BackgroundMapUnit(Vector2 vt2Position, 
            int iSprite,
            bool bClone)
        {
            //this._imgSprites = ResourceManager._rsTexture2Ds;
            this._vt2Position = vt2Position;
            this._fDepth = 1.0f;

            if (bClone)
            {
                _iSprite = iSprite;
            }
        }

        public virtual void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch,
            MapResourceManager mrm, 
            Vector2 v2CurrentRootCoordinate,
            float fScale)
        {
            mrm.Draw(spriteBatch, _iSprite, new Vector2(_vt2Position.X - v2CurrentRootCoordinate.X,
                _vt2Position.Y - v2CurrentRootCoordinate.Y),
                new Vector2(),
                _fDepth,
                fScale);
        }

        public static BackgroundMapUnit Clone(Vector2 vtPosition, int iIDName,
            MapResourceManager mrm)
        {
            //random _isprite trước khi clone
            int iSprite = mrm._arrIndexStart[iIDName].X + GlobalVar.glRandom.Next(mrm._arrIndexStart[iIDName].Y);

            return new BackgroundMapUnit(vtPosition, iSprite, true);
        }
    }
}
