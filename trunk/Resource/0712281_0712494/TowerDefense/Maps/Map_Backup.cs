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

using System.IO;

namespace TowerDefense
{
    public class Map
    {
        string _strMapFile;

        //Texture2D[] m_PrototypeCells;// bỏ, lấy trực tiếp từ resource manager
        Texture2D[,] m_MapCells;
        Vector2 m_Size;//size của MapCells

        Vector2 m_CellSize;
        Vector2 m_CurrentRootCoordinate;
        Vector2 m_OldCursor;

        public Map(string strMapFile)
        {
            _strMapFile = strMapFile;

            m_CurrentRootCoordinate = new Vector2(0, 0);
            MouseState mouseState = Mouse.GetState();
            m_OldCursor = new Vector2(mouseState.X, mouseState.Y);
        }
        public void LoadMap(ContentManager theContentManager)
        {
            LoadContent(theContentManager);
            ReadMap(_strMapFile);
        }

        //=============================================================
        //dùng để load tất cả resource liên quan đến map
        //List<Texture2D> m_Texture2Ds;       
        Texture2D[] m_Texture2Ds;
        SpriteFont[] m_Fonts;

        public void LoadContent(ContentManager theContentManager)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(@"Content\Maps\Tiles");
            if (!dirInfo.Exists)
            {
                return;
            }
            string[] tiles = System.IO.Directory.GetFiles(@"Content\Maps\Tiles");

            m_Texture2Ds = new Texture2D[tiles.Length];

            for (int i = 0; i < tiles.Length; i++)
            {
                string[] strLink = tiles[i].Split(new char[] { '.', '\\' });
                string strLinkResult = string.Empty;
                for (int j = 1; j < strLink.Length - 2; j++)
                {
                    strLinkResult += (strLink[j] + "\\");
                }
                strLinkResult += strLink[strLink.Length - 2];
                m_Texture2Ds[i] = theContentManager.Load<Texture2D>(strLinkResult);
            }
        }

        public Texture2D[] GetTiles()
        {
            return m_Texture2Ds;
        }

        //===============================================
        //bổ sung
        public Texture2D GetTiles(int iIndex)
        {
            int iOffset = 0;//có thể thay đổi nếu trước danh sách tile có 
            //các texture khác

            return m_Texture2Ds[iOffset + iIndex];
        }
        //===============================================

        public void ReadMap(string filename)
        {
            //=============================================================
            //reference
            //http://www.devhood.com/tutorials/tutorial_details.aspx?tutorial_id=400
            //http://bytes.com/topic/c-sharp/answers/268853-reading-numbers-text-file
            //bổ sung đọc file
            FileStream fStream;

            fStream = new FileStream(filename,
                FileMode.Open,
                FileAccess.Read);

            StreamReader sr = new StreamReader(fStream);

            //doc tung dong
            string strBufferLine = string.Empty;

            strBufferLine = sr.ReadLine();
            //int iSpaceIndex = strBufferLine.IndexOf(' ');
            //m_Size = new Vector2(int.Parse(strBufferLine.Substring(0, iSpaceIndex)),
            //    int.Parse(strBufferLine.Substring(iSpaceIndex + 1, strBufferLine.Length - (iSpaceIndex - 1))));
            string[] strSize = strBufferLine.Split(' ');
            m_Size = new Vector2(Convert.ToInt32(strSize[0], 10),
                Convert.ToInt32(strSize[1], 10));

            //m_CellSize = new Vector2(m_PrototypeCells[0].Width / 2, m_PrototypeCells[0].Height / 2);
            m_CellSize = new Vector2(GetTiles(0).Width,
                GetTiles(0).Height);

            m_MapCells = new Texture2D[(int)m_Size.X, (int)m_Size.Y];
            for (int i = 0; i < m_Size.X; i++)
            {
                strBufferLine = sr.ReadLine();
                string[] strMapCellsFromFile = strBufferLine.Split(' ');
                for (int j = 0; j < m_Size.Y; j++)
                {
                    int iIndexCellOfPrototypeCells = Convert.ToInt32(strMapCellsFromFile[j]);
                    //m_MapCells[i, j] = m_PrototypeCells[iIndexCellOfPrototypeCells];
                    m_MapCells[i, j] = GetTiles(iIndexCellOfPrototypeCells);
                }
            }

            sr.Close();
            fStream.Close();
            //=============================================================
        }

