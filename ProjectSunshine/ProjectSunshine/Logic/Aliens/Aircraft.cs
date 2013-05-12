using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectSunshine.Logic.Aliens
{
    public class Aircraft : Alien
    {
        public Aircraft(int x, int y, Direction position)
            : base(x, y, 40, 60, position)
        {
            m_cash = 50;
            m_speed = 4;
            m_health = 40;
        }

        public override bool Shoot(DateTime now, List<Bullet> bullets)
        {
            return false;
            //Nothing!
        }
    }
}
