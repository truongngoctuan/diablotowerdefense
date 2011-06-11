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
using TowerDefense.Menu;

namespace TowerDefense
{
    /// <summary>
    /// control animation of menu item
    /// depend on state of main menu such as: opening, opened, losing, closed
    /// future: convert this state to classes by using state pattern, but, this will make this class more complex
    /// </summary>
    public class MyAnimatedMenu : AbstractSubjectForm, Base.BaseGameTemplate
    {
        #region NewItem
        MenuItemNode _CurrentNode = null;

        public MenuItemNode CurrentNode
        {
            get { return _CurrentNode; }
            set { _CurrentNode = value; }
        }

        public Vector2 glViewport;
        #endregion

        #region oldcode
        

        MenuAnimationState menuAnimationState;
        
        MyAnimatedMenu _pParent;
        public bool IsRootMain()
        {
            if (_pParent != null)
            {
                return false;
            }
            return true;
        }
        //List<MenuItem> _menuItems;
        
        int _nOpeningStep;
        int _iOpeningStep;

        int _nOpeningMenuItemStep;
        int _iOpeningMenuItemStep;
        int _iOpeningMenuItem;

        int _nClosingMenuItemStep;
        int _iClosingMenuItemStep;

        

        string _strXmlFileName;

        public static Texture2D[] _rsTexture2Ds;

        public MyAnimatedMenu(string xmlFilename)
        {
            _pParent = null;
            //_menuItems = new List<MenuItem>();
            _strXmlFileName = xmlFilename;
        }

        public MyAnimatedMenu(MyAnimatedMenu parent)
        {
            _pParent = parent;
            //_menuItems = new List<MenuItem>();
        }

        public MyAnimatedMenu UpToParent()
        {
            if (_pParent == null)
                return this;
            _pParent.menuAnimationState = MenuAnimationState.Opening;
            return _pParent;
        }
        #endregion

        #region Center
        Vector2 _vtCenter;
        Color _colorCenter;
        Texture2D _imgCenter;
        float _frotateCenter;
        #endregion

        #region BaseGameTemplate Members
        public void Initialize()
        {
            glViewport = new Vector2(GlobalVar.glGraphics.GraphicsDevice.Viewport.Width, GlobalVar.glGraphics.GraphicsDevice.Viewport.Height);
            //ReadXML(_strXmlFileName);
            CurrentNode = UltiMenuItemNode.ReadXML(_strXmlFileName);

            menuAnimationState = MenuAnimationState.Opening;
            _vtCenter = new Vector2(glViewport.X / 2, glViewport.Y / 2);

            _nOpeningStep = 50;
            _iOpeningStep = 0;

            _nOpeningMenuItemStep = 50;
            _iOpeningMenuItemStep = 0;
            _iOpeningMenuItem = 0;

            _nClosingMenuItemStep = 50;
            _iClosingMenuItemStep = 0;

            _frotateCenter = 0;

            AtachAllChildNode();
        }

        public void AtachAllChildNode()
        {
            for (int i = 0; i < CurrentNode.Children.Count; i++)
            {
                Atach(CurrentNode.Children[i]);
            }
        }

        public void DetachAll()
        {
            Observer.Clear();
        }

        public void LoadContent(ContentManager content)
        {
            _rsTexture2Ds = new Texture2D[4];
            _rsTexture2Ds[0] = content.Load<Texture2D>(@"Menu\Background");
            _rsTexture2Ds[1] = content.Load<Texture2D>(@"Menu\CenterItem");
            _rsTexture2Ds[2] = content.Load<Texture2D>(@"Menu\MenuItem");
            _rsTexture2Ds[3] = content.Load<Texture2D>(@"Menu\MenuItem_Hovered");

            _imgCenter = _rsTexture2Ds[1];

            //chi can load 1 lan
            CurrentNode.LoadContent(content);
        }

