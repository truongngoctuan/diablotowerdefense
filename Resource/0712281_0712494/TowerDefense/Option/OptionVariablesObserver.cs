using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Microsoft.Xna.Framework;

namespace TowerDefense.Option
{
    public class OptionVariablesObserver:AbstractObserver
    {
        //public TowerDefense.Option.RadioButton.OptionRadioState FullScreenState;
        //public TowerDefense.Option.RadioButton.OptionRadioState MuteSoundState;

        private float fVolume;

        public float Volume
        {
            get { return fVolume; }
            set { fVolume = value; }
        }
        private bool bISFullScreen;

        public bool IsFullScreen
        {
            get { return bISFullScreen; }
            set { bISFullScreen = value; }
        }
        private bool bIsMuteSound;

        public bool IsMuteSound
        {
            get { return bIsMuteSound; }
            set { bIsMuteSound = value; }
        }

        public void ReadFromFile()
        {
            //khởi tạo
            FileStream fStream;

            fStream = new FileStream(@"option.conf",
                FileMode.Open,
                FileAccess.Read);

            StreamReader sr = new StreamReader(fStream);

            //lấy khung
            string strBuffer;

            strBuffer = sr.ReadLine();
            //FullScreenState = (TowerDefense.Option.RadioButton.OptionRadioState)int.Parse(strBuffer);
            bISFullScreen = bool.Parse(strBuffer);

            strBuffer = sr.ReadLine();
            //MuteSoundState = (TowerDefense.Option.RadioButton.OptionRadioState)int.Parse(strBuffer);
            bIsMuteSound = bool.Parse(strBuffer);

            strBuffer = sr.ReadLine();
            fVolume = (float)int.Parse(strBuffer) * 0.1f;

            sr.Close();
            fStream.Close();
        }

        public void WriteToFile()
        {
            //khởi tạo
            FileStream fStream;

            fStream = new FileStream(@"option.conf",
                FileMode.Create,
                FileAccess.Write);

            StreamWriter sr = new StreamWriter(fStream);

            //lấy khung

            //sr.WriteLine(((int)FullScreenState).ToString());
            //sr.WriteLine(((int)MuteSoundState).ToString());
            sr.WriteLine(bISFullScreen.ToString());
            sr.WriteLine(bIsMuteSound.ToString());
            
            sr.WriteLine(((int)(fVolume * 10f)).ToString());

            sr.Close();
            fStream.Close();
        }

        #region function

        public void MuteSound()
        {
            //if (MuteSoundState == TowerDefense.Option.RadioButton.OptionRadioState.Checked)
            if (bIsMuteSound)
            {
                //mute
                AudioPlayer.Mute(true);
            }
            else
            {
                //unmute
                AudioPlayer.Mute(false);
            }
        }


        public void ChangeVolume()
        {
            AudioPlayer.ChangeVolume(fVolume);
        }

        public void ToggleFullScreen()
        {
            //bISFullScreen = !bISFullScreen;
            //if (FullScreenState == TowerDefense.Option.RadioButton.OptionRadioState.Normal && GlobalVar.glGraphics.IsFullScreen == true)
            //if (!bISFullScreen && GlobalVar.glGraphics.IsFullScreen == true)

            //GlobalVar.glGraphics.PreferredBackBufferWidth = 1366;
            //GlobalVar.glGraphics.PreferredBackBufferHeight = 768;

            //GlobalVar.glViewport.X = 1366;
            //GlobalVar.glViewport.Y = 768;

            if (bISFullScreen != GlobalVar.glGraphics.IsFullScreen)
            {
                GlobalVar.glGraphics.ToggleFullScreen();
                
            }
        }
        #endregion

        //public override void Update(string strCommand)
        //{
        //}

        //public void DoUpdateOption()
        //{
        //    ToggleFullScreen();
        //    MuteSound();
        //    ChangeVolume();
        //}

        public override void Update(GameTime gametime)
        {
            //throw new NotImplementedException();
        }
    }
}
