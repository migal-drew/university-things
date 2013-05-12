using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectSunshine.Logic
{
    using Aliens;

    public class StageDirector
    {
        private Random m_rnd;

        private WMPLib.WindowsMediaPlayer wmp;

        private DateTime m_lastFill;
        private DateTime m_startLevel;

        private bool m_bossFight;

        private int m_width;
        private int m_height;

        private int m_interval;

        private SpacePoint m_left;
        private SpacePoint m_center;
        private SpacePoint m_right;

        private bool m_playing;
        public bool Playing
        {
            get
            {
                return m_playing;
            }
        }

        /// <summary>
        /// Границы космоса
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public StageDirector(int width, int height)
        {
            m_width = width;
            m_height = height;

            m_interval = width / 3;

            int m_step = -40;

            m_left = new SpacePoint(m_interval / 2, m_step);
            m_right = new SpacePoint(m_width - m_interval / 2, m_step);
            m_center = new SpacePoint(m_width / 2, m_step);

            m_rnd = new Random();
            m_playing = false;

            wmp = new WMPLib.WindowsMediaPlayer();
            wmp.settings.setMode("Loop", true);
            wmp.settings.volume = 90;
        }

        public void Start(Space s, DateTime now)
        {
            s.Loading = true;
            m_playing = true;
            m_startLevel = now;
            s.Type = m_rnd.Next(3);
            wmp.URL = "Music\\" + m_rnd.Next(1, 8).ToString() + ".mp3";
        }

        public void Stop(Space s)
        {
            m_playing = false;
            s.ClearAll();
            s.InitialLevel();
            s.GetShip.Restart();
        }

        public void MusicStart()
        {
            wmp.controls.play();
        }

        public void MusicPause()
        {
            wmp.controls.pause();
        }

        public void MusicStop()
        {
            wmp.controls.stop();
        }

        private SpacePoint PositionPoint(Direction position)
        {
            if (position == Direction.Left)
                return m_left;
            if (position == Direction.Right)
                return m_right;
            else
                return m_center;
        }

        private void AddRandomEnemy(List<Alien> aliens, int x, int y, Direction position)
        {
            switch ((Enemy)m_rnd.Next(6))
            {
                case Enemy.Aircraft: aliens.Add(new Aircraft(x, y, position));
                    break;
                case Enemy.Fighter: aliens.Add(new Fighter(x, y, position));
                    break;
                case Enemy.HeavyFighter: aliens.Add(new HeavyFighter(x, y, position));
                    break;
                case Enemy.GunBoat: aliens.Add(new GunBoat(x, y, position));
                    break;
                case Enemy.Bomber: aliens.Add(new Bomber(x, y, position));
                    break;
                case Enemy.ArmoredBomber: aliens.Add(new ArmoredBomber(x, y, position));
                    break;
            }
        }

        private void One(Direction position, List<Alien> aliens)
        {
            SpacePoint p = PositionPoint(position);
            int x, y;
            x = p.X;
            y = p.Y;

            AddRandomEnemy(aliens, x, y, position);
            //aliens.Add(new Alien(x, y, 70, 65,
            //    position));
        }

        private void Two(Direction position, List<Alien> aliens)
        {
            SpacePoint p = PositionPoint(position);

            //AddRandomEnemy(aliens, p.X, p.Y + 60, position);
            AddRandomEnemy(aliens, p.X + 50, p.Y, position);
            AddRandomEnemy(aliens, p.X - 50, p.Y, position);
        }

        private void Three(Direction position, List<Alien> aliens)
        {
            SpacePoint p = PositionPoint(position);

            AddRandomEnemy(aliens, p.X, p.Y + 60, position);
            AddRandomEnemy(aliens, p.X + 50, p.Y, position);
            AddRandomEnemy(aliens, p.X - 50, p.Y, position);
        }

        public void RandomFill(List<Alien> aliens, DateTime now)
        {
            if (!m_bossFight)
            {
                if (now.Subtract(m_lastFill).Seconds > 4)
                {
                    switch (m_rnd.Next(3))
                    {
                        case 0: Two((Direction)m_rnd.Next(3), aliens);
                            break;
                        case 1: One((Direction)m_rnd.Next(3), aliens);
                            break;
                        case 2: Three((Direction)m_rnd.Next(3), aliens);
                            break;
                    }
                    m_lastFill = now;
                }
            }
        }

        public void Recalculate(Space s, DateTime now)
        {
            if (s.GetShip.GetLifes == 0 && s.GetShip.GetArmor == 0)
            {
                m_playing = false;
                return;
            }

            if (now.Subtract(m_startLevel).TotalSeconds > 5)
                s.Loading = false;

            if (now.Subtract(m_startLevel).TotalMinutes > 2)
            {
                if (!m_bossFight)
                {
                    s.GetAliens.Add(new Boss(m_width / 2, -200, Direction.Center));
                    m_bossFight = true;
                }

                if (s.GetAliens.Count == 0)
                {
                    m_bossFight = false;
                    m_startLevel = now;
                    s.NextLevel(m_rnd.Next(3));
                    s.Loading = true;
                    wmp.controls.stop();
                    wmp.URL = "Music\\" + m_rnd.Next(1, 8).ToString() + ".mp3";
                }
            }
            else
            {
                if (now.Subtract(m_startLevel).TotalSeconds > 8)
                {
                    RandomFill(s.GetAliens, now);
                }
            }
        }
    }
}
