using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Xml;
using TowerDefense.Units.Real_Units;

namespace TowerDefense
{
    public class UnitManager
    {
        Texture2D listTowerBackground;
        List<Unit> _Units;
        List<Unit> _prototypeUnits;
        List<Unit> _prototypeTowers;
        List<Unit> _towers;
        List<Unit> _buildings;
        List<Unit> _creeps;

        Tower _selectedTower;

        public UnitManager()
        {
            _Units = new List<Unit>();
            _prototypeUnits = new List<Unit>();
            _prototypeTowers = new List<Unit>();
            _towers = new List<Unit>();
            _buildings = new List<Unit>();
            _creeps = new List<Unit>();

            _selectedTower = null;
            listTowerBackground = GlobalVar.glContentManager.Load<Texture2D>("TowerList_background");
        }

        public void LoadPrototypeUnits(string strPrototypeUnitFile)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(strPrototypeUnitFile);

            XmlNodeList xmlUnitList = xmlDoc.FirstChild.ChildNodes;
            foreach (XmlNode xmlUnit in xmlUnitList)
            {
                switch (xmlUnit.Name)
                {
                    case "Monster0":
                        {
                            Monster0 m0 = new Monster0(new Vector2());
                            m0.Initialize(xmlUnit.ChildNodes);
                            //m0.LoadResource();
                            _prototypeUnits.Add(m0);
                            break;
                        }
                    //case "Monster1":
                    //    {
                    //        Monster1.Initialize(xmlUnit.ChildNodes);
                    //        _prototypeUnits.Add(new Monster1(new Vector2()));
                    //        break;
                    //    }
                    //case "Monster2":
                    //    {
                    //        Monster2.Initialize(xmlUnit.ChildNodes);
                    //        _prototypeUnits.Add(new Monster2(new Vector2()));
                    //        break;
                    //    }
                    //case "Monster3":
                    //    {
                    //        Monster3.Initialize(xmlUnit.ChildNodes);
                    //        _prototypeUnits.Add(new Monster3(new Vector2()));
                    //        break;
                    //    }
                    //case "Monster4":
                    //    {
                    //        Monster4.Initialize(xmlUnit.ChildNodes);
                    //        _prototypeUnits.Add(new Monster4(new Vector2()));
                    //        break;
                    //    }
                    //case "Monster6":
                    //    {
                    //        Monster6.Initialize(xmlUnit.ChildNodes);
                    //        _prototypeUnits.Add(new Monster6(new Vector2()));
                    //        break;
                    //    }
                    //case "Monster8":
                    //    {
                    //        Monster8.Initialize(xmlUnit.ChildNodes);
                    //        _prototypeUnits.Add(new Monster8(new Vector2()));
                    //        break;
                    //    }
                    //case "Monster9":
                    //    {
                    //        Monster9.Initialize(xmlUnit.ChildNodes);
                    //        _prototypeUnits.Add(new Monster9(new Vector2()));
                    //        break;
                    //    }
                    default:
                        {
                            break;
                        }
                }
            }
            LoadCreepResource();
        }

        public void LoadBuildings(string strBuildingsFile)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(strBuildingsFile);

