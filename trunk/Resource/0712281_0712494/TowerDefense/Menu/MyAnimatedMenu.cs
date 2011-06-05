using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using System.Numeric;

namespace TowerDefense
{
    public class MyAnimatedMenu
    {
        static Texture2D _imgCenterItem;

        MyAnimatedMenuState myAnimatedMenuState;
        
        MyAnimatedMenu _pParent;
        public bool IsRootMain()
        {
            if (_pParent != null)
            {
                return false;
            }
            return true;
        }
        List<MenuItem> _menuItems;
        Vector2 _vtCenter;
        int _iSelectedItem;

        int _nOpeningStep;
        int _iOpeningStep;

        int _nOpeningMenuItemStep;
        int _iOpeningMenuItemStep;
        int _iOpeningMenuItem;

        int _nClosingMenuItemStep;
        int _iClosingMenuItemStep;

        public MyAnimatedMenu(string xmlFilename)
        {
            _pParent = null;
            _menuItems = new List<MenuItem>();
            ReadXML(xmlFilename);
        }

        public MyAnimatedMenu(MyAnimatedMenu parent)
        {
            _pParent = parent;
            _menuItems = new List<MenuItem>();
        }

        public void ReadXML(string xmlFilename)
        {
            XmlTextReader reader = new XmlTextReader(xmlFilename);
            while(reader.Read())
                if (reader.NodeType == XmlNodeType.Element)
                    if (reader.Name == "Menu")
                    {
                        ReadMenuItem(reader);
                        break;
                    }
        }

        public void ReadMenuItem(XmlTextReader reader)
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
                                    MenuItem menuItem = new MenuItem(gameStage, name);
                                    _menuItems.Add(menuItem);
                                }
                                else if (type == "SubMenu")
                                {
                                    MyAnimatedMenu myMenu = new MyAnimatedMenu(this);
                                    myMenu.ReadMenuItem(reader);
                                    MenuItem menuItem = new MenuItem(myMenu, name);
                                    _menuItems.Add(menuItem);
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
            Initialize();
        }

        public static void LoadResource()
        {
            _imgCenterItem = GameState.MainMenuGameState._rsTexture2Ds[1];
        }

        public void Initialize()
        {
            myAnimatedMenuState = MyAnimatedMenuState.Opening;
            _iSelectedItem = -1;
            _vtCenter = new Vector2(GlobalVar.glViewport.X / 2, GlobalVar.glViewport.Y / 2);

            _nOpeningStep = 50;
            _iOpeningStep = 0;

            _nOpeningMenuItemStep = 50;
            _iOpeningMenuItemStep = 0;
            _iOpeningMenuItem = 0;

            _nClosingMenuItemStep = 50;
            _iClosingMenuItemStep = 0;
        }

        Vector2 GetOpeningMenuItemPosition(Vector2 vtCenter, int nMenuItems, int _iIndex, int _nStep, int _iStep)
        {
            int iRadius = 200;
            float fAlpha;//góc alpha của dưởng thẳng qua tâm, cắt đường tròn
            float fAlphaQuater1;

            fAlpha = 360.0f / (float)nMenuItems * (float)_iIndex + 90;
            //====
            //bổ sung: tạo hiệu ứng từ từ chuyển sang vị trí mới củ từng menuitems

            int iLastIndex = _iIndex - 1;
            if (iLastIndex < 0)
            {
                iLastIndex = 0;
            }
            float fLastAlpha = 360.0f / (float)nMenuItems * (float)iLastIndex + 90;

            fAlpha = fLastAlpha + (fAlpha - fLastAlpha) * (float)_iStep / (float)_nStep;

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
            vtResult.X += vtCenter.X;
            vtResult.Y += vtCenter.Y;

            //chuyen tu toa do Oxy sang toa do man hinh
            vtResult.Y = GlobalVar.glViewport.Y - vtResult.Y;

            return vtResult;
        }
        Vector2 GetOpenedMenuItemPosition(Vector2 vtCenter, int nMenuItems, int _iIndex)
        {
            int iRadius = 200;
            Vector2 vtResult;
            float fAlpha = 360.0f / (float)nMenuItems * (float)_iIndex + 90;
            vtResult.X = vtCenter.X + (float)iRadius * (float)Math.Cos(fAlpha * MathHelper.Pi / 180.0f);
            vtResult.Y = vtCenter.Y + (float)iRadius * (float)Math.Sin(fAlpha * MathHelper.Pi / 180.0f);

            //chuyen tu toa do Oxy sang toa do man hinh
            vtResult.Y = GlobalVar.glViewport.Y - vtResult.Y;
            
            return vtResult;
        }

