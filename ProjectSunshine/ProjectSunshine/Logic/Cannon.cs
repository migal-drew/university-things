using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectSunshine.Logic
{
    public class Cannon
    {
        private List<Bullet> m_magazine;

        private int m_onX;
        private int m_onY;

        private int m_dx;
        public int dx
        {
            get
            {
                return m_dx;
            }
            set
            {
                m_dx = value;
            }
        }
        private int m_dy;
        public int dy
        {
            get
            {
                return m_dy;
            }
            set
            {
                m_dy = value;
            }
        }

        private Ship m_ship;

        private bool m_isActivated;
        public bool IsActivated
        {
            get
            {
                return m_isActivated;
            }
            set
            {
                m_isActivated = value;
            }
        }

        /// <summary>
        /// При инициализации пушка не является активной. Требуется явная активация.
        /// Привязка к кораблю. Смещение относительно корпуса(onX, onY).
        /// shift и speed: параметры для снарядов, смещение по X и Y соответственно.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="magazine"></param>
        public Cannon(Ship s, int onX, int onY, int dx, int dy)
        {
            m_onX = onX;
            m_onY = onY;
            m_ship = s;
            m_magazine = s.GetAmmunition;
            m_isActivated = false;
            m_dx = dx;
            m_dy = dy;
        }

        public bool GunShot(Bullet b)
        {
            if (m_isActivated)
            {
                if (b.IsCharged)
                {
                    b.Discharge(m_ship.Xpoint + m_onX, m_ship.Ypoint + m_onY,
                        m_dx, m_dy);
                    return true;
                }
            }
            return false;
        }
    }
}
