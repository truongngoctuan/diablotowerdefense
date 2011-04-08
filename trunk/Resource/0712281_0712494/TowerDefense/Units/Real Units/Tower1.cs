using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Xml;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TowerDefense;

namespace TowerDefense.Units.Real_Units
{
    public class Tower1 : Tower
    {
        static BulletParticleSystem bulletParticleSystem;
        static string strResourceFolder;
        static int sNumSprite;
        static int sBaseSprite;
        static int sCost;
        static int sMaxLife;
        static int sMaxEnergy;
        static int sBaseRange;
        static int sBaseDamage;
        static int sBaseAttackSpeed;
        static int sIntervalTime;
        static DamageType sDamageType = DamageType.Normal;
        static Effect sEffect;        

        public Tower1()
        {
            this._vt2Position = new Vector2(0,0);
            this._fDepth = 1.0f;
            this._iTimeTillNextShot = 0; 

            this._iSprite = 0;
            this._nSprite = Tower1.sNumSprite;
            this._iFirstSprite = Tower1.sBaseSprite;            

            this._nCost = Tower1.sCost;
            this._iMaxLife = Tower1.sMaxLife;
            this._iMaxEnergy = Tower1.sMaxEnergy;
            this._iBaseDamage = Tower1.sBaseDamage;
            this._iBaseAttackSpeed = Tower1.sBaseAttackSpeed;
            this._iBaseRange = Tower1.sBaseRange;
            this._damageType = Tower1.sDamageType;
            this._iBaseIntervalTime = Tower1.sIntervalTime;

            this._iLife = this._iMaxLife;
            this._iEnergy = this._iMaxEnergy;            
            this._iDamage = this._iBaseDamage;            
            this._iRange = this._iBaseRange;            
            this._iAttackSpeed = this._iBaseAttackSpeed;
            this._bSelected = false;

            this._effect = null;
        }

        public Tower1(Vector2 vt2Position)
        {
            this._vt2Position = vt2Position;
            this._fDepth = 1.0f;
            this._iTimeTillNextShot = 0;

            this._iSprite = 0;
            this._nSprite = Tower1.sNumSprite;
            this._iFirstSprite = Tower1.sBaseSprite;

            this._nCost = Tower1.sCost;
            this._iMaxLife = Tower1.sMaxLife;
            this._iMaxEnergy = Tower1.sMaxEnergy;
            this._iBaseDamage = Tower1.sBaseDamage;
            this._iBaseAttackSpeed = Tower1.sBaseAttackSpeed;
            this._iBaseRange = Tower1.sBaseRange;
            this._damageType = Tower1.sDamageType;
            this._iBaseIntervalTime = Tower1.sIntervalTime;

            this._iLife = this._iMaxLife;
            this._iEnergy = this._iMaxEnergy;
            this._iDamage = this._iBaseDamage;
            this._iRange = this._iBaseRange;
            this._iAttackSpeed = this._iBaseAttackSpeed;
            this._bSelected = false;

            this._effect = null;
        }

