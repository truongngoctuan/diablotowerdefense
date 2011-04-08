using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using FileAndFolderAccess;

namespace ConvertWidthHeightMul4
{
    public class ImageConvertWidthHeight4
    {
        string[] m_strFolderName;

        public ImageConvertWidthHeight4(string[] strFolderName)
        {
            m_strFolderName = strFolderName;
        }

        public void ConvertToImageWidthHeightMultiple4()
        {
            //đọc tất cả các file trong các folder
                //với mỗi file
                //lấy width height, tính ra width height mới
                //ghi vào file.bat
            //run file .bat

            //tạo file để ghi...
            FileStream fStream;

            fStream = new FileStream(Directory.GetCurrentDirectory() + "\\" + "ConvertToWidthHeightMultiple4.bat",
                FileMode.Create,
                FileAccess.Write);

            StreamWriter sw = new StreamWriter(fStream);
            sw.WriteLine(@"@ECHO off");

            for (int i = 0; i < m_strFolderName.Length; i++)
            {
                sw.WriteLine("ECHO convert image in folder: " + m_strFolderName[i]);

                string[] arrFiles = FileAccessHelper.GetPathAndFileNameInFolder(m_strFolderName[i], "");

                int iNewWidth, iNewHeight;

                for (int j = 0; j < arrFiles.Length; j++)
                {
                    Bitmap bm = new Bitmap(arrFiles[j]);
                    if (bm.Width % 4 != 0)
                    {
                        iNewWidth = (int)(bm.Width / 4 + 1) * 4;
                    }
                    else
                    {
                        iNewWidth = (int)(bm.Width / 4) * 4;
                    }

                    if (bm.Height % 4 != 0)
                    {
                        iNewHeight = (int)(bm.Height / 4 + 1) * 4;
                    }
                    else
                    {
                        iNewHeight = (int)(bm.Height / 4) * 4;
                    }

                    sw.WriteLine(string.Format(@"start {0} /min /wait {1} {2} -background #FF00FF -gravity northwest -extent {3}x{4}",
                        "\"\"",
                        "\"c:\\Program Files\\ImageMagick-6.6.2-Q8\\convert.exe\"",
                        arrFiles[j],
                        iNewWidth,
                        iNewHeight));
                }
            }


            sw.WriteLine("exit");

            sw.Close();
            fStream.Close();

            FileAccessHelper.RunFile("ConvertToWidthHeightMultiple4.bat",
                Directory.GetCurrentDirectory(),
                "");
        }
    }
}
