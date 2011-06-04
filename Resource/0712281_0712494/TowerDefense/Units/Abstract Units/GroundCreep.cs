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
    }
}
