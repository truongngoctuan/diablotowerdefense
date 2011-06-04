using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.IO;

namespace TowerDefense
{
    public class UtilReadFile
    {
        static Vector2[] ReadDataFromOffsetFile(string strFileName, ref List<Vector2> vt2BoundSprite)
        {
            //khởi tạo
            FileStream fStream;

            fStream = new FileStream(strFileName,
                FileMode.Open,
                FileAccess.Read);

            StreamReader sr = new StreamReader(fStream);

            //lấy khung
            string strBuffer;
            strBuffer = sr.ReadLine();
            string[] strBoundSpliter = strBuffer.Split(new char[] { ' ' });
            vt2BoundSprite.Add(new Vector2(int.Parse(strBoundSpliter[0]), int.Parse(strBoundSpliter[1])));

            //lấy số lương cần chạy
            strBuffer = sr.ReadLine();
            int iCount = int.Parse(strBuffer);

            Vector2[] vt2Offset = new Vector2[iCount];

            //đọc từng cụm monster
            for (int i = 0; i < iCount; i++)
            {
                strBuffer = sr.ReadLine();
                string[] strOffsetSpliter = strBuffer.Split(new char[] { ' ' });

                vt2Offset[i] = new Vector2(int.Parse(strOffsetSpliter[1]), int.Parse(strOffsetSpliter[2]));
            }

            sr.Close();
            fStream.Close();

            return vt2Offset;
        }

        public static void ReadDataFromOffsetFile(ref List<Vector2[]> vt2OffsetSprite, ref List<Vector2> vt2BoundSprite,
            string strMovingResourceFolder, string strAttackedResourceFolder, string strDyingResourceFolder,
            string strOffsetFilename)
        {
            vt2OffsetSprite = new List<Vector2[]>();

            vt2OffsetSprite.Add(UtilReadFile.ReadDataFromOffsetFile(strMovingResourceFolder + "\\" + strOffsetFilename, ref vt2BoundSprite));
            vt2OffsetSprite.Add(UtilReadFile.ReadDataFromOffsetFile(strAttackedResourceFolder + "\\" + strOffsetFilename, ref vt2BoundSprite));
            vt2OffsetSprite.Add(UtilReadFile.ReadDataFromOffsetFile(strDyingResourceFolder + "\\" + strOffsetFilename, ref vt2BoundSprite));
        }
    }
}
