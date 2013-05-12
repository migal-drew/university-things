using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectSunshine.Logic.Bonuses
{
    public class Spanner : Bonus
    {
        public Spanner()
            : base()
        {
            m_needForDrop = 1;
        }

        public override void Improve(Ship s)
        {
            if (s.GetArmor == 100)
                s.AddCash(100);
            else
                s.GetArmor = 100;
        }
    }
}