        public void Draw(SpriteBatch theSpriteBatch)
        {
            //dek hỉu mài vít gì hết =.=!

            //row1, col1 là cột/dòng biên bên trái
            //row2, col2 là giới hạn bên phải
            int row1, col1, row2, col2;
            row1 = (int)(m_CurrentRootCoordinate.Y / m_CellSize.Y);
            col1 = (int)(m_CurrentRootCoordinate.X / m_CellSize.X);
            row2 = (int)((m_CurrentRootCoordinate.Y + GlobalVar.glViewport.Y) / m_CellSize.Y);
            col2 = (int)((m_CurrentRootCoordinate.X + GlobalVar.glViewport.X) / m_CellSize.X);

            //row1 = row1 >= 0 ? row1 : 0;
            //col1 = col1 >= 0 ? col1 : 0;
            row2 = row2 < m_Size.X ? row2 : (int)(m_Size.X - 1);
            col2 = col2 < m_Size.Y ? col2 : (int)(m_Size.Y - 1);

            for (int row = 0; row < m_Size.Y; row++)
            {
                for (int col = 0; col < m_Size.X; col++)

            //for (int row = row1; row <= row2; row++)
            //{
            //    for (int col = col1; col <= col2; col++)
                {
                    //theSpriteBatch.Draw(m_MapCells[row, col],
                    //    new Rectangle((int)(col * m_CellSize.X - m_CurrentRootCoordinate.X),
                    //                    (int)(row * m_CellSize.Y - m_CurrentRootCoordinate.Y),
                    //                    (int)m_CellSize.X, (int)m_CellSize.Y), Color.White);

                    theSpriteBatch.Draw(m_MapCells[row, col],
                        new Rectangle((int)(m_Size.X * m_CellSize.X / 2.0f - m_CellSize.X / 2 + col * m_CellSize.X / 2 - row * m_CellSize.X / 2 - m_CurrentRootCoordinate.X),
                                        (int)(col * m_CellSize.Y / 2 + row * m_CellSize.Y / 2 - m_CurrentRootCoordinate.Y),
                                        (int)m_CellSize.X, (int)m_CellSize.Y), Color.White);
                }
            }
        }

        //biến xử lý vụ click chuột
        bool m_bIsLeftButtonDown = false;
        public void Update(GameTime theGameTime)
        {
            //bổ sung
            if (!m_bIsLeftButtonDown)
            {
                MouseState ms = Mouse.GetState();
                if (ms.LeftButton == ButtonState.Pressed)
                {
                    m_bIsLeftButtonDown = true;
                    m_OldCursor = new Vector2(ms.X, ms.Y);
                }
            }
            else
            {
                MouseState ms = Mouse.GetState();
                if (ms.LeftButton == ButtonState.Released)
                {
                    m_bIsLeftButtonDown = false;
                }
            }
            //=================

            if (m_bIsLeftButtonDown)
            {
                MouseState mouseState = Mouse.GetState();
                Vector2 curCursor = new Vector2();
                curCursor.X = mouseState.X;
                curCursor.Y = mouseState.Y;

                m_CurrentRootCoordinate += (curCursor - m_OldCursor);

                //bổ sung
                if (m_CurrentRootCoordinate.X < 0)
                {
                    m_CurrentRootCoordinate.X = 0;
                }

                if (m_CurrentRootCoordinate.Y < 0)
                {
                    m_CurrentRootCoordinate.Y = 0;
                }

                //new Rectangle((int)(m_Size.X * m_CellSize.X / 2.0f 
                //- m_CellSize.X / 2 + col * m_CellSize.X / 2
                //- row * m_CellSize.X / 2 - m_CurrentRootCoordinate.X),
                //                        (int)(col * m_CellSize.Y / 2 + row * m_CellSize.Y / 2 - m_CurrentRootCoordinate.Y)

                if (m_CurrentRootCoordinate.X + GlobalVar.glViewport.X > m_Size.X * m_CellSize.X / 2.0f 
                                                                        - m_CellSize.X / 2
                                                                        + m_Size.X * m_CellSize.X / 2
                                                                        - 0 * m_CellSize.X / 2
                                                                        + m_CellSize.X / 2)
                {
                    m_CurrentRootCoordinate.X = m_Size.X * m_CellSize.X / 2.0f
                                                - m_CellSize.X / 2
                                                + m_Size.X * m_CellSize.X / 2
                                                - 0 * m_CellSize.X / 2
                                                + m_CellSize.X / 2
                                                - GlobalVar.glViewport.X;
                }

                if (m_CurrentRootCoordinate.Y + GlobalVar.glViewport.Y > m_Size.X * m_CellSize.Y / 2
                                                                        + m_Size.Y * m_CellSize.Y / 2)
                {
                    m_CurrentRootCoordinate.Y = m_Size.X * m_CellSize.Y / 2
                                                + m_Size.Y * m_CellSize.Y / 2
                                                - GlobalVar.glViewport.Y;
                }

                //if (m_CurrentRootCoordinate.X + GlobalVar.gl_Viewport.X > m_Size.X * m_CellSize.X)
                //{
                //    m_CurrentRootCoordinate.X = m_Size.X * m_CellSize.X - GlobalVar.gl_Viewport.X;
                //}

                //if (m_CurrentRootCoordinate.Y + GlobalVar.gl_Viewport.Y> m_Size.Y * m_CellSize.Y)
                //{
                //    m_CurrentRootCoordinate.Y = m_Size.Y * m_CellSize.Y - GlobalVar.gl_Viewport.Y;
                //}

                //=========

                m_OldCursor = curCursor;
            }
        }
 
    }
}
