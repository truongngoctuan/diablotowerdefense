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
    public class StageManager
    {
        public int _iCurrentStage;
        public List<Stage> _stageList;

        public StageManager()
        {
            _stageList = new List<Stage>();
            _iCurrentStage = 0;
        }

        public void LoadStages(string strStageManagerFile)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(strStageManagerFile);
            XmlNodeList xmlStageList = xmlDoc.GetElementsByTagName("Stage");

            foreach (XmlNode xmlStage in xmlStageList)
            {
                Stage newStage = new Stage();
                XmlNodeList xmlNodeList = xmlStage.ChildNodes;
                foreach (XmlNode xmlNode in xmlNodeList)
                {
                    switch (xmlNode.Name)
                    {
                        case "Map":
                            {
                                newStage._strMapFile = xmlNode.InnerText;
                                break;
                            }
                        case "PrototypeUnits":
                            {
                                newStage._strPrototypeUnitsFile = xmlNode.InnerText;
                                break;
                            }
                        case "Units":
                            {
                                newStage._strBuildingsFile = xmlNode.InnerText;
                                break;
                            }
                    }
                }
                _stageList.Add(newStage);
            }
        }

        public void GoToStage(int iNewStage)
        {
            _iCurrentStage = iNewStage;
            _stageList[_iCurrentStage].LoadStage();
            GlobalVar.glWorldSpace = new WorldSpace((int)GlobalVar.glMapSize.X, (int)GlobalVar.glMapSize.Y);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _stageList[_iCurrentStage].Draw(spriteBatch);
        }

        public void Update(GameTime gameTime, KeyboardState keyboardState, MouseState mouseState)
        {
            _stageList[_iCurrentStage].Update(gameTime, keyboardState, mouseState);
        }
    }
}
