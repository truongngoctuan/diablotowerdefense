using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Microsoft.Xna.Framework;

namespace TowerDefense.Menu
{
    public class UltiMenuItemNode
    {
        #region XML
        public static MenuItemNode ReadXML(string xmlFilename)
        {
            MenuItemNode nodeRoot = new MenuItemNode(GameStage.MainMenu, "root");

            XmlTextReader reader = new XmlTextReader(xmlFilename);
            while (reader.Read())
                if (reader.NodeType == XmlNodeType.Element)
                    if (reader.Name == "Menu")
                    {
                        ReadMenuItem(reader, nodeRoot);
                        break;
                    }

            return nodeRoot;
        }

        static void ReadMenuItem(XmlTextReader reader, MenuItemNode node)
        {
            bool bEndLoop = false;
            while (bEndLoop == false)
            {
                reader.Read();
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        {
                            string name, type, stage;
                            if (reader.Name == "Menu")
                            {
                                name = reader.GetAttribute("Name");
                                type = reader.GetAttribute("Type");
                                if (type == "Task")
                                {
                                    stage = reader.GetAttribute("Function");
                                    GameStage gameStage = GlobalVar.StringToGameStage(stage);
                                    //MenuItem menuItem = new MenuItem(gameStage, name);
                                    //_menuItems.Add(menuItem);

                                    MenuItemBaseNode item = new MenuItemLeaf(gameStage, name);
                                    node.Children.Add(item);
                                }
                                else if (type == "SubMenu")
                                {
                                    //MyAnimatedMenu myMenu = new MyAnimatedMenu(this);
                                    //myMenu.ReadMenuItem(reader);
                                    //MenuItem menuItem = new MenuItem(myMenu, name);
                                    //_menuItems.Add(menuItem);

                                    MenuItemNode item = new MenuItemNode(GameStage.MainMenu, name);
                                    node.Children.Add(item);

                                    ReadMenuItem(reader, (MenuItemNode)node.Children[node.Children.Count - 1]);
                                }
                            }
                            break;
                        }
                    case XmlNodeType.EndElement:
                        {
                            bEndLoop = true;
                            break;
                        }
                }
            }
            //InitializeSubItem();
        }

        #endregion

        #region support animation ItemNode position
//        Vector2 GetOpeningMenuItemPosition(int index)
        public static Vector2 GetOpeningMenuItemPosition(Vector2 v2Center, int index, int iTotal, int iStep, int nStep)
        {
            if (index == 0)
                return GetOpenedMenuItemPosition(v2Center, index, iTotal);
            int iRadius = 200;
            float fAlpha;//góc alpha của dưởng thẳng qua tâm, cắt đường tròn
            float fAlphaQuater1;

            fAlpha = 360.0f / (float)iTotal * (float)index + 90;
            //====
            //bổ sung: tạo hiệu ứng từ từ chuyển sang vị trí mới củ từng menuitems

            int iLastIndex = index - 1;
            float fLastAlpha = 360.0f / (float)iTotal * (float)iLastIndex + 90;

            fAlpha = fLastAlpha + (fAlpha - fLastAlpha) * (float)iStep / (float)nStep;

            //====
            fAlphaQuater1 = fAlpha % 90;// góc alpha khi chuyển sang góc phần tư thứ I
            double dTanAlpha = Math.Tan(fAlphaQuater1 * MathHelper.Pi / 180.0);

            Vector2 vt2 = new Vector2();
            vt2.X = (float)Math.Sqrt((Math.Pow(iRadius, 2)) / (1.0 + Math.Pow(dTanAlpha, 2)));
            vt2.Y = (int)(dTanAlpha * vt2.X);

            //deltaalpha la góc cần xoay từ góc phần 4 thứ I đến đích
            double dCosDeltaAlpha = Math.Cos((fAlpha - fAlphaQuater1) * MathHelper.Pi / 180.0);
            double dSinDeltaAlpha = Math.Sin((fAlpha - fAlphaQuater1) * MathHelper.Pi / 180.0);

            Vector2 vtResult = new Vector2();
            vtResult.X = (float)(vt2.X * dCosDeltaAlpha - vt2.Y * dSinDeltaAlpha);
            vtResult.Y = (float)(vt2.X * dSinDeltaAlpha + vt2.Y * dCosDeltaAlpha);

            //chuyen toa do (0, 0) sang center
            vtResult.X += v2Center.X;
            vtResult.Y += v2Center.Y;

            //chuyen tu toa do Oxy sang toa do man hinh
            vtResult.Y = GlobalVar.glViewport.Y - vtResult.Y;

            return vtResult;
        }
        public static Vector2 GetOpenedMenuItemPosition(Vector2 v2Center, int index, int iTotal)
        {
            int iRadius = 200;
            Vector2 vtResult;
            float fAlpha = 360.0f / (float)iTotal * (float)index + 90;
            vtResult.X = v2Center.X + (float)iRadius * (float)Math.Cos(fAlpha * MathHelper.Pi / 180.0f);
            vtResult.Y = v2Center.Y + (float)iRadius * (float)Math.Sin(fAlpha * MathHelper.Pi / 180.0f);

            //chuyen tu toa do Oxy sang toa do man hinh
            vtResult.Y = GlobalVar.glViewport.Y - vtResult.Y;

            return vtResult;
        }
        public static Vector2 GetClosingMenuItemPosition(Vector2 v2Center, int index, int iTotal, int iStep, int nStep)
        {
            Vector2 vtResult = v2Center - (v2Center - GetOpenedMenuItemPosition(v2Center, index, iTotal)) / (float)nStep * ((float)nStep - (float)iStep);
            return vtResult;
        }

        #endregion
    }
}
