using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectSunshine.Logic.Bonuses
{
    public class Life : Bonus
    {
        public Life()
            : base()
        {
            m_needForDrop = 1;
        }

        public override void Improve(Ship s)
        {
            if (s.GetLifes == 3)
            {
                s.GetShield = 100;
                s.GetArmor = 100;
            }
            else
            {
                s.AddLife();
            }
        }
    }
}
