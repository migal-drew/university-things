using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectSunshine.Logic.Bonuses
{
    public class EnhanceWeapon : Bonus
    {
        public EnhanceWeapon()
            : base()
        {
            m_needForDrop = 1;
        }

        public override void Improve(Ship s)
        {
            if (s.PowerLevel == 5)
                s.AddCash(500);
            else
                s.SetPowerLevel(s.PowerLevel + 1);
        }
    }
}
