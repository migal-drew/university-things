using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectSunshine.Logic
{
    public class Ship : SpaceObject //sealed
    {
        private WMPLib.WindowsMediaPlayer wmp;
        private int m_prevShield;
        private int m_prevArmor;

        protected SpacePoint[] m_points;
        public SpacePoint[] GetPoints
        {
            get
            {
                return m_points;
            }
        }

        protected int m_armor;
        public int GetArmor
        {
            get
            {
                return m_armor;
            }
            set
            {
                if (value <= 100)
                    m_armor = value;
            }
        }

        protected int m_shield;
        public int GetShield
        {
            get
            {
                return m_shield;
            }
            set
            {
                if (value <= 100)
                    m_shield = value;
            }
        }

        protected int m_direction;
        public int GetDirection
        {
            get
            {
                return m_direction;
            }
        }

        protected List<Bullet> m_magazine;
        public List<Bullet> GetAmmunition
        {
            get
            {
                return m_magazine;
            }
        }

        protected DateTime m_lastRefresh;
        public DateTime GetLastRefresh
        {
            get
            {
                return m_lastRefresh;
            }
        }

        protected int m_lives;
        public int GetLifes
        {
            get
            {
                return m_lives;
            }
        }

        protected DateTime m_startOfInvulnerability;
        public DateTime StartOfInvulnerability
        {
            get
            {
                return m_startOfInvulnerability;
            }
            set
            {
                m_startOfInvulnerability = value;
            }
        }
        protected bool m_invulnerability;
        public bool IsInvulnerable
        {
            get
            {
                return m_invulnerability;
            }
            set
            {
                m_invulnerability = value;
            }
        }
        protected int m_secondsOfInvulnerability;

        private bool m_resurrection;
        public bool Resurrection
        {
            get
            {
                return m_resurrection;
            }
            set
            {
                m_resurrection = value;
            }
        }

        protected DateTime m_cooling;

        protected bool m_overheat;
        public bool Overheat
        {
            get
            {
                return m_overheat;
            }
        }
        protected int m_heat; //0-100
        public int GetHeat
        {
            get
            {
                return m_heat;
            }
        }

        protected DateTime m_lastShot;
        public DateTime GetLastShot
        {
            get
            {
                return m_lastShot;
            }
            set
            {
                m_lastShot = value;
            }
        }

        private int m_currentBullet;

        private Cannon m_mainCannon;
        public Cannon MainCannon
        {
            get
            {
                return m_mainCannon;
            }
        }
        private Cannon m_leftCannon;
        public Cannon LeftCannon
        {
            get
            {
                return m_leftCannon;
            }
        }
        private Cannon m_rightCannon;
        public Cannon RightCannon
        {
            get
            {
                return m_rightCannon;
            }
        }
        private Cannon m_leftSideCannon;
        public Cannon LeftSideCannon
        {
            get
            {
                return m_leftSideCannon;
            }
        }
        private Cannon m_rightSideCannon;
        public Cannon RightSideCannon
        {
            get
            {
                return m_rightSideCannon;
            }
        }

        protected int m_powerLevel;
        public int PowerLevel
        {
            get
            {
                return m_powerLevel;
            }
        }

        protected int m_cash;
        public int GetCash
        {
            get
            {
                return m_cash;
            }
        }

        /// <summary>
        /// Параметры: начальные координаты и размеры корабля.
        /// </summary>
        /// <param name="y"></param>
        public Ship(int x, int y, int width, int height)
        {
            m_x = x;
            m_y = y;
            m_direction = 0;

            m_armor = 100;
            m_shield = 100;
            m_prevArmor = m_armor;
            m_prevShield = m_shield;

            m_lives = 2;
            m_cash = 0;
            m_powerLevel = 1;
            m_invulnerability = false;
            m_secondsOfInvulnerability = 10;
            m_resurrection = false;

            m_overheat = false;

            m_height = height;
            m_width = width;
            m_doubleSquare = m_height * m_width;

            m_points = new SpacePoint[3];
            m_points[0] = new SpacePoint(m_x - m_width / 2, m_y + m_height / 2);
            m_points[1] = new SpacePoint(m_x, m_y - m_height / 2);
            m_points[2] = new SpacePoint(m_x + m_width / 2, m_y + m_height / 2);

            //Инициализация боеприпасов
            m_magazine = new List<Bullet>();
            for (int i = 0; i < 50; i++)
            {
                m_magazine.Add(new Bullet());
            }
            m_currentBullet = 0;

            int speed = -10;
            int shift = 0;
            //Размещение оружия относительно корпуса корабля
            m_mainCannon = new Cannon(this, 0, - m_height / 2, shift, speed);
            m_leftCannon = new Cannon(this, -m_width / 2 + 10, -m_height / 2 + 5, shift, speed);
            m_rightCannon = new Cannon(this, m_width / 2 - 10, -m_height / 2 + 5, shift, speed);
            m_leftSideCannon = new Cannon(this, -m_width / 2 + 3, 0, shift, speed);
            m_rightSideCannon = new Cannon(this, m_width / 2 - 3, 0, shift, speed);

            SetPowerLevel(1);

            m_lastRefresh = DateTime.Now;

            wmp = new WMPLib.WindowsMediaPlayer();
        }

        public void Move(int x, int y)
        {
            if (x < m_x)
                m_direction = 1;
            else
                if (x > m_x)
                    m_direction = 2;
                else
                    m_direction = 0;
            m_x = x;
            m_y = y;

            //Пересчет координат треугольника
            m_points[0].X = m_x - m_width / 2;
            m_points[0].Y = m_y + m_height / 2;
            m_points[1].X = m_x;
            m_points[1].Y = m_y - m_height / 2;
            m_points[2].X = m_x + m_width / 2;
            m_points[2].Y = m_points[0].Y;
        }

        private void MoveShutter()
        {
            m_currentBullet++;
            if (m_currentBullet > m_magazine.Count - 1)
                m_currentBullet = 0;
        }

        public bool Shoot()
        {
            bool success = false;
            if (!m_overheat)
            {
                if (m_mainCannon.GunShot(m_magazine[m_currentBullet]))
                {
                    MoveShutter();
                    success = true;
                }
                if (m_leftCannon.GunShot(m_magazine[m_currentBullet]))
                {
                    MoveShutter();
                    success = true;
                }
                if (m_rightCannon.GunShot(m_magazine[m_currentBullet]))
                {
                    MoveShutter();
                    success = true;
                }
                if (m_rightSideCannon.GunShot(m_magazine[m_currentBullet]))
                {
                    MoveShutter();
                    success = true;
                }
                if (m_leftSideCannon.GunShot(m_magazine[m_currentBullet]))
                {
                    MoveShutter();
                    success = true;
                }
            }
            
            if (success)
            {
                m_heat += 6 * PowerLevel;
                if (m_heat >= 100)
                {
                    m_heat = 100;
                    m_overheat = true;
                    m_cooling = DateTime.Now;
                }
            }

            return success;
        }

        public void Damaged(Bullet b)
        {
            if (m_invulnerability != true)
            {
                int damage = b.GetDamage;

                if (m_shield > 0)
                    damage = (int)(b.GetDamage * 1.1);

                if (damage > m_shield)
                {
                    damage -= m_shield;
                    m_shield = 0;
                    m_armor -= damage;
                    if (m_armor < 0)
                        m_armor = 0;
                }
                else
                    m_shield -= damage;
            }
        }
        
        /// <summary>
        /// Ускоренное уничтожение обшивки
        /// Parameters: Позиция для воскрешения, если корпус уничтожен
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="now"></param>
        public void Collapse(int x, int y, DateTime now)
        {
            if (m_invulnerability != true)
            {
                //SetPowerLevel(PowerLevel - 1);
                //if (m_lifes > 0)
                //{
                //    m_lifes--;
                //    m_invulnerability = true;
                //    m_startOfInvulnerability = now;
                //    m_shield = 100;
                //    m_armor = 100;
                //    Move(x, y);
                //    m_resurrection = true;
                //}
                //else
                //{
                //    m_shield = 0;
                //    m_armor = 0;
                //}
                m_armor -= 5;
                if (m_armor <= 0)
                {
                    m_armor = 0;
                    if (m_lives > 0)
                    {
                        m_lives--;
                        m_invulnerability = true;
                        m_startOfInvulnerability = now;
                        m_shield = 100;
                        m_armor = 100;
                        Move(x, y);
                        m_resurrection = true;
                    }
                    else
                    {
                        m_shield = 0;
                        m_armor = 0;
                    }
                }
            }
        }

        public void SetPowerLevel(int level)
        {
            if (level == 1)
            {
                MainCannon.IsActivated = true;
                LeftCannon.IsActivated = false;
                RightCannon.IsActivated = false;
                LeftSideCannon.IsActivated = false;
                RightSideCannon.IsActivated = false;
                m_powerLevel = level;
                return;
            }
            if (level == 2)
            {
                MainCannon.IsActivated = false;
                LeftCannon.IsActivated = true;
                RightCannon.IsActivated = true;
                LeftSideCannon.IsActivated = false;
                RightSideCannon.IsActivated = false;

                LeftCannon.dx = 0;
                RightCannon.dx = 0;

                m_powerLevel = level;
                return;
            }
            if (level == 3)
            {
                MainCannon.IsActivated = true;
                LeftCannon.IsActivated = false;
                RightCannon.IsActivated = false;
                LeftSideCannon.IsActivated = true;
                RightSideCannon.IsActivated = true;

                LeftSideCannon.dx = -4;
                RightSideCannon.dx = 4;

                m_powerLevel = level;
                return;
            }
            if (level == 4)
            {
                MainCannon.IsActivated = false;
                LeftCannon.IsActivated = true;
                RightCannon.IsActivated = true;
                LeftSideCannon.IsActivated = true;
                RightSideCannon.IsActivated = true;

                LeftCannon.dx = 0;
                RightCannon.dx = 0;
                LeftSideCannon.dx = -4;
                RightSideCannon.dx = 4;

                m_powerLevel = level;
                return;
            }
            if (level == 5)
            {
                MainCannon.IsActivated = true;
                LeftCannon.IsActivated = true;
                RightCannon.IsActivated = true;
                LeftSideCannon.IsActivated = true;
                RightSideCannon.IsActivated = true;

                LeftCannon.dx = -2;
                RightCannon.dx = 2;
                LeftSideCannon.dx = -4;
                RightSideCannon.dx = 4;

                m_powerLevel = level;
                return;
            }
        }

        /// <summary>
        /// Регенерация щита
        /// </summary>
        private void RefreshShield(DateTime now)
        {
            if (m_shield > 0)
            {
                if (now.Subtract(m_lastRefresh).Seconds > 4)
                {
                    m_shield += 10;
                    m_lastRefresh = DateTime.Now;
                }
                if (m_shield > 100)
                    m_shield = 100;
            }
            else
            {
                if (now.Subtract(m_lastRefresh).Seconds > 15)
                {
                    m_shield += 50;
                    m_lastRefresh = DateTime.Now;
                }
            }
        }

        public void AddLife()
        {
            if (m_lives < 3)
                m_lives++;
        }

        public void AddCash(int cash)
        {
            m_cash += cash;
        }

        /// <summary>
        /// Пересчет характеристик корабля.
        /// </summary>
        /// <param name="now"></param>
        public void Recalculate(DateTime now)
        {
            RefreshShield(now);
            if (m_prevShield != m_shield)
            {
                if (m_prevShield == 0 && m_shield > 0)
                {
                    wmp.URL = "Sound\\compRestoredShield.wav";
                }
                if (m_prevShield > 0 && m_shield == 0)
                {
                    wmp.URL = "Sound\\compShieldFailed.wav";
                }
            }

            if (m_prevArmor > 20 && m_armor < 20)
            {
                wmp.URL = "Sound\\compShipDamage.wav";
                //m_prevArmor = m_armor;
            }

            if (m_overheat)
            {
                if (now.Subtract(m_cooling).Milliseconds > 500)
                {
                    m_heat -= 20;
                    m_cooling = now;
                    if (m_heat <= 0)
                    {
                        m_heat = 0;
                        m_overheat = false;
                    }
                }
            }
            else
            {
                if (now.Subtract(m_cooling).TotalMilliseconds > 100)
                {
                    m_heat -= 6;
                    m_cooling = now;
                    if (m_heat < 0)
                        m_heat = 0;
                }
            }
            
            if (m_invulnerability == true)
            {
                if (now.Subtract(m_startOfInvulnerability).Seconds >
                    m_secondsOfInvulnerability)
                    m_invulnerability = false;
            }

            m_prevShield = m_shield;
            m_prevArmor = m_armor;
        }

        public void Restart()
        {
            m_prevArmor = 100;
            m_prevShield = 100;
            m_armor = 100;
            m_shield = 100;
            m_cash = 0;
            SetPowerLevel(1);
            m_lives = 2;
        }
    }
}
