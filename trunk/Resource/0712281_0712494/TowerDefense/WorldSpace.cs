using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TowerDefense
{
    public class WorldSpace
    {
        char [,] _worldSpace2D;
        //byte[][][] _worldSpace3D;

        public enum CellState { Free, Effected };

        public WorldSpace(int width, int height)
        {
            _worldSpace2D = new char[height,width];
        }
        public void SetWorldCell(int row, int col)
        {
            try
            {
                _worldSpace2D[row, col]++;
            }
            catch (Exception)
            {
            }
        }
        public void FreeWorldCell(int row, int col)
        {
            try
            {
                _worldSpace2D[row, col]--;
            }
            catch (Exception)
            {
            }
        }
        public char GetWorldCell(int row, int col)
        {
            char cell = (char)0;
            try 
            {
                cell = _worldSpace2D[row,col];;
            }
            catch(Exception)
            {
            }
            return cell;
        }
    }
}
