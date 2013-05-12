using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Media;

namespace ProjectSunshine.Sound
{
    public class MySoundPlayer
    {
        //static SoundPlayer player = new SoundPlayer();
        private SoundPlayer pl;
        public MySoundPlayer()
        {
            pl = new SoundPlayer();
            pl.SoundLocation = "Sound\\laser.wav";
            pl.LoadAsync();
        }

        public void PlayShot()
        {
            pl.Play();
        }
    }
}
