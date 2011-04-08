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

namespace TowerDefense
{
    public class Map
    {
        string _strMapFile;

        //Texture2D[] m_PrototypeCells;// bỏ, lấy trực tiếp từ resource manager
        int[,] m_MapCellsUnFormat;
        BackgroundMapUnit[,] m_MapCells;
        Vector2 m_Size;//size của MapCells
        Vector2 m_CellSize;
        const float m_fScale = 1.0f;

        //mảng này lưu trữ ma trận đường đi (ma trận 2), lLƯU TRỪ OBJECT TRONG NÀY LUÔN
        //VÌ BẢN CHẤT ROAD CŨNG LÀ OBJECT THÔI
        //CÁC OBJECT CHO RANDOM 
        //đối với các object thì trên matrận đường đi (road sẽ đc đánh dấu là -2)
        //đối với các tiles bình thường thì là -1
        //đcih1 là 0
        const float m_fRatioAppear = 0.05f;
        int[,] m_MapCellsRoadUnFormat;

        public int[,] MapCellsRoad
        {
            get { return m_MapCellsRoadUnFormat; }
            //set { m_MapCellsRoadUnFormat = value; }
        }
        BackgroundMapUnit[,] m_MapCellsRoad;

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
        BackgroundMapUnit[] m_bgUnitPrototype;

