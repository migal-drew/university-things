using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using WMPLib;

namespace ProjectSunshine.Logic
{
    using Bonuses;

    public class Space
    {
        private static WindowsMediaPlayer pl = new WindowsMediaPlayer();
        private static Random m_rnd = new Random();

        private int m_width;
        public int GetWidth
        {
            get
            {
                return m_width;
            }
        }
        private int m_height;
        public int GetHeight
        {
            get
            {
                return m_height;
            }
        }

        private List<Bullet> m_enemyBullets;
        public List<Bullet> GetEnemyBullets
        {
            get
            {
                return m_enemyBullets;
            }
        }

        private List<Alien> m_aliens;
        public List<Alien> GetAliens
        {
            get
            {
                return m_aliens;
            }
        }

        private Ship m_ship;
        public Ship GetShip
        {
            get
            {
                return m_ship;
            }
        }

        private int m_counterBonus;
        private int m_needForDrop;
        private Bonus m_bonus;
        public Bonus GetBonus
        {
            get
            {
                return m_bonus;
            }
        }

        private List<Bonus> m_bonuses;
        public List<Bonus> GetBonuses
        {
            get
            {
                return m_bonuses;
            }
        }

        private int m_type;
        public int Type
        {
            get
            {
                return m_type;
            }
            set
            {
                m_type = value;
            }
        }

        private int m_level;
        public int GetLevel
        {
            get
            {
                return m_level;
            }
        }

        private bool m_loading;
        public bool Loading
        {
            get
            {
                return m_loading;
            }
            set
            {
                m_loading = value;
            }
        }

        /// <summary>
        /// Parameters: Размеры космоса(PictureBox)
        /// </summary>
        public Space(int width, int height)
        {
            m_type = 0;
            m_level = 1;

            m_ship = new Ship(width / 2, height - 50, 40, 68);
            m_width = width;
            m_height = height;
            m_aliens = new List<Alien>();
            m_enemyBullets = new List<Bullet>();

            m_counterBonus = 0;
            m_needForDrop = 5;
            m_bonuses = new List<Bonus>();
            m_bonuses.Add(new Battery());
            m_bonuses.Add(new Spanner());
            m_bonuses.Add(new EnhanceWeapon());
            m_bonuses.Add(new Life());
            m_bonuses.Add(new Invulnerability());

            pl.settings.volume = 70;
        }

        public void NextLevel(int type)
        {
            m_level++;
            m_type = type;
        }

        public void InitialLevel()
        {
            m_level = 1;
        }

        /// <summary>
        /// Расчет удвоенной площади треугольника
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        private int DoubleSquareTriangle(SpacePoint[] points)
        {
            return Math.Abs((points[0].X - points[2].X) * (points[1].Y - points[2].Y) -
                (points[1].X - points[2].X) * (points[0].Y - points[2].Y));
        }

        private int DoubleSquareTriangle(int x1, int y1, int x2, int y2, int x3, int y3)
        {
            return Math.Abs((x1 - x3) * (y2 - y3) - (x2 - x3) * (y1 - y3));
        }

        /// <summary>
        /// Объекты считаются прямоугольниками
        /// </summary>
        /// <param name="so1"></param>
        /// <param name="so2"></param>
        /// <returns></returns>
        public bool Collision(SpaceObject so1, SpaceObject so2)
        {
            if (Math.Abs(so1.Ypoint - so2.Ypoint) < (so1.GetHeight + so2.GetHeight) / 2)
                if (Math.Abs(so1.Xpoint - so2.Xpoint) < (so1.GetWidth + so2.GetWidth) / 2)
                    return true;
            return false;
        }

