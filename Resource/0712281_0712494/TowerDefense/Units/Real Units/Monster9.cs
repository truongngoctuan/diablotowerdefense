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
    public class Monster9 : GroundCreep
    {
        static int nMovingSpriteOffset;
        static int nAttackedSpriteOffset;
        static int nDyingSpriteOffset;

        static int nMovingSpritesPerDirection;
        static int nAttackedSpritesPerDirection;
        static int nDyingSpritesPerDirection;

        static string strMovingResourceFolder;
        static string strAttackedResourceFolder;
        static string strDyingResourceFolder;

        static int iIntervalTime;
        static int iBaseMaxLife;
        static int iBaseMaxEnergy;
        static int iBaseSpeed;
        static int iBaseDefense;

        static int iBaseSprite;
        static int iHeight;
        static int iWidth;

        public static void Initialize(XmlNodeList xmlNodeList)
        {
            #region "Load Common Properties"
            foreach (XmlNode xmlNode in xmlNodeList)
            {
                switch (xmlNode.Name)
                {
                    case "Description":
                        {
                            string strDescriptionFile = xmlNode.InnerText;
                            LoadDescription(strDescriptionFile);
                            break;
                        }
                    case "IntervalTime":
                        {
                            Monster9.iIntervalTime = int.Parse(xmlNode.InnerText);
                            break;
                        }
                    case "MaxLife":
                        {
                            Monster9.iBaseMaxLife = int.Parse(xmlNode.InnerText);
                            break;
                        }
                    case "MaxEnergy":
                        {
                            Monster9.iBaseMaxEnergy = int.Parse(xmlNode.InnerText);
                            break;
                        }
                    case "Speed":
                        {
                            Monster9.iBaseSpeed = int.Parse(xmlNode.InnerText);
                            break;
                        }
                    case "Defense":
                        {
                            Monster9.iBaseDefense = int.Parse(xmlNode.InnerText);
                            break;
                        }
                }
            }
            #endregion
        }

        private static void LoadDescription(string strDescriptionFile)
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
                            iBaseSprite = ResourceManager.nCreepSprites;
                            int nTotalSprite = int.Parse(xmlNode.InnerText);
                            ResourceManager.nCreepSprites += nTotalSprite;
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

            ReadDataFromOffsetFile();
        }

        protected override void ChangeState(State newState)
        {
            #region "Change State => Sprite"
            _state = newState;
            //int iOrientation = (int)Enum.Parse(typeof(Orientation), _orientation.ToString());
            int iOrientation = (int)this._orientation;
            switch (_state)
            {
                case State.Moving:
                    {
                        _iFirstSprite = iBaseSprite + nMovingSpriteOffset + iOrientation * nMovingSpritesPerDirection;
                        _iSprite = 0;
                        _nSprite = nMovingSpritesPerDirection;
                        break;
                    }
                case State.Attacked:
                    {
                        _iFirstSprite = iBaseSprite + nAttackedSpriteOffset + iOrientation * nAttackedSpritesPerDirection;
                        _iSprite = 0;
                        _nSprite = nAttackedSpritesPerDirection;
                        break;
                    }
                case State.Dying:
                    {
                        _iFirstSprite = iBaseSprite + nDyingSpriteOffset + iOrientation * nDyingSpritesPerDirection;
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

        public Monster9(Vector2 vt2Position)
        {
            // gán giá trị static (nhập vào từ file) cho object mới
            this._vt2Position = vt2Position;

            this._iMaxLife = Monster9.iBaseMaxLife;
            this._iMaxEnergy = Monster9.iBaseMaxEnergy;
            this._iBaseSpeed = Monster9.iBaseSpeed;
            this._iBaseDefense = Monster9.iBaseDefense;
            this._iBaseIntervalTime = Monster9.iIntervalTime;

            this._iLife = this._iMaxLife;
            this._iEnergy = this._iMaxEnergy;
            this._iSpeed = this._iBaseSpeed;
            this.IDefense = this._iBaseDefense;

            this._bSelected = false;
            this._iHeight = Monster9.iHeight;
            this._iWidth = Monster9.iWidth;
            this._effect = null;
            this._fDepth = 1.0f;

            this._state = State.Moving;
            this._creepState = CreepState.Normal;
            //PickParticleDirection();
            ChangeState(State.Moving);
        }

        private void ChangeDirection()
        {
            #region "Change Direction => Sprite"
            //int iOrientation = (int)Enum.Parse(typeof(Orientation), _orientation.ToString());
            int iOrientation = (int)this._orientation;
            switch (_state)
            {
                case State.Moving:
                    {
                        _iFirstSprite = iBaseSprite + nMovingSpriteOffset + iOrientation * nMovingSpritesPerDirection;
                        _nSprite = nMovingSpritesPerDirection;
                        break;
                    }
                case State.Attacked:
                    {
                        _iFirstSprite = iBaseSprite + nAttackedSpriteOffset + iOrientation * nAttackedSpritesPerDirection;
                        _nSprite = nAttackedSpritesPerDirection;
                        break;
                    }
                case State.Dying:
                    {
                        _iFirstSprite = iBaseSprite + nDyingSpriteOffset + iOrientation * nDyingSpritesPerDirection;
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
                            PickParticleDirection();
                            ChangeDirection();

                            _iSprite = (_iSprite + 1) % _nSprite;
                            _vt2Position += _iSpeed * _vt2Direction;

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

        //public override void Draw(SpriteBatch spriteBatch)
        //{
        //    Texture2D[] imgSprites = ResourceManager._rsCreepSprites;
        //    int iSprite = _iFirstSprite + _iSprite;

        //    ResourceManager.Draw(spriteBatch, imgSprites[iSprite], new Vector2(imgSprites[iSprite].Width / 2, imgSprites[iSprite].Height / 2), _vt2Position, _fScale, _fDepth);
        //}

        public override Unit Clone(Vector2 vt2Position)
        {
            //return new Monster3(_vt2Position, _direction,_iSprite);
            return new Monster9(vt2Position);
        }


        #region BonusOffset
        //tạo các biến lưu trư offset tương ứng cho từng frame
        //hàm đọc file offset và lưu biến
        //cập nhật lại hàm Draw
        static List<Vector2[]> _vt2OffsetSprite;
        static List<Vector2> _vt2BoundSprite = new List<Vector2>();//khung chứa sprite, width height
        static string _strOffsetFilename = "MonsOffset.ini";

        int iSpriteNonOffset;
        //làm 2 chuyện:
        //cập nhật non offset và cập nhật top left position frame, iwdth, height
        void ChangeSpriteNonOffset()
        {
            if (_state == State.Died)
            {
                return;
            }

            int iSprite = _iFirstSprite + _iSprite;
            iSpriteNonOffset = iSprite - iBaseSprite;
            switch (_state)
            {
                case State.Moving:
                    {
                        iSpriteNonOffset -= nMovingSpriteOffset;
                        break;
                    }
                case State.Attacked:
                    {
                        iSpriteNonOffset -= nAttackedSpriteOffset;
                        break;
                    }
                case State.Dying:
                    {
                        iSpriteNonOffset -= nDyingSpriteOffset;
                        break;
                    }
            }

            if (_vt2BoundSprite != null &&
                _vt2OffsetSprite != null)
            {
                _vt2CurrentPositionTopLeftOfFrame = _vt2Position
                    + (-_vt2BoundSprite[(int)_state] / 2
                    + _vt2OffsetSprite[(int)_state][iSpriteNonOffset]) * _fScale;

                Texture2D[] imgSprites = ResourceManager._rsCreepSprites;

                _iWidth = (int)(imgSprites[iSprite].Width * _fScale);
                _iHeight = (int)(imgSprites[iSprite].Height * _fScale);
            }
        }
        static Vector2[] ReadDataFromOffsetFile(string strFileName)
        {
            //khởi tạo
            FileStream fStream;

            fStream = new FileStream(strFileName,
                FileMode.Open,
                FileAccess.Read);

            StreamReader sr = new StreamReader(fStream);

            //lấy khung
            string strBuffer;
            strBuffer = sr.ReadLine();
            string[] strBoundSpliter = strBuffer.Split(new char[] { ' ' });
            _vt2BoundSprite.Add(new Vector2(int.Parse(strBoundSpliter[0]), int.Parse(strBoundSpliter[1])));

            //lấy số lương cần chạy
            strBuffer = sr.ReadLine();
            int iCount = int.Parse(strBuffer);

            Vector2[] vt2Offset = new Vector2[iCount];

            //đọc từng cụm monster
            for (int i = 0; i < iCount; i++)
            {
                strBuffer = sr.ReadLine();
                string[] strOffsetSpliter = strBuffer.Split(new char[] { ' ' });

                vt2Offset[i] = new Vector2(int.Parse(strOffsetSpliter[1]), int.Parse(strOffsetSpliter[2]));
            }

            sr.Close();
            fStream.Close();

            return vt2Offset;
        }

        static void ReadDataFromOffsetFile()
        {
            _vt2OffsetSprite = new List<Vector2[]>();

            _vt2OffsetSprite.Add(ReadDataFromOffsetFile(strMovingResourceFolder + "\\" + _strOffsetFilename));
            _vt2OffsetSprite.Add(ReadDataFromOffsetFile(strAttackedResourceFolder + "\\" + _strOffsetFilename));
            _vt2OffsetSprite.Add(ReadDataFromOffsetFile(strDyingResourceFolder + "\\" + _strOffsetFilename));
        }

        //public override void Draw(SpriteBatch spriteBatch)
        //{
        //    Texture2D[] imgSprites = ResourceManager._rsCreepSprites;
        //    int iSprite = _iFirstSprite + _iSprite;

        //    ResourceManager.Draw(spriteBatch, imgSprites[iSprite], new Vector2(imgSprites[iSprite].Width / 2, imgSprites[iSprite].Height / 2), _vt2Position, _fScale, _fDepth);
        //}

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (_state == State.Died)
            {
                return;
            }

            Texture2D[] imgSprites = ResourceManager._rsCreepSprites;
            int iSprite = _iFirstSprite + _iSprite;

            //Vector2 vt2NewPosition = _vt2Position 
            //    - _vt2BoundSprite[(int)_state] / 2
            //    + _vt2OffsetSprite[(int)_state][iSpriteNonOffset];

            //ResourceManager.Draw(spriteBatch,
            //    imgSprites[iSprite],
            //    new Vector2(0, 0),
            //    vt2NewPosition, 1.0f,
            //    _fDepth);

            ResourceManager.Draw(spriteBatch,
                imgSprites[iSprite],
                new Vector2(0, 0),
                _vt2CurrentPositionTopLeftOfFrame, _fScale,
                _fDepth);
        }
        #endregion
    }
}
