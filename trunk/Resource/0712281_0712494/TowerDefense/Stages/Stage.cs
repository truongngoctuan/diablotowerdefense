using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using System.Xml;

namespace TowerDefense
{
    public class Stage
    {
        public string _strMapFile;
        public string _strPrototypeUnitsFile;
        public string _strBuildingsFile;
        Map _map;

        public Stage()
        {
        }

        public void LoadStage()
        {
            _map = new Map(_strMapFile);
            _map.LoadMap(GlobalVar.glContentManager);

            #region TimDuongDI
            GlobalVar.glCurrentMap = _map;
            #endregion

            GlobalVar.glUnitManager.LoadPrototypeUnits(_strPrototypeUnitsFile);
            GlobalVar.glUnitManager.LoadBuildings(_strBuildingsFile);
        }

        private void Dispose()
        {
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _map.Draw(spriteBatch);
        }

        public void Update(GameTime gameTime, KeyboardState keyboardState, MouseState mouseState)
        {
            _map.Update(gameTime);
        }
    }
}
