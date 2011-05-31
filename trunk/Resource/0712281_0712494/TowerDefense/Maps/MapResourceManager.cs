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
    public class MapResourceManager
    {
        //int _iStartIndex = 0;

        //public Texture2D[] _rsTexture2Ds;
        public List<Texture2D> _rsTexture2Ds;

        public List<Point> _arrIndexStart;//luu thong tin dung de clone cua tung loai doi tuong

        public void LoadContent(ContentManager Content)
        {
            // TODO: use this.Content to load your game content here
            _rsTexture2Ds = new List<Texture2D>();
            _arrIndexStart = new List<Point>();

            int iIndex = 0;
            int iNum = 0;
            iNum = LoadAllFileInFolder(Content, @"Maps\Map\NotMove");
            _arrIndexStart.Add(new Point(iIndex, iNum));

            iIndex += iNum;
            iNum = LoadAllFileInFolder(Content, @"Maps\Map\Move");
            _arrIndexStart.Add(new Point(iIndex, iNum));

            iIndex += iNum;
            iNum = LoadAllFileInFolder(Content, @"Maps\Map\Object");
            _arrIndexStart.Add(new Point(iIndex, iNum));
            
            //LoadAllFileInFolder(Content, @"Maps\Map\Desert");
            //LoadAllFileInFolder(Content, @"Maps\Map\Grass");

            //LoadAllFileInFolder(Content, @"Maps\Map\MoreDesertLessGrass");
            //LoadAllFileInFolder(Content, @"Maps\Map\LessDesertMoreGrass");
            //LoadAllFileInFolder(Content, @"Maps\Map\DesertEqualGrass");

            //LoadAllFileInFolder(Content, @"Maps\Map\Road\BrickLeftToRight");
            //LoadAllFileInFolder(Content, @"Maps\Map\Road\BrickRightToLeft");

            //LoadAllFileInFolder(Content, @"Maps\Map\Object");
        }

        int LoadAllFileInFolder(ContentManager Content, string strPath)
        {
            string[] movingSprites = System.IO.Directory.GetFiles(@"Content\" + strPath);

            for (int i = 0; i < movingSprites.Length; i++)
            {
                string strNewPathFile = movingSprites[i].Substring(8, movingSprites[i].Length - 4 - 8);
                _rsTexture2Ds.Add(Content.Load<Texture2D>(strNewPathFile));
                //_iStartIndex++;
            }

            return movingSprites.Length;
        }

        public void Draw(SpriteBatch spriteBatch, int iTextureIndex, Vector2 vt2Position, Vector2 vt2Center, float _fDepth,
            float fScale)
        {
            //Vector2 vt2Center = new Vector2(_rsTexture2Ds[iTextureIndex].Width / 2,
            //    _rsTexture2Ds[iTextureIndex].Height / 2);

            spriteBatch.Draw(_rsTexture2Ds[iTextureIndex],
                vt2Position,
                null,
                Color.White,
                0.0f,
                vt2Center,
                fScale,
                SpriteEffects.None,
                _fDepth);
            
            //spriteBatch.Draw(_rsTexture2Ds[iTextureIndex],
            //    vt2Position,
            //    Color.White);
        }
    }
}
