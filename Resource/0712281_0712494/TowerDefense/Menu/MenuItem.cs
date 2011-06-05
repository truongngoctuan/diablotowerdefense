using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace TowerDefense
{
    public class MenuItem
    {
        static Texture2D _menuItemBG;
        static Texture2D _menuItemBG_Hovered;
        static SpriteFont _font;
        static int _iRadius;
        public int iRadius
        {
            get { return _iRadius; }
            set { _iRadius = value; }
        }

        bool _bSelected;
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
       
        public MyAnimatedMenu _subMenu = null;

        GameStage _gameStage;  
        MenuItemState menuItemState;
        MenuItemType menuItemType;

        public static void LoadResource()
        {
            _menuItemBG = GameState.MainMenuGameState._rsTexture2Ds[2];
            _menuItemBG_Hovered = GameState.MainMenuGameState._rsTexture2Ds[3];
            _font = ResourceManager._rsFonts[0];
            _iRadius = _menuItemBG.Width / 2;
        }

        public MenuItem(MyAnimatedMenu submenu, string str)
        {
            menuItemType = MenuItemType.SubMenu;
            menuItemState = MenuItemState.Normal;
            _subMenu = submenu;
            _Name = str;
            _bSelected = false;
        }

        public MenuItem(GameStage stage, string str)
        {
            menuItemType = MenuItemType.Task;
            menuItemState = MenuItemState.Normal;
            _gameStage = stage;
            _Name = str;
            _bSelected = false;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, Color color)
        {
            switch (menuItemState)
            {
                case MenuItemState.Normal:
                    {
                        spriteBatch.Draw(_menuItemBG, position, null, color, 0.0f,
                            new Vector2(_menuItemBG.Width / 2, _menuItemBG.Height / 2), 1.0f, SpriteEffects.None, 1.0f);
                        break;
                    }
                case MenuItemState.Hovered:
                    {
                        spriteBatch.Draw(_menuItemBG_Hovered, position, null, color, 0.0f,
                            new Vector2(_menuItemBG.Width / 2, _menuItemBG.Height / 2), 1.0f, SpriteEffects.None, 1.0f);
                        break;
                    }
                case MenuItemState.Pressed:
                    {
                        spriteBatch.Draw(_menuItemBG_Hovered, position, null, color, 0.0f,
                            new Vector2(_menuItemBG.Width / 2, _menuItemBG.Height / 2), 0.8f, SpriteEffects.None, 1.0f);
                        break;
                    }
                case MenuItemState.Released:
                    {
                        spriteBatch.Draw(_menuItemBG_Hovered, position, null, color, 0.0f,
                            new Vector2(_menuItemBG.Width / 2, _menuItemBG.Height / 2), 1.0f, SpriteEffects.None, 1.0f);
                        break;
                    }
            }
            
            Vector2 textSize = new Vector2();
            textSize.X = _font.MeasureString(_Name).X;
            textSize.Y = _font.MeasureString(_Name).Y;
            spriteBatch.DrawString(_font, _Name, position, color, 0.0f, textSize/2, 1.0f, SpriteEffects.None, 1.0f);
        }

        public void Update(MouseState mouseState, Vector2 position)
        {
            if ((position.X - _iRadius < mouseState.X && mouseState.X < position.X + _iRadius)
                && (position.Y - _iRadius < mouseState.Y && mouseState.Y < position.Y + _iRadius))
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
                        GameState.MainMenuGameState.glAnimatedMenu = _subMenu;
                        break;
                    }
            }
        }
    }
}