        Vector2 GetOpeningMenuItemPosition(int index)
        {
            if (index == 0)
                return GetOpenedMenuItemPosition(index);
            int iRadius = 200;
            float fAlpha;//góc alpha của dưởng thẳng qua tâm, cắt đường tròn
            float fAlphaQuater1;

            fAlpha = 360.0f / (float)_menuItems.Count * (float)index + 90;
            //====
            //bổ sung: tạo hiệu ứng từ từ chuyển sang vị trí mới củ từng menuitems

            int iLastIndex = index - 1;
            float fLastAlpha = 360.0f / (float)_menuItems.Count * (float)iLastIndex + 90;

            fAlpha = fLastAlpha + (fAlpha - fLastAlpha) * (float)_iOpeningMenuItemStep / (float)_nOpeningMenuItemStep;

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
            vtResult.X += _vtCenter.X;
            vtResult.Y += _vtCenter.Y;

            //chuyen tu toa do Oxy sang toa do man hinh
            vtResult.Y = GlobalVar.glViewport.Y - vtResult.Y;

            return vtResult;
        }
        Vector2 GetOpenedMenuItemPosition(int index)
        {
            int iRadius = 200;
            Vector2 vtResult;
            float fAlpha = 360.0f / (float)_menuItems.Count * (float)index + 90;
            vtResult.X = _vtCenter.X + (float)iRadius * (float)Math.Cos(fAlpha * MathHelper.Pi / 180.0f);
            vtResult.Y = _vtCenter.Y + (float)iRadius * (float)Math.Sin(fAlpha * MathHelper.Pi / 180.0f);

            //chuyen tu toa do Oxy sang toa do man hinh
            vtResult.Y = GlobalVar.glViewport.Y - vtResult.Y;

            return vtResult;
        }
        Vector2 GetClosingMenuItemPosition(int index)
        {
            Vector2 vtResult = _vtCenter - (_vtCenter - GetOpenedMenuItemPosition(index)) / _nClosingMenuItemStep * (_nClosingMenuItemStep - _iClosingMenuItemStep);
            return vtResult;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            switch (myAnimatedMenuState)
            {
                case MyAnimatedMenuState.Opening:
                    {
                        Color color = new Color(Color.White, 1.0f * (float)_iOpeningStep / (float)_nOpeningStep);
                        //float fRotate = (float)_iOpeningStep / (float)_nOpeningStep * 360.0f;
                        // Draw Menu
                        spriteBatch.Draw(_imgCenterItem, _vtCenter, null, color, 0.0f,
                            new Vector2(_imgCenterItem.Width / 2, _imgCenterItem.Height / 2), 1.0f, SpriteEffects.None, 1.0f);

                        break;
                    }
                case MyAnimatedMenuState.OpeningMenuItems:
                    {
                        // Draw Menu
                        spriteBatch.Draw(_imgCenterItem, _vtCenter, null, Color.White, 0.0f,
                            new Vector2(_imgCenterItem.Width / 2, _imgCenterItem.Height / 2), 1.0f, SpriteEffects.None, 1.0f);

                        // Draw OpenedMenuItems
                        Vector2 _vtMenuItemPos;
                        for (int i = 0; i < _iOpeningMenuItem; i++)
                        {
                            _vtMenuItemPos = GetOpenedMenuItemPosition(i);
                            _menuItems[i].Draw(spriteBatch, _vtMenuItemPos, Color.White);
                        }

                        // Draw OpeningMenuItems
                        _vtMenuItemPos = GetOpeningMenuItemPosition(_iOpeningMenuItem);
                        Color color = new Color(Color.White, 1.0f * (float)_iOpeningMenuItemStep / (float)_nOpeningMenuItemStep);
                        _menuItems[_iOpeningMenuItem].Draw(spriteBatch, _vtMenuItemPos, color);
                                                   
                        break;
                    }
                case MyAnimatedMenuState.Opened:
                    {
                        // Draw Menu
                        spriteBatch.Draw(_imgCenterItem, _vtCenter, null, Color.White, 0.0f,
                            new Vector2(_imgCenterItem.Width / 2, _imgCenterItem.Height / 2), 1.0f, SpriteEffects.None, 1.0f);

                        // Draw OpenedMenuItems
                        Vector2 _vtMenuItemPos;
                        for (int i = 0; i < _menuItems.Count; i++)
                        {
                            _vtMenuItemPos = GetOpenedMenuItemPosition(i);
                            _menuItems[i].Draw(spriteBatch, _vtMenuItemPos, Color.White);
                        }
                        break;
                    }
                case MyAnimatedMenuState.Closing:
                    {
                        float fRotate = (float)_iClosingMenuItemStep / (float)_nClosingMenuItemStep * 360.0f;
                        // Draw Menu
                        spriteBatch.Draw(_imgCenterItem, _vtCenter, null, Color.White, fRotate,
                            new Vector2(_imgCenterItem.Width / 2, _imgCenterItem.Height / 2), 1.0f, SpriteEffects.None, 1.0f);

                        // Draw OpeningMenuItems
                        Vector2 _vtMenuItemPos;
                        Color color = new Color(Color.White, 1.0f - 1.0f * (float)_iClosingMenuItemStep / (float)_nClosingMenuItemStep);
                        for (int i = 0; i < _menuItems.Count; i++)
                        {
                            _vtMenuItemPos = GetClosingMenuItemPosition(i);
                            _menuItems[i].Draw(spriteBatch, _vtMenuItemPos, color);
                        }
                        
                        break;
                    }
                case MyAnimatedMenuState.Closed:
                    {
                        break;
                    }
            }
        }

