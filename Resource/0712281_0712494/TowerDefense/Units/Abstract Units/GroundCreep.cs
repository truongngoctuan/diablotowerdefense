using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TowerDefense
{
    public abstract class GroundCreep : Creep
    {
        protected string strDescriptionFile;
        protected int nMovingSpritesPerDirection;
        protected int nAttackedSpritesPerDirection;
        protected int nDyingSpritesPerDirection;

        protected string strMovingResourceFolder;
        protected string strAttackedResourceFolder;
        protected string strDyingResourceFolder;

        //protected int iIntervalTime;
        //protected int iBaseMaxLife;
        //protected int iBaseMaxEnergy;
        //protected int iBaseSpeed;
        //protected int iBaseDefense;

        protected int iHeight;
        protected int iWidth;

        protected int nMovingSpriteOffset;
        protected int nAttackedSpriteOffset;
        protected int nDyingSpriteOffset;

        protected int iBaseSprite;
 
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (_state == State.Died)
            {
                return;
            }

            Texture2D[] imgSprites = ResourceManager._rsCreepSprites;
            int iSprite = _iFirstSprite + _iSprite;

            //Vector2 vt2NewPosition = _vt2Position 
            //    - _vt2BoundSprite[(int)_state] / 2
            //    + _vt2OffsetSprite[(int)_state][iSpriteNonOffset];

            //ResourceManager.Draw(spriteBatch,
            //    imgSprites[iSprite],
            //    new Vector2(0, 0),
            //    vt2NewPosition, 1.0f,
            //    _fDepth);

            ResourceManager.Draw(spriteBatch,
                imgSprites[iSprite],
                new Vector2(0, 0),
                _vt2CurrentPositionTopLeftOfFrame, _fScale,
                _fDepth);
        }


        #region BonusOffset
        //tạo các biến lưu trư offset tương ứng cho từng frame
        //hàm đọc file offset và lưu biến
        //cập nhật lại hàm Draw
        protected List<Vector2[]> _vt2OffsetSprite = null;
        protected List<Vector2> _vt2BoundSprite = null;//khung chứa sprite, width height
        protected string _strOffsetFilename = "MonsOffset.ini";

        protected int iSpriteNonOffset;
        //làm 2 chuyện:
        //cập nhật non offset và cập nhật top left position frame, iwdth, height
        protected void ChangeSpriteNonOffset()
        {
            if (_state == State.Died)
            {
                return;
            }

            int iSprite = _iFirstSprite + _iSprite;
            iSpriteNonOffset = iSprite - iBaseSprite;
            switch (_state)
            {
                case State.Moving:
                    {
                        iSpriteNonOffset -= nMovingSpriteOffset;
                        break;
                    }
                case State.Attacked:
                    {
                        iSpriteNonOffset -= nAttackedSpriteOffset;
                        break;
                    }
                case State.Dying:
                    {
                        iSpriteNonOffset -= nDyingSpriteOffset;
                        break;
                    }
            }

            if (_vt2BoundSprite != null &&
                _vt2OffsetSprite != null)
            {
                _vt2CurrentPositionTopLeftOfFrame = _vt2Position
                    + (-_vt2BoundSprite[(int)_state] / 2
                    + _vt2OffsetSprite[(int)_state][iSpriteNonOffset]) * _fScale;

                Texture2D[] imgSprites = ResourceManager._rsCreepSprites;

                _iWidth = (int)(imgSprites[iSprite].Width * _fScale);
                _iHeight = (int)(imgSprites[iSprite].Height * _fScale);
            }
        }
        #endregion
    }
}
