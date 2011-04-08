using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace TowerDefense
{
    public class AudioPlayer
    {
        static Song[] backgroundMusics;
        static SoundEffect explosion;
        static SoundEffect click;

        public static void LoadContent()
        {
            backgroundMusics = new Song[3];
            backgroundMusics[0] = GlobalVar.glContentManager.Load<Song>("Track1");
            backgroundMusics[1] = GlobalVar.glContentManager.Load<Song>("Track2");
            backgroundMusics[2] = GlobalVar.glContentManager.Load<Song>("Track3");

            explosion = GlobalVar.glContentManager.Load<SoundEffect>("explosion");
            click = GlobalVar.glContentManager.Load<SoundEffect>("click");
        }

        public static void PlayBackgroundMusic()
        {
            MediaPlayer.Stop();
            switch (GlobalVar.glGameStage)
            {
                case GameStage.MainMenu:
                    {
                        MediaPlayer.Play(backgroundMusics[2]);
                        break;
                    }
                case GameStage.Loading:
                    {
                        MediaPlayer.Play(backgroundMusics[0]);
                        break;
                    }
                case GameStage.SinglePlayer:
                    {
                        MediaPlayer.Play(backgroundMusics[1]);
                        break;
                    }
            }
            
        }

        public static void PlaySoundEffect()
        {
            switch (GlobalVar.glGameStage)
            {
                case GameStage.MainMenu:
                    {
                        click.Play();
                        break;
                    }
                case GameStage.SinglePlayer:
                    {
                        explosion.Play();
                        break;
                    }
            }
        }

        public static void Mute(bool bMute)
        {
            MediaPlayer.IsMuted = bMute;
        }

        public static void ChangeVolume(float fVolume)
        {
            MediaPlayer.Volume = fVolume;
        }
    }
}
