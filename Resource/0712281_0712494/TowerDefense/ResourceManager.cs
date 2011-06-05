using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using System.Text;

namespace TowerDefense
{
    public class ResourceManager
    {
        //public static Texture2D[] _rsTexture2Ds;
        //public static Texture2D[] _rsMapTiles;
        public static SpriteFont[] _rsFonts;

        public static Texture2D[] _rsCreepSprites;
        public static int nCreepSprites = 0;

        public static Texture2D[] _rsTowerSprites;
        public static int nTowerSprites = 0;

        public static void Draw(SpriteBatch spriteBatch,  Texture2D texture, Vector2 vt2Center, Vector2 vt2Position, float _fScale, float _fDepth)
        {
            spriteBatch.Draw(texture, vt2Position - GlobalVar.glRootCoordinate, null, Color.White, 0.0f, vt2Center, _fScale, SpriteEffects.None, _fDepth);
            //spriteBatch.Draw(texture, vt2Position - GlobalVar.glRootCoordinate, null, Color.White, 0.0f, vt2Center, _fScale, SpriteEffects.None, vt2Position.Y / GlobalVar.glViewport.Y);
        }

        public static void DrawIcon(SpriteBatch spriteBatch, Texture2D texture, Vector2 vt2Position, float _fDepth)
        {
            spriteBatch.Draw(texture, new Rectangle((int)(vt2Position.X - GlobalVar.glIconSize.X/2), (int)(vt2Position.Y - GlobalVar.glIconSize.Y/2), (int)GlobalVar.glIconSize.X, (int)GlobalVar.glIconSize.Y), null, Color.White);
        }

        public static void DrawModel(SpriteBatch spriteBatch, Texture2D texture, Vector2 vt2Center, Vector2 vt2Position, float _fScale, float _fDepth)
        {
            Color color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
            spriteBatch.Draw(texture, vt2Position, null, color, 0.0f, vt2Center, _fScale, SpriteEffects.None, _fDepth);
        }

        //public static void LoadMapTiles()
        //{
        //}
    }
}
