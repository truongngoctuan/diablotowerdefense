using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numeric;

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
using TowerDefense.Maps;

namespace TowerDefense
{
    public class Map
    {
        string _strMapFile;

        int[,] m_MapCellsUnFormat;
        BackgroundMapUnit[,] m_MapCells;
        Vector2 m_Size;//size của MapCells
        Vector2 m_CellSize;
        const float m_fScale = 1.0f;

        //mảng này lưu trữ ma trận đường đi (ma trận 2), 
        //quyet dinh quai di theo duong nao
        int[,] m_MapCellsRoadUnFormat;

        public int[,] MapCellsRoad
        {
            get { return m_MapCellsRoadUnFormat; }
            //set { m_MapCellsRoadUnFormat = value; }
        }
        //BackgroundMapUnit[,] m_MapCellsRoad;

        public Map(string strMapFile)
        {
            _strMapFile = strMapFile;
        }
        public void LoadMap(ContentManager theContentManager)
        {
            LoadContent(theContentManager);
            ReadMap(_strMapFile);
        }

        //=============================================================
        //dùng để load tất cả resource liên quan đến map
        //List<Texture2D> m_Texture2Ds;       
        MapResourceManager m_mapResMan = new MapResourceManager();

        public void LoadContent(ContentManager theContentManager)
        {
            m_mapResMan.LoadContent(theContentManager);
        }

        public List<Texture2D> GetTiles()
        {
            return m_mapResMan._rsTexture2Ds;
        }

        //===============================================
        //bổ sung
        public Texture2D GetTiles(int iIndex)
        {
            int iOffset = 0;//có thể thay đổi nếu trước danh sách tile có 
            //các texture khác

            return GetTiles()[iOffset + iIndex];
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

            //ma trận 1
            strBufferLine = sr.ReadLine();
            string[] strSize = strBufferLine.Split(' ');
            m_Size = new Vector2(Convert.ToInt32(strSize[0], 10),
                Convert.ToInt32(strSize[1], 10));

            m_CellSize = new Vector2(GetTiles(0).Width,
                GetTiles(0).Height);

            m_MapCellsUnFormat = new int[(int)m_Size.X, (int)m_Size.Y];
            for (int i = 0; i < m_Size.X; i++)
            {
                strBufferLine = sr.ReadLine();
                string[] strMapCellsFromFile = strBufferLine.Split(' ');
                for (int j = 0; j < m_Size.Y; j++)
                {
                    int iIndexCellOfPrototypeCells = Convert.ToInt32(strMapCellsFromFile[j]);

                    m_MapCellsUnFormat[i, j] = iIndexCellOfPrototypeCells;
                }
            }

            //ma trận 2
            strBufferLine = sr.ReadLine();
            m_MapCellsRoadUnFormat = new int[(int)m_Size.X, (int)m_Size.Y];

            for (int i = 0; i < m_Size.X; i++)
            {
                strBufferLine = sr.ReadLine();
                string[] strMapCellsFromFile = strBufferLine.Split(' ');
                for (int j = 0; j < m_Size.Y; j++)
                {
                    int iIndexCellOfPrototypeCells = Convert.ToInt32(strMapCellsFromFile[j]);

                    m_MapCellsRoadUnFormat[i, j] = iIndexCellOfPrototypeCells;
                }
            }

            //la61y vitrí bắt đầu ra creep
            strBufferLine = sr.ReadLine();
            string[] strStart = strBufferLine.Split(' ');
            GlobalVar.glvt2StartTile = new Vector2(Convert.ToInt32(strStart[0]),
                Convert.ToInt32(strStart[1]));

            sr.Close();
            fStream.Close();
            //=============================================================

            //build phan backgroud cho map + cay coi
            BuildMapCell();

            //-------------------------------------------------------------
            //update 2 giá trị liên quan đến map
            GlobalVar.glvtCellSize = m_CellSize;
            GlobalVar.glMapSize = m_Size;
        }

        void BuildMapCell()
        {
            m_MapCells = new BackgroundMapUnit[(int)m_Size.X, (int)m_Size.Y];
            for (int i = 0; i < m_Size.Y; i++)
            {
                for (int j = 0; j < m_Size.X; j++)
                {
                    int iIndexCellOfPrototypeCells = m_MapCellsUnFormat[i, j];
                    if (iIndexCellOfPrototypeCells == 2)
                    {//object --> vua co background, vua co object
                        Vector2 NewPosition = new Vector2(this.ConvertToNewXPos(i, j) * m_fScale,
                            this.ConvertToNewYPos(i, j) * m_fScale);

                        m_MapCells[i, j] = ObjectMapUnit.Clone(NewPosition,
                                                iIndexCellOfPrototypeCells,
                                                m_mapResMan);
                    }
                    else
                    {
                        Vector2 NewPosition = new Vector2(this.ConvertToNewXPos(i, j) * m_fScale,
                            this.ConvertToNewYPos(i, j) * m_fScale);

                        m_MapCells[i, j] = BackgroundMapUnit.Clone(NewPosition,
                                                iIndexCellOfPrototypeCells,
                                                m_mapResMan);
                    }
                }
            }
        }

        //-----------------------------------------------------------------------
        //hàm vẽ
        public void Draw(SpriteBatch theSpriteBatch)
        {
            //draw background
            for (int row = 0; row < m_Size.Y; row++)
            {
                for (int col = 0; col < m_Size.X; col++)
                {
                    m_MapCells[row, col].Draw(theSpriteBatch,
                        m_mapResMan, GlobalVar.glRootCoordinate, m_fScale);
                }
            }
        }

        //---------------------------------------------------------------
        //xử lý click chuột và kéo map
        //biến xử lý vụ click chuột
        public void Update(GameTime theGameTime)
        {
            // đã đưa sang bên Game1.cs
            // khi load map lên thì nạp kích thước map vào GlobalVar.glMapSize
        }

        //----------------------------------------------------
        //công thức chuyển đổi vị trí
        float ConvertToNewXPos(float i, float j)
        {
            //return m_CellSize.X / 2.0f * (m_Size.X - 1 - i + j);
            return m_CellSize.X * j;
        }

        float ConvertToNewYPos(float i, float j)
        {
            //return m_CellSize.Y / 2 * (i + j);
            return m_CellSize.Y * i;
        }
    }
}
