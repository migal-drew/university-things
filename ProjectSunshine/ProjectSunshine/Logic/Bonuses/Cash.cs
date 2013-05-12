using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectSunshine.Logic.Bonuses
{
    public class Cash : Bonus
    {
        private int m_cash;

        public Cash()
            : base()
        {
            m_cash = 500;
        }

        public override void Improve(Ship s)
        {
            s.AddCash(m_cash);
        }
    }
}
