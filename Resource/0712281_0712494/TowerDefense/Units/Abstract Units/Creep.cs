using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TowerDefense
{
    public abstract class Creep : MainUnit
    {
        protected int _iBaseSpeed;
        protected int _iSpeed;

        protected int _iBaseDefense;
        protected int _iDefense;
        public int IDefense
        {
            get { return _iDefense; }
            set { _iDefense = value; }
        }

        protected Vector2 _vt2Direction;
        protected Orientation _orientation = Orientation.Right;

        protected State _state;
        public State State
        {
            get { return _state; }
        }
        protected enum CreepState { Normal, Wounded, Rampage };
        protected CreepState _creepState;

        protected Vector2 _vt2OldPositionTiles = new Vector2(-1, -1);

        public Creep()
        {
            this._fScale = 0.5f;
        }

        protected abstract void ChangeState(State newState);

        //protected void PickParticleDirection()
        //{
        //    // Point the particles somewhere between 80 and 100 degrees.
        //    // tweak this to make the smoke have more or less spread.
        //    float angle = RandomHelper.RandomBetween(-22.5f, 337.4f);
        //    float radians = MathHelper.ToRadians(angle);

        //    this._vt2Direction = Vector2.Zero;
        //    // from the unit circle, cosine is the x coordinate and sine is the
        //    // y coordinate. We're negating y because on the screen increasing y moves
        //    // down the monitor.
        //    this._vt2Direction.X = (float)Math.Cos(radians);
        //    this._vt2Direction.Y = -(float)Math.Sin(radians);

        //    this._orientation = (Orientation)((angle + 22.5f) / 45.0f);
        //}

        void AddNeighborIfLowerValue(int iCurrentValue,
            int iXCurrent,
            int iYCurrent,
            int iOrientation,
            ref List<int> listiNeighborX,
            ref List<int> listiNeighborY,
            ref List<int> listiNeighborValue,
            ref List<int> listiNeighborOrientation)
        {
            //   3    2   1
            //     \  |  /
            //      \ | /
            //       \|/
            //   4 ---*---0
            //       /|\
            //      / | \
            //     /  |  \
            //   5    6   7
            switch (iOrientation)
            {
                case 0:
                    {
                        iXCurrent += -1;
                        iYCurrent += 1;
                        break;
                    }
                case 1:
                    {
                        iXCurrent += -1;
                        iYCurrent += 0;
                        break;
                    }
                case 2:
                    {
                        iXCurrent += -1;
                        iYCurrent += -1;
                        break;
                    }
                case 3:
                    {
                        iXCurrent += 0;
                        iYCurrent += -1;
                        break;
                    }
                case 4:
                    {
                        iXCurrent += 1;
                        iYCurrent += -1;
                        break;
                    }
                case 5:
                    {
                        iXCurrent += 1;
                        iYCurrent += 0;
                        break;
                    }
                case 6:
                    {
                        iXCurrent += 1;
                        iYCurrent += 1;
                        break;
                    }
                case 7:
                    {
                        iXCurrent += 0;
                        iYCurrent += 1;
                        break;
                    }
            }

            if (iXCurrent < 0) return;
            if (iYCurrent < 0) return;

            if (iXCurrent >= GlobalVar.glMapSize.Y) return;
            if (iYCurrent >= GlobalVar.glMapSize.X) return;

            int iNeighborValue = GlobalVar.glCurrentMap.MapCellsRoad[iXCurrent, iYCurrent];
            if (iNeighborValue >= 0 && 
                iCurrentValue >= iNeighborValue)
            {
                listiNeighborX.Add(iXCurrent);
                listiNeighborY.Add(iYCurrent);
                listiNeighborValue.Add(iNeighborValue);
                listiNeighborOrientation.Add(iOrientation);
            }
        }

        protected Vector2 _v2NextMovePoint; //diem tiep theo can di chuyen toi gan do
        protected void PickParticleDirection()
        {
            #region TimDuongDi
            //xac dinh tile hien tai
            //lấy danh sách tile xung quanh tile hiện tại bằng cách lấy trong 
            //map - ma trận 2
            //xác định tiles nào nhỏ hơn tile hiện tại
            //random các tiles đó

            this._vt2Direction = new Vector2(0, 0);
            this._orientation = Orientation.Right;

            Vector2 vt2CurrentTile = GlobalVar.ConvertPixelToTile(Position);
            Debug.Logging("Position: " + Position.X.ToString() + Position.Y.ToString());
            Debug.Logging("ConvertPixelToTile: " + vt2CurrentTile.X.ToString() + vt2CurrentTile.Y.ToString());
            
            if (vt2CurrentTile == _vt2OldPositionTiles) return;

            int ii = (int)vt2CurrentTile.Y;
            int ij = (int)vt2CurrentTile.X;

            int iCurrentValue = GlobalVar.glCurrentMap.MapCellsRoad[ii, ij];

            Debug.Logging("ii ij iCurrentValue: " + ii.ToString() + " " + ij.ToString() +" " + iCurrentValue.ToString());
            

            if (ij - 1 < 0)
            {
            }
            else
            {
                if (GlobalVar.glCurrentMap.MapCellsRoad[ii, ij - 1] > 0 &&
                    GlobalVar.glCurrentMap.MapCellsRoad[ii, ij - 1] < iCurrentValue)
                {
                    this._vt2Direction = new Vector2(-1, 0);
                    this._orientation = Orientation.Left;
                    _v2NextMovePoint = GlobalVar.ConvertTileToPixelCenter(ii, ij - 1);
                }
            }

            if (ij + 1 > GlobalVar.glMapSize.X)
            {
            }
            else
            {
                if (GlobalVar.glCurrentMap.MapCellsRoad[ii, ij + 1] > 0 &&
                    GlobalVar.glCurrentMap.MapCellsRoad[ii, ij + 1] < iCurrentValue)
                {
                    this._vt2Direction = new Vector2(1, 0);
                    this._orientation = Orientation.Right;
                    _v2NextMovePoint = GlobalVar.ConvertTileToPixelCenter(ii, ij + 1);
                }
            }

            if (ii - 1 < 0)
            {
            }
            else
            {
                if (GlobalVar.glCurrentMap.MapCellsRoad[ii - 1, ij] > 0 &&
                    GlobalVar.glCurrentMap.MapCellsRoad[ii - 1, ij] < iCurrentValue)
                {
                    this._vt2Direction = new Vector2(0, -1);
                    this._orientation = Orientation.Top;
                    _v2NextMovePoint = GlobalVar.ConvertTileToPixelCenter(ii - 1, ij);
                }
            }

            if (ii + 1 > GlobalVar.glMapSize.Y)
            {
            }
            else
            {
                if (GlobalVar.glCurrentMap.MapCellsRoad[ii + 1, ij] > 0 &&
                    GlobalVar.glCurrentMap.MapCellsRoad[ii + 1, ij] < iCurrentValue)
                {
                    this._vt2Direction = new Vector2(0, 1);
                    this._orientation = Orientation.Bottom;
                    _v2NextMovePoint = GlobalVar.ConvertTileToPixelCenter(ii + 1, ij);
                }
            }

            Debug.Logging("orientation: " + this._orientation.ToString());

            //List<int> listiNeighborX = new List<int>();
            //List<int> listiNeighborY = new List<int>();
            //List<int> listiNeighborValue = new List<int>();
            //List<int> listiNeighborOrientation = new List<int>();

            //for (int i = 0; i < 8; i++)
            //{
            //    AddNeighborIfLowerValue(iCurrentValue,
            //        iX,
            //        iY,
            //        i,
            //        ref listiNeighborX,
            //        ref listiNeighborY,
            //        ref listiNeighborValue,
            //        ref listiNeighborOrientation);
            //}
            //if (listiNeighborX.Count == 0) return;
            //int iChosenNeighbor = GlobalVar.glRandom.Next(listiNeighborX.Count);

            //float angle = -22.5f + listiNeighborOrientation[iChosenNeighbor] * 45f + GlobalVar.glRandom.Next(44);
            //float radians = MathHelper.ToRadians(angle);

            //this._vt2Direction = Vector2.Zero;
            //this._vt2Direction.X = (float)Math.Cos(radians);
            //this._vt2Direction.Y = -(float)Math.Sin(radians);

            //this._orientation = (Orientation)listiNeighborOrientation[iChosenNeighbor];
            #endregion

            //// Point the particles somewhere between 80 and 100 degrees.
            //// tweak this to make the smoke have more or less spread.
            //float angle = RandomHelper.RandomBetween(-22.5f, 337.4f);
            //float radians = MathHelper.ToRadians(angle);            

            //this._vt2Direction = Vector2.Zero;
            //// from the unit circle, cosine is the x coordinate and sine is the
            //// y coordinate. We're negating y because on the screen increasing y moves
            //// down the monitor.
            //this._vt2Direction.X = (float)Math.Cos(radians);
            //this._vt2Direction.Y = -(float)Math.Sin(radians);

            //this._orientation = (Orientation)((angle + 22.5f) / 45.0f);
        }

        //lưu giá tr5i top left tuơgn ứng với frame hiện tại, có thể dung cái này kết hợp với 
        //iwidth cà height để xét collision
        protected Vector2 _vt2CurrentPositionTopLeftOfFrame;
        public bool CheckHit(Vector2 vt2Position)
        {

            //bool bHit = false;
            //if (_vt2Position.X - _iWidth / 2 < vt2Position.X && vt2Position.X < _vt2Position.X + _iWidth / 2)
            //{
            //    if (_vt2Position.Y - IHeight / 2 < vt2Position.Y && vt2Position.Y < _vt2Position.Y + IHeight / 2)
            //    {
            //        bHit = true;
            //    }
            //}
            //return bHit;

            bool bHit = false;
            if (_vt2CurrentPositionTopLeftOfFrame.X + _iWidth * 0.1f < vt2Position.X && vt2Position.X < _vt2CurrentPositionTopLeftOfFrame.X + _iWidth * 0.9f &&
                _vt2CurrentPositionTopLeftOfFrame.Y + _iHeight * 0.1f < vt2Position.Y && vt2Position.Y < _vt2CurrentPositionTopLeftOfFrame.Y + _iHeight * 0.9f)
            {
                bHit = true;
            }
            return bHit;
        }        

        public void Hit()
        {
            if (_iLife <= 0)
            {
                ChangeState(State.Dying);
            }
            else if (_state == State.Moving)
            {
                ChangeState(State.Attacked);
            }
        }
    }
}
