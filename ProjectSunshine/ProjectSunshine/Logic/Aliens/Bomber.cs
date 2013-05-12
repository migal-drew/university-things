using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectSunshine.Logic.Aliens
{
    public class Bomber : Alien
    {
        public Bomber(int x, int y, Direction position)
            : base(x, y, 60, 60, position)
        {
            m_cash = 100;
            m_speed = 2;
            m_health = 100;
        }
    }
}
