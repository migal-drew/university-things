using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectSunshine.Logic
{
    public class SpacePoint
    {
        private int m_x;
        public int X
        {
            get
            {
                return m_x;
            }
            set
            {
                m_x = value;
            }
        }

        private int m_y;
        public int Y
        {
            get
            {
                return m_y;
            }
            set
            {
                m_y = value;
            }
        }

        public SpacePoint(int x, int y)
        {
            m_x = x;
            m_y = y;
        }
    }
}