        public void LoadContent(ContentManager theContentManager)
        {
            m_mapResMan.LoadContent(theContentManager);

            m_bgUnitPrototype = new BackgroundMapUnit[8];

            //background
            m_bgUnitPrototype[0] = new BackgroundMapUnit(new Vector2(),
                (int)BackgroundMapUnitName.Desert);
            m_bgUnitPrototype[1] = new BackgroundMapUnit(new Vector2(),
                (int)BackgroundMapUnitName.Grass);

            //border
            m_bgUnitPrototype[2] = new BackgroundMapUnit(new Vector2(),
                (int)BackgroundMapUnitName.MoreDesertLessGrass);
            m_bgUnitPrototype[3] = new BackgroundMapUnit(new Vector2(),
                (int)BackgroundMapUnitName.LessDesertMoreGrass);
            m_bgUnitPrototype[4] = new BackgroundMapUnit(new Vector2(),
                (int)BackgroundMapUnitName.DesertEqualGrass);

            //road
            m_bgUnitPrototype[5] = new BackgroundMapUnit(new Vector2(),
                (int)BackgroundMapUnitName.BrickLeftToRight);
            m_bgUnitPrototype[6] = new BackgroundMapUnit(new Vector2(),
                (int)BackgroundMapUnitName.BrickRightToLeft);

            m_bgUnitPrototype[7] = new BackgroundMapUnit(new Vector2(),
                (int)BackgroundMapUnitName.Object);
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

            //cần 1 hàm nạp lại nhiều 0 ít 1 và nhiều 1 ít 0 và 1 == 0
            BuildMapCell();
            BuildMapCellWithRoadAndRandomObject();

            //-------------------------------------------------------------
            //update 2 giá trị liện quan đến map
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

                    Vector2 NewPosition = new Vector2(this.ConvertToNewXPos(i, j) * m_fScale,
                        this.ConvertToNewYPos(i, j) * m_fScale);

                    //nạp 0 hoặc 1
                    //m_MapCells[i, j] = m_bgUnitPrototype[iIndexCellOfPrototypeCells].Clone(NewPosition,
                    //    iIndexCellOfPrototypeCells);

                    //tìm cách xác định xem nạp 1 trong 5 loại
                    int[] iCount = new int[2];
                    iCount[0] = 0;
                    iCount[1] = 0;

                    //left
                    if (j == 0)
                    {
                        iCount[iIndexCellOfPrototypeCells]++;
                    }
                    else
                    {
                        iCount[m_MapCellsUnFormat[i, j - 1]]++;
                    }

                    //top
                    if (i == 0)
                    {
                        iCount[iIndexCellOfPrototypeCells]++;
                    }
                    else
                    {
                        iCount[m_MapCellsUnFormat[i - 1, j]]++;
                    }

                    //right
                    if (j == m_Size.Y - 1)
                    {
                        iCount[iIndexCellOfPrototypeCells]++;
                    }
                    else
                    {
                        iCount[m_MapCellsUnFormat[i, j + 1]]++;
                    }

                    //bot
                    if (i == m_Size.X - 1)
                    {
                        iCount[iIndexCellOfPrototypeCells]++;
                    }
                    else
                    {
                        iCount[m_MapCellsUnFormat[i + 1, j]]++;
                    }

                    //chọn lựa nạp 1 trong 5 trường
                    //1
                    //1 nhiều 0 ít
                    //1 == 0
                    //0 nhiều 1 ít
                    //0
                    //trong mỗi trường này sẽ ramdom texture,
                    //mỗi trường lưu trong 1 folder riêng
                    if (iCount[(int)BackgroundMapUnitName.Desert] == 4 ||
                        iCount[(int)BackgroundMapUnitName.Grass] == 4)
                    {
                        m_MapCells[i, j] = m_bgUnitPrototype[iIndexCellOfPrototypeCells].Clone(NewPosition,
                        iIndexCellOfPrototypeCells,
                        m_mapResMan);
                        continue;
                    }

                    if (iCount[(int)BackgroundMapUnitName.Desert] == 3 &&
                        iCount[(int)BackgroundMapUnitName.Grass] == 1)
                    {
                        m_MapCells[i, j] = m_bgUnitPrototype[(int)BackgroundMapUnitName.MoreDesertLessGrass].Clone(NewPosition,
                        (int)BackgroundMapUnitName.MoreDesertLessGrass,
                        m_mapResMan);
                        continue;
                    }

                    if (iCount[(int)BackgroundMapUnitName.Desert] == 1 &&
                        iCount[(int)BackgroundMapUnitName.Grass] == 3)
                    {
                        m_MapCells[i, j] = m_bgUnitPrototype[(int)BackgroundMapUnitName.LessDesertMoreGrass].Clone(NewPosition,
                        (int)BackgroundMapUnitName.LessDesertMoreGrass,
                        m_mapResMan);
                        continue;
                    }

                    if (iCount[(int)BackgroundMapUnitName.Desert] == 2 &&
                        iCount[(int)BackgroundMapUnitName.Grass] == 2)
                    {
                        m_MapCells[i, j] = m_bgUnitPrototype[(int)BackgroundMapUnitName.DesertEqualGrass].Clone(NewPosition,
                        (int)BackgroundMapUnitName.DesertEqualGrass,
                        m_mapResMan);
                        continue;
                    }

                }
            }
        }

        void BuildMapCellWithRoadAndRandomObject()
        {
            m_MapCellsRoad = new BackgroundMapUnit[(int)m_Size.X, (int)m_Size.Y];
            for (int i = 0; i < m_Size.Y; i++)
            {
                for (int j = 0; j < m_Size.X; j++)
                {
                    int iIndexCellOfPrototypeCells = m_MapCellsRoadUnFormat[i, j];

                    if (m_MapCellsRoadUnFormat[i, j] >= 0)
                    {
                        Vector2 NewPosition = new Vector2(this.ConvertToNewXPos(i, j) * m_fScale,
                        this.ConvertToNewYPos(i, j) * m_fScale);

                        //kiểm tra xem nên vẽ lToR hay RToL
                        bool bIsLeftToRight = false;
                        //ltor
                        if ((j == 0 && m_MapCellsRoadUnFormat[i, j + 1] != 0) ||
                        (j == m_Size.Y - 1 && m_MapCellsRoadUnFormat[i, j - 1] != 0) ||
                        (j != 0 && j != m_Size.Y - 1 && (m_MapCellsRoadUnFormat[i, j + 1] != 0 || m_MapCellsRoadUnFormat[i, j - 1] != 0)))
                        {
                            bIsLeftToRight = true;
                        }

                        if (bIsLeftToRight)
                        {
                            m_MapCellsRoad[i, j] = m_bgUnitPrototype[(int)BackgroundMapUnitName.BrickLeftToRight].Clone(NewPosition,
                                (int)BackgroundMapUnitName.BrickLeftToRight,
                        m_mapResMan);
                        }
                        else
                        {
                            m_MapCellsRoad[i, j] = m_bgUnitPrototype[(int)BackgroundMapUnitName.BrickRightToLeft].Clone(NewPosition,
                                (int)BackgroundMapUnitName.BrickRightToLeft,
                        m_mapResMan);
                        }
                    }
                    else
                    {//tạo object theo tỉ lệ %
                        //nếu ko random đc thì là -1
                        //ngược lại là -2

                        if ((float)GlobalVar.glRandom.NextDouble() <= m_fRatioAppear)
                        {
                            //tạo object
                            m_MapCellsRoadUnFormat[i, j] = -2;

                            Vector2 NewPosition = new Vector2((this.ConvertToNewXPos(i, j) + m_mapResMan._rsTexture2Ds[0].Width / 2) * m_fScale,
                        (this.ConvertToNewYPos(i, j) + m_mapResMan._rsTexture2Ds[0].Height - m_CellSize.Y / 2) * m_fScale);

                            m_MapCellsRoad[i, j] = m_bgUnitPrototype[(int)BackgroundMapUnitName.Object].Clone(NewPosition,
                                (int)BackgroundMapUnitName.Object,
                        m_mapResMan);
                        }
                        else
                        {
                            m_MapCellsRoadUnFormat[i, j] = -1;
                        }
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
                    //xem chi tiết công thức trong tờ lịch ngày 1/6 ^^
                    //Vector2 NewPos = new Vector2(m_CellSize.X / 2.0f * (m_Size.X - 1 - row + col) - m_CurrentRootCoordinate.X,
                    //    m_CellSize.Y / 2 * (col + row) - m_CurrentRootCoordinate.Y);

                    //if (Math.Abs(NewPos.X) > m_CellSize.X &&
                    //    Math.Abs(NewPos.Y) > m_CellSize.Y)
                    //{//có vẽ ra cũng ko thấy
                    //    continue;
                    //}

                    m_MapCells[row, col].Draw(theSpriteBatch,
                        m_mapResMan, GlobalVar.glRootCoordinate, m_fScale);
                }
            }

            //draw road
            for (int row = 0; row < m_Size.Y; row++)
            {
                for (int col = 0; col < m_Size.X; col++)
                {
                    if (m_MapCellsRoadUnFormat[row, col] >= 0)
                    {
                        m_MapCellsRoad[row, col].Draw(theSpriteBatch,
                            m_mapResMan, GlobalVar.glRootCoordinate, m_fScale * 2f);
                    }
                }
            }

            //draw object
            //không có scale cho nó dễ
            for (int row = 0; row < m_Size.Y; row++)
            {
                for (int col = 0; col < m_Size.X; col++)
                {
                    if (m_MapCellsRoadUnFormat[row, col] == -2)
                    {
                        m_MapCellsRoad[row, col].Draw(theSpriteBatch,
                            m_mapResMan, GlobalVar.glRootCoordinate, 1f);
                    }

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
            return m_CellSize.X / 2.0f * (m_Size.X - 1 - i + j);
        }

        float ConvertToNewYPos(float i, float j)
        {
            return m_CellSize.Y / 2 * (i + j);
        }
    }
}
