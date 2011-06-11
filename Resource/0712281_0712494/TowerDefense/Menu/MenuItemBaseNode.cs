using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using TowerDefense.Option;

namespace TowerDefense.Menu
{
    public abstract class MenuItemBaseNode:AbstractObserver, Base.BaseGameTemplate
    {
        #region base properties
        protected Vector2 _vt2Position;

        public Vector2 Position
        {
            get { return _vt2Position; }
            set { _vt2Position = value; }
        }

        private int _iWidth;

        public int Width
        {
            get { return _iWidth; }
            set { _iWidth = value; }
        }
        private int _iHeight;

        public int Height
        {
            get { return _iHeight; }
            set { _iHeight = value; }
        }
        #endregion

        private MenuItemBaseNode _parent;

        public MenuItemBaseNode Parent
        {
            get { return _parent; }
            set { _parent = value; }
        }

        static Texture2D _menuItemBG;
        static Texture2D _menuItemBG_Hovered;
        static SpriteFont _font;
        public static int _iRadius;
        public int iRadius
        {
            get { return _iRadius; }
            set { _iRadius = value; }
        }

        public bool _bSelected;
        public bool bSelected
        {
            get { return _bSelected; }
            set { _bSelected = value; }
        }      
        
        string _Name;
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        GameStage _gameStage;

        public GameStage GameStage
        {
            get { return _gameStage; }
            set { _gameStage = value; }
        }  
       
        public MyAnimatedMenu _subMenu = null;
        public MenuItemState menuItemState;
        public MenuItemType menuItemType;

        public Vector2 _vtCenter;

        public MenuItemBaseNode()
        {
        }

        public MenuItemBaseNode(MyAnimatedMenu submenu, string str)
        {
            menuItemType = MenuItemType.SubMenu;
            menuItemState = MenuItemState.Normal;
            _subMenu = submenu;
            _Name = str;
            _bSelected = false;
        }

        public MenuItemBaseNode(GameStage stage, string str)
        {
            menuItemType = MenuItemType.Task;
            menuItemState = MenuItemState.Normal;
            _gameStage = stage;
            _Name = str;
            _bSelected = false;

            //LoadResource();
        }

        public int _iSelectedItem;

        public int _nOpeningStep = 50;
        public int _iOpeningStep;

        public int _nOpeningMenuItemStep = 50;
        public int _iOpeningMenuItemStep;
        public int _iOpeningMenuItem;

        public int _nClosingMenuItemStep = 50;
        public int _iClosingMenuItemStep;

        public MenuAnimationState AnimationState;
        //public List<MenuItem> _menuItems;

        private Color _colorMask;

        public Color ColorMask
        {
            get { return _colorMask; }
            set { _colorMask = value; }
        }

        //public abstract void Update(MouseState mouseState, Vector2 position);

        public void EventHandler()
        {
            switch (menuItemType)
            {
                case MenuItemType.Task:
                    {
                        //GlobalVar.SetGameStage(this._gameStage);
                        ((GameState.MainMenuGameState)GlobalVar.glGame.CurrentGameState).NextState(ref GlobalVar.glGame, this._gameStage);
                        break;
                    }
                case MenuItemType.SubMenu:
                    {
                        //GameState.MainMenuGameState.glAnimatedMenu = _subMenu;
                        break;
                    }
            }
        }

        #region BaseGameTemplate Members

        public void Initialize()
        {
            //throw new NotImplementedException();
            ColorMask = Color.White;
        }

        public void LoadContent(Microsoft.Xna.Framework.Content.ContentManager content)
        {//chi can load 1 lan
            _menuItemBG = MyAnimatedMenu._rsTexture2Ds[2];
            _menuItemBG_Hovered = MyAnimatedMenu._rsTexture2Ds[3];
            _font = ResourceManager._rsFonts[0];
            _iRadius = _menuItemBG.Width / 2;
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            MouseState mouseState = Mouse.GetState();

            //(x- x0)^2 + (y - y0)^2 = r^2
            if(Math.Pow((mouseState.X - Position.X), 2) + Math.Pow((mouseState.Y - Position.Y), 2) < _iRadius * _iRadius)
            //if ((position.X - _iRadius < mouseState.X && mouseState.X < position.X + _iRadius)
            //    && (position.Y - _iRadius < mouseState.Y && mouseState.Y < position.Y + _iRadius))
            {
                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    menuItemState = MenuItemState.Pressed;
                }
                else if (mouseState.LeftButton == ButtonState.Released && menuItemState == MenuItemState.Pressed)
                {
                    menuItemState = MenuItemState.Released;
                    _bSelected = true;
                    AudioPlayer.PlaySoundEffect();
                }
                else
                {
                    menuItemState = MenuItemState.Hovered;
                }
            }
            else
                menuItemState = MenuItemState.Normal;  
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            switch (menuItemState)
            {
                case MenuItemState.Normal:
                    {
                        spriteBatch.Draw(_menuItemBG, Position, null, ColorMask, 0.0f,
                            new Vector2(_menuItemBG.Width / 2, _menuItemBG.Height / 2), 1.0f, SpriteEffects.None, 1.0f);
                        break;
                    }
                case MenuItemState.Hovered:
                    {
                        spriteBatch.Draw(_menuItemBG_Hovered, Position, null, ColorMask, 0.0f,
                            new Vector2(_menuItemBG.Width / 2, _menuItemBG.Height / 2), 1.0f, SpriteEffects.None, 1.0f);
                        break;
                    }
                case MenuItemState.Pressed:
                    {
                        spriteBatch.Draw(_menuItemBG_Hovered, Position, null, ColorMask, 0.0f,
                            new Vector2(_menuItemBG.Width / 2, _menuItemBG.Height / 2), 0.8f, SpriteEffects.None, 1.0f);
                        break;
                    }
                case MenuItemState.Released:
                    {
                        spriteBatch.Draw(_menuItemBG_Hovered, Position, null, ColorMask, 0.0f,
                            new Vector2(_menuItemBG.Width / 2, _menuItemBG.Height / 2), 1.0f, SpriteEffects.None, 1.0f);
                        break;
                    }
            }

            Vector2 textSize = new Vector2();
            textSize.X = _font.MeasureString(_Name).X;
            textSize.Y = _font.MeasureString(_Name).Y;
            spriteBatch.DrawString(_font, _Name, Position, ColorMask, 0.0f, textSize / 2, 1.0f, SpriteEffects.None, 1.0f);
        }

        #endregion
    }
}
