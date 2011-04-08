using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace DiabloExRes
{
    public class FileAccessHelper
    {
        //-----------------------------------------------------------
        //thao tác với file

        //chạy file
        static public void RunFile(string strFileName,
            string strWorkingDirectory,
            string strArguments)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();

            startInfo.FileName = strFileName;
            startInfo.WorkingDirectory = strWorkingDirectory;
            //if (strArguments != "")
            {
                startInfo.Arguments = strArguments;
            }

            Process process = Process.Start(startInfo);
            process.WaitForExit();
        }

        //lấy tên + đường dẫn file trong folder
        static public string[] GetPathAndFileNameInFolder(string strDir,
            string strEx)
        {//strEx = "*.BMP"
            return Directory.GetFiles(strDir, strEx);
        }

        //lấy tên file trong folder
        static public string[] GetFileNameInFolder(string strDir,
            string strEx)
        {//strEx = "*.BMP"
            string[] strPathFile = GetPathAndFileNameInFolder(strDir, strEx);

            if (strPathFile == null ||
                strPathFile.Length == 0)
            {
                return null;
            }

            string[] strFileName = new string[strPathFile.Length];

            for (int i = 0; i < strFileName.Length; i++)
            {
                FileInfo finf = new FileInfo(strPathFile[i]);
                strFileName[i] = finf.Name;
            }
            
            return strFileName;
        }

        //xoá hết các file trong folder
        static public void DeleteFilesInFolder(string strDir,
            string strEx)
        {
            string[] arrFileBmp = FileAccessHelper.GetPathAndFileNameInFolder(strDir, strEx);
            for (int i = 0; i < arrFileBmp.Length; i++)
            {
                File.Delete(arrFileBmp[i]);
            }
        }

        //cut files
        public static void MoveFiles(string strScrDir,
            string strDesDir,
            string strEx)
        {
            //lấy danh sach file cần cut
            string[] arrFileBmp = GetFileNameInFolder(strScrDir,
                strEx);

            Directory.CreateDirectory(strDesDir);

            DeleteFilesInFolder(strDesDir, strEx);
            
            //move
            for (int i = 0; i < arrFileBmp.Length; i++)
            {
                File.Move(strScrDir + "\\" + arrFileBmp[i], 
                    strDesDir + "\\" + arrFileBmp[i]);
            }
        }

        //---------------------------------------------------------
        //thao tác với folder
        static public string[] GetFoldersInFolder(string strPath)
        {
            return Directory.GetDirectories(strPath);
        }

    }
}
