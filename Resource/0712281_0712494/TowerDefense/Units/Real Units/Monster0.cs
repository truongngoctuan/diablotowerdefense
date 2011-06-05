using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Xml;
using System.IO;

namespace TowerDefense
{
    public class Monster0 : GroundCreep
    {


        /// <summary>
        /// Initializes form the specified XML node list.
        /// in init, call func load description
        /// and load other properties
        /// </summary>
        /// <param name="xmlNodeList">The XML node list.</param>
        public void Initialize(XmlNodeList xmlNodeList)
        {
            #region "Load Common Properties"
            foreach (XmlNode xmlNode in xmlNodeList)
            {
                switch (xmlNode.Name)
                {
                    case "Description":
                        {
                            this.strDescriptionFile = xmlNode.InnerText;
                            LoadDescription(strDescriptionFile, ref ResourceManager.nCreepSprites);
                            break;
                        }
                    case "IntervalTime":
                        {
                            this._iBaseIntervalTime = int.Parse(xmlNode.InnerText);   
                            break;
                        }
                    case "MaxLife":
                        {
                            this._iMaxLife = int.Parse(xmlNode.InnerText);
                            break;
                        }
                    case "MaxEnergy":
                        {
                            this._iMaxEnergy = int.Parse(xmlNode.InnerText);
                            break;
                        }
                    case "Speed":
                        {
                            this._iMaxSpeed = int.Parse(xmlNode.InnerText);
                            break;
                        }
                    case "Defense":
                        {
                            this._iBaseDefense = int.Parse(xmlNode.InnerText);
                            break;
                        }
                }
            }
            #endregion
        }

        private void LoadDescription(string strDescriptionFile, ref int nCreepSprites)
        {
            #region "Load SpriteInfo"
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(strDescriptionFile);
            XmlNodeList xmlNodeList = xmlDoc.FirstChild.ChildNodes;
        
            foreach (XmlNode xmlNode in xmlNodeList)
            {
                switch (xmlNode.Name)
                {
                    case "Total":
                        {
                            iBaseSprite = nCreepSprites;
                            int nTotalSprite = int.Parse(xmlNode.InnerText);
                            nCreepSprites += nTotalSprite;
                            break;
                        }
                    case "States":
                        {
                            XmlNodeList xmlStateList = xmlNode.ChildNodes;
                            foreach (XmlNode xmlState in xmlStateList)
                            {
                                switch (xmlState.Name)
                                {
                                    case "Moving":
                                        {
                                            nMovingSpritesPerDirection = int.Parse(xmlState.Attributes["SpritePerDirection"].Value);
                                            nMovingSpriteOffset = int.Parse(xmlState.Attributes["Offset"].Value);
                                            strMovingResourceFolder = xmlState.Attributes["ResourceFolder"].Value;
                                            break;
                                        }
                                    case "Attacked":
                                        {
                                            nAttackedSpritesPerDirection = int.Parse(xmlState.Attributes["SpritePerDirection"].Value);
                                            nAttackedSpriteOffset = int.Parse(xmlState.Attributes["Offset"].Value);
                                            strAttackedResourceFolder = xmlState.Attributes["ResourceFolder"].Value;
                                            break;
                                        }
                                    case "Dying":
                                        {
                                            nDyingSpritesPerDirection = int.Parse(xmlState.Attributes["SpritePerDirection"].Value);
                                            nDyingSpriteOffset = int.Parse(xmlState.Attributes["Offset"].Value);
                                            strDyingResourceFolder = xmlState.Attributes["ResourceFolder"].Value;
                                            break;
                                        }
                                }
                            }
                            break;
                        }
                }
            }
            #endregion
        }
        /// <summary>
        /// Loads the resource.
        /// gan gia tri cho resource manager
        /// chi load 1 lan va mai mai
        /// </summary>
        public override void LoadResource()
        {
            

            #region Sprite Resource
            int iCurrentSprite = iBaseSprite;
            string[] movingSprites = System.IO.Directory.GetFiles(strMovingResourceFolder);
            for (int i = 0; i < movingSprites.Length && i < nMovingSpritesPerDirection * 8; i++)
            {
                string strPath = movingSprites[i].Substring(8, movingSprites[0].IndexOf('.') - 8);
                ResourceManager._rsCreepSprites[iCurrentSprite++] = GlobalVar.glContentManager.Load<Texture2D>(strPath);
            }

            string[] attackedSprites = System.IO.Directory.GetFiles(strAttackedResourceFolder);
            for (int i = 0; i < attackedSprites.Length && i < nAttackedSpritesPerDirection * 8; i++)
            {
                string strPath = attackedSprites[i].Substring(8, attackedSprites[0].IndexOf('.') - 8);
                ResourceManager._rsCreepSprites[iCurrentSprite++] = GlobalVar.glContentManager.Load<Texture2D>(strPath);
            }

            string[] dyingSprites = System.IO.Directory.GetFiles(strDyingResourceFolder);
            for (int i = 0; i < dyingSprites.Length && i < nDyingSpritesPerDirection * 8; i++)
            {
                string strPath = dyingSprites[i].Substring(8, dyingSprites[0].IndexOf('.') - 8);
                ResourceManager._rsCreepSprites[iCurrentSprite++] = GlobalVar.glContentManager.Load<Texture2D>(strPath);
            }
            #endregion
            
            // Giả sử các sprite kích thước bằng nhau
            iHeight = (int)(ResourceManager._rsCreepSprites[iBaseSprite].Height * _fScale);
            iWidth = (int)(ResourceManager._rsCreepSprites[iBaseSprite].Width * _fScale);

            _vt2OffsetSprite = new List<Vector2[]>();
            _vt2BoundSprite = new List<Vector2>();//khung chứa sprite, width height

            UtilReadFile.ReadDataFromOffsetFile(ref _vt2OffsetSprite, ref _vt2BoundSprite,
                strMovingResourceFolder, strAttackedResourceFolder, strDyingResourceFolder, _strOffsetFilename);
        }

