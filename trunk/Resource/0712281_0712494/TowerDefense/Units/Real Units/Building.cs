using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Xml;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TowerDefense
{
    public struct Batch
    {
        public int _nBatchSize;
        public int _nDelay;
        public int _nBatchDelay;
        public string _strUnitList;
    }

    public class Building : ActiveUnit
    {
        List<Batch> _batchList;
        TimeSpan _currentBatchTimeSpan;
        int _iCurrentBatch;
        int _iCurrentUnit;
        Vector2 _vt2Gate;       // vi tri xuat hien cua Unit
        bool _bStartGenerate;

        public Building()
        {
            _batchList = new List<Batch>();
            _iCurrentBatch = 0;
            _iCurrentUnit = 0;
            _currentBatchTimeSpan = TimeSpan.Zero;
            _bStartGenerate = false;
        }

        public void Initialize(XmlNodeList xmlNodeList)
        {
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
                    case "Position":
                        {
                            //_vt2Position.X = float.Parse(xmlNode.Attributes["X"].Value);
                            //_vt2Position.Y = float.Parse(xmlNode.Attributes["Y"].Value);
                            _vt2Position = GlobalVar.ConvertTileToPixelCenter(GlobalVar.glvt2StartTile);
                            _vt2Gate = _vt2Position;
                            break;
                        }
                    case "BatchGroup":
                        {
                            XmlNodeList xmlBatchList = xmlNode.ChildNodes;
                            foreach (XmlNode xmlBatch in xmlBatchList)
                            {
                                Batch batch = new Batch();
                                batch._nBatchSize = int.Parse(xmlBatch.Attributes["Size"].Value);
                                batch._nDelay = int.Parse(xmlBatch.Attributes["Delay"].Value);
                                batch._nBatchDelay = int.Parse(xmlBatch.Attributes["BatchDelay"].Value);
                                batch._strUnitList = xmlBatch.InnerText;

                                _batchList.Add(batch);
                            }
                            break;
                        }
                }
            }
        }

        private static void LoadDescription(string strDescriptionFile)
        {
        }

        public override void Update(GameTime gameTime, KeyboardState keyboardState, MouseState mouseState)
        {
            // đã xuất ra tất cả các Batch
            if (_iCurrentBatch >= _batchList.Count)
                return;
            
            _currentBatchTimeSpan += gameTime.ElapsedGameTime;
            if (_bStartGenerate == false)
            {
                // trước mỗi Batch có thời gian chờ, hết thời gian chờ mới được sinh lính
                if (_currentBatchTimeSpan.TotalMilliseconds >= _batchList[_iCurrentBatch]._nBatchDelay)
                {
                    _currentBatchTimeSpan -= TimeSpan.FromMilliseconds(_batchList[_iCurrentBatch]._nBatchDelay);
                    _bStartGenerate = true;
                }
            }

            if (_bStartGenerate == true)
            {
                // số lượng lính được sinh ra trong vòng lặp Update này
                int nNewUnit = _currentBatchTimeSpan.Milliseconds / _batchList[_iCurrentBatch]._nDelay;
                _currentBatchTimeSpan -= TimeSpan.FromMilliseconds(_batchList[_iCurrentBatch]._nDelay * nNewUnit);

                for (int i = 0; (i < nNewUnit) && (_iCurrentUnit < _batchList[_iCurrentBatch]._nBatchSize); i++, _iCurrentUnit++)
                {
                    int iUnitType = (int)_batchList[_iCurrentBatch]._strUnitList[_iCurrentUnit] - 48;
                    GlobalVar.glUnitManager.GenerateUnit(iUnitType, _vt2Gate);
                }

                // nếu sinh hết Batch này, thì chuyển sang Batch kế tiếp
                if (_iCurrentUnit >= _batchList[_iCurrentBatch]._nBatchSize)
                {
                    _iCurrentBatch++;
                    _iCurrentUnit = 0;
                    _bStartGenerate = false;
                }
            }            
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
        }

        public override Unit Clone(Vector2 vt2Position)
        {
            return new Building();
        }

        public override void LoadResource()
        {
            //throw new NotImplementedException();
        }

        //protected Building(Vector2 vt2Position, float fDepth, int iFirstSprite, int iSprite, int nSprite, int nIntervalTime, Effect effect, int nBatchSize, int nDelay, int nBatchDelay)
        //    : base(vt2Position, fDepth, iFirstSprite, iSprite, nSprite, nIntervalTime, effect)
        //{
        //    _nBatchSize = nBatchSize;
        //    _nDelay = nDelay;
        //    _nBatchDelay = nBatchDelay;
        //}
    }
}