            XmlNodeList xmlUnitList = xmlDoc.FirstChild.ChildNodes;
            foreach (XmlNode xmlUnit in xmlUnitList)
            {
                switch (xmlUnit.Name)
                {
                    case "Building":
                        {
                            Building building = new Building();
                            building.Initialize(xmlUnit.ChildNodes);
                            _buildings.Add(building);
                            building.LoadResource();
                            break;
                        }
                }
            }
        }

        private Vector2 GetIconPosition(int index)
        {
            Vector2 iconPosition = new Vector2();
            iconPosition.X = GlobalVar.glIconPosition.X;
            iconPosition.Y = GlobalVar.glIconPosition.Y + (GlobalVar.glIconSize.Y + 10) * index;
            return iconPosition;
        }

        public void LoadTowers(string strTowersFile)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(strTowersFile);

            XmlNodeList xmlUnitList = xmlDoc.FirstChild.ChildNodes;
            foreach (XmlNode xmlUnit in xmlUnitList)
            {
                switch (xmlUnit.Name)
                {
                    case "Tower1":
                        {
                            Tower1.Initialize(xmlUnit.ChildNodes);
                            Tower1 tower = new Tower1(GetIconPosition(_prototypeTowers.Count));
                            _prototypeTowers.Add(tower);
                            break;
                        }
                    case "Tower2":
                        {
                            Tower2.Initialize(xmlUnit.ChildNodes);
                            Tower2 tower = new Tower2(GetIconPosition(_prototypeTowers.Count));
                            _prototypeTowers.Add(tower);
                            break;
                        }
                    case "Tower3":
                        {
                            Tower3.Initialize(xmlUnit.ChildNodes);
                            Tower3 tower = new Tower3(GetIconPosition(_prototypeTowers.Count));
                            _prototypeTowers.Add(tower);
                            break;
                        }
                    case "Tower4":
                        {
                            Tower4.Initialize(xmlUnit.ChildNodes);
                            Tower4 tower = new Tower4(GetIconPosition(_prototypeTowers.Count));
                            _prototypeTowers.Add(tower);
                            break;
                        }
                    case "Tower5":
                        {
                            Tower5.Initialize(xmlUnit.ChildNodes);
                            Tower5 tower = new Tower5(GetIconPosition(_prototypeTowers.Count));
                            _prototypeTowers.Add(tower);
                            break;
                        }
                }
            }
            ResourceManager._rsTowerSprites = new Texture2D[ResourceManager.nTowerSprites];
            foreach (Tower tower in _prototypeTowers)
            {
                tower.LoadResource();
                tower.IHeight = (int)GlobalVar.glIconSize.Y;
                tower.IWidth = (int)GlobalVar.glIconSize.X;
            }
        }

        public void TargetCreep(Creep creep)
        {
            foreach(Tower tower in _towers)
            {
                tower.TargetCreep(creep);
            }
        }

        public void LoadCreepResource()
        {
            ResourceManager._rsCreepSprites = new Texture2D[ResourceManager.nCreepSprites];
            GlobalVar.glLoadScreen.enumerator = GlobalVar.glLoadScreen.loader.LoadCreepResource(this._prototypeUnits);
        }

        //public Unit GetUnit(string strUnitName)
        //{
        //    Unit newUnit = null;
        //    switch (strUnitName)
        //    {
        //        case "Angle":
        //            {
        //                newUnit = new Angle();
        //                break;
        //            }
        //        case "Monster3":
        //            {
        //                newUnit = new Monster3(new Vector2(400, 300), Direction.BottomRight, 0);
        //                break;
        //            }
        //    }
        //    return newUnit;
        //}

        public void GenerateUnit(int index, Vector2 vt2Position)
        {
            _creeps.Add(_prototypeUnits[index].Clone(vt2Position));
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < _creeps.Count; i++)
            {
                _creeps[i].Draw(spriteBatch);
            }
            for (int i = 0; i < _buildings.Count; i++)
            {
                _buildings[i].Draw(spriteBatch);
            }
            for (int i = 0; i < _towers.Count; i++)
            {
                _towers[i].Draw(spriteBatch);
            }

            if (_selectedTower != null)
            {
                _selectedTower.DrawModel(spriteBatch);
            }

            spriteBatch.Draw(listTowerBackground, new Rectangle((int)(GlobalVar.glViewport.X - GlobalVar.glIconSize.X - 30), 0, (int)(GlobalVar.glIconSize.X + 30), (int)(GlobalVar.glIconSize.Y + 10) * _prototypeTowers.Count + 30), Color.White);
            for (int i = 0; i < _prototypeTowers.Count; i++)
            {
                (_prototypeTowers[i] as Tower).DrawIcon(spriteBatch);
            }
        }
        public void Update(GameTime gameTime, KeyboardState keyboardState, MouseState mouseState)
        {
            
            for (int i = 0; i < _prototypeTowers.Count; i++)
            {
                Tower tower = _prototypeTowers[i] as Tower;
                if(tower != null)
                {
                    tower.CheckSelected(mouseState);
                    if ((_prototypeTowers[i] as Tower).BSelected)
                    {
                        _selectedTower = _prototypeTowers[i] as Tower;
                        break;
                    }
                }
                if (i == _prototypeTowers.Count)
                    _selectedTower = null;
            }

            for (int i = 0; i < _creeps.Count; i++)
            {
                Creep creep = _creeps[i] as Creep;
                if (creep != null)
                {
                    if (creep.State == State.Died)
                    {
                        _creeps.Remove(creep);
                        i--;
                    }
                    else
                    {
                        _creeps[i].Update(gameTime, keyboardState, mouseState);
                    }
                }
            }
            for (int i = 0; i < _buildings.Count; i++)
            {
                _buildings[i].Update(gameTime, keyboardState, mouseState);
            }
            for (int i = 0; i < _towers.Count; i++)
            {
                _towers[i].Update(gameTime, keyboardState, mouseState);
            }
        }

        public void AddSelectedTower(MouseState mouseState)
        {
            _towers.Add(_selectedTower.Clone(new Vector2(mouseState.X, mouseState.Y) + GlobalVar.glRootCoordinate));
            _selectedTower = null;
        }
    }
}
