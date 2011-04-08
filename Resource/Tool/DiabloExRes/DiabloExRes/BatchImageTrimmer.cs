using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;

namespace DiabloExRes
{
    public class BatchImageTrimmer
    {
        //đọc danh sách các folder trong folder mới gửi
        //với từng folder
            //đọc danhsách file
            //ghi cái width height vào
            //trim từng file và cho ghi thông tin vào
            //đồng thời chép vào thư mục khác đã quy định sẵn (nhớ tạo thư mục ra trước)

        //tạm thơi chép cùng thư mục cho nó dễ, sau khi làm xong, xoa hết file.BMP là xong
        //tạm thời ko nên xoá, vô tcmd copy ra cái mới 
        string m_strInPath;
        string m_strOutPath;
        public BatchImageTrimmer(string strInPath,
            string strOutPath)
        {
            m_strInPath = strInPath;
            m_strOutPath = strOutPath;
        }

        public void Run()
        {
            string[] arrDir = FileAccessHelper.GetFoldersInFolder(m_strInPath);

            for (int i = 0; i < arrDir.Length; i++)
            {
                string[] arrFiles = FileAccessHelper.GetPathAndFileNameInFolder(arrDir[i], "*.BMP");
                if (arrFiles.Length == 0)
                {
                    continue;
                }

                //tạo file để ghi...
                FileStream fStream;

                fStream = new FileStream(arrDir[i] + "\\" + "MonsOffset.ini",
                    FileMode.Create,
                    FileAccess.Write);

                StreamWriter sw = new StreamWriter(fStream);

                //viết thông tin cơ bản về width + hei của con mons ban đầu
                Bitmap img = new Bitmap(arrFiles[0]);
                sw.WriteLine(string.Format("{0} {1}",img.Width, img.Height));
                sw.WriteLine(string.Format("{0}", arrFiles.Length));

                GetOffsetAndWriteToFile(arrFiles, ref sw);

                sw.Close();
                fStream.Close();

                Console.WriteLine(i.ToString());

            }

            CreateBatchFile();
            FileAccessHelper.RunFile("ImageProcessing.bat",
                Directory.GetCurrentDirectory(),
                "");
        }

        private void GetOffsetAndWriteToFile(string[] arrFiles, ref StreamWriter sw)
        {
            for (int j = 0; j < arrFiles.Length; j++)
            {
                //xử lý tấm hình
                ImageTrimmer imgTrim = new ImageTrimmer(@"",
                    arrFiles[j],
                    Color.FromArgb(170, 170, 170),
                    @"");

                imgTrim.GetOffset();

                imgTrim.WriteToFile(ref sw);
            }
        }

        void CreateBatchFile()
        {
            //tạo file để ghi...
            FileStream fStream;

            fStream = new FileStream(Directory.GetCurrentDirectory() + "\\" + "ImageProcessing.bat",
                FileMode.Create,
                FileAccess.Write);

            StreamWriter sw = new StreamWriter(fStream);

            string[] arrFolder = FileAccessHelper.GetFoldersInFolder(m_strInPath);

            sw.WriteLine(@"@ECHO off");
            for (int i = 0; i < arrFolder.Length; i++)
            {
                sw.WriteLine(string.Format(@"start {0} /min /wait {1} -trim -fill #FF00FF -opaque #AAAAAA -format png {2}\*.BMP",
                    "\"\"",
                    "\"c:\\Program Files\\ImageMagick-6.6.2-Q8\\mogrify.exe\"",
                    arrFolder[i]));
                sw.WriteLine("ECHO processing folder: " + arrFolder[i]);
            }
            sw.WriteLine("exit");

            sw.Close();
            fStream.Close();
        }
    }
}