        protected override void ChangeState(State newState)
        {
            #region "Change State => Sprite" 
            _state = newState;
            //int iOrientation = (int)Enum.Parse(typeof(Orientation), _orientation.ToString());
            //int iOrientation = (int)this.Orientation;
            switch (_state)
            {
                case State.Moving:
                    {
                        _iFirstSprite = iBaseSprite + nMovingSpriteOffset + (int)this.Orientation *nMovingSpritesPerDirection;
                        _iSprite = 0;
                        _nSprite = nMovingSpritesPerDirection;
                        break;
                    }
                case State.Attacked:
                    {
                        _iFirstSprite = iBaseSprite + nAttackedSpriteOffset + (int)this.Orientation *nAttackedSpritesPerDirection;
                        _iSprite = 0;
                        _nSprite = nAttackedSpritesPerDirection;
                        break;
                    }
                case State.Dying:
                    {
                        _iFirstSprite = iBaseSprite + nDyingSpriteOffset + (int)this.Orientation *nDyingSpritesPerDirection;
                        _iSprite = 0;
                        _nSprite = nDyingSpritesPerDirection;
                        break;
                    }
            }
            #endregion

            #region offset
            ChangeSpriteNonOffset();
            #endregion
        }

        public Monster0(Vector2 vt2Position)
        {
            // gán giá trị static (nhập vào từ file) cho object mới
            this._vt2Position = vt2Position;
           
            //this._iMaxLife = iBaseMaxLife;
            //this._iMaxEnergy = iBaseMaxEnergy;
            //this._iBaseSpeed = iBaseSpeed;
            //this._iBaseDefense = iBaseDefense;
            //this._iBaseIntervalTime = iIntervalTime;            
            
            this._iLife = this._iMaxLife;
            this._iEnergy = this._iMaxEnergy;
            //this._iSpeed = this._iBaseSpeed;
            this.IDefense = this._iBaseDefense;

            this._bSelected = false;
            this._iHeight = iHeight;
            this._iWidth = iWidth;            
            this._effect = null;
            this._fDepth = 1.0f;

            this._state = State.Moving;
            this._creepState = CreepState.Normal;
            //PickParticleDirection();
            ChangeState(State.Moving);
            ChangeDirection();

            _v2NextMovePoint = vt2Position;
        }

