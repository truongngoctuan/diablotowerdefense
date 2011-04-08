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
        int _nSprite;
        Vector2 _vt2Position;
        int _iSprite;
        float _fDepth;
        int m_iIDName;

        public BackgroundMapUnit(Vector2 vt2Position,
            int iIDName):this(vt2Position,
             iIDName,
            false)
        {
        }
        public BackgroundMapUnit(Vector2 vt2Position, 
            int iIDName,
            bool bClone)
        {
            //this._imgSprites = ResourceManager._rsTexture2Ds;
            this._vt2Position = vt2Position;
            this._fDepth = 1.0f;

            //m_iIDName = iIDName;

            switch (iIDName)
            {
                case ((int)BackgroundMapUnitName.Desert):
                    {
                        this._nSprite = 9;
                        break;
                    }
                case ((int)BackgroundMapUnitName.Grass):
                    {
                        this._nSprite = 9;
                        break;
                    }
                case ((int)BackgroundMapUnitName.MoreDesertLessGrass):
                    {
                        this._nSprite = 4;
                        break;
                    }
                case ((int)BackgroundMapUnitName.LessDesertMoreGrass):
                    {
                        this._nSprite = 4;
                        break;
                    }
                case ((int)BackgroundMapUnitName.DesertEqualGrass):
                    {
                        this._nSprite = 4;
                        break;
                    }
                case ((int)BackgroundMapUnitName.BrickLeftToRight):
                    {
                        this._nSprite = 3;
                        break;
                    }
                case ((int)BackgroundMapUnitName.BrickRightToLeft):
                    {
                        this._nSprite = 3;
                        break;
                    }
                case ((int)BackgroundMapUnitName.Object):
                    {
                        this._nSprite = 8;
                        break;
                    }

            }

            if (bClone)
            {
                _iSprite = iIDName;
            }
            else
            {
                m_iIDName = iIDName;

                switch (iIDName)
                {
                    case ((int)BackgroundMapUnitName.Desert):
                        {
                            this._iSprite = 0;
                            break;
                        }
                    case ((int)BackgroundMapUnitName.Grass):
                        {
                            this._iSprite = 9;
                            break;
                        }
                    case ((int)BackgroundMapUnitName.MoreDesertLessGrass):
                        {
                            this._iSprite = 18;
                            break;
                        }
                    case ((int)BackgroundMapUnitName.LessDesertMoreGrass):
                        {
                            this._iSprite = 22;
                            break;
                        }
                    case ((int)BackgroundMapUnitName.DesertEqualGrass):
                        {
                            this._iSprite = 26;
                            break;
                        }
                    case ((int)BackgroundMapUnitName.BrickLeftToRight):
                        {
                            this._iSprite = 30;
                            break;
                        }
                    case ((int)BackgroundMapUnitName.BrickRightToLeft):
                        {
                            this._iSprite = 33;
                            break;
                        }
                    case ((int)BackgroundMapUnitName.Object):
                        {
                            this._iSprite = 36;
                            break;
                        }
                }
            }
        }

        public void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch,
            MapResourceManager mrm, 
            Vector2 v2CurrentRootCoordinate,
            float fScale)
        {
            List<Texture2D> imgSprites = mrm._rsTexture2Ds;

            //if ((int)BackgroundMapUnitName.Object == m_iIDName)
            //{
            //    mrm.Draw(spriteBatch, _iSprite,
            //        new Vector2(_vt2Position.X - v2CurrentRootCoordinate.X,
            //            _vt2Position.Y - v2CurrentRootCoordinate.Y),
            //        new Vector2(0,
            //            imgSprites[_iSprite].Height / 2),
            //        _fDepth,
            //        fScale);
            //    return;
            //}            

            //mrm.Draw(spriteBatch, _iSprite, _vt2Position, _fDepth);
            mrm.Draw(spriteBatch, _iSprite, new Vector2(_vt2Position.X - v2CurrentRootCoordinate.X,
                _vt2Position.Y - v2CurrentRootCoordinate.Y),
                new Vector2(),
                _fDepth,
                fScale);
        }

        public BackgroundMapUnit Clone(Vector2 vtPosition, int iIDName,
            MapResourceManager mrm)
        {
            //random _isprite ở đây trước khi clone

            //nếu là object thì phảirebuilt lại vị trí vì các kích thước không giống nhau
            

            int iSprite = _iSprite + GlobalVar.glRandom.Next(_nSprite);

            if ((int)BackgroundMapUnitName.Object == m_iIDName)
            {
                //vtPosition = new Vector2(vtPosition.X - (mrm._rsTexture2Ds[iSprite].Width / 2 - mrm._rsTexture2Ds[0].Width / 2 * 0.75f),
                //    vtPosition.Y - (mrm._rsTexture2Ds[iSprite].Height - mrm._rsTexture2Ds[0].Height * 0.75f));
                //thay vì làm như trên thì ta đứa cái có slace ra ngoài trừ trước rồi mới đứa vô
                vtPosition = new Vector2(vtPosition.X - mrm._rsTexture2Ds[iSprite].Width / 2,
                    vtPosition.Y - mrm._rsTexture2Ds[iSprite].Height);
            }

            return new BackgroundMapUnit(vtPosition, iSprite, true);
        }
    }
}
