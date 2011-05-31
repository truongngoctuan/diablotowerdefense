using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TowerDefense.Maps
{
    public class ObjectMapUnit : BackgroundMapUnit
    {
        BackgroundMapUnit _Object = null;

        public ObjectMapUnit(Vector2 vt2Position, 
            int iSprite,
            bool bClone):base(vt2Position, iSprite, bClone)
        {
        }

        public static BackgroundMapUnit Clone(Vector2 vt2Position, int iIDName, MapResourceManager mrm)
        {
            //return base.Clone(vtPosition, iIDName, mrm);
            //random _isprite trước khi clone
            int iSprite = mrm._arrIndexStart[0].X + GlobalVar.glRandom.Next(mrm._arrIndexStart[0].Y);
            //w/h background
            int bgw = mrm._rsTexture2Ds[iSprite].Width;
            int bgh = mrm._rsTexture2Ds[iSprite].Height; 

            ObjectMapUnit obj = new ObjectMapUnit(vt2Position, iSprite, true);

            iSprite = mrm._arrIndexStart[iIDName].X + GlobalVar.glRandom.Next(mrm._arrIndexStart[iIDName].Y);
            //tinh lai position phu hop

            //w/h object
            int w = mrm._rsTexture2Ds[iSprite].Width;
            int h = mrm._rsTexture2Ds[iSprite].Height;

            vt2Position = new Vector2(vt2Position.X + (float)bgw / 2.0f - (float) w / 2, 
                vt2Position.Y + (float)bgh / 2.0f - (float)h);
            obj._Object = new BackgroundMapUnit(vt2Position, iSprite, true);

            return obj;
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, MapResourceManager mrm, Microsoft.Xna.Framework.Vector2 v2CurrentRootCoordinate, float fScale)
        {
            base.Draw(spriteBatch, mrm, v2CurrentRootCoordinate, fScale);
            _Object.Draw(spriteBatch, mrm, v2CurrentRootCoordinate, fScale);
        }
    }
}
