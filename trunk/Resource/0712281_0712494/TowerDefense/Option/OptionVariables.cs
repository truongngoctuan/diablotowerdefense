using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace TowerDefense.Option
{
    public class OptionVariables
    {
        public RadioButton radioFullScreen;
        public RadioButton radioMuteSound;
        public VolumeButton volumebt;

        public TowerDefense.Option.RadioButton.OptionRadioState FullScreenState;
        public TowerDefense.Option.RadioButton.OptionRadioState MuteSoundState;

        public float fVolume;

        public bool bIsEnableVolume = true;

        public void ReadFromFile()
        {
            //khởi tạo
            FileStream fStream;

            fStream = new FileStream(@"Content\Options\option.conf",
                FileMode.Open,
                FileAccess.Read);

            StreamReader sr = new StreamReader(fStream);

            //lấy khung
            string strBuffer;

            strBuffer = sr.ReadLine();
            FullScreenState = (TowerDefense.Option.RadioButton.OptionRadioState)int.Parse(strBuffer);

            strBuffer = sr.ReadLine();
            MuteSoundState = (TowerDefense.Option.RadioButton.OptionRadioState)int.Parse(strBuffer);

            strBuffer = sr.ReadLine();
            fVolume = (float)int.Parse(strBuffer) * 0.1f;

            sr.Close();
            fStream.Close();
        }

        public void WriteToFile()
        {
            //khởi tạo
            FileStream fStream;

            fStream = new FileStream(@"Content\Options\option.conf",
                FileMode.Create,
                FileAccess.Write);

            StreamWriter sr = new StreamWriter(fStream);

            //lấy khung

            sr.WriteLine(((int)FullScreenState).ToString());
            sr.WriteLine(((int)MuteSoundState).ToString());
            sr.WriteLine(((int)(fVolume * 10f)).ToString());

            sr.Close();
            fStream.Close();
        }

        #region function

        public void MuteSound()
        {
            if (MuteSoundState == TowerDefense.Option.RadioButton.OptionRadioState.Checked)
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
            if (FullScreenState == TowerDefense.Option.RadioButton.OptionRadioState.Normal && GlobalVar.glGraphics.IsFullScreen == true)
            {
                GlobalVar.glGraphics.PreferredBackBufferWidth = 800;
                GlobalVar.glGraphics.PreferredBackBufferHeight = 600;

                GlobalVar.glViewport.X = 800;
                GlobalVar.glViewport.Y = 600;
                GlobalVar.glGraphics.ToggleFullScreen();
            }

            if (FullScreenState == TowerDefense.Option.RadioButton.OptionRadioState.Checked && GlobalVar.glGraphics.IsFullScreen == false)
            {
                GlobalVar.glGraphics.PreferredBackBufferWidth = 1024;
                GlobalVar.glGraphics.PreferredBackBufferHeight = 768;

                GlobalVar.glViewport.X = 1024;
                GlobalVar.glViewport.Y = 768;
                GlobalVar.glGraphics.ToggleFullScreen();
            }
        }
        #endregion
    }
}
