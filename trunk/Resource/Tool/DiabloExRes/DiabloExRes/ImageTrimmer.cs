using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Diagnostics;

namespace DiabloExRes
{
    public class ImageTrimmer
    {
        string m_strInPath;
        string m_strFileName;
        System.Drawing.Color m_colorKey;
        string m_strOutPath;

        System.Drawing.Bitmap img;

        int m_iWidth;
        int m_iHeight;
        //------------------------------------------------------
        //khởi tạo
        public ImageTrimmer(string strInPath,
            string strFileName, 
            System.Drawing.Color colorKey,
            string strOutPath)
        {
            m_strInPath = strInPath;
            m_strFileName = strFileName;
            m_colorKey = colorKey;
            m_strOutPath = strOutPath;

            img = new System.Drawing.Bitmap(m_strFileName);

            m_iWidth = img.Width;
            m_iHeight = img.Height;
        }

        //------------------------------------------------------
        //tìm toạ độ pixel khác màu chuẩn đầu tiên
        int m_iOffsetX;
        int m_iOffsetY;

        public int OffsetX
        {
            get { return m_iOffsetX; }
        }
        
        public int OffsetY
        {
            get { return m_iOffsetY; }
        }
        public void GetOffset()
        {
            m_iOffsetX = GetOffsetX();
            m_iOffsetY = GetOffsetY();
        }

        int GetOffsetX()
        {
            for (int i = 0; i < m_iWidth; i++)
            {
                for (int j = 0; j < m_iHeight; j++)
                {
                    if (img.GetPixel(i, j) != m_colorKey)
                    {
                        return i;
                    }
                }
            }

            return -1;
        }

        int GetOffsetY()
        {
            for (int i = 0; i < m_iHeight; i++)
            {
                for (int j = 0; j < m_iWidth; j++)
                {
                    if (img.GetPixel(j, i) != m_colorKey)
                    {
                        return i;
                    }
                }
            }

            return -1;
        }

        //------------------------------------------------------
        //xử lý tấm ảnh đó
        void ImageProcess()
        {//sau khi đã tính toán offset thì có thể xử lý tiếp với tấm ành

            //tạo tên file mới
            string strNewName = m_strFileName.Substring(0, m_strFileName.Length - 4);
            strNewName += ".png";

            ProcessStartInfo startInfo = new ProcessStartInfo();

            startInfo.FileName = @"convert.exe";
            startInfo.WorkingDirectory = @"c:\Program Files\ImageMagick-6.6.2-Q8";

            //trim
            startInfo.Arguments = m_strFileName 
                + " -trim "
                + strNewName;

            Process process = Process.Start(startInfo);
            process.WaitForExit(15000);

            //đổi màu
            startInfo.Arguments = strNewName
                + " -fill #FF00FF -opaque #AAAAAA "
                + strNewName;

            process = Process.Start(startInfo);
            process.WaitForExit(15000);
        }
        
        //------------------------------------------------------
        //ghi offset ra
        public void WriteToFile(ref StreamWriter sw)
        {
            FileInfo finf = new FileInfo(m_strFileName);
            sw.WriteLine(string.Format("{0} {1} {2}", finf.Name, OffsetX, OffsetY));
        }
    }
}