        /// <summary>
        /// Корабль рассматривается как треугольник, снаряд - как точка.
        /// Расчет через площади треугольников
        /// </summary>
        /// <param name="s"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public bool Collision(Ship s, Bullet b)
        {
            if (Math.Abs(b.Ypoint - s.Ypoint) < s.GetHeight / 2)
            {
                if (Math.Abs(b.Xpoint - s.Xpoint) < s.GetWidth / 2)
                {
                    if (
                    DoubleSquareTriangle(
                                b.Xpoint, b.Ypoint,
                                s.GetPoints[1].X, s.GetPoints[1].Y,
                                s.GetPoints[2].X, s.GetPoints[2].Y) +
                    DoubleSquareTriangle(
                                s.GetPoints[0].X, s.GetPoints[0].Y,
                                b.Xpoint, b.Ypoint,
                                s.GetPoints[2].X, s.GetPoints[2].Y) +
                    DoubleSquareTriangle(
                                s.GetPoints[0].X, s.GetPoints[0].Y,
                                s.GetPoints[1].X, s.GetPoints[1].Y,
                                b.Xpoint, b.Ypoint)
                    == s.GetDoubleSquare)
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Корабль - треугольник, противник - прямоугольник.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        public bool Collision(Ship s, Alien a)
        {
            if (Math.Abs(s.Ypoint - a.Ypoint) <= (s.GetHeight + a.GetHeight) / 2)
                if (Math.Abs(s.Xpoint - a.Xpoint) <= (s.GetWidth + a.GetWidth) / 2)
                {
                    foreach (SpacePoint p in s.GetPoints)
                        if (Math.Abs(p.Y - a.Ypoint) <= a.GetWidth / 2)
                            if (Math.Abs(p.X - a.Xpoint) <= a.GetHeight / 2)
                                return true; //Возможный выход из метода!

                    foreach (SpacePoint p in a.GetPoints)
                        if (
                            DoubleSquareTriangle(
                                        p.X, p.Y,
                                        s.GetPoints[1].X, s.GetPoints[1].Y,
                                        s.GetPoints[2].X, s.GetPoints[2].Y) +
                            DoubleSquareTriangle(
                                        s.GetPoints[0].X, s.GetPoints[0].Y,
                                        p.X, p.Y,
                                        s.GetPoints[2].X, s.GetPoints[2].Y) +
                            DoubleSquareTriangle(
                                        s.GetPoints[0].X, s.GetPoints[0].Y,
                                        s.GetPoints[1].X, s.GetPoints[1].Y,
                                        p.X, p.Y)
                            == s.GetDoubleSquare)
                            return true;    
                }
            return false;
        }

        /// <summary>
        /// Столкновение снаряда и противника. Снаряд - как точка.
        /// </summary>
        /// <param name="b"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        public bool Collision(Bullet b, Alien a)
        {
            if (Math.Abs(a.Ypoint - b.Ypoint) < a.GetHeight / 2)
                if (Math.Abs(a.Xpoint - b.Xpoint) < a.GetWidth / 2)
                    return true;

            return false;
        }

        public void Recalculate(DateTime now)
        {
            m_ship.Recalculate(now);
            //Баллистика собственных снарядов
            foreach (Bullet b in m_ship.GetAmmunition)
            {
                if (!b.IsCharged)
                    b.Fly();
                if (b.Ypoint < 0 || b.Xpoint < 0 || b.Xpoint > m_width)
                    b.IsCharged = true;
            }

            //Баллистика снарядов врагов
            for (int i = 0; i < m_enemyBullets.Count; i++)
            {
                m_enemyBullets[i].Fly();
                if (m_enemyBullets[i].Ypoint > GetHeight)
                    m_enemyBullets.RemoveAt(i);
            }

            //Враги и бонусы
            for (int i = 0; i < m_aliens.Count; i++)
            {
                if (m_aliens[i].GetHealth == 0)
                {
#warning !!!!!!!!!!!
                    
                    if (now.Subtract(m_aliens[i].TimeOfExplosion).Milliseconds > 500)
                    {
                        m_ship.AddCash(m_aliens[i].GetCash);
                        m_aliens.RemoveAt(i);
                        m_counterBonus++;
                        //pl.URL = "Sound\\explosion.wav";
                    }
                }
                else
                {
                    if (m_aliens[i].Ypoint > m_height)
                    {
                        m_aliens.RemoveAt(i);
                    }
                    else
                    {
                        m_aliens[i].Shoot(now, m_enemyBullets);
                        m_aliens[i].Move(m_width, m_height);
                    }
                }
            }

            //Проверка на попадание в алиенов и возвращение пули в магазин
            for (int i = 0; i < m_aliens.Count; i++)
            {
                foreach (Bullet b in m_ship.GetAmmunition)
                {
                    if (!b.IsCharged && m_aliens[i].GetHealth > 0)
                    {
                        if (Collision(b, m_aliens[i]))
                        {
                            m_aliens[i].Damaged(b, now);
                            b.IsCharged = true;
                        }
                    }
                }
            }

            //Повреждение корабля снарядами
            for (int i = 0; i < m_enemyBullets.Count; i++)
            {
                if (Collision(m_ship, m_enemyBullets[i]))
                {
                    m_ship.Damaged(m_enemyBullets[i]);
                    if (m_ship.GetArmor == 0)
                        m_ship.Collapse(m_width / 2, m_height - m_ship.GetHeight / 2,
                        now);
                    m_enemyBullets.RemoveAt(i);
                }
            }

            //Столкновения корабля и противников
            foreach (Alien a in m_aliens)
                if (Collision(m_ship, a))
                {
                    m_ship.Collapse(m_width / 2, m_height - m_ship.GetHeight / 2,
                        now);
                    break;
                }

            //Расчет падения бонусов
            if (m_bonus != null)
            {
                m_bonus.Fall();
                if (m_bonus.Ypoint > m_height)
                {
                    m_bonus = null;
                }
                else
                {
                    if (Collision(m_ship, m_bonus))
                    {
                        m_bonus.Improve(m_ship);
                        m_bonus = null;
                    }
                }
            }
            else
            {
                if (m_counterBonus >= m_needForDrop)
                {
                    m_counterBonus = 0;
                    m_bonus = m_bonuses[m_rnd.Next(m_bonuses.Count)];
                    m_bonus.Drop(m_rnd.Next(20, m_width - 20), 0);
                }
            }
        }

        public void ClearAll()
        {
            m_aliens.Clear();
            m_counterBonus = 0;
            m_enemyBullets.Clear();
            foreach (Bullet b in m_ship.GetAmmunition)
                b.IsCharged = true;
        }
    }
}
