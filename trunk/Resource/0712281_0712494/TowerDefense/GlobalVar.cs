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

public enum GameStage { Intro, MainMenu, HighScore, Options, SinglePlayer, Multiplayers, InGame, Loading};
public enum MyAnimatedMenuState { Opening, OpeningMenuItems, Opened, Closing, Closed };
public enum MenuItemType { None, Task, SubMenu };
public enum MenuItemState { Hovered, Pressed, Released, Normal };

public enum State { Moving = 0, Attacked , Dying, Died };
public enum Orientation { Right = 0, TopRight, Top, TopLeft, Left, BottomLeft, Bottom, BottomRight }
        

namespace TowerDefense
{
    class GlobalVar
    {
        public static GameStage glGameStage;
        public static Vector2 glViewport;
        //public static MyAnimatedMenu glAnimatedMenu;
        public static LoadScreen glLoadScreen;
        public static HighScoreScreen glHighScoreScreen;

//        public static OptionScreen glOptionScreen;
        public static GraphicsDeviceManager glGraphics;


        public static Game1 glGame;
        public static StageManager glStageManager;
        public static WorldSpace glWorldSpace;
        public static UnitManager glUnitManager;
        //public static VideoPlayer glIntroPlayer;
        //public static int iLogoTime = 2500;
        //public static int iTimeTillIntroMovie;

        public static ContentManager glContentManager;
        public static Vector2 glRootCoordinate;
        public static Vector2 glMapSize;
        public static Vector2 glIconSize;
        public static Vector2 glIconPosition;

        public static Option.OptionVariablesObserver optionVariables = new TowerDefense.Option.OptionVariablesObserver();

        #region maps
        public static Random glRandom = new Random(DateTime.Now.Millisecond);
        public static Vector2 glvtCellSize;
        public static float glMapScale = 1.0f;

        //update 2 giá trị liện quan đến map
        //trong hàm readmap trong map.cs
        //GlobalVar.glvtCellSize = m_CellSize;
        //GlobalVar.glMapSize = m_Size;

        //----------------------------------------------------
        //công thức chuyển đổi vị trí
        public static float ConvertTileToPixelX(float j, float i)
        {
            return glvtCellSize.X * j;
            //return glvtCellSize.X / 2.0f * (glMapSize.X - 1 - i + j);
        }

        public static float ConvertTileToPixelY(float j, float i)
        {
            return glvtCellSize.Y * i;
            //return glvtCellSize.Y / 2 * (i + j);
        }

        public static Vector2 ConvertTileToPixelCenter(Vector2 v2PixelPosition)
        {
            return ConvertTileToPixelCenter(v2PixelPosition.Y, v2PixelPosition.X);
            //return new Vector2(ConvertTileToPixelX(v2PixelPosition.X, v2PixelPosition.Y) + glvtCellSize.X / 2,
            //    ConvertTileToPixelY(v2PixelPosition.X, v2PixelPosition.Y) + glvtCellSize.Y / 2);
        }
        public static Vector2 ConvertTileToPixelCenter(float row, float col)
        {
            return new Vector2(ConvertTileToPixelX(col, row) + glvtCellSize.X / 2.0f,
                ConvertTileToPixelY(col, row) + glvtCellSize.Y / 2.0f);

            //return new Vector2(ConvertTileToPixelX(row, col) + glvtCellSize.X / 2,
            //    ConvertTileToPixelY(row, col) + glvtCellSize.Y / 2);
        }