        public void Update(MouseState mouseState)
        {
            switch (myAnimatedMenuState)
            {
                case MyAnimatedMenuState.Opening:
                    {
                        if (_iOpeningStep < _nOpeningStep)
                        {
                            _iOpeningStep++;
                        }
                        else
                        {
                            _iOpeningStep = 0;
                            myAnimatedMenuState = MyAnimatedMenuState.OpeningMenuItems;
                        }
                        break;
                    }
                case MyAnimatedMenuState.OpeningMenuItems:
                    {                             
                        if (_iOpeningMenuItemStep < _nOpeningMenuItemStep)
                        {
                            _iOpeningMenuItemStep++;
                        }
                        else
                        {
                            _iOpeningMenuItemStep = 0;
                            _iOpeningMenuItem++;
                            if (_iOpeningMenuItem == _menuItems.Count)
                            {
                                _iOpeningMenuItem = 0;
                                myAnimatedMenuState = MyAnimatedMenuState.Opened;
                            }
                        }
                        break;
                    }
                case MyAnimatedMenuState.Opened:
                    {
                        for (int i = 0; i < _menuItems.Count; i++)
                        {
                            Vector2 _vtMenuItemPos = GetOpenedMenuItemPosition(i);
                            _menuItems[i].Update(mouseState, _vtMenuItemPos);
                            if (_menuItems[i].bSelected == true)
                            {
                                myAnimatedMenuState = MyAnimatedMenuState.Closing;
                                _iSelectedItem = i;
                            }
                        }
                        break;
                    }
                case MyAnimatedMenuState.Closing:
                    {
                        if (_iClosingMenuItemStep < _nClosingMenuItemStep)
                        {
                            _iClosingMenuItemStep++;
                        }
                        else
                        {
                            _iClosingMenuItemStep = 0;
                            myAnimatedMenuState = MyAnimatedMenuState.Closed;
                        }
                        break;
                    }
                case MyAnimatedMenuState.Closed:
                    {
                        _menuItems[_iSelectedItem].EventHandler();
                        _menuItems[_iSelectedItem].bSelected = false;
                        break;
                    }
            }
        }

        public MyAnimatedMenu UpToParent()
        {
            if (_pParent == null)
                return this;
            _pParent.myAnimatedMenuState = MyAnimatedMenuState.Opening;
            return _pParent;
        }
    }
}
