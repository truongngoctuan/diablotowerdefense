using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace DiabloExRes
{
    public class CreateScript
    {
        //2 số này quyết định cho chạy những mons này 
        //để giảm thời gian đợi khi debug
        int m_iStartAcceptIndexMons;

        public int StartAcceptIndexMons
        {
            get { return m_iStartAcceptIndexMons; }
            set { m_iStartAcceptIndexMons = value;
            if (m_iStartAcceptIndexMons >= m_nMons)
            {
                m_iStartAcceptIndexMons = m_nMons - 1;
            }
            }
        }
        int m_iEndAcceptIndexMons;

        public int EndAcceptIndexMons
        {
            get { return m_iEndAcceptIndexMons; }
            set { m_iEndAcceptIndexMons = value;
            if (m_iEndAcceptIndexMons >= m_nMons)
            {
                m_iEndAcceptIndexMons = m_nMons - 1;
            }
            }
        }
        public CreateScript()
        {
        }

        //-----------------------------------------------------------
        //đọc dữ liệu từ file
        //số lượng mons
        //tên
        //danh sách layer
        //danh sách đối số tương ứng với layer
        int m_nMons;

        public int NMons
        {
            get { return m_nMons; }
            //set { m_nMons = value; }
        }
        List<string> m_listMonsterName = new List<string>();
        List<string[]> m_listLayerName = new List<string[]>();
        List<string[]> m_listLayerArg = new List<string[]>();
        public void ReadFromFile(string strSourceFileName)
        {
            //khởi tạo
            FileStream fStream;

            fStream = new FileStream(strSourceFileName,
                FileMode.Open,
                FileAccess.Read);

            StreamReader sr = new StreamReader(fStream);

            m_nMons = int.Parse(sr.ReadLine());

            //đọc từng cụm monster
            for (int i = 0; i < m_nMons; i++)
            {
                string strMonsName = sr.ReadLine();
                string[] arrMonsLayerName = sr.ReadLine().Split(new char[] { ' ' });
                string[] arrMonsLayerArg = sr.ReadLine().Split(new char[] { ' ' });

                m_listMonsterName.Add(strMonsName);
                m_listLayerName.Add(arrMonsLayerName);
                m_listLayerArg.Add(arrMonsLayerArg);
            }

            sr.Close();
            fStream.Close();
        }

        //-----------------------------------------------------------
        //ghi dữ liệu ra file 
        public void WriteDataToFile(string strFileName, string strScript)
        {
            FileStream fStream;

            fStream = new FileStream(strFileName,
                FileMode.Create,
                FileAccess.Write);

            StreamWriter sw = new StreamWriter(fStream);
            sw.Write(strScript);

            sw.Close();
            fStream.Close();
        }

        //-----------------------------------------------------------
        //hàm tạo script
        public string CreateScriptFromIndex(int iMonsIndex)
        {
            string strResult = string.Empty;
            //format
            strResult = "format=bmp\n";

            //cof
            strResult += "cof=" + m_listMonsterName[iMonsIndex] + "\n";

            //box
            if (m_bUseBoundBox)
            {
                //strResult += "box=-200,-200,200,200\n";
                strResult += string.Format("box=-{0},-{1},{0},{1}\n",
                    m_iarrBoxBoundHeight[iMonsIndex],
                    m_iarrBoxBoundWidth[iMonsIndex]);
            }
            else
            {
            }

            //layer
            int iCount = m_listLayerName[iMonsIndex].Length;
            if (iCount == 1 && m_listLayerName[iMonsIndex][0] == string.Empty)
            {
                strResult += "tr=lit:0\n";
            }
            else
            {
                for (int i = 0; i < iCount; i++)
                {
                    strResult += m_listLayerName[iMonsIndex][i] + "=";
                    strResult += m_listLayerArg[iMonsIndex][i] + ":0\n";
                }
            }

            return strResult;
        }

        //-----------------------------------------------------------
        //hàm tạo bound
        int[] m_iarrBoxBoundHeight;
        int[] m_iarrBoxBoundWidth;
        bool m_bUseBoundBox = false;
        public void GetBoxBound()
        {
            m_iarrBoxBoundHeight = new int[this.NMons];
            m_iarrBoxBoundWidth = new int[this.NMons];

            for (int i = m_iStartAcceptIndexMons; i <= m_iEndAcceptIndexMons; i++)
            {
                string strScript = this.CreateScriptFromIndex(i);
                this.WriteDataToFile("Merge_dcc.ini", strScript);

                FileAccessHelper.RunFile("Merge_dcc.exe",
                Directory.GetCurrentDirectory(),
                "");

                ReadBoundFromFile("stdout.txt", i);
            }

            m_bUseBoundBox = true;
        }

        //hàm lấy bound từ file
        public void ReadBoundFromFile(string strSourceFileName, int iIndexMons)
        {
            //khởi tạo
            FileStream fStream;

            fStream = new FileStream(strSourceFileName,
                FileMode.Open,
                FileAccess.Read);

            StreamReader sr = new StreamReader(fStream);

            //đọc từng dòng
            string strLine = string.Empty;
            string strBuffer = string.Empty;
            do{
                strBuffer = sr.ReadLine();
                if (strBuffer != null && strBuffer.StartsWith("box"))
                {
                    strLine = strBuffer;
                }
            }
            while (strBuffer != null);

            sr.Close();
            fStream.Close();

            if (strLine == string.Empty)
            {
                m_iarrBoxBoundHeight[iIndexMons] = 0;
                m_iarrBoxBoundWidth[iIndexMons] = 0;
                return;
            }

            //xử lý dòng dữ liệu vừa đọc đc
            //box = (-127, -124) - (125, 64) = 253 * 189 pixels
            string[] strSplited = strLine.Split(new string[] { 
            "box",
            " ",
            "(",
            ")",
            ",",
            "-",
            "=",
            "*",
            "pixels"},
            StringSplitOptions.RemoveEmptyEntries);

            m_iarrBoxBoundHeight[iIndexMons] = (int.Parse(strSplited[0]) > int.Parse(strSplited[2])) 
                ? int.Parse(strSplited[0]) 
                : int.Parse(strSplited[2]);
            m_iarrBoxBoundWidth[iIndexMons] = (int.Parse(strSplited[1]) > int.Parse(strSplited[3]))
                ? int.Parse(strSplited[1])
                : int.Parse(strSplited[3]);
        }

        //--------------------------------------------------------------
        //chạy script
        public void RunScript(string strNewPath)
        {
            this.GetBoxBound();

            for (int i = this.StartAcceptIndexMons; i <= this.EndAcceptIndexMons; i++)
            {
                string strScript = this.CreateScriptFromIndex(i);
                this.WriteDataToFile("Merge_dcc.ini", strScript);

                string strFolderName = "Mons" + ((int)(i / 3)).ToString("D2");
                switch (i % 3)
                {
                    case 0:
                        {
                            strFolderName += "Moving";
                            break;
                        }
                    case 1:
                        {
                            strFolderName += "Attacked";
                            break;
                        }
                    case 2:
                        {
                            strFolderName += "Dying";
                            break;
                        }
                }

                CreateBitmapFromScript(strNewPath + "\\" + strFolderName);
            }
        }

        public void CreateBitmapFromScript(string strFolderName)
        {
            Directory.CreateDirectory(strFolderName);

            FileAccessHelper.RunFile("Merge_dcc.exe",
                Directory.GetCurrentDirectory(),
                "");

            string[] strFilesResult = FileAccessHelper.GetPathAndFileNameInFolder(Directory.GetCurrentDirectory(), "*.BMP");
            if (strFilesResult == null ||
                strFilesResult.Length == 0)
            {
                return;
            }

            FileAccessHelper.DeleteFilesInFolder(strFolderName, "*.BMP");

            FileAccessHelper.MoveFiles(Directory.GetCurrentDirectory(),
                strFolderName,
                "*.BMP");

            RenameBmpFileInFolder(strFolderName);
        }

        //sửa tên các bitmap đó
        public void RenameBmpFileInFolder(string strFolderPath)
        {
            string[] arrFileNameBmpOld = FileAccessHelper.GetFileNameInFolder(strFolderPath, "*.BMP");

            //rename các tên đó
            string[] arrFileNameBmpNew = new string[arrFileNameBmpOld.Length];
            for (int i = 0; i < arrFileNameBmpOld.Length; i++)
            {
                //file xuất ra mặc định có dạng sau
                //(07)-D06-F005.BMP
                //D <direction in COF> - <direction in DCC> - F <frame> . PCX
                //   1    6   2
                //     \  |  /
                //      \ | /
                //       \|/
                //   5 ---*--- 7
                //       /|\
                //      / | \
                //     /  |  \
                //   0    4   3

                //theo thứ tự public enum Direction { 
                //TopLeft = 0, Top, TopRight, Left, Right, 
                //BottomLeft, Bottom, BottomRight }

                //đổi thành xx-yyy
                //xx: hướng
                //yy: số thứ tự frame
                //1 --> 3
                //6 --> 2
                //2 --> 1
                //7 --> 0
                //3 --> 7
                //4 --> 6
                //0 --> 5
                //5 --> 4
                string[] strSplited = arrFileNameBmpOld[i].Split(new char[] { 
                    '(', 
                    ')', 
                    '-',
                    'D',
                    'F',
                    '.'}, StringSplitOptions.RemoveEmptyEntries);

                switch (int.Parse(strSplited[0]))
                {
                    case 1:
                        {
                            strSplited[0] = "03";//((int)(0)).ToString("D2");
                            break;
                        }
                    case 6:
                        {
                            strSplited[0] = "02";
                            break;
                        }
                    case 2:
                        {
                            strSplited[0] = "01";
                            break;
                        }
                    case 7:
                        {
                            strSplited[0] = "00";
                            break;
                        }
                    case 3:
                        {
                            strSplited[0] = "07";
                            break;
                        }
                    case 4:
                        {
                            strSplited[0] = "06";
                            break;
                        }
                    case 0:
                        {
                            strSplited[0] = "05";
                            break;
                        }
                    case 5:
                        {
                            strSplited[0] = "04";
                            break;
                        }
                }

                arrFileNameBmpNew[i] = string.Format("{0}-{1}.{2}",
                    strSplited[0],
                    strSplited[2],
                    strSplited[3]);
            }

            //move
            for (int i = 0; i < arrFileNameBmpNew.Length; i++)
            {
                File.Move(strFolderPath + "\\" + arrFileNameBmpOld[i],
                    strFolderPath + "\\" + arrFileNameBmpNew[i]);
            }
        }
    }
}
