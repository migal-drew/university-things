using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectSunshine.Logic.Aliens
{
    public class GunBoat : Alien
    {
        private DateTime m_stop;

        public GunBoat(int x, int y, Direction position)
            : base(x, y, 60, 60, position)
        {
            m_cash = 200;
            m_speed = 2;
            m_health = 120;
            m_stop = DateTime.MaxValue;
        }

        public override void Move(int width, int height)
        {
            if (DateTime.Now.Subtract(m_stop).Seconds > 3)
                m_speed = 4;
            else
            {
                if (m_stop == DateTime.MaxValue && m_y >= height / 2)
                {
                    m_stop = DateTime.Now;
                    m_speed = 0;
                }
            }
            m_y += m_speed;
        }
    }
}
