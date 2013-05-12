using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

using WMPLib;

namespace ProjectSunshine
{
    using Logic;
    using Drawing;
    using Sound;

    public partial class frmMain : Form
    {
        Bitmap btmp;
        Graphics g;
        Font font;
        MyDrawing Draw;
        Space space;
        DateTime now;
        DateTime lastCurrent;

        StageDirector director;

        int x;
        int y;

        bool shooting = false;

        WindowsMediaPlayer wmp;
        MySoundPlayer pl;

        Random m_rnd;

        public frmMain()
        {
            InitializeComponent();
            
            //Инициализация всех компонентов для рисования.
            //Фактически, полная загрузка приложения.
            btmp = new Bitmap(pictureBox.Width, pictureBox.Height);
            g = Graphics.FromImage(btmp);
            font = new Font("Segoe print", 12);
            pictureBox.Image = btmp;

            wmp = new WindowsMediaPlayer();
            pl = new MySoundPlayer();
            wmp.URL = "Music\\menu.mp3";
            wmp.settings.setMode("Loop", true);
            wmp.settings.volume = 100;

            m_rnd = new Random();

            //ship = new Ship(pictureBox.Width, pictureBox.Height);
            space = new Space(pictureBox.Width, pictureBox.Height);
            Draw = new MyDrawing(g, space.GetWidth, space.GetHeight);

            director = new StageDirector(pictureBox.Width, pictureBox.Height);

            x = space.GetWidth / 2;
            y = space.GetHeight - 100;

            pictureBox.Cursor.Dispose();
            Draw.Menu(pictureBox.Width, pictureBox.Height);

            now = DateTime.Now;
            lastCurrent = now;
        }

        private void pictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (timerMain.Enabled)
            {
                if (e.X > space.GetShip.GetWidth / 2)
                    if (e.X < space.GetWidth - space.GetShip.GetWidth / 2)
                        x = e.X;
                if (e.Y > space.GetShip.GetHeight / 2)
                    if (e.Y < space.GetHeight - space.GetShip.GetHeight / 2)
                        y = e.Y;
            }
        }

        private void timerMain_Tick(object sender, EventArgs e)
        {
            //Отлов события уничтожения корабля
            if (space.GetShip.Resurrection == true)
            {
                Cursor.Position = new Point(
                    frmMain.ActiveForm.Location.X + space.GetShip.Xpoint, 
                    frmMain.ActiveForm.Location.Y + space.GetShip.Ypoint);
                space.GetShip.Resurrection = false;
                //x = space.GetShip.Xpoint;
                //y = space.GetShip.Ypoint;
            }
            else
            {
                space.GetShip.Move(x, y);
            }

            now = DateTime.Now;

            if (shooting)
                if (now.Subtract(space.GetShip.GetLastShot).Milliseconds > 500)
                {
                    space.GetShip.GetLastShot = now;

                    if (space.GetShip.Shoot())
                        pl.PlayShot();
                }

            director.Recalculate(space, now);
            space.Recalculate(now);
            if (!director.Playing)
            {
                timerDrawing.Enabled = false;
                timerMain.Enabled = false;
                Draw.GameOver(pictureBox.Width, pictureBox.Height);
                pictureBox.Invalidate();
            }
        }


        private void pictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                shooting = true;
            }
        }

        private void frmMain_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space && !timerMain.Enabled)
            {
                director.Start(space, DateTime.Now);
                wmp.controls.stop();
                timerMain.Start();
                timerDrawing.Start();
                Cursor.Position = new Point(
                            frmMain.ActiveForm.Location.X + space.GetShip.Xpoint,
                            frmMain.ActiveForm.Location.Y + space.GetShip.Ypoint);
            }
            if (director.Playing)
            {
                if (e.KeyCode == Keys.P)
                {
                    timerMain.Enabled = !timerMain.Enabled;
                    timerDrawing.Enabled = !timerDrawing.Enabled;
                    if (timerMain.Enabled)
                    {
                        director.MusicStart();
                        Cursor.Position = new Point(
                            frmMain.ActiveForm.Location.X + space.GetShip.Xpoint,
                            frmMain.ActiveForm.Location.Y + space.GetShip.Ypoint);
                    }
                    else
                    {
                        director.MusicPause();
                    }
                }
            }
            if (e.KeyCode == Keys.Escape)
            {
                director.MusicStop();
                director.Stop(space);
                timerMain.Enabled = false;
                timerDrawing.Enabled = false;
                wmp.controls.play();
                Draw.Menu(pictureBox.Width, pictureBox.Height);
                pictureBox.Invalidate();
            }
            if (e.KeyCode == Keys.F1)
                if (!director.Playing)
                {
                    Draw.About(pictureBox.Width, pictureBox.Height);
                    pictureBox.Invalidate();
                }
        }

        private void pictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                shooting = false;
        }

        private void timerDrawing_Tick(object sender, EventArgs e)
        {
            Draw.Space(space);
            pictureBox.Invalidate();
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Утилизация WMP.
            //Есть сомнения в целесообразности этого кода
            wmp.close();
            Marshal.FinalReleaseComObject(wmp); 
            wmp = null;
            GC.Collect();
        }
    }
}