        public static void Initialize(XmlNodeList xmlNodeList)
        {           
            foreach (XmlNode xmlNode in xmlNodeList)
            {
                switch (xmlNode.Name)
                {
                    case "Tower":
                        {
                            foreach (XmlNode xmlProperty in xmlNode)
                            {
                                switch (xmlProperty.Name)
                                {
                                    case "NumSprites":
                                        {
                                            sBaseSprite = ResourceManager.nTowerSprites;
                                            sNumSprite = int.Parse(xmlProperty.InnerText);
                                            ResourceManager.nTowerSprites += sNumSprite;
                                            break;
                                        }
                                    case "ResourceFolder":
                                        {
                                            strResourceFolder = xmlProperty.InnerText;
                                            break;
                                        }
                                    case "MaxLife":
                                        {
                                            sMaxLife = int.Parse(xmlProperty.InnerText);
                                            break;
                                        }
                                    case "MaxEnergy":
                                        {
                                            sMaxEnergy = int.Parse(xmlProperty.InnerText);
                                            break;
                                        }
                                    case "Cost":
                                        {
                                            sCost = int.Parse(xmlProperty.InnerText);
                                            break;
                                        }
                                    case "BaseDamage":
                                        {
                                            sBaseDamage = int.Parse(xmlProperty.InnerText);
                                            break;
                                        }
                                    case "BaseRange":
                                        {
                                            sBaseRange = int.Parse(xmlProperty.InnerText);
                                            break;
                                        }
                                    case "BaseAttackSpeed":
                                        {
                                            sBaseAttackSpeed = int.Parse(xmlProperty.InnerText);
                                            break;
                                        }
                                    case "DamageType":
                                        {
                                            sDamageType = Str2DamageType(xmlProperty.InnerText);
                                            break;
                                        }
                                    case "IntervalTime":
                                        {
                                            sIntervalTime = int.Parse(xmlProperty.InnerText);
                                            break;
                                        }
                                }
                            }
                            break;
                        }
                    case "Bullet":
                        {
                            int iBaseSprite = ResourceManager.nTowerSprites;
                            int iNumBulletSprite = 0;
                            string strBulletResourceFolder = null;
                            foreach (XmlNode xmlProperty in xmlNode)
                            {
                                switch (xmlProperty.Name)
                                {
                                    case "NumSprites":
                                        {
                                            iNumBulletSprite = int.Parse(xmlProperty.InnerText);
                                            break;
                                        }
                                    case "ResourceFolder":
                                        {
                                            strBulletResourceFolder = xmlProperty.InnerText;
                                            break;
                                        }
                                }
                            }
                            ResourceManager.nTowerSprites += iNumBulletSprite;
                            bulletParticleSystem = new BulletParticleSystem(iBaseSprite, iNumBulletSprite, strBulletResourceFolder);

                            ParticleSystemManager.particleSystems.Add(bulletParticleSystem);
                            break;
                        }
                }
            }
        }
       

        public override void LoadResource()
        {
            int iCurrentSprite = sBaseSprite;
            string[] strSprites = System.IO.Directory.GetFiles(strResourceFolder);
            for (int i = 0; i < _nSprite; i++)
            {
                string strPath = strSprites[i].Substring(8, strSprites[0].IndexOf('.') - 8);
                ResourceManager._rsTowerSprites[iCurrentSprite++] = GlobalVar.glContentManager.Load<Texture2D>(strPath);
            }

            _iHeight = (int)(ResourceManager._rsTowerSprites[_iFirstSprite].Height * _fScale);
            _iWidth = (int)(ResourceManager._rsTowerSprites[_iFirstSprite].Width * _fScale);
            bulletParticleSystem.LoadContent();
        }       
        

        public override void Update(GameTime gameTime, KeyboardState keyboardState, MouseState mouseState)
        {
            CheckSelected(mouseState);
            int iDelta = gameTime.ElapsedGameTime.Milliseconds;
            _iTimeTillNextShot -= iDelta;

            if (_iTimeTillNextUpdate < 0)
            {              
                _iSprite = (_iSprite + 1) % _nSprite;
                _iTimeTillNextUpdate = _iBaseIntervalTime;
            }
            else
            {
                _iTimeTillNextUpdate -= iDelta;
            }

            for (int i = 0; i < bullets.Count; )
            {
                if (bullets[i].bHit)
                    bullets.Remove(bullets[i]);
                else
                    i++;
            }            

            foreach (Bullet bullet in bullets)
            {
                bullet.Update(gameTime);
            }

            bulletParticleSystem.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Texture2D[] imgSprites = ResourceManager._rsTowerSprites;
            int iSprite = _iFirstSprite + _iSprite;

            ResourceManager.Draw(spriteBatch, imgSprites[iSprite], new Vector2(imgSprites[iSprite].Width / 2, imgSprites[iSprite].Height / 2), Position, _fScale, _fDepth);
        }

        public override Unit Clone(Vector2 vt2Position)
        {
            Unit newTower = new Tower1(vt2Position);
            GlobalVar.SetWorldCell(vt2Position, _iRange);
            return newTower;
        }

        public override void TargetCreep(Creep creep)
        {
            if (_iTimeTillNextShot < 0)
            {
                if (GetDistance(creep.Position) <= _iRange)
                {
                    bullets.Add(new Bullet(this, _vt2Position, creep, bulletParticleSystem));
                }
                _iTimeTillNextShot = _iAttackSpeed;
            }
        }

        public override void Hit(Creep creep)
        {
            AudioPlayer.PlaySoundEffect();
            if (_iDamage > creep.IDefense)
            {
                creep.ILife = creep.ILife + creep.IDefense - _iDamage;
                creep.Hit();
            }
        }
    }
}