        static Vector2[] vtMien = new Vector2[] { 
            new Vector2(0, 0),
            new Vector2(0, -1),
            new Vector2(-1, 0),
            new Vector2(1, 0),
            new Vector2(0, 1)};
        public static Vector2 ConvertPixelToTile(Vector2 v2PixelPosition)
        {
            Vector2 v2tile = new Vector2();
            v2tile.X = (int)((v2PixelPosition.X) / glvtCellSize.X);
            v2tile.Y = (int)((v2PixelPosition.Y) / glvtCellSize.Y);
            return v2tile;

            ////làm tròn tọa độ
            //Vector2 v2PixelPosiionRound = new Vector2((int)(v2PixelPosition.X / glvtCellSize.X * 2) / 2f * 2,
            //    (int)(v2PixelPosition.Y / glvtCellSize.Y * 2) / 2f * 2);

            //if ((v2PixelPosiionRound.X % 2 == 0 &&
            //    v2PixelPosiionRound.Y % 2 != 0) ||
            //(v2PixelPosiionRound.X % 2 != 0 &&
            //    v2PixelPosiionRound.Y % 2 == 0))
            //{
            //    if (v2PixelPosition.X - ConvertTileToPixelX(v2PixelPosiionRound.X - 1f, v2PixelPosiionRound.Y) < glvtCellSize.X)
            //    {
            //        v2PixelPosiionRound = new Vector2(v2PixelPosiionRound.X - 1, v2PixelPosiionRound.Y);
            //    }
            //    else
            //    {
            //        v2PixelPosiionRound = new Vector2(v2PixelPosiionRound.X, v2PixelPosiionRound.Y - 1);
            //    }
            //}

            //float ii = (v2PixelPosiionRound.Y
            //    - v2PixelPosiionRound.X
            //    - 1
            //    + glMapSize.X) / 2;

            //float ij = (v2PixelPosiionRound.Y
            //    + v2PixelPosiionRound.X
            //    + 1
            //    - glMapSize.X) / 2;

            ////kiểm tra thuộc vùng nào
            //float tx = v2PixelPosition.X - ConvertTileToPixelX(ii, ij);
            //float ty = v2PixelPosition.Y - ConvertTileToPixelY(ii, ij);
            //float h = glvtCellSize.X / 2f;

            //int iMien;

            //if (tx <= h)
            //    if (ty <= h / 2)
            //        if (tx + 2f * ty - h < 0)
            //            iMien = 1;
            //        else
            //            iMien = 0;
            //    else
            //        if (tx - 2f * ty + h < 0)
            //            iMien = 3;
            //        else
            //            iMien = 0;
            //else
            //    if (ty <= h / 2)
            //        if (tx - 2f * ty - h > 0)
            //            iMien = 2;
            //        else
            //            iMien = 0;
            //    else
            //        if (tx + 2f * ty - 3f * h > 0)
            //            iMien = 4;
            //        else
            //            iMien = 0;

            //Vector2 vt = new Vector2(ii + vtMien[iMien].X,
            //    ij + vtMien[iMien].Y);

            //if (vt.X < 0) vt = new Vector2(0, vt.Y);
            //if (vt.Y < 0) vt = new Vector2(vt.X, 0);

            //if (vt.X >= glMapSize.Y) vt = new Vector2((int)glMapSize.Y - 1, vt.Y);
            //if (vt.Y >= glMapSize.X) vt = new Vector2(vt.X, (int)glMapSize.X - 1);

            //return vt;
        }
        #endregion

        #region TimDuongDi
        public static Map glCurrentMap;
        public static Vector2 glvt2StartTile;
        #endregion


        public static GameStage StringToGameStage(string stage)
        {
            if (stage == "Intro")
                return GameStage.Intro;
            else if (stage == "MainMenu")
                return GameStage.MainMenu;
            else if (stage == "Options")
                return GameStage.Options;
            else if (stage == "HighScore")
                return GameStage.HighScore;
            else if (stage == "Loading")
                return GameStage.Loading;
            else if (stage == "SinglePlayer")
                return GameStage.SinglePlayer;
            else if (stage == "Multiplayers")
                return GameStage.Multiplayers;
            else if (stage == "InGame")
                return GameStage.InGame;            
            return glGameStage;
        }

