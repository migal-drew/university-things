using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectSunshine.Logic
{
    public abstract class SpaceObject
    {
        //Координаты центра!
        protected int m_x;
        public int Xpoint
        {
            get
            {
                return m_x;
            }
        }

        protected int m_y;
        public int Ypoint
        {
            get
            {
                return m_y;
            }
        }

        protected int m_width;
        public int GetWidth
        {
            get
            {
                return m_width;
            }
        }

        protected int m_height;
        public int GetHeight
        {
            get
            {
                return m_height;
            }
        }

        protected int m_doubleSquare;
        public int GetDoubleSquare
        {
            get
            {
                return m_doubleSquare;
            }
        }
    }
}