        private void ChangeDirection()
        {
            #region "Change Direction => Sprite"
            //int iOrientation = (int)Enum.Parse(typeof(Orientation), _orientation.ToString());
            switch (_state)
            {
                case State.Moving:
                    {
                        _iFirstSprite = iBaseSprite + nMovingSpriteOffset + (int)this.Orientation *nMovingSpritesPerDirection;
                        _nSprite = nMovingSpritesPerDirection;
                        break;
                    }
                case State.Attacked:
                    {
                        _iFirstSprite = iBaseSprite + nAttackedSpriteOffset + (int)this.Orientation *nAttackedSpritesPerDirection;
                        _nSprite = nAttackedSpritesPerDirection;
                        break;
                    }
                case State.Dying:
                    {
                        _iFirstSprite = iBaseSprite + nDyingSpriteOffset + (int)this.Orientation *nDyingSpritesPerDirection;
                        _nSprite = nDyingSpritesPerDirection;
                        break;
                    }
            }
            #endregion
        }

        public override void Update(GameTime gameTime, KeyboardState keyboardState, MouseState mouseState)
        {
            if (_iTimeTillNextUpdate <= 0)
            {
                switch (_state)
                {
                    case State.Moving:
                        {
                            //Debug.Logging("Math.Abs(_v2NextMovePoint.Length() - _vt2Position.Length()) < _iSpeed: " + Math.Abs(_v2NextMovePoint.Length() - _vt2Position.Length()).ToString() + 
                            //    " " + _iSpeed.ToString());
                            if (Math.Abs(_v2NextMovePoint.Length() - _vt2Position.Length()) <= _iSpeed)
                            {
                                PickParticleDirection();
                                ChangeDirection();
                                //Debug.Logging("after PickParticleDirection: " + this._vt2Direction.X.ToString() + this._vt2Direction.Y.ToString());
                                //Debug.Logging("after ChangeDirection: " + this._orientation);
                            }
                            _iSprite = (_iSprite + 1) % _nSprite;
                            _vt2Position += _iSpeed * Direction;

                            if (GlobalVar.GetWorldCell(_vt2Position) > 0)
                            {
                                GlobalVar.glUnitManager.TargetCreep(this);
                            }
                            break;
                        }
                    case State.Attacked:
                        {
                            if (_iSprite == _nSprite - 1)
                            {
                                ChangeState(State.Moving);
                            }
                            else
                            {
                                _iSprite = (_iSprite + 1) % _nSprite;
                            }
                            break;
                        }
                    case State.Dying:
                        {
                            if (_iSprite == _nSprite - 1)
                            {
                                ChangeState(State.Died);
                            }
                            else
                            {
                                _iSprite = (_iSprite + 1) % _nSprite;
                            }
                            break;
                        }
                    case State.Died:
                        {
                            return;
                        }
                }

                //cập nhật biến iSpriteNonOffset, biến này dựa vào cái state
                //có thể lấy đc offset tấm hình + width + height
                ChangeSpriteNonOffset();
 
                _iTimeTillNextUpdate = _iBaseIntervalTime;
            }
            else
            {
                _iTimeTillNextUpdate -= gameTime.ElapsedGameTime.Milliseconds;
            }
        }


        public override Unit Clone(Vector2 vt2Position)
        {            
            //return new Monster3(_vt2Position, _direction,_iSprite);
            Monster0 m0 = this;
            m0._vt2Position = vt2Position;
            m0._v2NextMovePoint = vt2Position;

            this._iSpeed = this._iMaxSpeed;
            return m0;
            //return new Monster0(vt2Position);

        }
    }
}
       