        public static void SetGameStage(GameStage newGameStage)
        {
            glGameStage = newGameStage;
            switch (glGameStage)
            {
                case GameStage.Intro:
                    {
                        //iTimeTillIntroMovie = iLogoTime;
                        break;
                    }
                case GameStage.MainMenu:
                    {
                        glGame.CurrentGameState.NextState(ref glGame);
                        AudioPlayer.PlayBackgroundMusic();
                        
                        break;
                    }
                case GameStage.HighScore:
                    {
                        glHighScoreScreen = new HighScoreScreen();
                        glHighScoreScreen.LoadResource();
                        glHighScoreScreen.LoadContent();                        
                        break;
                    }
                case GameStage.Loading:
                    {                        
                        glGame.IsFixedTimeStep = false;
                        glLoadScreen = new LoadScreen();
                        glStageManager.LoadStages(@"Stages\Stages.xml");
                        glUnitManager.LoadTowers(@"Content\Towers.xml");
                        glStageManager.GoToStage(0);
                        break;
                    }
                case GameStage.SinglePlayer:
                    {
                        AudioPlayer.PlayBackgroundMusic();
                        glGame.IsFixedTimeStep = true;
                        break;
                    }
                case GameStage.Options:
                    {
                        glGame.CurrentGameState.NextState(ref glGame);

                        break;
                    }

            }
        }


        public static void SetWorldCell(Vector2 vt2Position, int range)
        {
            //vt2Position += glRootCoordinate;
            Vector2 vt2TilePosition = ConvertPixelToTile(vt2Position);
            float fDelta = vt2Position.X - range;

            int tRangeTile = (int)((float)fDelta / glvtCellSize.X + 0.5f);

            int iIMinTile = (int)vt2TilePosition.Y - tRangeTile;
            int iIMaxTile = (int)vt2TilePosition.Y + tRangeTile;

            int iJMinTile = (int)vt2TilePosition.X - tRangeTile;
            int iJMaxTile = (int)vt2TilePosition.X + tRangeTile;

            if (iIMinTile < 0) iIMinTile = 0;
            if (iJMinTile < 0) iJMinTile = 0;

            if (iIMaxTile >= glMapSize.Y) iIMaxTile = (int)glMapSize.Y - 1;
            if (iJMaxTile >= glMapSize.X) iJMaxTile = (int)glMapSize.X - 1;

            for (int i = iIMinTile; i <= iIMaxTile; i++)
            {
                for (int j = iJMinTile; j <= iJMaxTile; j++)
                {
                    glWorldSpace.SetWorldCell(i, j);
                }
            }
        }

        public static void FreeWorldCell(Vector2 vt2Position, int range)
        {
            //vt2Position += glRootCoordinate;
            Vector2 vt2TilePosition = ConvertPixelToTile(vt2Position);
            float fDelta = vt2Position.X - range;

            int tRangeTile = (int)((float)fDelta / glvtCellSize.X + 0.5f);

            int iIMinTile = (int)vt2TilePosition.Y - tRangeTile;
            int iIMaxTile = (int)vt2TilePosition.Y + tRangeTile;

            int iJMinTile = (int)vt2TilePosition.X - tRangeTile;
            int iJMaxTile = (int)vt2TilePosition.X + tRangeTile;

            if (iIMinTile < 0) iIMinTile = 0;
            if (iJMinTile < 0) iJMinTile = 0;

            if (iIMaxTile >= glMapSize.Y) iIMaxTile = (int)glMapSize.Y - 1;
            if (iJMaxTile >= glMapSize.X) iJMaxTile = (int)glMapSize.X - 1;

            for (int i = iIMinTile; i <= iIMaxTile; i++)
            {
                for (int j = iJMinTile; j <= iJMaxTile; j++)
                {
                    glWorldSpace.FreeWorldCell(i, j);
                }
            }
        }

        public static char GetWorldCell(Vector2 vt2Position)
        {
            //vt2Position += glRootCoordinate;
            Vector2 vt2TilesPosition = ConvertPixelToTile(vt2Position);
            return glWorldSpace.GetWorldCell((int)vt2TilesPosition.X, (int)vt2TilesPosition.Y);
        }
    }
}
