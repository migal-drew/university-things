using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectSunshine.Logic.Bonuses
{
    public class Battery : Bonus
    {
        public Battery()
            : base()
        {
            m_needForDrop = 1;
        }

        public override void Improve(Ship s)
        {
            if (s.GetShield == 100)
                s.AddCash(100);
            else
                s.GetShield = 100;
        }
    }
}