        public void Update(GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState();
            switch (menuAnimationState)
            {
                case MenuAnimationState.Opening:
                    {
                        if (_iOpeningStep < _nOpeningStep)
                        {
                            _iOpeningStep++;
                            _colorCenter = new Color(Color.White, 1.0f * (float)_iOpeningStep / (float)_nOpeningStep);
                        }
                        else
                        {
                            _iOpeningStep = 0;
                            menuAnimationState = MenuAnimationState.OpeningMenuItems;
                        }
                        break;
                    }
                case MenuAnimationState.OpeningMenuItems:
                    {
                        if (_iOpeningMenuItemStep < _nOpeningMenuItemStep)
                        {
                            _iOpeningMenuItemStep++;
                            CurrentNode.Children[_iOpeningMenuItem].Position = UltiMenuItemNode.GetOpeningMenuItemPosition(_vtCenter, _iOpeningMenuItem, CurrentNode.Children.Count, _iOpeningMenuItemStep, _nOpeningMenuItemStep);
                            CurrentNode.Children[_iOpeningMenuItem].ColorMask = new Color(Color.White, 1.0f * (float)_iOpeningMenuItemStep / (float)_nOpeningMenuItemStep);
                        }
                        else
                        {
                            _iOpeningMenuItemStep = 0;
                            _iOpeningMenuItem++;//cho chay tung doi tuong
                            if (_iOpeningMenuItem == CurrentNode.Children.Count)
                            {
                                _iOpeningMenuItem = 0;
                                menuAnimationState = MenuAnimationState.Opened;
                            }
                        }

                        break;
                    }
                case MenuAnimationState.Opened:
                    {
                        UpdateAllObserver(gameTime);

                        for (int i = 0; i < CurrentNode.Children.Count; i++)
                        {
                            if (CurrentNode.Children[i].bSelected)
                            {
                                menuAnimationState = MenuAnimationState.Closing;
                            }
                        }
                        break;
                    }
                case MenuAnimationState.Closing:
                    {
                        if (_iClosingMenuItemStep < _nClosingMenuItemStep)
                        {
                            _iClosingMenuItemStep++;

                            //update center
                            _frotateCenter = (float)_iClosingMenuItemStep / (float)_nClosingMenuItemStep * 360.0f;

                            // Draw OpeningMenuItems
                            for (int i = 0; i < CurrentNode.Children.Count; i++)
                            {
                                CurrentNode.Children[i].Position = UltiMenuItemNode.GetClosingMenuItemPosition(_vtCenter, i, CurrentNode.Children.Count, _iClosingMenuItemStep, _nClosingMenuItemStep);
                                CurrentNode.Children[i].ColorMask = new Color(Color.White, 1.0f - 1.0f * (float)_iClosingMenuItemStep / (float)_nClosingMenuItemStep);
                            }
                            
                        }
                        else
                        {
                            _iClosingMenuItemStep = 0;
                            menuAnimationState = MenuAnimationState.Closed;
                        }
                        break;
                    }
                case MenuAnimationState.Closed:
                    {
                        //find selected
                        //if node:
                        //current node --> selected node
                        //reset state and var

                        //if task:
                        //change to next state

                        for (int i = 0; i < CurrentNode.Children.Count; i++)
                        {
                            if (CurrentNode.Children[i].bSelected)
                            {
                                CurrentNode.Children[i].EventHandler();
                                CurrentNode.Children[i].bSelected = false;

                                if (CurrentNode.Children[i].menuItemType == MenuItemType.SubMenu)
                                {
                                    //clean, reset
                                    CurrentNode = (MenuItemNode)CurrentNode.Children[i];
                                }
                            }
                        }
                        break;
                    }
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //draw background
            spriteBatch.Draw(_rsTexture2Ds[0],
                                new Rectangle(0, 0, (int)glViewport.X, (int)glViewport.Y),
                                Color.White);

            switch (menuAnimationState)
            {
                case MenuAnimationState.Opening:
                    {
                        // Draw center image
                        spriteBatch.Draw(_imgCenter, _vtCenter, null, _colorCenter, 0.0f,
                            new Vector2(_imgCenter.Width / 2, _imgCenter.Height / 2), 1.0f, SpriteEffects.None, 1.0f);
                        break;
                    }
                case MenuAnimationState.OpeningMenuItems:
                    {
                        // Draw center image
                        spriteBatch.Draw(_imgCenter, _vtCenter, null, Color.White, 0.0f,
                            new Vector2(_imgCenter.Width / 2, _imgCenter.Height / 2), 1.0f, SpriteEffects.None, 1.0f);

                        // Draw Opening menu items
                        for (int i = 0; i <= _iOpeningMenuItem; i++)
                        {
                            CurrentNode.Children[i].Draw(gameTime, spriteBatch);
                        }
                        break;
                    }
                case MenuAnimationState.Opened:
                    {
                        // Draw Menu
                        spriteBatch.Draw(_imgCenter, _vtCenter, null, Color.White, 0.0f,
                            new Vector2(_imgCenter.Width / 2, _imgCenter.Height / 2), 1.0f, SpriteEffects.None, 1.0f);

                        // Draw OpenedMenuItems
                        for (int i = 0; i < CurrentNode.Children.Count; i++)
                        {
                            CurrentNode.Children[i].Draw(gameTime, spriteBatch);
                        }
                        break;
                    }
                case MenuAnimationState.Closing:
                    {
                        // Draw center
                        spriteBatch.Draw(_imgCenter, _vtCenter, null, Color.White, _frotateCenter,
                            new Vector2(_imgCenter.Width / 2, _imgCenter.Height / 2), 1.0f, SpriteEffects.None, 1.0f);

                        // Draw closing menu items
                        for (int i = 0; i < CurrentNode.Children.Count; i++)
                        {
                            CurrentNode.Children[i].Draw(gameTime, spriteBatch);
                        }

                        break;
                    }
                case MenuAnimationState.Closed:
                    {
                        break;
                    }
            }
        }

        #endregion
    }
}
